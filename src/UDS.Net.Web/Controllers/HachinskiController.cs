using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using UDS.Net.Data;
using UDS.Net.Data.Entities;
using UDS.Net.Data.Enums;
using UDS.Net.Web.Services;

namespace UDS.Net.Web.Controllers
{
    public class HachinskiController : PacketFormController
    {
        public HachinskiController(UdsContext context, IParticipantsService participantsService, IChecklistService checklistService) : base(context, participantsService, checklistService)
        {
        }

        // GET: Hachinski
        public async Task<IActionResult> Index()
        {
            var udsContext = _context.HachinskiScores.Include(h => h.Visit);
            return View(await udsContext.ToListAsync());
        }

        // GET: Hachinski/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var hachinski = await _context.HachinskiScores
                .Include(h => h.Visit)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (hachinski == null)
            {
                return NotFound();
            }

            return View(hachinski);
        }

        // GET: Hachinski/Create
        public async Task<IActionResult> Create(int id)
        {
            var hachinski = await _context.HachinskiScores.FindAsync(id);

            if (hachinski == null)
            {
                hachinski = new Hachinski
                {
                    Id = id,
                    FormStatus = FormStatus.Incomplete
                };
                _context.Add(hachinski);
                await _context.SaveChangesAsync(HttpContext.User.Identity.Name);
            }
            return RedirectToAction("Edit", new { id = hachinski.Id });
        }

        // POST: Hachinski/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Hachinski hachinski)
        {
            if (ModelState.IsValid)
            {
                _context.Add(hachinski);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["Id"] = new SelectList(_context.Visits, "Id", "Id", hachinski.Id);
            return View(hachinski);
        }

        // GET: Hachinski/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var hachinski = await _context.HachinskiScores
                .Include(c => c.Visit)
                    .ThenInclude(v => v.Participant)
                .AsNoTracking()
                .FirstOrDefaultAsync(c => c.Id == id);

            if (hachinski == null)
            {
                return NotFound();
            }
            else if (!FormCanBeEdited(hachinski.Visit.Status))
            {
                return View("Details", hachinski);
            }

            var participantIdentity = await _participantsService.GetParticipantAsync(hachinski.Visit.Participant.Id);
            hachinski.Visit.Participant.Profile = participantIdentity;

            return View(hachinski);
        }

        // POST: Hachinski/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Hachinski hachinski, string save, string complete)
        {
            if (id != hachinski.Id)
            {
                return NotFound();
            }

            var visit = await _context.Visits
                .Include("Participant")
                .AsNoTracking() // only the form is being modified, so we don't need to track the visit
                .FirstOrDefaultAsync(v => v.Id == hachinski.Id);

            if (!FormCanBeEdited(visit.Status))
            {
                ModelState.AddModelError("FormStatus", "Form cannot be modified because packet is complete.");
                return View(hachinski);
            }

            hachinski.Visit = visit;

            var participantIdentity = await _participantsService.GetParticipantAsync(hachinski.Visit.Participant.Id);
            hachinski.Visit.Participant.Profile = participantIdentity;

            if (!String.IsNullOrEmpty(save))
            {
                hachinski.FormStatus = FormStatus.Incomplete;
            }
            else if (!String.IsNullOrEmpty(complete))
            {
                hachinski.FormStatus = FormStatus.Complete;
                if (!TryValidateModel(hachinski))
                {
                    return View(hachinski);
                }
            }
            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(hachinski);
                    await _context.SaveChangesAsync(HttpContext.User.Identity.Name);
                    await _checklistService.ValidateAndUpdateChecklistStatus(visit, typeof(Hachinski));
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!HachinskiExists(hachinski.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction("Details","Visit", new { id = hachinski.Id });
            }
            return View(hachinski);
        }

        // GET: Hachinski/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var hachinski = await _context.HachinskiScores
                .Include(h => h.Visit)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (hachinski == null)
            {
                return NotFound();
            }

            return View(hachinski);
        }

        // POST: Hachinski/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var hachinski = await _context.HachinskiScores.FindAsync(id);
            _context.HachinskiScores.Remove(hachinski);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool HachinskiExists(int id)
        {
            return _context.HachinskiScores.Any(e => e.Id == id);
        }
    }
}
