using HealthInsurance.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace HealthInsurance.Controllers
{
    public class AdminController : Controller
    {
        private readonly ModelContext _context;

        public AdminController(ModelContext context)
        {
            this._context = context;
        }
        public IActionResult Index()
        {
            ViewBag.id = HttpContext.Session.GetInt32("Id");
            ViewBag.name = HttpContext.Session.GetString("Name");
            ViewBag.email = HttpContext.Session.GetString("Email");
            ViewBag.phoneNumber = HttpContext.Session.GetString("PhoneNumber");
            ViewBag.profilePic = HttpContext.Session.GetString("ProfilePic");
            ViewBag.UsersCount = _context.Users.Count();
            ViewBag.SubsCount = _context.Subscriptions.Count();
            ViewBag.CurrentDate = DateTime.Now;

            return View();
        }
    }
}
