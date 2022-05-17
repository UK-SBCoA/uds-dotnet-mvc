using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using UDS.Net.Data;
using UDS.Net.Data.Entities;
using UDS.Net.Data.Enums;
using UDS.Net.Web.ViewModels;
using UDS.Net.Web.Services;

namespace UDS.Net.Web.Controllers
{
    public class MedicationsReviewController : PacketFormController
    {
        public MedicationsReviewController(UdsContext context, IParticipantsService participantsService, IChecklistService checklistService) : base(context, participantsService, checklistService)
        {
        }

        // GET: MedicationsReview
        public async Task<IActionResult> Index()
        {
            var udsContext = _context.MedicationsReviews.Include(m => m.Visit);
            return View(await udsContext.ToListAsync());
        }

        // GET: MedicationsReview/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var medicationsReview = await _context.MedicationsReviews
                .Include(m => m.Visit)
                .ThenInclude(v => v.Participant)
                .Include(m => m.CurrentMedications)
                .ThenInclude(m => m.MedicationReference)
                .AsNoTracking()
                .FirstOrDefaultAsync(m => m.Id == id);

            if (medicationsReview == null)
            {
                return NotFound();
            }

            var participantIdentity = await _participantsService.GetParticipantAsync(medicationsReview.Visit.Participant.Id);
            medicationsReview.Visit.Participant.Profile = participantIdentity;

            return View(medicationsReview);
        }

        /// <summary>
        /// Medications relies heavily on child object, MedicationCurrent, so using a different pattern here than the usual.
        /// This method creates and redirects to edit.
        /// </summary>
        /// <param name="id">Visit Id</param>
        /// <returns></returns>
        public async Task<IActionResult> Create(int id)
        {
            var medicationsReview = await _context.MedicationsReviews.FindAsync(id);

            if (medicationsReview == null)
            {
                medicationsReview = new MedicationsReview
                {
                    Id = id,
                    FormStatus = FormStatus.Incomplete
                };
                _context.Add(medicationsReview);
                await _context.SaveChangesAsync(HttpContext.User.Identity.Name);
            }

            return RedirectToAction("Edit", new { id = medicationsReview.Id });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<JsonResult> UpdateCurrent(MedicationSelected medicationSelected)
        {
            var medicationsReview = await _context.MedicationsReviews.FirstOrDefaultAsync(m => m.Id == medicationSelected.VisitId);
            if (medicationSelected.Selected)
            {
                // check to see if it already exists
                var existingMedication = await _context.MedicationCurrent
                    .Where(m => m.DrugId == medicationSelected.MedicationReference.DrugId
                    && m.MedicationsReviewId == medicationSelected.VisitId)
                    .FirstOrDefaultAsync();

                if (existingMedication == null)
                {
                    // create
                    var newCurrentMedication = new MedicationCurrent
                    {
                        MedicationsReviewId = medicationSelected.VisitId,
                        DrugId = medicationSelected.MedicationReference.DrugId,
                        Notes = medicationSelected.MedicationCurrent.Notes
                    };

                    _context.Add(newCurrentMedication);
                    await _context.SaveChangesAsync(HttpContext.User.Identity.Name);

                    medicationSelected.MedicationCurrent = newCurrentMedication;
                }
                else
                {
                    medicationSelected.MedicationCurrent = existingMedication;
                }
            }
            else
            {
                // delete
                _context.Remove(medicationSelected.MedicationCurrent);
                await _context.SaveChangesAsync(HttpContext.User.Identity.Name);
            }
            return Json(medicationSelected);
        }

        // POST: MedicationsReview/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("CurrentlyTakingMedications,Comments,Id,ExaminerInitials,FormStatus")] MedicationsReview medicationsReview)
        {
            if (ModelState.IsValid)
            {
                _context.Add(medicationsReview);
                await _context.SaveChangesAsync(HttpContext.User.Identity.Name);
                return RedirectToAction(nameof(Index));
            }

            return View(medicationsReview);
        }

        // GET: MedicationsReview/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var medicationsReview = await _context.MedicationsReviews
                .Include(m => m.CurrentMedications)
                .Include(c => c.Visit)
                    .ThenInclude(v => v.Participant)
                .AsNoTracking()
                .FirstOrDefaultAsync(m => m.Id == id);

            if (medicationsReview == null)
            {
                return NotFound();
            }
            else if (!FormCanBeEdited(medicationsReview.Visit.Status))
            {
                return View("Details", medicationsReview);
            }
            
            await SetMedicationViewbag(medicationsReview.Id, medicationsReview);

            var participantIdentity = await _participantsService.GetParticipantAsync(medicationsReview.Visit.Participant.Id);
            medicationsReview.Visit.Participant.Profile = participantIdentity;
            return View(medicationsReview);
        }

