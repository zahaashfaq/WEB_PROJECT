namespace SystemProject_Hotel_Management_.Models
{
    public class OrderViewModel
    {
        public int UserID { get; set; }
        public List<int> FoodItemIDs { get; set; }
        public List<int> Quantities { get; set; }
    }
}
