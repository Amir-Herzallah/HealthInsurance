using Microsoft.AspNetCore.Mvc;

namespace HealthInsurance.Controllers
{
    public class AdminController : Controller
    {
        public IActionResult Index()
        {
            ViewBag.name = HttpContext.Session.GetString("Name");
            ViewBag.email = HttpContext.Session.GetString("Email");
            ViewBag.phoneNumber = HttpContext.Session.GetString("PhoneNumber");
            ViewBag.profilePic = HttpContext.Session.GetString("ProfilePic");
            return View();
        }
    }
}
