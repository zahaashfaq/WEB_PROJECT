using SystemProject_Hotel_Management_.Entities;

namespace SystemProject_Hotel_Management_.Models
{
    public class Reservation1
    {
        public int Id { get; set; }
        public DateTime ReservationDate { get; set; }
        public string ReservationTime { get; set; }
        public int PartySize { get; set; }
        public string Status { get; set; } // "Booked"

        // Foreign Key for User
        public int UserId { get; set; }

        // Navigation Property to User
        public User User { get; set; }

        // Foreign Key for Table
        public int TableId { get; set; }

        // Navigation Property to Table
        public Table Table { get; set; }
    }
}