        private async Task SetMedicationViewbag(int formId, MedicationsReview medicationsReview)
        {
            var medicationsReferenceWithSelected = await GetMedicationReferenceWithSelected(formId, medicationsReview); 
            // Default = true && OverTheCounter = false and ALL SELECTED
            var defaultRef = GetDefaultMedicationsWithSelected(medicationsReferenceWithSelected).ToList();
            ViewData["DefaultRef"] = defaultRef;
            // Default = true && OverTheCounter = true OR Default = false && OverTheCounter = true
            ViewData["OverTheCounterRef"] = GetOverTheCounterMedications(medicationsReferenceWithSelected).Except(defaultRef).ToList();
            // Default = false && OverTheCounter = false
            ViewData["NonDefaultRef"] = GetNonDefaultMedications(medicationsReferenceWithSelected).ToList();
        }

        private IEnumerable<MedicationSelected> GetDefaultMedicationsWithSelected(List<MedicationSelected> medications)
        {
            var defaultMeds = medications.Where(x => x.MedicationReference.FromNACCDefaultReference && !x.MedicationReference.IsOverTheCounter);
            var nonDefualt = GetNonDefaultMedicationsSelected(medications);
            var allMedsWithSelected = defaultMeds.Concat(nonDefualt).OrderBy(x => x.MedicationReference.DisplayName);
            return allMedsWithSelected;
        }

        private IEnumerable<MedicationSelected> GetNonDefaultMedications(List<MedicationSelected> medications)
        {
            return medications.Where(x => !x.MedicationReference.FromNACCDefaultReference && !x.MedicationReference.IsOverTheCounter && !x.Selected);
        }

        private IEnumerable<MedicationSelected> GetNonDefaultMedicationsSelected(List<MedicationSelected> medications)
        {
            return medications.Where(x => !x.MedicationReference.FromNACCDefaultReference && !x.MedicationReference.IsOverTheCounter && x.Selected);
        }

        private IEnumerable<MedicationSelected> GetOverTheCounterMedications(List<MedicationSelected> medications)
        {
            return medications.Where(x => x.MedicationReference.IsOverTheCounter);
        }

        private async Task<List<MedicationSelected>> GetMedicationReferenceWithSelected(int id, MedicationsReview medicationsReview)
        {
            // Get reference medications
            var medicationsReferenceWithSelected = new List<MedicationSelected>(); 
            var medicationsReference = await _context.MedicationReferences
                                        .OrderBy(m => m.DisplayName)
                                        .AsNoTracking()
                                        .ToListAsync();

            foreach (var medRef in medicationsReference)
            {
                var selected = false;
                var currentMed = medicationsReview.CurrentMedications?.Where(c => c.DrugId == medRef.DrugId).FirstOrDefault();

                if (currentMed != null)
                {
                    selected = true;
                }

                medicationsReferenceWithSelected.Add(new MedicationSelected
                {
                    VisitId = id,
                    MedicationReference = medRef,
                    Selected = selected,
                    MedicationCurrent = currentMed
                });
            }

            return medicationsReferenceWithSelected;
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, MedicationsReview medicationsReview, string[] MedicationsSelected, string save, string complete)
        {
            if (id != medicationsReview.Id)
            {
                return NotFound();
            }

            var visit = await _context.Visits
                .AsNoTracking()
                .Include("Participant")
                .FirstOrDefaultAsync(v => v.Id == medicationsReview.Id);

            if (!FormCanBeEdited(visit.Status))
            {
                ModelState.AddModelError("FormStatus", "Form cannot be modified because packet is complete.");
                return View(medicationsReview);
            }

            medicationsReview.Visit = visit;

            var participantIdentity = await _participantsService.GetParticipantAsync(medicationsReview.Visit.Participant.Id);
            medicationsReview.Visit.Participant.Profile = participantIdentity;

            // Set Viewbag
            await SetMedicationViewbag(medicationsReview.Id, medicationsReview);

            if (!String.IsNullOrEmpty(save))
            {
                medicationsReview.FormStatus = FormStatus.Incomplete;
            }
            else if (!String.IsNullOrEmpty(complete))
            {
                medicationsReview.FormStatus = FormStatus.Complete;
                if (!TryValidateModel(medicationsReview))
                {
                    return View(medicationsReview);
                }
            }
            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(medicationsReview);
                    await _context.SaveChangesAsync(HttpContext.User.Identity.Name);
                    await _checklistService.ValidateAndUpdateChecklistStatus(visit, typeof(MedicationsReview));
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!MedicationsReviewExists(medicationsReview.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction("Details", "Visit", new { id = medicationsReview.Id });
            }
            return View(medicationsReview);
        }

        // GET: MedicationsReview/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var medicationsReview = await _context.MedicationsReviews
                .Include(m => m.Visit)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (medicationsReview == null)
            {
                return NotFound();
            }

            return View(medicationsReview);
        }

        // POST: MedicationsReview/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var medicationsReview = await _context.MedicationsReviews.FindAsync(id);
            _context.MedicationsReviews.Remove(medicationsReview);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool MedicationsReviewExists(int id)
        {
            return _context.MedicationsReviews.Any(e => e.Id == id);
        }
    }
}
