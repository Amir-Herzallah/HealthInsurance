using HealthInsurance.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

namespace HealthInsurance.Controllers
{
    public class HomeController : Controller
    {
        private readonly ModelContext _context;
        private readonly IWebHostEnvironment webHostEnvironment;
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger, ModelContext context, IWebHostEnvironment webHostEnvironment)
        {
            _logger = logger;
            _context = context;
            this.webHostEnvironment = webHostEnvironment;
        }

        public IActionResult Index()
        {
            ViewBag.id = HttpContext.Session.GetInt32("Id");
            ViewBag.name = HttpContext.Session.GetString("Name");
            ViewBag.email = HttpContext.Session.GetString("Email");
            ViewBag.phoneNumber = HttpContext.Session.GetString("PhoneNumber");
            ViewBag.profilePic = HttpContext.Session.GetString("ProfilePic");
            ViewBag.CurrentDate = DateTime.Now;
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        // GET:
        public async Task<IActionResult> EditProfile(decimal? id)
        {
            if (id == null || _context.Users == null)
            {
                return NotFound();
            }

            var users = await _context.Users.FindAsync(id);
            if (users == null)
            {
                return NotFound();
            }
            ViewData["Roleid"] = new SelectList(_context.Roles, "Id", "Id", users.Roleid);
            return View(users);
        }

        // POST
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditProfile(decimal id, [Bind("Id,Username,Password,Email,PhoneNumber,RegistrationDate,Roleid,ProfilePictureUrl,ProfilePictureFile")] Users users)
        {
            if (id != users.Id)
            {
                return NotFound();
            }
            try
            {
                var existingUser = await _context.Users.FindAsync(users.Id);

                if (existingUser != null)
                {
                    // Update only the fields that the user has modified and are not empty
                    if (!string.IsNullOrEmpty(users.Username))
                    {
                        existingUser.Username = users.Username;
                    }
                    if (!string.IsNullOrEmpty(users.Password))
                    {
                        existingUser.Password = users.Password;
                    }
                    if (!string.IsNullOrEmpty(users.Email))
                    {
                        existingUser.Email = users.Email;
                    }
                    if (!string.IsNullOrEmpty(users.PhoneNumber))
                    {
                        existingUser.PhoneNumber = users.PhoneNumber;
                    }
                    if (!string.IsNullOrEmpty(users.ProfilePictureUrl))
                    {
                        existingUser.ProfilePictureUrl = users.ProfilePictureUrl;
                    }

                    // Handle file upload (ProfilePictureFile) if needed
                    if (users.ProfilePictureFile != null)
                    {
                        string wwwRootPath = webHostEnvironment.WebRootPath;
                        string fileName = Guid.NewGuid().ToString() + users.ProfilePictureFile.FileName;
                        string path = Path.Combine(wwwRootPath, "images", fileName);

                        using (var fileStream = new FileStream(path, FileMode.Create))
                        {
                            await users.ProfilePictureFile.CopyToAsync(fileStream);
                        }

                        existingUser.ProfilePictureUrl = fileName;
                    }

                    _context.Update(existingUser);
                    await _context.SaveChangesAsync();

                    if (existingUser.Roleid == 1)
                    {
                        return RedirectToAction("Index", "Admin");
                    }
                    else if (existingUser.Roleid == 2)
                    {
                        return RedirectToAction("Index", "Home");
                    }
                }
                else
                {
                    return NotFound();
                }
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UsersExists(users.Id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            ViewData["Roleid"] = new SelectList(_context.Roles, "Id", "Id", users.Roleid);
            return View(users);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
        private bool UsersExists(decimal id)
        {
            return (_context.Users?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}