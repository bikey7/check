using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using restaurantfinalupdated.Data;
using restaurantfinalupdated.Models;

namespace restaurantfinalupdated.Controllers
{
    public class RestaurantController : Controller
    {
        private readonly ApplicationDbContext _context;

        public RestaurantController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Restaurant (Public listing for customers)
        public async Task<IActionResult> Index(string cuisine, string searchString)
        {
            IQueryable<Restaurant> restaurants = _context.Restaurants
                .Include(r => r.Owner)
                .Include(r => r.Reviews)
                .Where(r => r.IsApproved);

            // Filter by cuisine
            if (!string.IsNullOrEmpty(cuisine))
            {
                restaurants = restaurants.Where(r => r.CuisineType == cuisine);
            }

            // Search by name or description
            if (!string.IsNullOrEmpty(searchString))
            {
                restaurants = restaurants.Where(r =>
                    r.Name.Contains(searchString) ||
                    (r.Description != null && r.Description.Contains(searchString)) ||
                    (r.CuisineType != null && r.CuisineType.Contains(searchString)));
            }

            var restaurantList = await restaurants.ToListAsync();

            // Pass cuisine list for filter dropdown
            ViewBag.Cuisines = await _context.Restaurants
                .Where(r => r.IsApproved && r.CuisineType != null)
                .Select(r => r.CuisineType)
                .Distinct()
                .ToListAsync();

            ViewBag.CurrentCuisine = cuisine;
            ViewBag.CurrentSearch = searchString;

            return View(restaurantList);
        }

        // GET: Restaurant/MyRestaurants (For business owners)
        public async Task<IActionResult> MyRestaurants()
        {
            var userId = HttpContext.Session.GetString("UserId");
            if (string.IsNullOrEmpty(userId))
            {
                return RedirectToAction("Login", "Account");
            }

            var restaurants = await _context.Restaurants
                .Include(r => r.Reviews)
                .Where(r => r.OwnerId == int.Parse(userId))
                .ToListAsync();

            return View(restaurants);
        }

        // GET: Restaurant/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var restaurant = await _context.Restaurants
                .Include(r => r.Owner)
                .Include(r => r.Reviews)
                    .ThenInclude(r => r.User)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (restaurant == null)
            {
                return NotFound();
            }

            return View(restaurant);
        }

        // GET: Restaurant/Create (For business owners)
        public IActionResult Create()
        {
            var userId = HttpContext.Session.GetString("UserId");
            if (string.IsNullOrEmpty(userId))
            {
                return RedirectToAction("Login", "Account");
            }

            return View();
        }

        // POST: Restaurant/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Name,Description,Location,City,PhoneNumber,CuisineType,OpeningHours,Website,ImageUrl,PriceRange,OwnerEmail")] Restaurant restaurant)
        {
            var userId = HttpContext.Session.GetString("UserId");
            if (string.IsNullOrEmpty(userId))
            {
                return RedirectToAction("Login", "Account");
            }

            // Remove validation errors for navigation properties
            ModelState.Remove("Owner");
            ModelState.Remove("Reviews");

            if (ModelState.IsValid)
            {
                restaurant.OwnerId = int.Parse(userId);
                restaurant.CreatedDate = DateTime.Now;
                restaurant.IsApproved = false; // Requires admin approval
                restaurant.Rating = 0; // Initial rating

                _context.Add(restaurant);
                await _context.SaveChangesAsync();

                TempData["Message"] = "Restaurant created successfully and pending approval";
                return RedirectToAction(nameof(MyRestaurants));
            }
            return View(restaurant);
        }

        // GET: Restaurant/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var userId = HttpContext.Session.GetString("UserId");
            if (string.IsNullOrEmpty(userId))
            {
                return RedirectToAction("Login", "Account");
            }

            var restaurant = await _context.Restaurants.FindAsync(id);
            if (restaurant == null)
            {
                return NotFound();
            }

            // Check if user owns this restaurant
            if (restaurant.OwnerId != int.Parse(userId))
            {
                return Forbid();
            }

            return View(restaurant);
        }

        // POST: Restaurant/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Description,Location,City,PhoneNumber,CuisineType,OpeningHours,Website,ImageUrl,PriceRange,OwnerEmail")] Restaurant restaurant)
        {
            if (id != restaurant.Id)
            {
                return NotFound();
            }

            var userId = HttpContext.Session.GetString("UserId");
            if (string.IsNullOrEmpty(userId))
            {
                return RedirectToAction("Login", "Account");
            }

            var existingRestaurant = await _context.Restaurants.FindAsync(id);
            if (existingRestaurant == null)
            {
                return NotFound();
            }

            // Check if user owns this restaurant
            if (existingRestaurant.OwnerId != int.Parse(userId))
            {
                return Forbid();
            }

            // Remove validation errors for navigation properties
            ModelState.Remove("Owner");
            ModelState.Remove("Reviews");

            if (ModelState.IsValid)
            {
                try
                {
                    // Update only the fields that can be edited
                    existingRestaurant.Name = restaurant.Name;
                    existingRestaurant.Description = restaurant.Description;
                    existingRestaurant.Location = restaurant.Location;
                    existingRestaurant.City = restaurant.City;
                    existingRestaurant.PhoneNumber = restaurant.PhoneNumber;
                    existingRestaurant.CuisineType = restaurant.CuisineType;
                    existingRestaurant.OpeningHours = restaurant.OpeningHours;
                    existingRestaurant.Website = restaurant.Website;
                    existingRestaurant.ImageUrl = restaurant.ImageUrl;
                    existingRestaurant.PriceRange = restaurant.PriceRange;
                    existingRestaurant.OwnerEmail = restaurant.OwnerEmail;

                    _context.Update(existingRestaurant);
                    await _context.SaveChangesAsync();

                    TempData["Message"] = "Restaurant updated successfully";
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!RestaurantExists(restaurant.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(MyRestaurants));
            }
            return View(restaurant);
        }

        // GET: Restaurant/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var userId = HttpContext.Session.GetString("UserId");
            if (string.IsNullOrEmpty(userId))
            {
                return RedirectToAction("Login", "Account");
            }

            var restaurant = await _context.Restaurants
                .Include(r => r.Owner)
                .Include(r => r.Reviews)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (restaurant == null)
            {
                return NotFound();
            }

            // Check if user owns this restaurant
            if (restaurant.OwnerId != int.Parse(userId))
            {
                return Forbid();
            }

            return View(restaurant);
        }

        // POST: Restaurant/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var userId = HttpContext.Session.GetString("UserId");
            if (string.IsNullOrEmpty(userId))
            {
                return RedirectToAction("Login", "Account");
            }

            var restaurant = await _context.Restaurants.FindAsync(id);
            if (restaurant != null)
            {
                // Check if user owns this restaurant
                if (restaurant.OwnerId != int.Parse(userId))
                {
                    return Forbid();
                }

                _context.Restaurants.Remove(restaurant);
                await _context.SaveChangesAsync();

                TempData["Message"] = "Restaurant deleted successfully";
            }

            return RedirectToAction(nameof(MyRestaurants));
        }

        private bool RestaurantExists(int id)
        {
            return _context.Restaurants.Any(e => e.Id == id);
        }
    }
}