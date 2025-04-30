namespace SystemProject_Hotel_Management_.Models
{
    public class CheckoutViewModel
    {
        public List<CartItemViewModel> CartItems { get; set; } = new List<CartItemViewModel>();
        public decimal TotalAmount { get; set; }
        public string StreetAddress { get; set; }
        public string City { get; set; }
        public string PhoneNumber { get; set; }
    }

}
