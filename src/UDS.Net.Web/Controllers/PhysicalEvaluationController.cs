using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using UDS.Net.Data;
using UDS.Net.Data.Entities;
using UDS.Net.Data.Enums;
using UDS.Net.Web.Services;

namespace UDS.Net.Web.Controllers
{
    public class PhysicalEvaluationController : PacketFormController
    {
        public PhysicalEvaluationController(UdsContext context, IParticipantsService participantsService, IChecklistService checklistService) : base(context, participantsService, checklistService)
        {
        }

        // GET: PhysicalEvaluation
        public async Task<IActionResult> Index()
        {
            var udsContext = _context.PhysicalEvaluation.Include(p => p.Visit);
            return View(await udsContext.ToListAsync());
        }

        // GET: PhysicalEvaluation/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var physicalEvaluation = await _context.PhysicalEvaluation
                .Include(p => p.Visit)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (physicalEvaluation == null)
            {
                return NotFound();
            }

            return View(physicalEvaluation);
        }
        public async Task<IActionResult> Create(int id)
        {
            var physicalEvaluation = await _context.PhysicalEvaluation.FindAsync(id);
            if (physicalEvaluation == null)
            {
                physicalEvaluation = new PhysicalEvaluation
                {
                    Id = id,
                    FormStatus = FormStatus.Incomplete
                };
                _context.Add(physicalEvaluation);
                await _context.SaveChangesAsync(HttpContext.User.Identity.Name);
            }
            return RedirectToAction("Edit", new { id = physicalEvaluation.Id });
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var physicalEvaluation = await _context.PhysicalEvaluation
                .Include(c => c.Visit)
                    .ThenInclude(v => v.Participant)
                .AsNoTracking()
                .FirstOrDefaultAsync(c => c.Id == id);

            if (physicalEvaluation == null)
            {
                return NotFound();
            }
            else if (!FormCanBeEdited(physicalEvaluation.Visit.Status))
            {
                return View("Details", physicalEvaluation);
            }

            var participantIdentity = await _participantsService.GetParticipantAsync(physicalEvaluation.Visit.Participant.Id);
            physicalEvaluation.Visit.Participant.Profile = participantIdentity;

            return View(physicalEvaluation);
        }

        // POST: HealthSurvey/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, PhysicalEvaluation physicalEvaluation, string save, string complete)
        {
            if (id != physicalEvaluation.Id)
            {
                return NotFound();
            }
            var visit = await _context.Visits
                .AsNoTracking()
                .Include("Participant")
                .FirstOrDefaultAsync(v => v.Id == physicalEvaluation.Id);

            if (!FormCanBeEdited(visit.Status))
            {
                ModelState.AddModelError("FormStatus", "Form cannot be modified because packet is complete.");
                return View(physicalEvaluation);
            }

            physicalEvaluation.Visit = visit;

            var participantIdentity = await _participantsService.GetParticipantAsync(physicalEvaluation.Visit.Participant.Id);
            physicalEvaluation.Visit.Participant.Profile = participantIdentity;

            if (!String.IsNullOrEmpty(save))
            {
                physicalEvaluation.FormStatus = FormStatus.Incomplete;
            }
            else if (!String.IsNullOrEmpty(complete))
            {
                physicalEvaluation.FormStatus = FormStatus.Complete;
                if (!TryValidateModel(physicalEvaluation))
                {
                    return View(physicalEvaluation);
                }
            }
            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(physicalEvaluation);
                    await _context.SaveChangesAsync(HttpContext.User.Identity.Name);
                    await _checklistService.ValidateAndUpdateChecklistStatus(visit, typeof(PhysicalEvaluation));
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PhysicalEvaluationExists(physicalEvaluation.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction("Details", "Visit", new { id = physicalEvaluation.Id });
            }
            return View(physicalEvaluation);
        }

        // GET: PhysicalEvaluation/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var physicalEvaluation = await _context.PhysicalEvaluation
                .Include(p => p.Visit)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (physicalEvaluation == null)
            {
                return NotFound();
            }

            return View(physicalEvaluation);
        }

        // POST: PhysicalEvaluation/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var physicalEvaluation = await _context.PhysicalEvaluation.FindAsync(id);
            _context.PhysicalEvaluation.Remove(physicalEvaluation);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool PhysicalEvaluationExists(int id)
        {
            return _context.PhysicalEvaluation.Any(e => e.Id == id);
        }
    }
}
