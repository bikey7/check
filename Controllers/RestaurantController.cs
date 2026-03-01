using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using restaurantfinalupdated.Data;
using restaurantfinalupdated.Models;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace restaurantfinalupdated.Controllers
{
    [Authorize]
    public class RestaurantController : Controller
    {
        private readonly ApplicationDbContext _context;

        public RestaurantController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> List(string search)
        {
            var restaurants = string.IsNullOrEmpty(search)
                ? await _context.Restaurants.ToListAsync()
                : await _context.Restaurants.Where(r => r.City.Contains(search)).ToListAsync();

            return View(restaurants);
        }

        public IActionResult Create() => View();

        [HttpPost]
        public async Task<IActionResult> Create(string name, string city)
        {
            var currentUserEmail = User.FindFirstValue(ClaimTypes.Email);

            if (await _context.Restaurants.AnyAsync(r => r.OwnerEmail == currentUserEmail))
            {
                TempData["Error"] = "You already own a restaurant!";
                return RedirectToAction("List");
            }

            var restaurant = new Restaurant
            {
                Name = name,
                City = city,
                OwnerEmail = currentUserEmail
            };

            _context.Restaurants.Add(restaurant);
            await _context.SaveChangesAsync();

            return RedirectToAction("List");
        }
    }
}