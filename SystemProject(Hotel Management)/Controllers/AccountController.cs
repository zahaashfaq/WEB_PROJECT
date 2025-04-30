using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SystemProject_Hotel_Management_.Entities;
using SystemProject_Hotel_Management_.Models;

namespace SystemProject_Hotel_Management_.Controllers
{
    public class AccountController : Controller
    {
        private readonly BiteHubDbContext _context;

        public AccountController(BiteHubDbContext context)
        {
            _context = context;
        }

        // GET: Login Page
        public IActionResult Login()
        {
            return View(new LoginViewModel());
        }

        // POST: Login Page (Form Submission)
        [HttpPost]
        public IActionResult Login(LoginViewModel model, string? returnUrl)
        {
            if (ModelState.IsValid)
            {
                // Check if admin is logging in
                if (model.Email == "admin@bitehub.com" && model.Password == "123")
                {
                    return RedirectToAction("AdminDashboard");
                }

                // Check user credentials in the database
                var user = _context.Users.FirstOrDefault(u => u.Email == model.Email && u.Password == model.Password);
                if (user != null)
                {
                    // Store userId in session
                    HttpContext.Session.SetInt32("UserId", user.ID);

                    // Redirect based on user role
                    if (!string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl))
                    {
                        return Redirect(returnUrl);
                    }
                    else
                    {
                        return RedirectToAction("ViewMenu", "Menu");
                    }
                }

                ViewBag.Message = "Invalid email or password.";
            }

            return View(model);
        }


        // Admin Dashboard
        public IActionResult AdminDashboard()
        {
            return View();
        }

        // Customer Dashboard (Optional)
        public IActionResult CustomerDashboard()
        {
            return View();
        }
    

    // GET: Register Page
    public IActionResult Register()
        {
            return View(new RegistrationViewModel());
        }

        // POST: Register Page (Form Submission)
        [HttpPost]
        public IActionResult Register(RegistrationViewModel model)
        {
            if (ModelState.IsValid)
            {
                var existingUser = _context.Users.FirstOrDefault(u => u.Email == model.Email);
                if (existingUser != null)
                {
                    ViewBag.Message = "Email already exists!";
                    return View(model);  // Return the view with a message
                }

                // Create a new User object
                var user = new User
                {
                    Name = model.Name,
                    Email = model.Email,
                    Password = model.Password,  // You should hash the password in a real application
                    Role = model.Role
                };

                // Add the user to the database
                _context.Users.Add(user);
                 _context.SaveChanges();
                HttpContext.Session.SetInt32("UserId", user.ID);

                return RedirectToAction("AvailableTables" , "Reservation");  // Redirect to the Login page
            }

            return View(model);  // Return to the same view if model validation fails
        }
        //Forgot password
        [HttpGet]
        public IActionResult ForgotPassword()
        {
            return View();
        }
        [HttpPost]
        public IActionResult ForgotPassword(ForgotPasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                if (model.NewPassword != model.ConfirmPassword)
                {
                    ViewBag.Message = "Passwords do not match!";
                    return View(model);
                }

                var user = _context.Users.FirstOrDefault(u => u.Email == model.Email);
                if (user == null)
                {
                    ViewBag.Message = "User not found!";
                    return View(model);
                }

                user.Password = model.NewPassword; 
                _context.Users.Update(user);
                _context.SaveChanges();

                return RedirectToAction("ViewMenu", "Menu");
            }

            return View(model);
        }


        // Log Out
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Login");
        }
    }

}
