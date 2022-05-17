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
    public class ParticipantDemographicsController : PacketFormController
    {
        public ParticipantDemographicsController(UdsContext context, IParticipantsService participantsService, IChecklistService checklistService) : base(context, participantsService, checklistService)
        {
        }

        // GET: ParticipantDemographics
        public async Task<IActionResult> Index()
        {
            var udsContext = _context.ParticipantDemographics.Include(s => s.Visit);
            return View(await udsContext.ToListAsync());
        }

        // GET: ParticipantDemographics/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var ParticipantDemographics = await _context.ParticipantDemographics
                .Include(s => s.Visit)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (ParticipantDemographics == null)
            {
                return NotFound();
            }

            return View(ParticipantDemographics);
        }

        // GET: ParticipantDemographics/Create
        public async Task<IActionResult> Create(int id)
        {
            var ParticipantDemographics = await _context.ParticipantDemographics.FindAsync(id);

            if (ParticipantDemographics == null)
            {
                var visit = await _context.Visits.Include(v => v.Participant).Where(v => v.Id == id).FirstOrDefaultAsync();

                /// TODO Include sex, education, zip, marital status, etc.
                var participantIdentity = await _participantsService.GetParticipantAsync(visit.Participant.Id);
                int? birthMonth = null;
                int? birthYear = null;

                if (participantIdentity != null && participantIdentity.DateOfBirth.HasValue)
                {
                    birthMonth = participantIdentity.DateOfBirth.Value.Month;
                    birthYear = participantIdentity.DateOfBirth.Value.Year;
                }

                ParticipantDemographics = new ParticipantDemographics
                {
                    Id = id,
                    FormStatus = FormStatus.Incomplete,
                    BirthMonth = birthMonth,
                    BirthYear = birthYear
                };

                _context.Add(ParticipantDemographics);
                await _context.SaveChangesAsync(HttpContext.User.Identity.Name);
            }

            return RedirectToAction("Edit", new { id = ParticipantDemographics.Id });
        }

        // POST: ParticipantDemographics/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Reason,ReferralSource,Learned,EnrollmentDiseaseStatus,PresumedParticipation,EnrollmentType,BirthMonth,BirthYear,Sex,HispanicLatinoEthnicity,HispanicLatinoOrigins,HispanicLatinoOriginsOther,Race,RaceOther,SecondaryRace,SecondaryRaceOther,AdditionalRace,AdditionalRaceOther,PrimaryLanguage,PrimaryLanguageOther,Education,MarriageStatus,LivingSituation,LevelOfIndependence,Residence,Zip,Handedness,Id,ExaminerInitials,FormStatus,Version")] ParticipantDemographics ParticipantDemographics)
        {
            if (ModelState.IsValid)
            {
                _context.Add(ParticipantDemographics);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            
            return View(ParticipantDemographics);
        }

        // GET: ParticipantDemographics/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var participantDemographics = await _context.ParticipantDemographics
                .Include(c => c.Visit)
                    .ThenInclude(v => v.Participant)
                .AsNoTracking()
                .FirstOrDefaultAsync(c => c.Id == id);

            if (participantDemographics == null)
            {
                return NotFound();
            }
            else if (!FormCanBeEdited(participantDemographics.Visit.Status))
            {
                return View("Details", participantDemographics);
            }


            var participantIdentity = await _participantsService.GetParticipantAsync(participantDemographics.Visit.Participant.Id);
            participantDemographics.Visit.Participant.Profile = participantIdentity;

            return View(participantDemographics);
        }

        // POST: ParticipantDemographics/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Reason,ReferralSource,Learned,EnrollmentDiseaseStatus,PresumedParticipation,EnrollmentType,BirthMonth,BirthYear,Sex,HispanicLatinoEthnicity,HispanicLatinoOrigins,HispanicLatinoOriginsOther,Race,RaceOther,SecondaryRace,SecondaryRaceOther,AdditionalRace,AdditionalRaceOther,PrimaryLanguage,PrimaryLanguageOther,Education,MarriageStatus,LivingSituation,LevelOfIndependence,Residence,Zip,Handedness,Id,ExaminerInitials,FormStatus,Version")] ParticipantDemographics participantDemographics, string save, string complete)
        {
             if (id != participantDemographics.Id)
            {
                return NotFound();
            }

            var visit = await _context.Visits
               .AsNoTracking()
               .Include("Participant")
               .FirstOrDefaultAsync(v => v.Id == participantDemographics.Id);

            if (!FormCanBeEdited(visit.Status))
            {
                ModelState.AddModelError("FormStatus", "Form cannot be modified because packet is complete.");
                return View(participantDemographics);
            }

            participantDemographics.Visit = visit;

            var participantIdentity = await _participantsService.GetParticipantAsync(participantDemographics.Visit.Participant.Id);
            participantDemographics.Visit.Participant.Profile = participantIdentity;

            if (!String.IsNullOrEmpty(save))
            {
                participantDemographics.FormStatus = FormStatus.Incomplete;
            }
            else if (!String.IsNullOrEmpty(complete))
            {
                participantDemographics.FormStatus = FormStatus.Complete;
                if (TryValidateModel(participantDemographics))
                {
                    if (participantDemographics.Visit.VisitType == VisitType.IVP)
                    {
                        // several fields are required for initial visits
                        if (!participantDemographics.Reason.HasValue)
                            ModelState.AddModelError("Reason", "Provide a reason");
                        if (!participantDemographics.ReferralSource.HasValue)
                            ModelState.AddModelError("ReferralSource", "Provide the referral source");
                        if (!participantDemographics.EnrollmentDiseaseStatus.HasValue)
                            ModelState.AddModelError("EnrollmentDiseaseStatus", "Indicate the presumed the disease status");
                        if (!participantDemographics.PresumedParticipation.HasValue)
                            ModelState.AddModelError("PresumedParticipation", "Indicate the participation");
                        if (!participantDemographics.EnrollmentType.HasValue)
                            ModelState.AddModelError("EnrollmentType", "Indicate the enrollment type");
                        if (!participantDemographics.HispanicLatinoEthnicity.HasValue)
                            ModelState.AddModelError("HispanicLatinoEthnicity", "Indicate ethnicity");
                        if (!participantDemographics.Race.HasValue)
                            ModelState.AddModelError("Race", "Indicate race");
                        if (!participantDemographics.SecondaryRace.HasValue)
                            ModelState.AddModelError("SecondaryRace", "Indicate secondary race");
                        if (!participantDemographics.AdditionalRace.HasValue)
                            ModelState.AddModelError("AdditionalRace", "Indicate additional race");
                        if (!participantDemographics.PrimaryLanguage.HasValue)
                            ModelState.AddModelError("PrimaryLanguage", "Indicate primary language");
                        if (!participantDemographics.Education.HasValue)
                            ModelState.AddModelError("Education", "Indicate education level");
                        if (!participantDemographics.Handedness.HasValue)
                            ModelState.AddModelError("Handedness", "Indicate participant's handedness");

                    }

                    if (!ModelState.IsValid) // if any of the above added errors return the view
                        return View(participantDemographics);
                }
                else
                {
                    return View(participantDemographics);
                }
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(participantDemographics);
                    await _context.SaveChangesAsync(HttpContext.User.Identity.Name);
                    await _checklistService.ValidateAndUpdateChecklistStatus(visit, typeof(ParticipantDemographics));
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ParticipantDemographicsExists(participantDemographics.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction("Details", "Visit", new { id = participantDemographics.Id });
            }

            return View(participantDemographics);
        }

        // GET: ParticipantDemographics/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var ParticipantDemographics = await _context.ParticipantDemographics
                .Include(s => s.Visit)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (ParticipantDemographics == null)
            {
                return NotFound();
            }

            return View(ParticipantDemographics);
        }

        // POST: ParticipantDemographics/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var ParticipantDemographics = await _context.ParticipantDemographics.FindAsync(id);
            _context.ParticipantDemographics.Remove(ParticipantDemographics);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ParticipantDemographicsExists(int id)
        {
            return _context.ParticipantDemographics.Any(e => e.Id == id);
        }
    }
}
