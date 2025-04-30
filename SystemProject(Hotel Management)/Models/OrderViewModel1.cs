namespace SystemProject_Hotel_Management_.Models
{
    public class OrderViewModel1
    {
        public int OrderID { get; set; }
        public string CustomerName { get; set; }  // Name of the user
        public DateTime OrderDate { get; set; }   // Date when the order was created
        public decimal TotalAmount { get; set; }  // Total amount of the order
        public string Status { get; set; }        // Status of the order (e.g., Pending, Completed)
        public string StreetAddress { get; set; } // Street address of the customer
        public string City { get; set; }          // City of the customer
        public string PhoneNumber { get; set; }
    }
}

