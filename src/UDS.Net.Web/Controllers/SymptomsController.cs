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
using UDS.Net.Data.Dtos;

namespace UDS.Net.Web.Controllers
{
    public class SymptomsController : Controller
    {
        private readonly UdsContext _context;
        private readonly IParticipantsService _participantsService;
        private List<NonMatchingValueDto> _nonMatchingValues = new List<NonMatchingValueDto>();

        public SymptomsController(UdsContext context, IParticipantsService participantsService)
        {
            _context = context;
            _participantsService = participantsService;
        }

        // GET: Symptoms
        public async Task<IActionResult> Index()
        {
            var udsContext = _context.Symptoms.Include(n => n.Visit);
            return View(await udsContext.ToListAsync());
        }

        // GET: Symptoms/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var Symptoms = await _context.Symptoms
                .Include(n => n.Visit)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (Symptoms == null)
            {
                return NotFound();
            }

            return View(Symptoms);
        }

        // GET: Symptoms/Create
        public async Task<IActionResult> Create(int id)
        {
            var Symptoms = await _context.Symptoms.FindAsync(id);

            if (Symptoms == null)
            {
                Symptoms = new Symptoms();
                Symptoms.InitializeForm(id);
                _context.Add(Symptoms);
                await _context.SaveChangesAsync(HttpContext.User.Identity.Name);
            }

            return RedirectToAction("Edit", new { id = Symptoms.Id });
        }

        // POST: Symptoms/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("SubjectMemory,CoParticipantMemory,CognitionImpairment,Memory,Orientation,ExecutiveFunction,Language,VisuospatialFunction,AttentionCconcentration,FluctuatingCognition,FluctuatingCognitionAge,ImpairedOther,ImpairedOtherSpecify,PredominantSymptom,PredominantSymptomOther,CognitiveSymptoms,CognitiveSymptomsOther,AgeOfDecline,BehavioralSymptoms,ApathyWithdrawal,DepressedMood,VisualHallucinations,DetailedHallucinations,HallucinationsAge,AuditoryHallucinations,AbnormalBeliefs,Disinhibition,Irritability,Agitation,PersonalityChange,RemDisorder,RemDisorderAge,Anxiety,ChangeInBehavior,ChangeInBehaviorOther,BehaviorDecline,BehaviorDeclineOther,OnsetBehavioral,OnsetBehavioralOther,BehaviorSymptomsAge,MotorSymptoms,GaitDisorder,Falls,Tremor,Slowness,PredominantMotorDecline,ModeOfMotorSymptoms,ModeOfMotorSymptomsOther,SuggestiveOfParkinsonism,SuggestiveOfParkinsonismAge,SuggestiveOfSclerosis,SuggestiveOfSclerosisAge,AssessmentOfSclerosisAge,OverallDecline,PredominantDomain,LewyBodyDisease,FrontotemporalLobarDegeneration,")] Symptoms Symptoms)
        {
            if (ModelState.IsValid)
            {
                _context.Add(Symptoms);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }


            return View(Symptoms);
        }
        [HttpGet]
        // GET: Symptoms/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var currentForm =
            await _context.Symptoms
            .Include(c => c.Visit)
            .ThenInclude(c => c.Participant)
            .AsNoTracking()
            .FirstOrDefaultAsync(c => c.Id == id);
            if (currentForm == null)
            {
                return NotFound();
            }
            var participantIdentity = await _participantsService.GetParticipantAsync(currentForm.Visit.Participant.Id);
            currentForm.Visit.Participant.Profile = participantIdentity;

            var previousForm = GetPreviousForm(currentForm.Visit.VisitNumber, currentForm.Visit.Participant.Id);

            CheckValue(previousForm, currentForm, "AgeOfDecline", 777);
            CheckValue(previousForm, currentForm, "HallucinationsAge", 777);
            CheckValue(previousForm, currentForm, "BehaviorDecline", 0);
            CheckValue(previousForm, currentForm, "BehaviorSymptomsAge", 777);
            CheckValue(previousForm, currentForm, "PredominantMotorDecline", 0);
            CheckValue(previousForm, currentForm, "SuggestiveOfParkinsonismAge", 777);
            CheckValue(previousForm, currentForm, "SuggestiveOfSclerosisAge", 777);
            CheckValue(previousForm, currentForm, "AssessmentOfSclerosisAge", 777);
            CheckValue(previousForm, currentForm, "PredominantDomain", 0);

            ViewBag.NonmatchingValues = _nonMatchingValues;
            ViewBag.VisitType = currentForm.Visit.VisitType;

