using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SystemProject_Hotel_Management_.Entities
{
    public class OrderItem
    {
        [Key]
        public int ID { get; set; }
        public int OrderID { get; set; } // Foreign key for Order
        public int FoodItemID { get; set; } // Foreign key for FoodItem
        public int Quantity { get; set; }
        public decimal Subtotal { get; set; }

        // Navigation properties
        [ForeignKey("OrderID")]
        public Order Order { get; set; }

        [ForeignKey("FoodItemID")]
        public FoodItem FoodItem { get; set; }
    }
}
