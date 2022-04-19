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
    public class NeuropsychologicalBatteryScoresController : Controller
    {
        private readonly UdsContext _context;
        private readonly IParticipantsService _participantService;

        public NeuropsychologicalBatteryScoresController(UdsContext context, IParticipantsService participantsService)
        {
            _context = context;
            _participantService = participantsService;_participantService = participantsService;
        }

        // GET: neuropsychologicalBatteryScores
        public async Task<IActionResult> Index()

        {
            var udsContext = _context.NeuropsychologicalBatteryScores.Include(n => n.Visit);
            return View(await udsContext.ToListAsync());
        }

        // GET: neuropsychologicalBatteryScores/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var neuropsychologicalBatteryScores = await _context.NeuropsychologicalBatteryScores
                .Include(n => n.Visit)
                .ThenInclude(v => v.Participant)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (neuropsychologicalBatteryScores == null)
            {
                return NotFound();
            }

            var participantIdentity = await _participantService.GetParticipantAsync(neuropsychologicalBatteryScores.Visit.Participant.Id);
            neuropsychologicalBatteryScores.Visit.Participant.Profile = participantIdentity;

            return View(neuropsychologicalBatteryScores);
        }

        // GET: neuropsychologicalBatteryScores/Create
        public async Task<IActionResult> Create(int id)
        {
             var visit = await _context.Visits.FindAsync(id);

            if (visit == null)
            {
                return NotFound();
            }

            var neuropsychologicalBatteryScores = await _context.NeuropsychologicalBatteryScores.FindAsync(id);

            if (neuropsychologicalBatteryScores == null)
            {
                neuropsychologicalBatteryScores = new NeuropsychologicalBatteryScores 
                {
                    Id = id,
                    FormStatus = FormStatus.Incomplete
                };

                _context.Add(neuropsychologicalBatteryScores);
                await _context.SaveChangesAsync(HttpContext.User.Identity.Name);
            }

            return RedirectToAction("Edit", new { id = neuropsychologicalBatteryScores.Id });
        }

        // POST: neuropsychologicalBatteryScores/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(NeuropsychologicalBatteryScores neuropsychologicalBatteryScores)
        {
            if (ModelState.IsValid)
            {
                _context.Add(neuropsychologicalBatteryScores);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["Id"] = new SelectList(_context.Visits, "Id", "Id", neuropsychologicalBatteryScores.Id);
            return View(neuropsychologicalBatteryScores);
        }

        // GET: neuropsychologicalBatteryScores/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var neuropsychologicalBatteryScores = await _context.NeuropsychologicalBatteryScores
                .Include(c => c.Visit)
                    .ThenInclude(v => v.Participant)
                .AsNoTracking()
                .FirstOrDefaultAsync(c => c.Id == id);

            if (neuropsychologicalBatteryScores == null)
            {
                return NotFound();
            }
            ViewData["Id"] = new SelectList(_context.Visits, "Id", "Id", neuropsychologicalBatteryScores.Id);

            var participantIdentity = await _participantService.GetParticipantAsync(neuropsychologicalBatteryScores.Visit.Participant.Id);
            neuropsychologicalBatteryScores.Visit.Participant.Profile = participantIdentity;

            return View(neuropsychologicalBatteryScores);
        }

        // POST: neuropsychologicalBatteryScores/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, NeuropsychologicalBatteryScores neuropsychologicalBatteryScores, string save, string complete)
        {
           
            if (id != neuropsychologicalBatteryScores.Id)
            {
                return NotFound();
            }

            var visit = await _context.Visits
                .Include("Participant")
                .AsNoTracking()
                .FirstOrDefaultAsync(v => v.Id == neuropsychologicalBatteryScores.Id);

            neuropsychologicalBatteryScores.Visit = visit;

            var participantIdentity = await _participantService.GetParticipantAsync(neuropsychologicalBatteryScores.Visit.Participant.Id);
            neuropsychologicalBatteryScores.Visit.Participant.Profile = participantIdentity;

            if (visit == null)
            {
                return NotFound();
            }

            neuropsychologicalBatteryScores.Visit = visit;

            var viewToReturn = "Edit";

            if (!String.IsNullOrEmpty(save))
            {
                neuropsychologicalBatteryScores.FormStatus = FormStatus.Incomplete;
            }
            else if (!String.IsNullOrEmpty(complete))
            {
                neuropsychologicalBatteryScores.FormStatus = FormStatus.Complete;
                if (!TryValidateModel(neuropsychologicalBatteryScores))
                {
                    return View(viewToReturn, neuropsychologicalBatteryScores);
                }
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(neuropsychologicalBatteryScores);
                    await _context.SaveChangesAsync(HttpContext.User.Identity.Name);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!neuropsychologicalBatteryScoresExists(neuropsychologicalBatteryScores.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction("Details", "Visit", new { id = neuropsychologicalBatteryScores.Id });
            }

            return View(viewToReturn, neuropsychologicalBatteryScores);
        }

        // GET: neuropsychologicalBatteryScores/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var neuropsychologicalBatteryScores = await _context.NeuropsychologicalBatteryScores
                .Include(n => n.Visit)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (neuropsychologicalBatteryScores == null)
            {
                return NotFound();
            }

            return View(neuropsychologicalBatteryScores);
        }

        // POST: neuropsychologicalBatteryScores/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var neuropsychologicalBatteryScores = await _context.NeuropsychologicalBatteryScores.FindAsync(id);
            _context.NeuropsychologicalBatteryScores.Remove(neuropsychologicalBatteryScores);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool neuropsychologicalBatteryScoresExists(int id)
        {
            return _context.NeuropsychologicalBatteryScores.Any(e => e.Id == id);
        }
    }
}
