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
    public class UnifiedParkinsonsDiseaseRatingScaleController : PacketFormController
    {
        private ProtocolVariable[] _protocolVariables;
        private ProtocolVariable _speech;
        private ProtocolVariable _facialExpression;
        private ProtocolVariable _tremorAtRest;
        private ProtocolVariable _actionOrPosturalTremorOfHands;
        private ProtocolVariable _rigidity;
        private ProtocolVariable _fingerTaps;
        private ProtocolVariable _handRapidLeg;
        private ProtocolVariable _arisingFromChair;
        private ProtocolVariable _posture;
        private ProtocolVariable _gait;
        private ProtocolVariable _postureStability;
        private ProtocolVariable _bodyBradykinesia;
        private ProtocolVariable _chorea;
        private ProtocolVariable _myoclonusRating;
        private ProtocolVariable _otherDyskinesias;
        private ProtocolVariable _receivedDrug;
        private ProtocolVariable _antiparkinsonianDrugs;

        public UnifiedParkinsonsDiseaseRatingScaleController(UdsContext context, IParticipantsService participantsService, IChecklistService checklistService) : base(context, participantsService, checklistService)
        {
            string jsonString = System.IO.File.ReadAllText("App_Data/UPDRSVariableCodes.json");
            _protocolVariables = JsonSerializer.Deserialize<ProtocolVariable[]>(jsonString);
            _speech = _protocolVariables.Where(pr => pr.Name == "Speech").Single();
            _facialExpression = _protocolVariables.Where(fa => fa.Name == "FacialExpression").Single();
            _tremorAtRest = _protocolVariables.Where(tar => tar.Name == "TremorAtRest").Single();
            _actionOrPosturalTremorOfHands = _protocolVariables.Where(act => act.Name == "ActionOrPosturalTremorOfHands").Single();
            _rigidity = _protocolVariables.Where(act => act.Name == "Rigidity").Single();
            _fingerTaps = _protocolVariables.Where(act => act.Name == "FingerTaps").Single();
            _handRapidLeg = _protocolVariables.Where(act => act.Name == "HandRapidLeg").Single();
            _arisingFromChair = _protocolVariables.Where(chair => chair.Name == "ArisingFromChair").Single();
            _posture = _protocolVariables.Where(chair => chair.Name == "Posture").Single();
            _gait = _protocolVariables.Where(g => g.Name == "Gait").Single();
            _postureStability = _protocolVariables.Where(p => p.Name == "PostureStability").Single();
            _bodyBradykinesia = _protocolVariables.Where(p => p.Name == "BodyBradykinesiaAndHypokinesia").Single();
            _chorea =  _protocolVariables.Where(p => p.Name == "Chorea").Single();
            _myoclonusRating = _protocolVariables.Where(p => p.Name == "MyoclonusRating").Single();
            _otherDyskinesias = _protocolVariables.Where(p => p.Name == "OtherDyskinesias").Single();
            _receivedDrug = _protocolVariables.Where(p => p.Name == "ReceivedDrug").Single();
            _antiparkinsonianDrugs = _protocolVariables.Where(p => p.Name == "ReceivedDrug").Single();

        }

        // GET: UnifiedParkinsonsDiseaseRatingScale
        public async Task<IActionResult> Index()
        {
            var udsContext = _context.UnifiedParkinsonsDiseaseRatingScale.Include(u => u.Visit);
            return View(await udsContext.ToListAsync());
        }

        // GET: UnifiedParkinsonsDiseaseRatingScale/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var unifiedParkinsonsDiseaseRatingScale = await _context.UnifiedParkinsonsDiseaseRatingScale
                .Include(u => u.Visit)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (unifiedParkinsonsDiseaseRatingScale == null)
            {
                return NotFound();
            }

            return View(unifiedParkinsonsDiseaseRatingScale);
        }

        // GET: UnifiedParkinsonsDiseaseRatingScale/Create
        public async Task<IActionResult> Create(int id)
        {
            var unifiedParkinsonsDiseaseRatingScale = await _context.UnifiedParkinsonsDiseaseRatingScale.FindAsync(id);

            if (unifiedParkinsonsDiseaseRatingScale == null)
            {
                unifiedParkinsonsDiseaseRatingScale = new UnifiedParkinsonsDiseaseRatingScale
                {
                    Id = id,
                    FormStatus = FormStatus.Incomplete
                };

                _context.Add(unifiedParkinsonsDiseaseRatingScale);
                await _context.SaveChangesAsync(HttpContext.User.Identity.Name);
            }
            return RedirectToAction("Edit", new { id = unifiedParkinsonsDiseaseRatingScale.Id });
        }

        // GET: UnifiedParkinsonsDiseaseRatingScale/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            ViewBag.Speech = _speech;
            ViewBag.FacialExpression = _facialExpression;
            ViewBag.TremorAtRest = _tremorAtRest;
            ViewBag.ActionOrPostural = _actionOrPosturalTremorOfHands;
            ViewBag.Rigidity = _rigidity;
            ViewBag.FingerTaps = _fingerTaps;
            ViewBag.HandRapidLeg = _handRapidLeg;
            ViewBag.ArisingFromChair = _arisingFromChair;
            ViewBag.Posture = _posture;
            ViewBag.Gait = _gait;
            ViewBag.PostureStability = _postureStability;
            ViewBag.BodyBradykinesia = _bodyBradykinesia;
            ViewBag.Chorea = _chorea;
            ViewBag.MyclounusRating = _myoclonusRating;
            ViewBag.OtherDyskinesias = _otherDyskinesias;
            ViewBag.ReceivedDrug = _receivedDrug;
            ViewBag.AntiparkinsonianDrugs = _antiparkinsonianDrugs;

            var unifiedParkinsonsDiseaseRatingScale = await _context.UnifiedParkinsonsDiseaseRatingScale
                .Include(c => c.Visit)
                    .ThenInclude(v => v.Participant)
                .AsNoTracking()
                .FirstOrDefaultAsync(c => c.Id == id);

            if (unifiedParkinsonsDiseaseRatingScale == null)
            {
                return NotFound();
            }
            else if (!FormCanBeEdited(unifiedParkinsonsDiseaseRatingScale.Visit.Status))
            {
                return View("Details", unifiedParkinsonsDiseaseRatingScale);
            }

            var participantIdentity = await _participantsService.GetParticipantAsync(unifiedParkinsonsDiseaseRatingScale.Visit.Participant.Id);
            unifiedParkinsonsDiseaseRatingScale.Visit.Participant.Profile = participantIdentity;

            if (unifiedParkinsonsDiseaseRatingScale.Visit.VisitType == VisitType.FVP || unifiedParkinsonsDiseaseRatingScale.Visit.VisitType == VisitType.TFP)
            {
                return View("Edit", unifiedParkinsonsDiseaseRatingScale);
            }
            return View(unifiedParkinsonsDiseaseRatingScale);
        }

        // POST: UnifiedParkinsonsDiseaseRatingScale/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, UnifiedParkinsonsDiseaseRatingScale unifiedParkinsonsDiseaseRatingScale, string save, string complete)
        {
            if (id != unifiedParkinsonsDiseaseRatingScale.Id)
            {
                return NotFound();
            }

            ViewBag.Speech = _speech;
            ViewBag.FacialExpression = _facialExpression;
            ViewBag.TremorAtRest = _tremorAtRest;
            ViewBag.ActionOrPostural = _actionOrPosturalTremorOfHands;
            ViewBag.Rigidity = _rigidity;
            ViewBag.FingerTaps = _fingerTaps;
            ViewBag.HandRapidLeg = _handRapidLeg;
            ViewBag.ArisingFromChair = _arisingFromChair;
            ViewBag.Posture = _posture;
            ViewBag.Gait = _gait;
            ViewBag.PostureStability = _postureStability;
            ViewBag.BodyBradykinesia = _bodyBradykinesia;
            ViewBag.Chorea = _chorea;
            ViewBag.MyclounusRating = _myoclonusRating;
            ViewBag.OtherDyskinesias = _otherDyskinesias;
            ViewBag.ReceivedDrug = _receivedDrug;
            ViewBag.AntiparkinsonianDrugs = _antiparkinsonianDrugs;

            var visit = await _context.Visits
                .AsNoTracking()
                .Include("Participant")
                .FirstOrDefaultAsync(v => v.Id == unifiedParkinsonsDiseaseRatingScale.Id);

            if (!FormCanBeEdited(visit.Status))
            {
                ModelState.AddModelError("FormStatus", "Form cannot be modified because packet is complete.");
                return View(unifiedParkinsonsDiseaseRatingScale);
            }

            unifiedParkinsonsDiseaseRatingScale.Visit = visit;
            var participantIdentity = await _participantsService.GetParticipantAsync(unifiedParkinsonsDiseaseRatingScale.Visit.Participant.Id);
            unifiedParkinsonsDiseaseRatingScale.Visit.Participant.Profile = participantIdentity;

            if (!String.IsNullOrEmpty(save))
            {
                unifiedParkinsonsDiseaseRatingScale.FormStatus = FormStatus.Incomplete;
            }
            else if (!String.IsNullOrEmpty(complete))
            {
                unifiedParkinsonsDiseaseRatingScale.FormStatus = FormStatus.Complete;
                if (!TryValidateModel(unifiedParkinsonsDiseaseRatingScale))
                {
                    return View(unifiedParkinsonsDiseaseRatingScale);
                }
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(unifiedParkinsonsDiseaseRatingScale);
                    await _context.SaveChangesAsync(HttpContext.User.Identity.Name);
                    await _checklistService.ValidateAndUpdateChecklistStatus(visit, typeof(UnifiedParkinsonsDiseaseRatingScale));
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!UnifiedParkinsonsDiseaseRatingScaleExists(unifiedParkinsonsDiseaseRatingScale.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction("Details", "Visit", new { id = unifiedParkinsonsDiseaseRatingScale.Id });
            }

            return View(unifiedParkinsonsDiseaseRatingScale);
        }

        // GET: UnifiedParkinsonsDiseaseRatingScale/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var unifiedParkinsonsDiseaseRatingScale = await _context.UnifiedParkinsonsDiseaseRatingScale
                .Include(u => u.Visit)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (unifiedParkinsonsDiseaseRatingScale == null)
            {
                return NotFound();
            }

            return View(unifiedParkinsonsDiseaseRatingScale);
        }

        // POST: UnifiedParkinsonsDiseaseRatingScale/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var unifiedParkinsonsDiseaseRatingScale = await _context.UnifiedParkinsonsDiseaseRatingScale.FindAsync(id);
            _context.UnifiedParkinsonsDiseaseRatingScale.Remove(unifiedParkinsonsDiseaseRatingScale);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool UnifiedParkinsonsDiseaseRatingScaleExists(int id)
        {
            return _context.UnifiedParkinsonsDiseaseRatingScale.Any(e => e.Id == id);
        }
    }
}
