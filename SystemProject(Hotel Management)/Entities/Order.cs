using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SystemProject_Hotel_Management_.Entities
{
    public class Order
    {
        [Key]
        public int ID { get; set; }
        public int UserID { get; set; }
        public User User { get; set; }
        public decimal TotalAmount { get; set; }
        public string Status { get; set; }
        public DateTime CreatedAt { get; set; }
        public string StreetAddress { get; set; }
        public string City { get; set; }
       
        public string PhoneNumber { get; set; }
        
        public List<OrderItem> OrderItems { get; set; }
    }

}
