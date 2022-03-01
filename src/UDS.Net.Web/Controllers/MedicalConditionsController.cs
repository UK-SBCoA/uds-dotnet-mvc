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
using UDS.Net.Web.ViewModels;
using UDS.Net.Web.Services;

namespace UDS.Net.Web.Controllers
{
    public class MedicalConditionsController : Controller
    {
        private readonly UdsContext _context;
        private readonly IParticipantsService _participantsService;
        private ProtocolVariable _findings;

        public MedicalConditionsController(UdsContext context, IParticipantsService participantsService)
        {
            _context = context;
            _participantsService = participantsService;

            _findings = new ProtocolVariable
            {
                Codes = new Dictionary<string, string>()
                {
                    { "0", "No" },
                    { "1", "Yes" },
                    { "8", "Unknown/ not assessed" }
                }
            };
        }

        // GET: MedicalConditions
        public async Task<IActionResult> Index()
        {
            var udsContext = _context.MedicalConditions.Include(m => m.Visit);
            return View(await udsContext.ToListAsync());
        }

        // GET: MedicalConditions/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var medicalConditions = await _context.MedicalConditions
                .Include(m => m.Visit)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (medicalConditions == null)
            {
                return NotFound();
            }

            return View(medicalConditions);
        }

        // GET: MedicalConditions/Create
        public async Task<IActionResult> Create(int id)
        {
            var medicalConditions = await _context.MedicalConditions.FindAsync(id);

            if (medicalConditions == null)
            {
                medicalConditions = new MedicalConditions
                {
                    Id = id,
                    FormStatus = FormStatus.Incomplete
                };
                _context.Add(medicalConditions);
                await _context.SaveChangesAsync(HttpContext.User.Identity.Name);
            }
            return RedirectToAction("Edit", new { id = medicalConditions.Id });
        }

        // POST: MedicalConditions/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Cancer,CancerSite,Diabetes,MyocardialInfarct,CongestiveHeartFailure,AtrialFibrillation,Hypertension,Angina,Hypercholesterolemia,B12Deficiency,ThyroidDisease,Arthritis,ArthritisType,ArthritisTypeSpecified,ArthritisRegionUpperExtremity,ArthritisRegionLowerExtremity,ArthritisRegionUnknown,IncontinenceUrinary,IncontinenceBowel,SleepApnea,REMSleepBehaviorDisorder,HyposomniaInsomnia,OtherSleepDisorder,OtherSleepDisorderSpecified,CarotidProcedure,PercutaneousCoronaryIntervention,PacemakerDefibrillator,HeartValveReplacementRepair,AntibodyMediatedEncephalopathy,AntibodyMediatedEncephalopathySpecified,OtherMedicalCondition,OtherMedicalConditionSpecified,Id,ExaminerInitials,FormStatus,Version")] MedicalConditions medicalConditions)
        {
            if (ModelState.IsValid)
            {
                _context.Add(medicalConditions);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["Id"] = new SelectList(_context.Visits, "Id", "Id", medicalConditions.Id);
            return View(medicalConditions);
        }

        // GET: MedicalConditions/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var medicalConditions = await _context.MedicalConditions
                .Include(c => c.Visit)
                    .ThenInclude(v => v.Participant)
                .AsNoTracking()
                .FirstOrDefaultAsync(c => c.Id == id);

            if (medicalConditions == null)
            {
                return NotFound();
            }

            var participantIdentity = await _participantsService.GetParticipantAsync(medicalConditions.Visit.Participant.Id);
            medicalConditions.Visit.Participant.Profile = participantIdentity;

            ViewBag.Findings = _findings;

            return View(medicalConditions);
        }

        // POST: MedicalConditions/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Cancer,CancerSite,Diabetes,MyocardialInfarct,CongestiveHeartFailure,AtrialFibrillation,Hypertension,Angina,Hypercholesterolemia,B12Deficiency,ThyroidDisease,Arthritis,ArthritisType,ArthritisTypeSpecified,ArthritisRegionUpperExtremity,ArthritisRegionLowerExtremity,ArthritisRegionSpine,ArthritisRegionUnknown,IncontinenceUrinary,IncontinenceBowel,SleepApnea,REMSleepBehaviorDisorder,HyposomniaInsomnia,OtherSleepDisorder,OtherSleepDisorderSpecified,CarotidProcedure,PercutaneousCoronaryIntervention,PacemakerDefibrillator,HeartValveReplacementRepair,AntibodyMediatedEncephalopathy,AntibodyMediatedEncephalopathySpecified,OtherMedicalCondition,OtherMedicalConditionSpecified,Id,ExaminerInitials,FormStatus,Version")] MedicalConditions medicalConditions, string save, string complete)
        {
            if (id != medicalConditions.Id)
            {
                return NotFound();
            }

            var visit = await _context.Visits
                .AsNoTracking()
                .Include("Participant")
                .FirstOrDefaultAsync(v => v.Id == medicalConditions.Id);

            medicalConditions.Visit = visit;

            var participantIdentity = await _participantsService.GetParticipantAsync(medicalConditions.Visit.Participant.Id);
            medicalConditions.Visit.Participant.Profile = participantIdentity;

            ViewBag.Findings = _findings;

            if (!String.IsNullOrEmpty(save))
            {
                medicalConditions.FormStatus = FormStatus.Incomplete;
            }
            else if (!String.IsNullOrEmpty(complete))
            {
                medicalConditions.FormStatus = FormStatus.Complete;
                if (!TryValidateModel(medicalConditions))
                {
                    return View(medicalConditions);
                }
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(medicalConditions);
                    await _context.SaveChangesAsync(HttpContext.User.Identity.Name);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!MedicalConditionsExists(medicalConditions.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction("Details", "Visit", new { id = medicalConditions.Id });
            }

            return View(medicalConditions);
        }

        // GET: MedicalConditions/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var medicalConditions = await _context.MedicalConditions
                .Include(m => m.Visit)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (medicalConditions == null)
            {
                return NotFound();
            }

            return View(medicalConditions);
        }

        // POST: MedicalConditions/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var medicalConditions = await _context.MedicalConditions.FindAsync(id);
            _context.MedicalConditions.Remove(medicalConditions);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool MedicalConditionsExists(int id)
        {
            return _context.MedicalConditions.Any(e => e.Id == id);
        }
    }
}
