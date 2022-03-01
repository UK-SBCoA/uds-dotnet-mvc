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
    public class ParticipantDemographicsController : Controller
    {
        private readonly UdsContext _context;
        private readonly IParticipantsService _participantsService;

        public ParticipantDemographicsController(UdsContext context, IParticipantsService participantsService)
        {
            _context = context;
            _participantsService = participantsService;
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

            var ParticipantDemographics = await _context.ParticipantDemographics
                .Include("Visit")
                .Include(c => c.Visit)
                    .ThenInclude(v => v.Participant)
                .AsNoTracking()
                .FirstOrDefaultAsync(c => c.Id == id);
            if (ParticipantDemographics == null)
            {
                return NotFound();
            }


            var participantIdentity = await _participantsService.GetParticipantAsync(ParticipantDemographics.Visit.Participant.Id);
            ParticipantDemographics.Visit.Participant.Profile = participantIdentity;

            return View(ParticipantDemographics);
        }

        // POST: ParticipantDemographics/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Reason,ReferralSource,Learned,EnrollmentDiseaseStatus,PresumedParticipation,EnrollmentType,BirthMonth,BirthYear,Sex,HispanicLatinoEthnicity,HispanicLatinoOrigins,HispanicLatinoOriginsOther,Race,RaceOther,SecondaryRace,SecondaryRaceOther,AdditionalRace,AdditionalRaceOther,PrimaryLanguage,PrimaryLanguageOther,Education,MarriageStatus,LivingSituation,LevelOfIndependence,Residence,Zip,Handedness,Id,ExaminerInitials,FormStatus,Version")] ParticipantDemographics ParticipantDemographics, string save, string complete)
        {
             if (id != ParticipantDemographics.Id)
            {
                return NotFound();
            }

            var visit = await _context.Visits
               .AsNoTracking()
               .Include("Participant")
               .FirstOrDefaultAsync(v => v.Id == ParticipantDemographics.Id);

            ParticipantDemographics.Visit = visit;

            var participantIdentity = await _participantsService.GetParticipantAsync(ParticipantDemographics.Visit.Participant.Id);
            ParticipantDemographics.Visit.Participant.Profile = participantIdentity;

            if (!String.IsNullOrEmpty(save))
            {
                ParticipantDemographics.FormStatus = FormStatus.Incomplete;
            }
            else if (!String.IsNullOrEmpty(complete))
            {
                ParticipantDemographics.FormStatus = FormStatus.Complete;
                if (TryValidateModel(ParticipantDemographics))
                {
                    if (ParticipantDemographics.Visit.VisitType == VisitType.IVP)
                    {
                        // several fields are required for initial visits
                        if (!ParticipantDemographics.Reason.HasValue)
                            ModelState.AddModelError("Reason", "Provide a reason");
                        if (!ParticipantDemographics.ReferralSource.HasValue)
                            ModelState.AddModelError("ReferralSource", "Provide the referral source");
                        if (!ParticipantDemographics.EnrollmentDiseaseStatus.HasValue)
                            ModelState.AddModelError("EnrollmentDiseaseStatus", "Indicate the presumed the disease status");
                        if (!ParticipantDemographics.PresumedParticipation.HasValue)
                            ModelState.AddModelError("PresumedParticipation", "Indicate the participation");
                        if (!ParticipantDemographics.EnrollmentType.HasValue)
                            ModelState.AddModelError("EnrollmentType", "Indicate the enrollment type");
                        if (!ParticipantDemographics.HispanicLatinoEthnicity.HasValue)
                            ModelState.AddModelError("HispanicLatinoEthnicity", "Indicate ethnicity");
                        if (!ParticipantDemographics.Race.HasValue)
                            ModelState.AddModelError("Race", "Indicate race");
                        if (!ParticipantDemographics.SecondaryRace.HasValue)
                            ModelState.AddModelError("SecondaryRace", "Indicate secondary race");
                        if (!ParticipantDemographics.AdditionalRace.HasValue)
                            ModelState.AddModelError("AdditionalRace", "Indicate additional race");
                        if (!ParticipantDemographics.PrimaryLanguage.HasValue)
                            ModelState.AddModelError("PrimaryLanguage", "Indicate primary language");
                        if (!ParticipantDemographics.Education.HasValue)
                            ModelState.AddModelError("Education", "Indicate education level");
                        if (!ParticipantDemographics.Handedness.HasValue)
                            ModelState.AddModelError("Handedness", "Indicate participant's handedness");

                    }

                    if (!ModelState.IsValid) // if any of the above added errors return the view
                        return View(ParticipantDemographics);
                }
                else
                {
                    return View(ParticipantDemographics);
                }
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(ParticipantDemographics);
                    await _context.SaveChangesAsync(HttpContext.User.Identity.Name);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ParticipantDemographicsExists(ParticipantDemographics.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction("Details", "Visit", new { id = ParticipantDemographics.Id });
            }

            return View(ParticipantDemographics);
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
