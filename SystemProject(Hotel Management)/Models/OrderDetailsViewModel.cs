namespace SystemProject_Hotel_Management_.Models
{
    public class OrderDetailsViewModel
    {
        public int OrderID { get; set; }
        public string UserName { get; set; }
        public decimal TotalAmount { get; set; }
        public DateTime CreatedAt { get; set; }
        public string StreetAddress { get; set; }
        public string City { get; set; }
        public string PhoneNumber { get; set; }
        public string Status { get; set; }
        public List<OrderItemViewModel> OrderItems { get; set; } = new List<OrderItemViewModel>();
    }
}
