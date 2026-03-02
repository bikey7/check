using System;
using System.ComponentModel.DataAnnotations;

namespace restaurantfinalupdated.Models
{
    public class Review
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int RestaurantId { get; set; }

        [Required]
        public int UserId { get; set; }

        [Required(ErrorMessage = "Please select a rating")]
        [Range(1, 5, ErrorMessage = "Rating must be between 1 and 5")]
        [Display(Name = "Rating")]
        public int Rating { get; set; }

        [Required(ErrorMessage = "Please write your review")]
        [StringLength(1000, MinimumLength = 10, ErrorMessage = "Review must be between 10 and 1000 characters")]
        [Display(Name = "Your Review")]
        public string Comment { get; set; } = string.Empty;

        [Display(Name = "Review Date")]
        public DateTime Date { get; set; } = DateTime.Now;

        // Navigation properties
        public Restaurant? Restaurant { get; set; }
        public User? User { get; set; }
    }
}