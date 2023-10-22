using HealthInsurance.Models;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using SQLitePCL;

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
        public async Task<IActionResult> Register([Bind("Id,Username,Password,Email,PhoneNumber,RegistrationDate,ProfilePictureUrl,ProfilePictureFile")] Users users)
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
                    return RedirectToAction("Login", "Auth");
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
                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, auth.Username),
                    new Claim(ClaimTypes.Email, auth.Email),

                };

                var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                var principal = new ClaimsPrincipal(identity);

                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);

                switch (auth.Roleid)
                {
                    case 1:
                        HttpContext.Session.SetInt32("Id", (Int32)auth.Id);
                        HttpContext.Session.SetString("Name", auth.Username);
                        HttpContext.Session.SetString("Email", auth.Email);
                        HttpContext.Session.SetString("PhoneNumber", auth.PhoneNumber);
                        HttpContext.Session.SetString("ProfilePic", auth.ProfilePictureUrl);
                        
                        HttpContext.Session.SetInt32("userLoginId", (Int32)auth.Id);
                        HttpContext.Session.SetString("userLoginEmail", auth.Email);
                        HttpContext.Session.SetString("userLoginName", auth.Username);
                        return RedirectToAction("Index", "Admin");
                    case 2:
                        HttpContext.Session.SetInt32("Id", (Int32)auth.Id);
                        HttpContext.Session.SetString("Name", auth.Username);
                        HttpContext.Session.SetString("Email", auth.Email);
                        HttpContext.Session.SetString("PhoneNumber", auth.PhoneNumber);
                        HttpContext.Session.SetString("ProfilePic", auth.ProfilePictureUrl);
                       
                        HttpContext.Session.SetInt32("userLoginId", (Int32)auth.Id);
                        HttpContext.Session.SetString("userLoginEmail", auth.Email);
                        HttpContext.Session.SetString("userLoginName", auth.Username);
                        return RedirectToAction("Index", "Home");
                }
            }
            else
            {
                ViewBag.Error = "Invalid Credentials!";
            }

            return View();
        }

        public async Task<IActionResult> Logout()
        {
            HttpContext.Session.Clear();

            await Task.Run(async () => await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme));

            return RedirectToAction("Index", "Home");
        }
    }
}
