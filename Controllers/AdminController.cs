using HealthInsurance.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace HealthInsurance.Controllers
{
    public class AdminController : Controller
    {
        private readonly ModelContext _context;
        private readonly IWebHostEnvironment webHostEnvironment;
        public AdminController(ModelContext context, IWebHostEnvironment webHostEnvironment)
        {
            this._context = context;
            this.webHostEnvironment = webHostEnvironment;
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
        public IActionResult RegUsers()
        {
            var usersInfo = _context.Users.Include(s=>s.Subscription.Beneficiaries).ToList();

            return View(usersInfo);
        }
        //GET
        public IActionResult SubSearch()
        {
            var model = _context.Users.Include(p => p.Subscription).Where(x => x.Subscription != null).ToList();
            return View(model);
        }

        [HttpPost]
        public IActionResult SubSearch(DateTime? startDate, DateTime? endDate)
        {
            var model = _context.Users.Include(p => p.Subscription).Where(x => x.Subscription.Status != null).ToList();
            if (startDate == null && endDate == null)
            {
                return View(model);
            }
            else if (startDate != null && endDate == null)
            {
                var result = model.Where(x => x.Subscription.StartDate.Date >= startDate);
                return View(result);
            }

            else if (startDate == null && endDate != null)
            {
                var result = model.Where(x => x.Subscription.StartDate.Date <= endDate);
                return View(result);
            }
            else
            {
                var result = model.Where
                    (x => x.Subscription.StartDate.Date >= startDate && x.Subscription.StartDate.Date <= endDate);

                return View(result);
            }
        }
    }
}
