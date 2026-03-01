using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using restaurantfinalupdated.Data;
using restaurantfinalupdated.Models;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace restaurantfinalupdated.Controllers
{
    [Authorize]
    public class ReviewController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ReviewController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Submit(int restaurantId)
        {
            ViewBag.RestaurantId = restaurantId;
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Submit(int restaurantId, int rating, string comment)
        {
            var userEmail = User.FindFirstValue(ClaimTypes.Email);

            var review = new Review
            {
                RestaurantId = restaurantId,
                Rating = rating,
                Comment = comment,
                UserEmail = userEmail
            };

            _context.Reviews.Add(review);
            await _context.SaveChangesAsync();

            return RedirectToAction("List", "Restaurant");
        }
    }
}