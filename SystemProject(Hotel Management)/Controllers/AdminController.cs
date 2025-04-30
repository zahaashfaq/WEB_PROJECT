using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using System.Reflection.Metadata;
using SystemProject_Hotel_Management_.Entities;
using SystemProject_Hotel_Management_.Hubs;
using SystemProject_Hotel_Management_.Models;

namespace SystemProject_Hotel_Management_.Controllers
{
    public class AdminController : Controller
    {
        private readonly BiteHubDbContext _context;
        public AdminController(BiteHubDbContext context)
        {
            _context = context;
        }

        [HttpPost]
        public async Task<IActionResult> UpdateOrderStatus(int orderId, string status)
        {
            var order = _context.Orders.FirstOrDefault(o => o.ID == orderId);

            if (order == null)
            {
                return NotFound();
            }

            order.Status = status;
            _context.SaveChanges();

            // Notify all clients  about the update
            var hubContext = HttpContext.RequestServices.GetRequiredService<IHubContext<OrderHub>>();
            await hubContext.Clients.All.SendAsync("ReceiveOrderStatusUpdate", orderId, status);

            return Ok(new { success = true });
        }

        // GET: Admin/ViewOrders
        public IActionResult ViewOrders()
        {
            // Fetch all orders from the database (you can modify this to filter as per your needs)
            var orders = _context.Orders
                 .Include(order => order.User)  // Include the User data
                 .Select(order => new OrderViewModel1
                 {
                     OrderID = order.ID,
                     CustomerName = order.User.Name,  // Assuming User has a Name property
                     OrderDate = order.CreatedAt,
                     TotalAmount = order.TotalAmount,
                     Status = order.Status,
                     StreetAddress = order.StreetAddress,
                     City = order.City,
                     PhoneNumber = order.PhoneNumber
                 })
                 .ToList();

            return View(orders);  // Pass the orders to the ViewOrders view.
        }
        public IActionResult ManageUsers()
        {
            // Fetch all users from the database
            var users = _context.Users.Select(u => new UserViewModel
            {
                UserId = u.ID,
                Name = u.Name,
                Email = u.Email,
                Role = u.Role,

            }).ToList();

            return View(users);
        }

        [HttpGet]
        public IActionResult EditUser(int id)
        {
            var user = _context.Users.FirstOrDefault(u => u.ID == id);

            if (user == null)
            {
                return NotFound(); // Handle the case where the user doesn't exist
            }

            var model = new UserViewModel
            {
                UserId = user.ID,
                Name = user.Name,
                Email = user.Email,
                Role = user.Role,

            };

            return View(model);
        }

        // POST: Save the updated user details
        [HttpPost]
        public IActionResult EditUser(UserViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model); // Return the form with validation errors
            }

            var user = _context.Users.FirstOrDefault(u => u.ID == model.UserId);

            if (user == null)
            {
                return NotFound();
            }

            // Update user details
            user.Name = model.Name;
            user.Email = model.Email;
            user.Role = model.Role;

            _context.SaveChanges();

