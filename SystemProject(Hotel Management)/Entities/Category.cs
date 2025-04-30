using System.ComponentModel.DataAnnotations;

namespace SystemProject_Hotel_Management_.Entities
{
    public class Category
    {
        [Key]
        public int ID { get; set; }
        public string Name { get; set; }

        // Navigation property to FoodItems
        public ICollection<FoodItem> FoodItems { get; set; }
    }
}
