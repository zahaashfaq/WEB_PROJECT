using System.ComponentModel.DataAnnotations;

namespace SystemProject_Hotel_Management_.Entities
{
    public class Cart
    {
        [Key]
        public int ID { get; set; }
        public int UserID { get; set; }
        public User User { get; set; }
        public ICollection<CartItem> CartItems { get; set; }
    }
}
