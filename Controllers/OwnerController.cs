using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace restaurantfinalupdated.Controllers
{
    [Authorize(Roles = "Owner")]
    public class OwnerController : Controller
    {
        public IActionResult Index() => View(); // Owner dashboard
    }
}