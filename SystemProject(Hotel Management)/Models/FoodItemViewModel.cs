using System.ComponentModel.DataAnnotations;

namespace SystemProject_Hotel_Management_.Models
{
    public class FoodItemViewModel
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
        public string Description { get; set; }
        public int CategoryID { get; set; }
        public string PictureURL { get; set; }
    }

}
