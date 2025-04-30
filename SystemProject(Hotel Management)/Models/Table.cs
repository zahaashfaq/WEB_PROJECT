using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using SystemProject_Hotel_Management_.Entities;

namespace SystemProject_Hotel_Management_.Models
{
    public class Table
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Table Number is required.")]
        [Range(1, 100, ErrorMessage = "Table Number must be between 1 and 100.")]
        public int TableNumber { get; set; }

        [Required(ErrorMessage = "Seats are required.")]
        [Range(1, 20, ErrorMessage = "Seats must be between 1 and 20.")]
        public int Seats { get; set; }

        [Required(ErrorMessage = "Status is required.")]
        [StringLength(50, ErrorMessage = "Status cannot exceed 50 characters.")]
        public string Status { get; set; }
       
        public List<Reservation1>? Reservations1 { get; set; }
    }
}
