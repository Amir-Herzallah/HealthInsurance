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
    public class BeneficiariesController : Controller
    {
        private readonly ModelContext _context;

        public BeneficiariesController(ModelContext context)
        {
            _context = context;
        }

        // GET: Beneficiaries
        public async Task<IActionResult> Index()
        {
            var modelContext = _context.Beneficiaries.Include(b => b.Subscription);
            return View(await modelContext.ToListAsync());
        }

        // GET: Beneficiaries/Details/5
        public async Task<IActionResult> Details(decimal? id)
        {
            if (id == null || _context.Beneficiaries == null)
            {
                return NotFound();
            }

            var beneficiaries = await _context.Beneficiaries
                .Include(b => b.Subscription)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (beneficiaries == null)
            {
                return NotFound();
            }

            return View(beneficiaries);
        }

        // GET: Beneficiaries/Create
        public IActionResult Create()
        {
            ViewData["Subscriptionid"] = new SelectList(_context.Subscriptions, "Id", "Id");
            return View();
        }

        // POST: Beneficiaries/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Subscriptionid,Name,DateOfBirth,Gender,RelationshipToSubscriber,Status,BeneficiaryImagePath,BeneficiaryCreationDate")] Beneficiaries beneficiaries)
        {
            if (ModelState.IsValid)
            {
                _context.Add(beneficiaries);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["Subscriptionid"] = new SelectList(_context.Subscriptions, "Id", "Id", beneficiaries.Subscriptionid);
            return View(beneficiaries);
        }

        // GET: Beneficiaries/Edit/5
        public async Task<IActionResult> Edit(decimal? id)
        {
            if (id == null || _context.Beneficiaries == null)
            {
                return NotFound();
            }

            var beneficiaries = await _context.Beneficiaries.FindAsync(id);
            if (beneficiaries == null)
            {
                return NotFound();
            }
            ViewData["Subscriptionid"] = new SelectList(_context.Subscriptions, "Id", "Id", beneficiaries.Subscriptionid);
            return View(beneficiaries);
        }

        // POST: Beneficiaries/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(decimal id, [Bind("Id,Subscriptionid,Name,DateOfBirth,Gender,RelationshipToSubscriber,Status,BeneficiaryImagePath,BeneficiaryCreationDate")] Beneficiaries beneficiaries)
        {
            if (id != beneficiaries.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(beneficiaries);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!BeneficiariesExists(beneficiaries.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["Subscriptionid"] = new SelectList(_context.Subscriptions, "Id", "Id", beneficiaries.Subscriptionid);
            return View(beneficiaries);
        }

        // GET: Beneficiaries/Delete/5
        public async Task<IActionResult> Delete(decimal? id)
        {
            if (id == null || _context.Beneficiaries == null)
            {
                return NotFound();
            }

            var beneficiaries = await _context.Beneficiaries
                .Include(b => b.Subscription)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (beneficiaries == null)
            {
                return NotFound();
            }

            return View(beneficiaries);
        }

        // POST: Beneficiaries/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(decimal id)
        {
            if (_context.Beneficiaries == null)
            {
                return Problem("Entity set 'ModelContext.Beneficiaries'  is null.");
            }
            var beneficiaries = await _context.Beneficiaries.FindAsync(id);
            if (beneficiaries != null)
            {
                _context.Beneficiaries.Remove(beneficiaries);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool BeneficiariesExists(decimal id)
        {
          return (_context.Beneficiaries?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
