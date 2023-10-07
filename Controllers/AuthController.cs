using HealthInsurance.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace HealthInsurance.Controllers
{
    public class AuthController : Controller
    {
        private readonly ModelContext _context;
        private readonly IWebHostEnvironment webHostEnvironment;

        public AuthController(ModelContext context, IWebHostEnvironment webHostEnvironment)
        {
            _context = context;
            this.webHostEnvironment = webHostEnvironment;
        }
        public IActionResult Register()
        {
            return View();

        }
        public IActionResult Login()
        {
            ViewBag.Name = HttpContext.Session.GetString("Username");
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Username,Password,Email,PhoneNumber,RegistrationDate,ProfilePictureUrl,ProfilePictureFile")] Users users)
        {
            if (ModelState.IsValid)
            {
                if (users.ProfilePictureFile != null)
                {
                    string wwwRootPath = webHostEnvironment.WebRootPath;

                    string fileName = Guid.NewGuid().ToString() + users.ProfilePictureFile.FileName;

                    string path = Path.Combine(wwwRootPath + "/images/" + fileName);

                    using (var fileStream = new FileStream(path, FileMode.Create))
                    {
                        await users.ProfilePictureFile.CopyToAsync(fileStream);
                    }

                    users.ProfilePictureUrl = fileName;
                }
                var user = _context.Users.Where(x => x.Email == users.Email).FirstOrDefault();

                if (user == null)
                {
                    users.Roleid = 2;
                    _context.Add(users);
                    await _context.SaveChangesAsync();
                    //Action //Controller
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    ViewBag.Error = "Email Already Used, try another email.";
                }
                
            }
            
            return View("~/Home/Index");
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login([Bind("Id,Email,Password")] Users userLogin)
        {
            var auth = _context.Users.Where(x => x.Email == userLogin.Email && x.Password == userLogin.Password).FirstOrDefault();
            if (auth != null)
            {
                switch (auth.Roleid)
                {
                    case 1:
                        HttpContext.Session.SetString("Name", auth.Username);
                        HttpContext.Session.SetString("Email", auth.Email);
                        HttpContext.Session.SetString("PhoneNumber", auth.PhoneNumber);
                        HttpContext.Session.SetString("ProfilePic", auth.ProfilePictureUrl);

                        return RedirectToAction("Index", "Admin");
                    case 2:
                        HttpContext.Session.SetString("Name", auth.Username);
                        HttpContext.Session.SetString("Email", auth.Email);
                        HttpContext.Session.SetString("PhoneNumber", auth.PhoneNumber);
                        HttpContext.Session.SetString("ProfilePic", auth.ProfilePictureUrl);

                        return RedirectToAction("Index", "Home");
                }

            }
            else
            {
                ViewBag.Error = "Invaild Credentials!";
            }

            return View();
        }

    }
}
