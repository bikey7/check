using Microsoft.EntityFrameworkCore;
using restaurantfinalupdated.Models;

namespace restaurantfinalupdated.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Restaurant> Restaurants { get; set; }
        public DbSet<Review> Reviews { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configure relationships
            modelBuilder.Entity<Restaurant>()
                .HasOne(r => r.Owner)
                .WithMany(u => u.OwnedRestaurants)
                .HasForeignKey(r => r.OwnerId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Review>()
                .HasOne(r => r.Restaurant)
                .WithMany(r => r.Reviews)
                .HasForeignKey(r => r.RestaurantId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Review>()
                .HasOne(r => r.User)
                .WithMany(u => u.Reviews)
                .HasForeignKey(r => r.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            // Seed users
            modelBuilder.Entity<User>().HasData(
                new User
                {
                    Id = 1,
                    Name = "Admin User",
                    Email = "admin@restaurant.com",
                    Password = "password123",
                    Role = "Admin",
                    IsActive = true,
                    RegistrationDate = DateTime.Now
                },
                new User
                {
                    Id = 2,
                    Name = "John Owner",
                    Email = "owner@example.com",
                    Password = "password123",
                    Role = "Owner",
                    IsActive = true,
                    RegistrationDate = DateTime.Now
                },
                new User
                {
                    Id = 3,
                    Name = "Jane Customer",
                    Email = "customer@example.com",
                    Password = "password123",
                    Role = "Customer",
                    IsActive = true,
                    RegistrationDate = DateTime.Now
                }
            );

            // Seed restaurants - UPDATED property names
            modelBuilder.Entity<Restaurant>().HasData(
                new Restaurant
                {
                    Id = 1,
                    Name = "Italian Bistro",
                    Description = "Authentic Italian cuisine with homemade pasta",
                    Location = "123 Main St, New York, NY",  // Changed from Address
                    City = "New York",
                    CuisineType = "Italian",  // Changed from Cuisine
                    PhoneNumber = "555-0101",
                    OpeningHours = "11AM-10PM",
                    Website = "https://italianbistro.com",
                    ImageUrl = "/images/italian.jpg",
                    PriceRange = "$$",
                    Rating = 4.5,
                    OwnerEmail = "owner@example.com",
                    OwnerId = 2,
                    IsApproved = true,
                    CreatedDate = DateTime.Now
                },
                new Restaurant
                {
                    Id = 2,
                    Name = "Sushi Master",
                    Description = "Fresh sushi and Japanese cuisine",
                    Location = "456 Oak Ave, Los Angeles, CA",  // Changed from Address
                    City = "Los Angeles",
                    CuisineType = "Japanese",  // Changed from Cuisine
                    PhoneNumber = "555-0102",
                    OpeningHours = "12PM-10PM",
                    Website = "https://sushimaster.com",
                    ImageUrl = "/images/sushi.jpg",
                    PriceRange = "$$$",
                    Rating = 4.8,
                    OwnerEmail = "owner@example.com",
                    OwnerId = 2,
                    IsApproved = true,
                    CreatedDate = DateTime.Now
                },
                new Restaurant
                {
                    Id = 3,
                    Name = "Taco Fiesta",
                    Description = "Authentic Mexican street tacos",
                    Location = "789 Pine St, San Antonio, TX",  // Changed from Address
                    City = "San Antonio",
                    CuisineType = "Mexican",  // Changed from Cuisine
                    PhoneNumber = "555-0103",
                    OpeningHours = "10AM-11PM",
                    Website = "https://tacofiesta.com",
                    ImageUrl = "/images/mexican.jpg",
                    PriceRange = "$",
                    Rating = 4.2,
                    OwnerEmail = "owner@example.com",
                    OwnerId = 2,
                    IsApproved = false,
                    CreatedDate = DateTime.Now
                }
            );

            // Seed reviews
            modelBuilder.Entity<Review>().HasData(
                new Review
                {
                    Id = 1,
                    RestaurantId = 1,
                    UserId = 3,
                    Rating = 5,
                    Comment = "Amazing pasta! Best Italian food in town.",
                    Date = DateTime.Now.AddDays(-5)
                },
                new Review
                {
                    Id = 2,
                    RestaurantId = 1,
                    UserId = 3,
                    Rating = 4,
                    Comment = "Great atmosphere and delicious food.",
                    Date = DateTime.Now.AddDays(-3)
                },
                new Review
                {
                    Id = 3,
                    RestaurantId = 2,
                    UserId = 3,
                    Rating = 5,
                    Comment = "Best sushi in town!",
                    Date = DateTime.Now.AddDays(-2)
                }
            );
        }
    }
}