            return RedirectToAction("ManageUsers"); // Redirect back to the user management page
        }
        [HttpGet]
        public IActionResult DeleteUser(int id)
        {
            var user = _context.Users.FirstOrDefault(u => u.ID == id);

            if (user == null)
            {
                return NotFound(); // Handle the case where the user doesn't exist
            }

            var model = new UserViewModel
            {
                UserId = user.ID,
                Name = user.Name,
                Email = user.Email
            };

            return View(model);
        }

        // POST: Delete the user
        [HttpPost]
        public IActionResult DeleteUserConfirmed(int id)
        {
            var user = _context.Users.FirstOrDefault(u => u.ID == id);

            if (user == null)
            {
                return NotFound();
            }

            _context.Users.Remove(user);
            _context.SaveChanges();

            return RedirectToAction("ManageUsers"); // Redirect back to the user management page
        }
        public IActionResult ViewReports()
        {
            // Example: Fetch orders and food items from the database to create reports
            var reports = GenerateReport();

            return View(reports);
        }

        // Generate a dynamic report based on data from your system
        private List<ReportViewModel> GenerateReport()
        {
            // Example of dynamically creating a report by fetching orders and related food items from the database
            var reports = _context.Orders
                .Select(order => new ReportViewModel
                {
                    ReportId = order.ID,
                    Title = $"Order Report - {order.ID}",
                    DateGenerated = DateTime.Now, // Set as current date or specific report date
                    Description = "Details about the order and food items.",
                    Orders = new List<OrderViewModel1>
                    {
                        new OrderViewModel1
                        {
                            OrderID = order.ID,
                            CustomerName = order.User.Name,
                            TotalAmount = order.TotalAmount,
                            OrderDate = order.CreatedAt,
                        }
                    }
                })
                .ToList();

            return reports;
        }
        public IActionResult ManageProduct()
        {
            var foodItems = _context.FoodItems
                .Include(f => f.Category)  
                .Select(f => new FoodItemViewModel
                {
                    ID = f.ID,
                    Name = f.Name,
                    Price = f.Price,
                    Description = f.Description,
                    CategoryID = f.CategoryID,
                    PictureURL = f.PictureURL
                })
                .ToList();

            return View(foodItems);
        }

        // GET: EditProduct
        public IActionResult EditProduct(int id)
        {
            var foodItem = _context.FoodItems
                .Include(f => f.Category)
                .FirstOrDefault(f => f.ID == id);

            if (foodItem == null)
            {
                return NotFound();
            }

            var viewModel = new FoodItemViewModel
            {
                ID = foodItem.ID,
                Name = foodItem.Name,
                Price = foodItem.Price,
                Description = foodItem.Description,
                CategoryID = foodItem.CategoryID,
                PictureURL = foodItem.PictureURL,
               
            };

            return View(viewModel);
        }



        // POST: EditProduct
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult EditProduct(FoodItemViewModel model)
        {
            if (ModelState.IsValid)
            {
                var foodItem = _context.FoodItems.Find(model.ID);
                if (foodItem == null)
                {
                    return NotFound();
                }

                foodItem.Name = model.Name;
                foodItem.Price = model.Price;
                foodItem.Description = model.Description;
                foodItem.CategoryID = model.CategoryID;
                foodItem.PictureURL = model.PictureURL;

                _context.SaveChanges();

                return RedirectToAction("ManageProduct");
            }

            return View(model);
        }

        // GET: DeleteProduct
        public IActionResult DeleteProduct(int id)
        {
            var foodItem = _context.FoodItems
                .Where(f => f.ID == id)
                .Select(f => new DeleteFoodItemViewModel
                {
                    ID = f.ID,
                    Name = f.Name,
                    Price = f.Price,
                    Description = f.Description,
                    PictureURL = f.PictureURL
                })
                .FirstOrDefault();

            if (foodItem == null)
            {
                return NotFound();
            }

            return View(foodItem);  // Return the View with the ViewModel
        }


        [HttpPost, ActionName("DeleteProduct")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            // Find the FoodItem by the ID
            var foodItem = _context.FoodItems.Find(id);
            if (foodItem == null)
            {
                return NotFound();
            }

            // Remove the food item from the database and save changes
            _context.FoodItems.Remove(foodItem);
            _context.SaveChanges();

            // Redirect to the ManageProduct page after deletion
            return RedirectToAction("ManageProduct");
        }


        // GET: AddProduct
        public IActionResult AddProduct()
        {
            // Fetch all categories from the database
            var categories = _context.Categories
                .Select(c => new { c.ID, c.Name })
                .ToList();

            // Populate the ViewData for the dropdown
            ViewData["Categories"] = new SelectList(categories, "ID", "Name");

            return View();
        }

        // POST: AddProduct
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult AddProduct(FoodItemViewModel model)
        {
            if (ModelState.IsValid)
            {
                var foodItem = new FoodItem
                {
                    Name = model.Name,
                    Price = model.Price,
                    Description = model.Description,
                    CategoryID = model.CategoryID, // Use the selected category ID
                    PictureURL = model.PictureURL
                };

                _context.FoodItems.Add(foodItem);
                _context.SaveChanges();

                return RedirectToAction("ManageProduct");
            }

            // If the model is not valid, return the view with validation errors
            return View(model);
        }
        public IActionResult EditTables()
        {
            var tables = _context.Tables.ToList();  
            return View(tables);  
        }

        public IActionResult EditTable(int id)
        {
            var table = _context.Tables.FirstOrDefault(t => t.Id == id);
            if (table == null)
            {
                TempData["Error"] = "Table not found!";
                return RedirectToAction("EditTables");  // Redirect to the list page
            }

            return View(table);
        }


        [HttpPost]
        public IActionResult EditTable(Table table)
        {
            if (!ModelState.IsValid)
            {
                foreach (var error in ModelState.Values.SelectMany(v => v.Errors))
                {
                    Console.WriteLine(error.ErrorMessage);
                }
                TempData["Error"] = "Table not found!";
                return View(table);  // Return the same view with validation errors
            }


            var existingTable = _context.Tables.FirstOrDefault(t => t.Id == table.Id);
            if (existingTable == null)
            {
                TempData["Error"] = "Table not found!";
                return RedirectToAction("EditTables");
            }

            // Update table details
            existingTable.TableNumber = table.TableNumber;
            existingTable.Seats = table.Seats;
            existingTable.Status = table.Status;

            //_context.Tables.Update(existingTable);
            _context.SaveChanges();

            TempData["Success"] = "Table information updated successfully!";
            return RedirectToAction("EditTables");  // Redirect back to the table list
        }


        public IActionResult ViewReservation()
        {
            var reservations = _context.Reservations1.Include(r => r.Table).Include(r => r.User)
                .OrderBy(r => r.ReservationDate).ToList();  // Fetch all reservations with their related table and user data
            return View(reservations);  // Pass the reservations list to the view
        }
        [HttpPost]
        public IActionResult CancelReservation(int reservationId)
        {
            // Find the reservation and the associated table
            var reservation = _context.Reservations1.Include(r => r.Table).FirstOrDefault(r => r.Id == reservationId);

            if (reservation == null)
            {
                TempData["Error"] = "Reservation not found.";
                return RedirectToAction("ViewReservation");
            }

            // Update the reservation status to 'Cancelled'
            reservation.Status = "Cancelled";

            // Update the associated table's status to 'Available'
            reservation.Table.Status = "Available";

            // Save changes to the database
            _context.SaveChanges();

            TempData["Success"] = "Reservation has been cancelled successfully.";
            return RedirectToAction("ViewReservation");
        }
    }
}
