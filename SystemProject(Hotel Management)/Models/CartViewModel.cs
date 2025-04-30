using SystemProject_Hotel_Management_.Entities;

namespace SystemProject_Hotel_Management_.Models
{
    public class CartViewModel
    {
        public List<CartItem> CartItems { get; set; }
        public double Subtotal { get; set; }
    }
}