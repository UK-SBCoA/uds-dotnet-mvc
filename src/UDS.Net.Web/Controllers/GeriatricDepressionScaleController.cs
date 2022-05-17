using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Web;
using UDS.Net.Data;
using UDS.Net.Data.Entities;
using UDS.Net.Data.Enums;
using UDS.Net.Web.Services;

namespace UDS.Net.Web.Controllers
{
    public class GeriatricDepressionScaleController : PacketFormController
    {
        public GeriatricDepressionScaleController(UdsContext context, IParticipantsService participantsService, IChecklistService checklistService) : base(context, participantsService, checklistService)
        {
        }

        // GET: GeriatricDepressionScale
        public async Task<IActionResult> Index()
        {
            var udsContext = _context.GeriatricDepressionScales.Include(g => g.Visit);
            return View(await udsContext.ToListAsync());
        }

        // GET: GeriatricDepressionScale/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var geriatricDepressionScale = await _context.GeriatricDepressionScales
                .Include(g => g.Visit)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (geriatricDepressionScale == null)
            {
                return NotFound();
            }

            return View(geriatricDepressionScale);
        }


        public async Task<IActionResult> Create(int id)
        {
            var geriatricDepressionScale = await _context.GeriatricDepressionScales.FindAsync(id);
            if (geriatricDepressionScale == null)
            {
                geriatricDepressionScale = new GeriatricDepressionScale
                {
                    Id = id,
                    FormStatus = FormStatus.Incomplete
                };
                _context.Add(geriatricDepressionScale);
                await _context.SaveChangesAsync(HttpContext.User.Identity.Name);
            }

            return RedirectToAction("Edit", new { id = geriatricDepressionScale.Id });
        }

        // GET: GeriatricDepressionScale/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var geriatricDepressionScale = await _context.GeriatricDepressionScales
                .Include(c => c.Visit)
                    .ThenInclude(v => v.Participant)
                .AsNoTracking()
                .FirstOrDefaultAsync(c => c.Id == id);

            if (geriatricDepressionScale == null)
            {
                return NotFound();
            }
            else if (!FormCanBeEdited(geriatricDepressionScale.Visit.Status))
            {
                return View("Details", geriatricDepressionScale);
            }

            var participantIdentity = await _participantsService.GetParticipantAsync(geriatricDepressionScale.Visit.Participant.Id);
            geriatricDepressionScale.Visit.Participant.Profile = participantIdentity;

            return View(geriatricDepressionScale);
        }

        // POST: GeriatricDepressionScale/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, GeriatricDepressionScale geriatricDepressionScale, string save, string complete)
        {
            if (id != geriatricDepressionScale.Id)
            {
                return NotFound();
            }

            var visit = await _context.Visits
                .AsNoTracking()
                .Include("Participant")
                .FirstOrDefaultAsync(v => v.Id == geriatricDepressionScale.Id);

            if (!FormCanBeEdited(visit.Status))
            {
                ModelState.AddModelError("FormStatus", "Form cannot be modified because packet is complete.");
                return View(geriatricDepressionScale);
            }

            geriatricDepressionScale.Visit = visit;

            var participantIdentity = await _participantsService.GetParticipantAsync(geriatricDepressionScale.Visit.Participant.Id);
            geriatricDepressionScale.Visit.Participant.Profile = participantIdentity;


            if (!String.IsNullOrEmpty(save))
            {
                geriatricDepressionScale.FormStatus = FormStatus.Incomplete;
            }
            else if (!String.IsNullOrEmpty(complete))
            {
                geriatricDepressionScale.FormStatus = FormStatus.Complete;
                if (!TryValidateModel(geriatricDepressionScale))
                {
                    return View(geriatricDepressionScale);
                }
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(geriatricDepressionScale);
                    await _context.SaveChangesAsync(HttpContext.User.Identity.Name);
                    await _checklistService.ValidateAndUpdateChecklistStatus(visit, typeof(GeriatricDepressionScale));
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!GeriatricDepressionScaleExists(geriatricDepressionScale.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction("Details", "Visit", new { id = geriatricDepressionScale.Id });
            }

            return View(geriatricDepressionScale);
        }

        // GET: GeriatricDepressionScale/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var geriatricDepressionScale = await _context.GeriatricDepressionScales
                .Include(g => g.Visit)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (geriatricDepressionScale == null)
            {
                return NotFound();
            }

            return View(geriatricDepressionScale);
        }

        // POST: GeriatricDepressionScale/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var geriatricDepressionScale = await _context.GeriatricDepressionScales.FindAsync(id);
            _context.GeriatricDepressionScales.Remove(geriatricDepressionScale);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool GeriatricDepressionScaleExists(int id)
        {
            return _context.GeriatricDepressionScales.Any(e => e.Id == id);
        }
    }
}
