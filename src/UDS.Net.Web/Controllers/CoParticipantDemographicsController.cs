
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
    public class CoParticipantDemographicsController : Controller
    {
        private readonly UdsContext _context;
        private readonly IParticipantsService _participantService;

        public CoParticipantDemographicsController(UdsContext context, IParticipantsService participantsService)
        {
            _context = context;
            _participantService = participantsService;
        }

        // GET: CoParticipantDemographics
        public async Task<IActionResult> Index()
        {
            var udsContext = _context.CoParticipantDemographics.Include(a => a.Visit);
            return View(await udsContext.ToListAsync());
        }

        // GET: CoParticipantDemographics/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var coParticipantDemographics = await _context.CoParticipantDemographics
                .Include("Visit")
                .FirstOrDefaultAsync(m => m.Id == id);

            if (coParticipantDemographics == null)
            {
                return NotFound();
            }

            return View(coParticipantDemographics);
        }

        // GET: CoParticipantDemographics/Create
        public async Task<IActionResult> Create(int id)
        {
            var visit = await _context.Visits.FindAsync(id);

            if (visit == null)
            {
                return NotFound();
            }

            var CoParticipantDemographics = await _context.CoParticipantDemographics.FindAsync(id);

            if (CoParticipantDemographics == null)
            {
                CoParticipantDemographics = new CoParticipantDemographics 
                {
                    Id = id,
                    FormStatus = FormStatus.Incomplete
                };

                _context.Add(CoParticipantDemographics);
                await _context.SaveChangesAsync(HttpContext.User.Identity.Name);
            }

            return RedirectToAction("Edit", new { id = CoParticipantDemographics.Id });
        }


        // POST: CoParticipantDemographics/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("BirthMonthUk,BirthDay,BirthYearUK,BirthMonth,BirthYear,Sex,HispanicLatinoEthnicity,EthnicOrigins,EthnicOriginsOther,Race,RaceOther,AdditionalRace,AdditionalRaceOther,YearsOfEducation,Relationship,YearsKnown,LivesWith,FrequencyOfVisit,FrequencyOfTele,Reliability,Id,ExaminerInitials,FormStatus")] CoParticipantDemographics coParticipantDemographics)
        {
            if (ModelState.IsValid)
            {
                _context.Add(coParticipantDemographics);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            
            return View(coParticipantDemographics);
        }

        // GET: CoParticipantDemographics/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var coParticipantDemographics = await _context.CoParticipantDemographics
                .Include("Visit")
                .Include(c => c.Visit)
                    .ThenInclude(v => v.Participant)
                .AsNoTracking()
                .FirstOrDefaultAsync(c => c.Id == id);

            if (coParticipantDemographics == null)
            {
                return NotFound();
            }
            else
            {
                var participantIdentity = await _participantService.GetParticipantAsync(coParticipantDemographics.Visit.Participant.Id);
                coParticipantDemographics.Visit.Participant.Profile = participantIdentity;

                if(coParticipantDemographics.Visit.VisitType == VisitType.FVP || coParticipantDemographics.Visit.VisitType == VisitType.TFP)
                {
                    return View("EditFVP", coParticipantDemographics);
                }
                return View(coParticipantDemographics);
            }
        }

        // POST: CoParticipantDemographics/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("BirthMonthUk,BirthDay,BirthYearUK,Name,ContactForResearch,BirthMonth,BirthYear,Sex,HispanicLatinoEthnicity,EthnicOrigins,EthnicOriginsOther,Race,RaceOther,SecondaryRace,SecondaryRaceOther,AdditionalRace,AdditionalRaceOther,YearsOfEducation,Relationship,YearsKnown,LivesWith,FrequencyOfVisit,FrequencyOfTele,Reliability,Id,ExaminerInitials,FormStatus,IsNewCoParticipant")] CoParticipantDemographics coParticipantDemographics, string save, string complete)
        {

            if (id != coParticipantDemographics.Id)
            {
                return NotFound();
            }

            var visit = await _context.Visits
                .Include("Participant")
                .AsNoTracking()
                .FirstOrDefaultAsync(v => v.Id == coParticipantDemographics.Id);

            coParticipantDemographics.Visit = visit;

            var participantIdentity = await _participantService.GetParticipantAsync(coParticipantDemographics.Visit.Participant.Id);
            coParticipantDemographics.Visit.Participant.Profile = participantIdentity;

            if (visit == null)
            {
                return NotFound();
            }

            coParticipantDemographics.Visit = visit;

            var viewToReturn = "Edit";

            if (coParticipantDemographics.Visit.VisitType == VisitType.FVP)
            {
                viewToReturn = "EditFVP";
            }


            if (!String.IsNullOrEmpty(save))
            {
                coParticipantDemographics.FormStatus = FormStatus.Incomplete;
            }
            else if (!String.IsNullOrEmpty(complete))
            {
                coParticipantDemographics.FormStatus = FormStatus.Complete;
                if (!TryValidateModel(coParticipantDemographics))
                {
                    return View(viewToReturn, coParticipantDemographics);
                }
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(coParticipantDemographics);
                    await _context.SaveChangesAsync(HttpContext.User.Identity.Name);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CoParticipantDemographicsExists(coParticipantDemographics.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction("Details", "Visit", new { id = coParticipantDemographics.Id });
            }

            return View(viewToReturn, coParticipantDemographics);
        }

        // GET: CoParticipantDemographics/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var coParticipantDemographics = await _context.CoParticipantDemographics
                .Include(a => a.Visit)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (coParticipantDemographics == null)
            {
                return NotFound();
            }

            return View(coParticipantDemographics);
        }

        // POST: CoParticipantDemographics/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var coParticipantDemographics = await _context.CoParticipantDemographics.FindAsync(id);
            _context.CoParticipantDemographics.Remove(coParticipantDemographics);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CoParticipantDemographicsExists(int id)
        {
            return _context.CoParticipantDemographics.Any(e => e.Id == id);
        }
    }
}
