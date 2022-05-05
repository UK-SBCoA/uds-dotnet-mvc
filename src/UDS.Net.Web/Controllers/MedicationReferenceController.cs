using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using UDS.Net.Data;
using UDS.Net.Data.Entities;
using UDS.Net.Web.ViewModels;

namespace UDS.Net.Web.Controllers
{
    public class MedicationReferenceController : Controller
    {
        private readonly UdsContext _context;

        public MedicationReferenceController(UdsContext context)
        {
            _context = context;
        }

        // GET: MedicationReference
        public async Task<IActionResult> Index()
        {
            return View(await _context.MedicationReferences.ToListAsync());
        }

        // GET: MedicationReference/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var medicationReference = await _context.MedicationReferences
                .FirstOrDefaultAsync(m => m.DrugId == id);
            if (medicationReference == null)
            {
                return NotFound();
            }

            return View(medicationReference);
        }

        // GET: MedicationReference/Create
        public IActionResult Create(int? visitId)
        {
            var medicationReference = new MedicationReferenceWithOriginatingVisit
            {
                VisitId = visitId.Value
            };
            return View(medicationReference);
        }

        // POST: MedicationReference/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(MedicationReferenceWithOriginatingVisit medicationReferenceWithOriginatingVisit)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    _context.Add(medicationReferenceWithOriginatingVisit.MedicationReference);
                    await _context.SaveChangesAsync(HttpContext.User.Identity.Name);

                    if (medicationReferenceWithOriginatingVisit.VisitId.HasValue)
                    {
                        var newCurrentMedication = new MedicationCurrent
                        {
                            MedicationsReviewId = medicationReferenceWithOriginatingVisit.VisitId.Value,
                            DrugId = medicationReferenceWithOriginatingVisit.MedicationReference.DrugId
                        };

                        _context.Add(newCurrentMedication);
                        await _context.SaveChangesAsync(HttpContext.User.Identity.Name);
                    }
                }
                catch (DbUpdateException)
                {
                    // Catches duplicate drugID keys
                    // One day highlight the already existing drug, for now just redirect to list
                    ModelState.AddModelError("DrugId", String.Format("<p class='text-danger'>This drug already exists in the medication reference list.</p><p><a href='/MedicationsReview/edit/{0}'>Go back and select this medication from the reference list.</a></p>", medicationReferenceWithOriginatingVisit.VisitId));
                    return View(medicationReferenceWithOriginatingVisit);
                }
                if (medicationReferenceWithOriginatingVisit.VisitId.HasValue)
                {
                    return RedirectToAction("Edit", "MedicationsReview", new { id = medicationReferenceWithOriginatingVisit.VisitId.Value });
                }
                else
                {
                    return RedirectToAction("Index");
                }
            }
            return View(medicationReferenceWithOriginatingVisit);
        }

        // GET: MedicationReference/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var medicationReference = await _context.MedicationReferences.FindAsync(id);
            if (medicationReference == null)
            {
                return NotFound();
            }
            return View(medicationReference);
        }

        // POST: MedicationReference/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("DrugId,DisplayName,FromNACCDefaultReference,IsOverTheCounter")] MedicationReference medicationReference)
        {
            if (id != medicationReference.DrugId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(medicationReference);
                    await _context.SaveChangesAsync(HttpContext.User.Identity.Name);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!MedicationReferenceExists(medicationReference.DrugId))
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
            return View(medicationReference);
        }

        // GET: MedicationReference/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var medicationReference = await _context.MedicationReferences
                .FirstOrDefaultAsync(m => m.DrugId == id);
            if (medicationReference == null)
            {
                return NotFound();
            }

            return View(medicationReference);
        }

        // POST: MedicationReference/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var medicationReference = await _context.MedicationReferences.FindAsync(id);
            _context.MedicationReferences.Remove(medicationReference);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool MedicationReferenceExists(string id)
        {
            return _context.MedicationReferences.Any(e => e.DrugId == id);
        }
    }
}
