using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace restaurantfinalupdated.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        public IActionResult Index() => View(); // Admin dashboard
    }
}