            return View(currentForm);
        }

        // POST: Symptoms/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Symptoms Symptoms, string save, string complete)
        {
            if (id != Symptoms.Id)
            {
                return NotFound();
            }
            var visit = await _context.Visits
                .AsNoTracking()
                .Include("Participant")
                .FirstOrDefaultAsync(v => v.Id == Symptoms.Id);
            ViewBag.NonmatchingValues = _nonMatchingValues;
            ViewBag.VisitType = visit.VisitType;
            Symptoms.Visit = visit;
            var participantIdentity = await _participantsService.GetParticipantAsync(Symptoms.Visit.Participant.Id);
            Symptoms.Visit.Participant.Profile = participantIdentity;
            if (!String.IsNullOrEmpty(save))
            {
                Symptoms.FormStatus = FormStatus.Incomplete;
            }
            else if (!String.IsNullOrEmpty(complete))
            {
                Symptoms.FormStatus = FormStatus.Complete;
                if (!TryValidateModel(Symptoms))
                {
                    return View(Symptoms);
                }
            }
            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(Symptoms);
                    await _context.SaveChangesAsync(HttpContext.User.Identity.Name);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!SymptomsExists(Symptoms.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction("Details", "Visit", new { id = Symptoms.Id });
            }
            return View(Symptoms);
        }

        // GET: Symptoms/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var Symptoms = await _context.Symptoms
                .Include(n => n.Visit)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (Symptoms == null)
            {
                return NotFound();
            }

            return View(Symptoms);
        }

        // POST: Symptoms/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var Symptoms = await _context.Symptoms.FindAsync(id);
            _context.Symptoms.Remove(Symptoms);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool SymptomsExists(int id)
        {
            return _context.Symptoms.Any(e => e.Id == id);
        }

        private Symptoms GetPreviousForm(int visitNumber, int currentParticipantId)
        {
            var previousForms = GetAllPreviousForms(currentParticipantId);

            var currentFormIndex = previousForms.FindIndex(c => c.Visit.VisitNumber == visitNumber);
            var previousFormIndex = currentFormIndex - 1;
            

            if(previousFormIndex >= 0)
            {
                return previousForms[previousFormIndex];
            }

            return null;
        }

        private List<Symptoms> GetAllPreviousForms(int currentParticipantId)
        {
            var previousForms = _context.Symptoms
                .Include(c => c.Visit)
                .ThenInclude(c => c.Participant)
                .Where(c => c.Visit.Participant.Id == currentParticipantId)
                .OrderBy(c => c.Visit.VisitNumber)
                .AsNoTracking()
                .ToList();

            return previousForms;
        }

        private string GetPreviousValueText(string target, int currentVisitId, int currentParticipantId)
        {
            string previousValuesText = null;

            var previousForms = GetAllPreviousForms(currentParticipantId);

            foreach (Symptoms form in previousForms)
            {
                if(form != null)
                {
                    var previousValue = form.GetType().GetProperty(target).GetValue(form, null);
                    var previousVisitNumber = form.Visit.VisitNumber;

                    previousValuesText += $"\nvist #{previousVisitNumber}: {previousValue}";

                    if(currentVisitId == form.Visit.VisitNumber)
                    {
                        previousValuesText += " (current)";
                    }
                }
            }

            return previousValuesText;
        }

        private void CheckValue(Symptoms previousForm, Symptoms currentForm, string target, int autoFillValue)
        {
            if (previousForm != null)
            {
                var currentValue = GetCurrentValue(currentForm, target);
                var previousValue = GetPreviousValue(previousForm, target);

                if (currentValue == null && previousValue != null)
                {
                    SetCurrentValue(currentForm, target, autoFillValue);
                }
                if (currentValue != previousValue && previousValue != null && currentValue != null)
                {
                    if (!Equals(currentValue, autoFillValue))
                    {
                        CreateNonMatchMsg(target, autoFillValue, currentForm.Visit.VisitNumber, currentForm.Visit.Participant.Id);
                    }
                }
            }
        }

        private static object GetPreviousValue(Symptoms previousForm, string target)
        {
            return previousForm.GetType().GetProperty(target).GetValue(previousForm, null);
        }

        private static object GetCurrentValue(Symptoms currentForm, string target)
        {
            return currentForm.GetType().GetProperty(target).GetValue(currentForm, null);
        }

        private static void SetCurrentValue(Symptoms currentForm, string target, int autoFillvalue)
        {
            currentForm.GetType().GetProperty(target).SetValue(currentForm, autoFillvalue);
        }

        private void CreateNonMatchMsg(string target, int autoFillValue, int currentVisitId, int currentParticipantId)
        {
            var previousValuesText = "The previously recorded values are:\n" + GetPreviousValueText(target, currentVisitId, currentParticipantId) + "\n\nplease input " + autoFillValue;

            var message = new NonMatchingValueDto
            {
                Name = target,
                Text = previousValuesText
            };

            _nonMatchingValues.Add(message);
        }
    }
}