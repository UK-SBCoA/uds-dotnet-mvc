using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;
using UDS.Net.Data;
using UDS.Net.Data.Entities;
using UDS.Net.Data.Enums;
using UDS.Net.Web.ViewModels;
using UDS.Net.Web.Services;


namespace UDS.Net.Web.Controllers
{
    /// <summary>
    /// Telephone Inclusion form
    /// </summary>
    public class InclusionController : PacketFormController
    {
        public InclusionController(UdsContext context, IParticipantsService participantsService, IChecklistService checklistService) : base(context, participantsService, checklistService)
        {
        }

        // GET: Inclusion/Create
        public async Task<IActionResult> Create(int id)
        {
            var inclusion = await _context.Inclusions.FindAsync(id);
            if (inclusion == null)
            {
                inclusion = new Inclusion
                {
                    Id = id,
                    FormStatus = FormStatus.Incomplete
                };
                _context.Add(inclusion);
                await _context.SaveChangesAsync(HttpContext.User.Identity.Name);
            }
            return RedirectToAction("Edit", new { id = inclusion.Id });
        }


        // GET: Inclusion/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var inclusion = await _context.Inclusions
                .Include(c => c.Visit)
                    .ThenInclude(v => v.Participant)
                .AsNoTracking()
                .FirstOrDefaultAsync(c => c.Id == id);

            if (inclusion == null)
            {
                return NotFound();
            }
            else if (!FormCanBeEdited(inclusion.Visit.Status))
            {
                return View("Details", inclusion);
            }

            var participantIdentity = await _participantsService.GetParticipantAsync(inclusion.Visit.Participant.Id);
            inclusion.Visit.Participant.Profile = participantIdentity;

            return View(inclusion);
        }

        // POST: Inclusion/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Inclusion inclusion, string save, string complete)
        {
            if (id != inclusion.Id)
            {
                return NotFound();
            }

            var visit = await _context.Visits
                .AsNoTracking()
                .Include("Participant")
                .FirstOrDefaultAsync(v => v.Id == inclusion.Id);

            if (!FormCanBeEdited(visit.Status))
            {
                ModelState.AddModelError("FormStatus", "Form cannot be modified because packet is complete.");
                return View(inclusion);
            }

            inclusion.Visit = visit;

            var participantIdentity = await _participantsService.GetParticipantAsync(inclusion.Visit.Participant.Id);
            inclusion.Visit.Participant.Profile = participantIdentity;

            if (!String.IsNullOrEmpty(save))
            {
                inclusion.FormStatus = FormStatus.Incomplete;
            }
            else if (!String.IsNullOrEmpty(complete))
            {
                inclusion.FormStatus = FormStatus.Complete;
                if (!TryValidateModel(inclusion))
                {
                    return View(inclusion);
                }
            }
            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(inclusion);
                    await _context.SaveChangesAsync(HttpContext.User.Identity.Name);
                    await _checklistService.ValidateAndUpdateChecklistStatus(visit, typeof(Inclusion));
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!InclusionExists(inclusion.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction("Details", "Visit", new { id = inclusion.Id });
            }
            return View(inclusion);
        }

        private bool InclusionExists(int id)
        {
            return _context.Inclusions.Any(e => e.Id == id);
        }

    }
}
