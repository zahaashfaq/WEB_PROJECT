using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using SystemProject_Hotel_Management_.Entities;
using SystemProject_Hotel_Management_.Hubs;
using SystemProject_Hotel_Management_.Models;

namespace SystemProject_Hotel_Management_.Controllers
{
    public class CartController : Controller
    {
        private readonly IHubContext<OrderHub> _hubContext;

        private readonly BiteHubDbContext _context;

        public CartController(BiteHubDbContext context , IHubContext<OrderHub> hubContext)
        {
            _context = context;
            _hubContext = hubContext;
        }

       
        [HttpPost]
        public async Task<IActionResult> UpdateOrderStatus([FromBody] OrderUpdateModel model)
        {
            if (model == null)
            {
                return BadRequest(new { success = false, message = "Invalid request" });
            }

            // Find the order and update the status in the database (pseudo-code)
            var order = _context.Orders.Find(model.OrderId);
            if (order == null)
            {
                return NotFound(new { success = false, message = "Order not found" });
            }

            order.Status = model.Status;
            _context.SaveChanges();

            await _hubContext.Clients.All.SendAsync("ReceiveOrderStatusUpdate", model.OrderId, model.Status);

            return Ok(new { success = true });
        }
        //Add item to cart
        [HttpPost]
        public IActionResult AddToCart(int foodItemId, int quantity)
        {

            var foodItem = _context.FoodItems.Find(foodItemId); // Ensure the FoodItem exists
            if (foodItem == null)
            {
                // Handle the case where the food item does not exist
                ModelState.AddModelError("FoodItemNotFound", "The selected food item does not exist.");
                return RedirectToAction("Index", "Main"); // or return an error view
            }

            var userId = HttpContext.Session.GetInt32("UserId");

            if (!userId.HasValue)
            {

                return RedirectToAction("Login", "Account", new { returnUrl = Request.Path });
                //return RedirectToAction("Login", "Account");
            }

            var cart = _context.Carts.Include(c => c.CartItems).FirstOrDefault(c => c.UserID == userId);
            if (cart == null)
            {
                cart = new Cart
                {
                    UserID = userId.Value,
                    CartItems = new List<CartItem>()
                };
                _context.Carts.Add(cart);
            }
            var cartItem = cart.CartItems.FirstOrDefault(ci => ci.FoodItemID == foodItemId);
            if (cartItem == null)
            {
                cart.CartItems.Add(new CartItem
                {
                    FoodItemID = foodItemId,
                    Quantity = quantity
                });
            }
            else
            {

                cartItem.Quantity += quantity;
            }

            _context.SaveChanges();
            return NoContent();

        }

        private int GetLoggedInUserId()
        {

            return 1; 
        }

        // View the cart
        public IActionResult ViewCart()
        {
            var userId = HttpContext.Session.GetInt32("UserId");

            if (userId == null)
            {
                return RedirectToAction("Login", "Account");
            }

            var cartItems = _context.CartItems
          .Include(ci => ci.FoodItem)
          .Where(ci => ci.Cart.UserID == userId)
          .ToList();

            // Mapping CartItems to CartItemViewModel
            var viewModel = cartItems.Select(ci => new CartItemViewModel
            {
                ID = ci.ID,
                FoodItemName = ci.FoodItem.Name,
                Quantity = ci.Quantity,
                Price = ci.FoodItem.Price
            }).ToList();

            return View(viewModel);


        }
        public IActionResult ClearCart()
        {
            var userId = HttpContext.Session.GetInt32("UserId");

            if (userId == null)
            {
                return RedirectToAction("Login", "Account");
            }

            var cart = _context.Carts.FirstOrDefault(c => c.UserID == userId);

            if (cart != null)
            {
                // Remove all CartItems for the specific cart
                var cartItems = _context.CartItems.Where(ci => ci.CartID == cart.ID).ToList();
                _context.CartItems.RemoveRange(cartItems);
                _context.SaveChanges();
            }

            return RedirectToAction("ViewCart");
        }

        [HttpPost]
        public IActionResult RemoveFromCart(int cartItemId)
        {
            var cartItem = _context.CartItems
                .Include(ci => ci.Cart)
                .FirstOrDefault(ci => ci.ID == cartItemId);

            if (cartItem == null)
            {
                return NotFound(); // Item not found
            }

            // Remove the specific cart item
            _context.CartItems.Remove(cartItem);
            _context.SaveChanges();

            // Redirect to the cart view to reflect changes
            return RedirectToAction("ViewCart");
        }

