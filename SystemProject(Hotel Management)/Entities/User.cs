using System.ComponentModel.DataAnnotations;
using System.Data;
using SystemProject_Hotel_Management_.Models;

namespace SystemProject_Hotel_Management_.Entities
{
    public class User
    {
        [Key]
        public int ID { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string Role { get; set; }
        public ICollection<Reservation1> Reservations1 { get; set; }

    }
}
