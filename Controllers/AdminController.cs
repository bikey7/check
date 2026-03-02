using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using restaurantfinalupdated.Data;
using restaurantfinalupdated.Models;

namespace restaurantfinalupdated.Controllers
{
    public class AdminController : Controller
    {
        private readonly ApplicationDbContext _context;

        public AdminController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Admin
        public async Task<IActionResult> Index()
        {
            var userRole = HttpContext.Session.GetString("UserRole");
            if (userRole != "Admin")
            {
                return RedirectToAction("Login", "Account");
            }

            var pendingRestaurants = await _context.Restaurants
                .Include(r => r.Owner)
                .Where(r => !r.IsApproved)
                .ToListAsync();

            return View(pendingRestaurants);
        }

        // POST: Admin/ApproveRestaurant/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ApproveRestaurant(int id)
        {
            var userRole = HttpContext.Session.GetString("UserRole");
            if (userRole != "Admin")
            {
                return RedirectToAction("Login", "Account");
            }

            var restaurant = await _context.Restaurants.FindAsync(id);
            if (restaurant != null)
            {
                restaurant.IsApproved = true;
                _context.Update(restaurant);
                await _context.SaveChangesAsync();
                TempData["Message"] = "Restaurant approved successfully";
            }

            return RedirectToAction(nameof(Index));
        }

        // POST: Admin/RejectRestaurant/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RejectRestaurant(int id)
        {
            var userRole = HttpContext.Session.GetString("UserRole");
            if (userRole != "Admin")
            {
                return RedirectToAction("Login", "Account");
            }

            var restaurant = await _context.Restaurants.FindAsync(id);
            if (restaurant != null)
            {
                _context.Restaurants.Remove(restaurant);
                await _context.SaveChangesAsync();
                TempData["Message"] = "Restaurant rejected and removed";
            }

            return RedirectToAction(nameof(Index));
        }

        // GET: Admin/Users
        public async Task<IActionResult> Users()
        {
            var userRole = HttpContext.Session.GetString("UserRole");
            if (userRole != "Admin")
            {
                return RedirectToAction("Login", "Account");
            }

            var users = await _context.Users.ToListAsync();
            return View(users);
        }
    }
}