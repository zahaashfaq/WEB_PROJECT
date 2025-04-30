using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SystemProject_Hotel_Management_.Models;
using static iTextSharp.text.pdf.AcroFields;

namespace SystemProject_Hotel_Management_.Controllers
{
    public class MenuController : Controller
    {
        private readonly BiteHubDbContext _context;

        public MenuController(BiteHubDbContext context)
        {
            _context = context;
        }

        public IActionResult ViewMenu()
        {

            var foodItems = _context.FoodItems
         .Select(f => new FoodItemViewModel
         {
             ID = f.ID,
             Name = f.Name,
             Price = f.Price,
             Description = f.Description,
             PictureURL = f.PictureURL,
             CategoryID = f.CategoryID
         })
         .ToList();
            return View(foodItems);
        }
        public IActionResult contactUs()
        {
            return View();
        }
        public IActionResult gallery()
        {
            return View();
        }
        public IActionResult AboutUs()
        {
            return View();
        }
        public IActionResult RenderOrderModal(int id)
        {
            var foodItem = _context.FoodItems
                                   .Where(f => f.ID == id)
                                   .Select(f => new FoodItemViewModel
                                   {
                                       ID = f.ID,
                                       Name = f.Name,
                                       Description = f.Description,
                                       Price = f.Price,
                                       PictureURL = f.PictureURL
                                   })
                                   .FirstOrDefault();

            if (foodItem == null)
            {
                return NotFound();
            }

            return PartialView("_OrderModal", foodItem);
        }
        public IActionResult Search(string query)
        {
            
            var foodItems = _context.FoodItems
                .Where(f => f.Name.Contains(query) || f.Description.Contains(query))
                .Select(f => new
                {
                    f.ID,
                    f.Name,
                    f.Price,
                    f.Description,
                    f.CategoryID,
                    f.PictureURL
                })
                .ToList();

            var model = foodItems.Select(item => new FoodItemViewModel
            {
                ID = item.ID,
                Name = item.Name,
                Price = item.Price,
                Description = item.Description,
                CategoryID = item.CategoryID,
                PictureURL = item.PictureURL
            }).ToList();

            
            return View(model);
        }


    }
}
