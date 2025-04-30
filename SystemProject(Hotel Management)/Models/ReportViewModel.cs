namespace SystemProject_Hotel_Management_.Models
{
    public class ReportViewModel
    {
        public int ReportId { get; set; }
        public string Title { get; set; }
        public DateTime DateGenerated { get; set; }
        public string Description { get; set; }
        public List<OrderViewModel1> Orders { get; set; }  // You can include related data like Orders
        public List<FoodItemViewModel> FoodItems { get; set; }  // Or Food Items
    }
}
