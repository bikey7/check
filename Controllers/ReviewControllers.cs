using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using restaurantfinalupdated.Data;
using restaurantfinalupdated.Models;

namespace restaurantfinalupdated.Controllers
{
    public class ReviewController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ReviewController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Review
        public async Task<IActionResult> Index()
        {
            var reviews = await _context.Reviews
                .Include(r => r.Restaurant)
                .Include(r => r.User)
                .OrderByDescending(r => r.Date)
                .ToListAsync();
            return View(reviews);
        }

        // GET: Review/Create/5
        public async Task<IActionResult> Create(int? restaurantId)
        {
            var userId = HttpContext.Session.GetString("UserId");
            if (string.IsNullOrEmpty(userId))
            {
                return RedirectToAction("Login", "Account");
            }

            if (restaurantId == null)
            {
                return NotFound();
            }

            var restaurant = await _context.Restaurants
                .FirstOrDefaultAsync(r => r.Id == restaurantId);

            if (restaurant == null)
            {
                return NotFound();
            }

            ViewBag.RestaurantName = restaurant.Name;
            ViewBag.RestaurantId = restaurantId;

            return View();
        }

        // POST: Review/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(int restaurantId, int rating, string comment)
        {
            var userId = HttpContext.Session.GetString("UserId");
            if (string.IsNullOrEmpty(userId))
            {
                return RedirectToAction("Login", "Account");
            }

            if (rating < 1 || rating > 5)
            {
                ModelState.AddModelError("Rating", "Rating must be between 1 and 5");
            }

            if (string.IsNullOrWhiteSpace(comment) || comment.Length < 10)
            {
                ModelState.AddModelError("Comment", "Comment must be at least 10 characters");
            }

            if (ModelState.IsValid)
            {
                var review = new Review
                {
                    RestaurantId = restaurantId,
                    UserId = int.Parse(userId),
                    Rating = rating,
                    Comment = comment,
                    Date = DateTime.Now
                };

                _context.Add(review);
                await _context.SaveChangesAsync();

                TempData["Message"] = "Review submitted successfully!";
                return RedirectToAction("Details", "Restaurant", new { id = restaurantId });
            }

            var restaurant = await _context.Restaurants.FindAsync(restaurantId);
            ViewBag.RestaurantName = restaurant?.Name;
            ViewBag.RestaurantId = restaurantId;

            return View();
        }

        // POST: Review/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            var userId = HttpContext.Session.GetString("UserId");
            if (string.IsNullOrEmpty(userId))
            {
                return RedirectToAction("Login", "Account");
            }

            var review = await _context.Reviews
                .Include(r => r.Restaurant)
                .FirstOrDefaultAsync(r => r.Id == id);

            if (review == null)
            {
                return NotFound();
            }

            if (review.UserId != int.Parse(userId))
            {
                return Forbid();
            }

            var restaurantId = review.RestaurantId;
            _context.Reviews.Remove(review);
            await _context.SaveChangesAsync();

            TempData["Message"] = "Review deleted successfully";
            return RedirectToAction("Details", "Restaurant", new { id = restaurantId });
        }
    }
}