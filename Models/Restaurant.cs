using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace restaurantfinalupdated.Models
{
    public class Restaurant
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Restaurant name is required")]
        [StringLength(100, MinimumLength = 2, ErrorMessage = "Name must be between 2 and 100 characters")]
        [Display(Name = "Restaurant Name")]
        public string Name { get; set; } = string.Empty;

        [StringLength(500, ErrorMessage = "Description cannot exceed 500 characters")]
        [Display(Name = "Description")]
        public string? Description { get; set; }

        [Required(ErrorMessage = "Location is required")]
        [StringLength(200, ErrorMessage = "Location cannot exceed 200 characters")]
        [Display(Name = "Location")]
        public string Location { get; set; } = string.Empty;  // Changed from Address

        [Display(Name = "City")]
        [StringLength(50)]
        public string? City { get; set; }

        [Display(Name = "Phone Number")]
        [Phone(ErrorMessage = "Invalid phone number")]
        public string? PhoneNumber { get; set; }

        [Display(Name = "Cuisine Type")]
        public string? CuisineType { get; set; }  // Changed from Cuisine

        [Range(0, 5, ErrorMessage = "Rating must be between 0 and 5")]
        [Display(Name = "Rating")]
        public double Rating { get; set; }

        [Display(Name = "Opening Hours")]
        public string? OpeningHours { get; set; }

        [Display(Name = "Website")]
        [Url(ErrorMessage = "Invalid URL")]
        public string? Website { get; set; }

        [Display(Name = "Image URL")]
        public string? ImageUrl { get; set; }

        [Display(Name = "Price Range")]
        public string? PriceRange { get; set; }

        [Display(Name = "Owner Email")]
        [EmailAddress]
        public string? OwnerEmail { get; set; }

        [Display(Name = "Owner ID")]
        public int? OwnerId { get; set; }

        [Display(Name = "Is Approved")]
        public bool IsApproved { get; set; } = true;

        [Display(Name = "Created Date")]
        public DateTime CreatedDate { get; set; } = DateTime.Now;

        // Navigation properties
        public User? Owner { get; set; }
        public ICollection<Review>? Reviews { get; set; }
    }
}