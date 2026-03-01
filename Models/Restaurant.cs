using System.ComponentModel.DataAnnotations;

namespace restaurantfinalupdated.Models
{
    public class Restaurant
    {
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public string City { get; set; }

        public string OwnerEmail { get; set; }

        public List<Review> Reviews { get; set; } = new List<Review>();
    }
}