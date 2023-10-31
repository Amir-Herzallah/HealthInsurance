using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using HealthInsurance.Models;

namespace HealthInsurance.Controllers
{
    public class HomePageController : Controller
    {
        private readonly ModelContext _context;

        public HomePageController(ModelContext context)
        {
            _context = context;
        }

        // GET: HomePage
        public async Task<IActionResult> Index()
        {
            ViewBag.id = HttpContext.Session.GetInt32("Id");
            ViewBag.name = HttpContext.Session.GetString("Name");
            ViewBag.email = HttpContext.Session.GetString("Email");
            ViewBag.phoneNumber = HttpContext.Session.GetString("PhoneNumber");
            ViewBag.profilePic = HttpContext.Session.GetString("ProfilePic");
            ViewBag.CurrentDate = DateTime.Now;
            ViewBag.userLoginId = HttpContext.Session.GetInt32("userLoginId");
            ViewBag.userLoginName = HttpContext.Session.GetString("userLoginName");
            ViewBag.userLoginEmail = HttpContext.Session.GetString("userLoginEmail");

            return _context.HomePage != null ? 
                          View(await _context.HomePage.ToListAsync()) :
                          Problem("Entity set 'ModelContext.HomePage'  is null.");
        }

        // GET: HomePage/Details/5
        public async Task<IActionResult> Details(decimal? id)
        {
            ViewBag.id = HttpContext.Session.GetInt32("Id");
            ViewBag.name = HttpContext.Session.GetString("Name");
            ViewBag.email = HttpContext.Session.GetString("Email");
            ViewBag.phoneNumber = HttpContext.Session.GetString("PhoneNumber");
            ViewBag.profilePic = HttpContext.Session.GetString("ProfilePic");
            ViewBag.UsersCount = _context.Users.Count();
            ViewBag.SubsCount = _context.Subscriptions.Count();
            ViewBag.CurrentDate = DateTime.Now;
            ViewBag.userLoginId = HttpContext.Session.GetInt32("userLoginId");
            ViewBag.userLoginName = HttpContext.Session.GetString("userLoginName");
            ViewBag.userLoginEmail = HttpContext.Session.GetString("userLoginEmail");
            if (id == null || _context.HomePage == null)
            {
                return NotFound();
            }

            var homePage = await _context.HomePage
                .FirstOrDefaultAsync(m => m.Id == id);
            if (homePage == null)
            {
                return NotFound();
            }

            return View(homePage);
        }

        // GET: HomePage/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: HomePage/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,LogoPath,HeaderComponent1,HeaderComponent2,FooterComponent1,FooterComponent2,ImagePath1,ImagePath2,Text1,Text2,Text3")] HomePage homePage)
        {
            if (ModelState.IsValid)
            {
                _context.Add(homePage);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(homePage);
        }

        // GET: HomePage/Edit/5
        public async Task<IActionResult> Edit(decimal? id)
        {
            ViewBag.id = HttpContext.Session.GetInt32("Id");
            ViewBag.name = HttpContext.Session.GetString("Name");
            ViewBag.email = HttpContext.Session.GetString("Email");
            ViewBag.phoneNumber = HttpContext.Session.GetString("PhoneNumber");
            ViewBag.profilePic = HttpContext.Session.GetString("ProfilePic");
            ViewBag.CurrentDate = DateTime.Now;
            ViewBag.userLoginId = HttpContext.Session.GetInt32("userLoginId");
            ViewBag.userLoginName = HttpContext.Session.GetString("userLoginName");
            ViewBag.userLoginEmail = HttpContext.Session.GetString("userLoginEmail");
            if (id == null || _context.HomePage == null)
            {
                return NotFound();
            }

            var homePage = await _context.HomePage.FindAsync(id);
            if (homePage == null)
            {
                return NotFound();
            }
            return View(homePage);
        }

        // POST: HomePage/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(decimal id, [Bind("Id,LogoPath,HeaderComponent1,HeaderComponent2,FooterComponent1,FooterComponent2,ImagePath1,ImagePath2,Text1,Text2,Text3")] HomePage homePage)
        {
            ViewBag.id = HttpContext.Session.GetInt32("Id");
            ViewBag.name = HttpContext.Session.GetString("Name");
            ViewBag.email = HttpContext.Session.GetString("Email");
            ViewBag.phoneNumber = HttpContext.Session.GetString("PhoneNumber");
            ViewBag.profilePic = HttpContext.Session.GetString("ProfilePic");
            ViewBag.UsersCount = _context.Users.Count();
            ViewBag.SubsCount = _context.Subscriptions.Count();
            ViewBag.CurrentDate = DateTime.Now;
            ViewBag.userLoginId = HttpContext.Session.GetInt32("userLoginId");
            ViewBag.userLoginName = HttpContext.Session.GetString("userLoginName");
            ViewBag.userLoginEmail = HttpContext.Session.GetString("userLoginEmail");
            if (id != homePage.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(homePage);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!HomePageExists(homePage.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                TempData["EditPagesSuccess"] = "Changes have been saved!";

                return RedirectToAction("Index", "Admin");
            }
            return View(homePage);
        }

        // GET: HomePage/Delete/5
        public async Task<IActionResult> Delete(decimal? id)
        {
            ViewBag.id = HttpContext.Session.GetInt32("Id");
            ViewBag.name = HttpContext.Session.GetString("Name");
            ViewBag.email = HttpContext.Session.GetString("Email");
            ViewBag.phoneNumber = HttpContext.Session.GetString("PhoneNumber");
            ViewBag.profilePic = HttpContext.Session.GetString("ProfilePic");
            ViewBag.UsersCount = _context.Users.Count();
            ViewBag.SubsCount = _context.Subscriptions.Count();
            ViewBag.CurrentDate = DateTime.Now;
            ViewBag.userLoginId = HttpContext.Session.GetInt32("userLoginId");
            ViewBag.userLoginName = HttpContext.Session.GetString("userLoginName");
            ViewBag.userLoginEmail = HttpContext.Session.GetString("userLoginEmail");
            if (id == null || _context.HomePage == null)
            {
                return NotFound();
            }

            var homePage = await _context.HomePage
                .FirstOrDefaultAsync(m => m.Id == id);
            if (homePage == null)
            {
                return NotFound();
            }

            return View(homePage);
        }

        // POST: HomePage/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(decimal id)
        {
            ViewBag.id = HttpContext.Session.GetInt32("Id");
            ViewBag.name = HttpContext.Session.GetString("Name");
            ViewBag.email = HttpContext.Session.GetString("Email");
            ViewBag.phoneNumber = HttpContext.Session.GetString("PhoneNumber");
            ViewBag.profilePic = HttpContext.Session.GetString("ProfilePic");
            ViewBag.UsersCount = _context.Users.Count();
            ViewBag.SubsCount = _context.Subscriptions.Count();
            ViewBag.CurrentDate = DateTime.Now;
            ViewBag.userLoginId = HttpContext.Session.GetInt32("userLoginId");
            ViewBag.userLoginName = HttpContext.Session.GetString("userLoginName");
            ViewBag.userLoginEmail = HttpContext.Session.GetString("userLoginEmail");
            if (_context.HomePage == null)
            {
                return Problem("Entity set 'ModelContext.HomePage'  is null.");
            }
            var homePage = await _context.HomePage.FindAsync(id);
            if (homePage != null)
            {
                _context.HomePage.Remove(homePage);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool HomePageExists(decimal id)
        {
          return (_context.HomePage?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
