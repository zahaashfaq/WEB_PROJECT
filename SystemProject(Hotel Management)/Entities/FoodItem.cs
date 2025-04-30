using System.ComponentModel.DataAnnotations;

namespace SystemProject_Hotel_Management_.Entities
{
    public class FoodItem
    {
        [Key]
        public int ID { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
        
        public string Description { get; set; }
        public int CategoryID { get; set; }
        public Category Category { get; set; }
        public string PictureURL { get; set; }
    }

}