        public IActionResult Checkout()
        {
            var userId = HttpContext.Session.GetInt32("UserId");

            if (userId == null)
            {
                return RedirectToAction("Login", "Account"); // Ensure the user is logged in
            }

            // Retrieve the user's cart
            var cart = _context.Carts
                .Include(c => c.CartItems)
                .ThenInclude(ci => ci.FoodItem)
                .FirstOrDefault(c => c.UserID == userId);

            if (cart == null || !cart.CartItems.Any())
            {
                return RedirectToAction("ViewCart", "Cart"); // If the cart is empty, redirect back to the cart
            }

            // Map cart items to CartItemViewModel
            var cartItems = cart.CartItems.Select(ci => new CartItemViewModel
            {
                ID = ci.ID,
                FoodItemName = ci.FoodItem.Name,
                Quantity = ci.Quantity,
                Price = ci.FoodItem.Price
            }).ToList();

            var checkoutModel = new CheckoutViewModel
            {
                CartItems = cartItems,
                TotalAmount = cartItems.Sum(ci => ci.Total)
            };

            return View(checkoutModel); // Pass the populated model to the view
        }



        [HttpPost]
        public IActionResult Checkout(CheckoutViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model); // If validation fails, show the form again
            }

            var userId = HttpContext.Session.GetInt32("UserId");

            if (userId == null)
            {
                return RedirectToAction("Login", "Account"); // Ensure the user is logged in
            }

            // Create and save the order in the database with delivery address details
            var order = new Order
            {
                UserID = (int)userId,
                TotalAmount = model.TotalAmount,
                Status = "Pending", // Order status can be Pending, Processing, etc.
                CreatedAt = DateTime.Now,
                StreetAddress = model.StreetAddress,
                City = model.City,
                
                PhoneNumber = model.PhoneNumber
            };

            _context.Orders.Add(order);
            _context.SaveChanges(); // Save the order

            // Now, create and save the order items
            //foreach (var cartItem in model.CartItems)
            //{
            //    var orderItem = new OrderItem
            //    {
            //        OrderID = order.ID,            // Link the order item to the current order
            //        FoodItemID = cartItem.ID, // Link to the food item
            //        Quantity = cartItem.Quantity,    // Quantity of the food item ordered
            //        Subtotal = cartItem.Quantity * cartItem.Price // Calculate subtotal
            //    };

            //    _context.OrderItems.Add(orderItem);
            //}
            foreach (var cartItem in model.CartItems)
            {
                var foodItem = _context.FoodItems.FirstOrDefault(f => f.Name == cartItem.FoodItemName);

                if (foodItem == null)
                {
                    Console.WriteLine($"Food item not found: {cartItem.FoodItemName}");
                    continue;
                }

                var orderItem = new OrderItem
                {
                    OrderID = order.ID,
                    FoodItemID = foodItem.ID,
                    Quantity = cartItem.Quantity,
                    Subtotal = cartItem.Total
                };

                _context.OrderItems.Add(orderItem);
            }

            try
            {
                _context.SaveChanges();
            }

            catch (Exception ex)
            {
                Console.WriteLine($"Error saving order items: {ex.Message}");
            }

            // After placing the order, clear the cart if needed
            var cart = _context.Carts.FirstOrDefault(c => c.UserID == userId);
            var cartItems = _context.CartItems.Where(ci => ci.CartID == cart.ID).ToList();
            _context.CartItems.RemoveRange(cartItems);
            _context.SaveChanges(); // Clear the cart

            // Redirect to a confirmation page or another appropriate view
            return RedirectToAction("OrderConfirmation", "Cart", new { orderId = order.ID });
        }
        public IActionResult OrderConfirmation(int orderId)
        {
            // Retrieve the order by ID
            var order = _context.Orders
                .Include(o => o.User) // Include user details if needed
                .Include(o => o.OrderItems)
                .ThenInclude(oi => oi.FoodItem) // Include food item details
                .FirstOrDefault(o => o.ID == orderId);

            if (order == null)
            {
                return NotFound(); // Return 404 if the order does not exist
            }

            // Map the order details to a view model
            var orderDetailsViewModel = new OrderDetailsViewModel
            {
                OrderID = order.ID,
                UserName = order.User?.Name ?? "Guest",
                TotalAmount = order.TotalAmount,
                CreatedAt = order.CreatedAt,
                StreetAddress = order.StreetAddress,
                City = order.City,
                PhoneNumber = order.PhoneNumber,
                Status = order.Status,
                OrderItems = order.OrderItems.Select(oi => new OrderItemViewModel
                {
                    FoodItemName = oi.FoodItem.Name,
                    Quantity = oi.Quantity,
                    Subtotal = oi.Subtotal
                }).ToList()
            };

            return View(orderDetailsViewModel); // Pass the view model to the view
        }

    }
}

public class OrderUpdateModel
{
    public int OrderId { get; set; }
    public string Status { get; set; }
}