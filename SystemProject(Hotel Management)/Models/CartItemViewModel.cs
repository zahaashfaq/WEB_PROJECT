namespace SystemProject_Hotel_Management_.Models
{
    public class CartItemViewModel
    {
        public int ID { get; set; }
        public string FoodItemName { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }
        public decimal Total => Quantity * Price;  // Calculate total on the fly
    }

}
