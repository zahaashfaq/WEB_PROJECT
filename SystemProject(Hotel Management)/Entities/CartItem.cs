using System.ComponentModel.DataAnnotations;

namespace SystemProject_Hotel_Management_.Entities
{
    public class CartItem
    {
        [Key]
        public int ID { get; set; }
        public int CartID { get; set; }
        public Cart Cart { get; set; }
        public int FoodItemID { get; set; }
        public FoodItem FoodItem { get; set; }
        public int Quantity { get; set; }
    }
}
