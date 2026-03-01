using System.ComponentModel.DataAnnotations;

namespace restaurantfinalupdated.Models
{
    public class Review
    {
        public int Id { get; set; }

        [Required]
        public int RestaurantId { get; set; }

        public Restaurant Restaurant { get; set; }

        [Required]
        public string UserEmail { get; set; }

        [Required, Range(1, 5)]
        public int Rating { get; set; }

        public string Comment { get; set; }
    }
}