using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using UDS.Net.Data;
using UDS.Net.Data.Entities;

namespace UDS.Net.Web.Controllers
{
    public class MedicationCurrentController : Controller
    {
        private readonly UdsContext _context;

        public MedicationCurrentController(UdsContext context)
        {
            _context = context;
        }

        // GET: MedicationCurrent
        public async Task<IActionResult> Index()
        {
            var udsContext = _context.MedicationCurrent.Include(m => m.MedicationsReview);
            return View(await udsContext.ToListAsync());
        }

        // GET: MedicationCurrent/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var medicationCurrent = await _context.MedicationCurrent
                .Include(m => m.MedicationsReview)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (medicationCurrent == null)
            {
                return NotFound();
            }

            return View(medicationCurrent);
        }

        // GET: MedicationCurrent/Create
        public IActionResult Create()
        {
            ViewData["MedicationsReviewId"] = new SelectList(_context.MedicationsReviews, "Id", "Id");
            return View();
        }

        // POST: MedicationCurrent/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,DrugId,Notes,MedicationsReviewId")] MedicationCurrent medicationCurrent)
        {
            if (ModelState.IsValid)
            {
                _context.Add(medicationCurrent);
                await _context.SaveChangesAsync(HttpContext.User.Identity.Name);
                return RedirectToAction(nameof(Index));
            }
            ViewData["MedicationsReviewId"] = new SelectList(_context.MedicationsReviews, "Id", "Id", medicationCurrent.MedicationsReviewId);
            return View(medicationCurrent);
        }

        // GET: MedicationCurrent/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var medicationCurrent = await _context.MedicationCurrent.FindAsync(id);
            if (medicationCurrent == null)
            {
                return NotFound();
            }
            ViewData["MedicationsReviewId"] = new SelectList(_context.MedicationsReviews, "Id", "Id", medicationCurrent.MedicationsReviewId);
            return View(medicationCurrent);
        }

        // POST: MedicationCurrent/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,DrugId,Notes,MedicationsReviewId")] MedicationCurrent medicationCurrent)
        {
            if (id != medicationCurrent.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(medicationCurrent);
                    await _context.SaveChangesAsync(HttpContext.User.Identity.Name);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!MedicationCurrentExists(medicationCurrent.Id))
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
            ViewData["MedicationsReviewId"] = new SelectList(_context.MedicationsReviews, "Id", "Id", medicationCurrent.MedicationsReviewId);
            return View(medicationCurrent);
        }

        // GET: MedicationCurrent/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var medicationCurrent = await _context.MedicationCurrent
                .Include(m => m.MedicationsReview)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (medicationCurrent == null)
            {
                return NotFound();
            }

            return View(medicationCurrent);
        }

        // POST: MedicationCurrent/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var medicationCurrent = await _context.MedicationCurrent.FindAsync(id);
            _context.MedicationCurrent.Remove(medicationCurrent);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool MedicationCurrentExists(int id)
        {
            return _context.MedicationCurrent.Any(e => e.Id == id);
        }
    }
}
