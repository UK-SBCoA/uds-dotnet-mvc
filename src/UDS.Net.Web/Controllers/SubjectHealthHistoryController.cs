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
    public class SubjectHealthHistoryController : PacketFormController
    {   
        private ProtocolVariable[] _protocolVariables;
        private ProtocolVariable _basicResponse;
        private ProtocolVariable _smokingFrequency;
        private ProtocolVariable _alchoholConsuption;
        private ProtocolVariable _conditionPresence;
        private ProtocolVariable _severity;
        private ProtocolVariable _diabeteis;
        private ProtocolVariable _arthritis;

        public SubjectHealthHistoryController(UdsContext context, IParticipantsService participantsService, IChecklistService checklistService) : base(context, participantsService, checklistService)
        {
            string jsonString = System.IO.File.ReadAllText("App_Data/HealthHistoryReference.json");
            _protocolVariables = JsonSerializer.Deserialize<ProtocolVariable[]>(jsonString);
            _basicResponse = _protocolVariables.Where(p => p.Name == "BASIC").FirstOrDefault();
            _smokingFrequency = _protocolVariables.Where(sf => sf.Name == "SMOKYRS").FirstOrDefault();
            _alchoholConsuption = _protocolVariables.Where(sf => sf.Name == "ALCFREQ").FirstOrDefault();
            _conditionPresence = _protocolVariables.Where(sf => sf.Name == "ConditionPresence").FirstOrDefault();
            _severity = _protocolVariables.Where(sev =>  sev.Name == "TBI_Severity").FirstOrDefault();
            _diabeteis = _protocolVariables.Where(sev =>  sev.Name == "Diabetes").FirstOrDefault();
            _arthritis = _protocolVariables.Where(sev =>  sev.Name == "Arthritis").FirstOrDefault();
        }

        // GET: SubjectHealthHistory
        public async Task<IActionResult> Index()
        {
            var udsContext = _context.SubjectHealthHistories.Include(s => s.Visit);
            return View(await udsContext.ToListAsync());
        }

        // GET: SubjectHealthHistory/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var subjectHealthHistory = await _context.SubjectHealthHistories
                .Include(s => s.Visit)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (subjectHealthHistory == null)
            {
                return NotFound();
            }

            return View(subjectHealthHistory);
        }

        // GET: SubjectHealthHistory/Create
        public async Task<IActionResult> Create(int id)
        {
            var subjectHealthHistory = await _context.SubjectHealthHistories.FindAsync(id);
            if (subjectHealthHistory == null)
            {
                subjectHealthHistory = new SubjectHealthHistory {
                    Id = id,
                    FormStatus = FormStatus.Incomplete
                };
                _context.SubjectHealthHistories.Add(subjectHealthHistory);
                await _context.SaveChangesAsync(HttpContext.User.Identity.Name);
            }
            return RedirectToAction("Edit", new { id = subjectHealthHistory.Id });
        }

        // POST: SubjectHealthHistory/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(int id, SubjectHealthHistory subjectHealthHistory)
        {
            if (ModelState.IsValid)
            {
                _context.Add(subjectHealthHistory);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["Id"] = subjectHealthHistory.Id;

            return View(subjectHealthHistory);
        }

        // GET: SubjectHealthHistory/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            ViewBag.BasicResponse = _basicResponse;
            ViewBag.SmokingYears = _smokingFrequency;
            ViewBag.ConditionPresence = _conditionPresence;
            ViewBag.AlcoholFrequency = _alchoholConsuption;
            ViewBag.Severity = _severity;
            ViewBag.Arthritis = _arthritis;
            ViewBag.Diabetes = _diabeteis;
            
            var subjectHealthHistory = await _context.SubjectHealthHistories
                .Include(c => c.Visit)
                    .ThenInclude(v => v.Participant)
                .AsNoTracking()
                .FirstOrDefaultAsync(c => c.Id == id);

            if (subjectHealthHistory == null)
            {
                return NotFound();
            }
            else if (!FormCanBeEdited(subjectHealthHistory.Visit.Status))
            {
                return View("Details", subjectHealthHistory);
            }

            var participantIdentity = await _participantsService.GetParticipantAsync(subjectHealthHistory.Visit.Participant.Id);
            subjectHealthHistory.Visit.Participant.Profile = participantIdentity;
            return View(subjectHealthHistory);
        }

        // POST: SubjectHealthHistory/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, SubjectHealthHistory subjectHealthHistory, string save, string complete)
        {
            if (id != subjectHealthHistory.Id)
            {
                return NotFound();
            }
            ViewBag.BasicResponse = _basicResponse;
            ViewBag.SmokingYears = _smokingFrequency;
            ViewBag.ConditionPresence = _conditionPresence;
            ViewBag.AlcoholFrequency = _alchoholConsuption;
            ViewBag.Severity = _severity;
            ViewBag.Arthritis = _arthritis;
            ViewBag.Diabetes = _diabeteis;

            var visit = await _context.Visits
                .AsNoTracking()
                .Include("Participant")
                .FirstOrDefaultAsync(v => v.Id == subjectHealthHistory.Id);

            if (!FormCanBeEdited(visit.Status))
            {
                ModelState.AddModelError("FormStatus", "Form cannot be modified because packet is complete.");
                return View(subjectHealthHistory);
            }

            subjectHealthHistory.Visit = visit;

            var participantIdentity = await _participantsService.GetParticipantAsync(subjectHealthHistory.Visit.Participant.Id);
            subjectHealthHistory.Visit.Participant.Profile = participantIdentity;

            if (!String.IsNullOrEmpty(save))
            {
                subjectHealthHistory.FormStatus = FormStatus.Incomplete;
            }
            else if (!String.IsNullOrEmpty(complete))
            {
                subjectHealthHistory.FormStatus = FormStatus.Complete;
                if (!TryValidateModel(subjectHealthHistory))
                {
                    var error = ModelState.Values.SelectMany(v => v.Errors).ToList();
                    return View(subjectHealthHistory);
                }
            }
            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(subjectHealthHistory);
                    await _context.SaveChangesAsync(HttpContext.User.Identity.Name);
                    await _checklistService.ValidateAndUpdateChecklistStatus(visit, typeof(SubjectHealthHistory));
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!SubjectHealthHistoryExists(subjectHealthHistory.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction("Details", "Visit", new { id = subjectHealthHistory.Id });
            }
            return View(subjectHealthHistory);
        }

        // GET: SubjectHealthHistory/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var subjectHealthHistory = await _context.SubjectHealthHistories
                .Include(s => s.Visit)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (subjectHealthHistory == null)
            {
                return NotFound();
            }

            return View(subjectHealthHistory);
        }

        // POST: SubjectHealthHistory/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var subjectHealthHistory = await _context.SubjectHealthHistories.FindAsync(id);
            _context.SubjectHealthHistories.Remove(subjectHealthHistory);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool SubjectHealthHistoryExists(int id)
        {
            return _context.SubjectHealthHistories.Any(e => e.Id == id);
        }
    }
}
