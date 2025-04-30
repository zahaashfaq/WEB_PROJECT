using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SystemProject_Hotel_Management_.Models;

namespace SystemProject_Hotel_Management_.Controllers
{
    public class ReservationController : Controller
    {
        private readonly BiteHubDbContext _context;

        public ReservationController(BiteHubDbContext context)
        {
            _context = context;
        }

        public IActionResult AvailableTables()
        {
            var tables = _context.Tables.Where(t => t.Status == "Available").ToList();
            return View(tables);
        }
        [HttpPost]
        public IActionResult ReserveTable(int tableId)
        {
            var userId = HttpContext.Session.GetInt32("UserId");
            if (!userId.HasValue)
            {

                return RedirectToAction("Register", "Account", new { returnUrl = Request.Path });
                //return RedirectToAction("Login", "Account");
            }
            // Check if the table is still available
            var table = _context.Tables.FirstOrDefault(t => t.Id == tableId && t.Status == "Available");

            if (table == null)
            {
                TempData["Error"] = "The selected table is no longer available.";
                return RedirectToAction("AvailableTables");
            }

            // Create a new reservation
            var reservation = new Reservation1
            {
                ReservationDate = DateTime.Today,
                ReservationTime = DateTime.Now.ToString("hh:mm tt"),
                PartySize = table.Seats,
                Status = "Booked",
                UserId = userId.Value,
                TableId = table.Id
            };

            Random random = new Random();
            string reservationNumber = "R-" + random.Next(1000, 9999).ToString();

            // Update table status and save reservation
            table.Status = "Booked";
            _context.Reservations1.Add(reservation);
            _context.SaveChanges();

            // Pass reservation details to the confirmation view
            var user = _context.Users.FirstOrDefault(u => u.ID == userId.Value);
            var confirmationModel = new ReservationConfirmationViewModel
            {
                ReservationNumber = reservationNumber,
                UserName = user?.Name ?? "Unknown",
                ReservationDate = reservation.ReservationDate,
                ReservationTime = reservation.ReservationTime,
                PartySize = reservation.PartySize,
                TableNumber = table.TableNumber
            };

            TempData["Success"] = "Table reserved successfully!";
            return View("ReservationConfirmation", confirmationModel);  
        }

    }
}
