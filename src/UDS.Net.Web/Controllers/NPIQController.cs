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
    public class NPIQController : PacketFormController
    {
        private ProtocolVariable _coParticipant;
        private ProtocolVariable _symptomPresent;
        private ProtocolVariable _symptomSeverity;
        private ProtocolVariable[] _protocolVariables;

        public NPIQController(UdsContext context, IParticipantsService participantsService, IChecklistService checklistService) : base(context, participantsService, checklistService)
        {
            string jsonString = System.IO.File.ReadAllText("App_Data/VariableCodesNames.json");
            _protocolVariables = JsonSerializer.Deserialize<ProtocolVariable[]>(jsonString);

            _coParticipant = _protocolVariables
                .Where(item => item.Name == "NPIQINF")
                .FirstOrDefault();

            _symptomPresent = _protocolVariables
                .Where(item => item.Name == "NPIQ_SYMPTOMS")
                .FirstOrDefault();

            _symptomSeverity = _protocolVariables
                .Where(item => item.Name == "NPIQ_SEVERITY")
                .FirstOrDefault();
        }

        // GET: NPIQ
        public async Task<IActionResult> Index()
        {
            var udsContext = _context.NPIQs.Include(n => n.Visit);
            return View(await udsContext.ToListAsync());
        }

        // GET: NPIQ/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var nPIQ = await _context.NPIQs
                .Include(n => n.Visit)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (nPIQ == null)
            {
                return NotFound();
            }

            return View(nPIQ);
        }

        // GET: NPIQ/Create
        public async Task<IActionResult> Create(int id)
        {
            var npiq = await _context.NPIQs.FindAsync(id);

            if(npiq  == null)
            {
                npiq = new NPIQ
                {
                    Id = id,
                    FormStatus = FormStatus.Incomplete
                };

                _context.Add(npiq);
                await _context.SaveChangesAsync(HttpContext.User.Identity.Name);
            }

            return RedirectToAction("Edit", new { id = npiq.Id });
        }

        // POST: NPIQ/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,CoParticipant,CoParticipantOther,Delusions,DelusionsSeverity,Hallucinations,HallucinationsSeverity,Agitation,AgitationSeverity,Anxiety,AnxietySeverity,Elation,ElationSeverity,Apathy,ApathySeverity,Disinhibition,DisinhibitionSeverity,Irritability,IrritabilitySeverity,MotorDisturbance,MotorDisturbanceSeverity,Nighttime,NighttimeSeverity,Appetite,AppetiteSeverity,Id,ExaminerInitials,FormStatus")] NPIQ nPIQ)
        {
            if (ModelState.IsValid)
            {
                _context.Add(nPIQ);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            
            return View(nPIQ);
        }

        // GET: NPIQ/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var nPIQ = await _context.NPIQs
                .Include(c => c.Visit)
                    .ThenInclude(v => v.Participant)
                .AsNoTracking()
                .FirstOrDefaultAsync(c => c.Id == id);

            if (nPIQ == null)
            {
                return NotFound();
            }
            else if (!FormCanBeEdited(nPIQ.Visit.Status))
            {
                return View("Details", nPIQ);
            }

            var participantIdentity = await _participantsService.GetParticipantAsync(nPIQ.Visit.Participant.Id);
            nPIQ.Visit.Participant.Profile = participantIdentity;

            ViewBag.CoParticipant = _coParticipant;
            ViewBag.SymptomPresent = _symptomPresent;
            ViewBag.SymptomSeverity = _symptomSeverity;

            return View(nPIQ);
        }

        // POST: NPIQ/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,CoParticipant,CoParticipantOther,Delusions,DelusionsSeverity,Hallucinations,HallucinationsSeverity,Agitation,AgitationSeverity,Anxiety,AnxietySeverity,Elation,ElationSeverity,Apathy,ApathySeverity,Depression,DepressionSeverity,Disinhibition,DisinhibitionSeverity,Irritability,IrritabilitySeverity,MotorDisturbance,MotorDisturbanceSeverity,Nighttime,NighttimeSeverity,Appetite,AppetiteSeverity,VisitId,ExaminerInitials,FormStatus")] NPIQ nPIQ, string save, string complete)
        {
            if (id != nPIQ.Id)
            {
                return NotFound();
            }

            var visit = await _context.Visits
                .AsNoTracking()
                .Include("Participant")
                .FirstOrDefaultAsync(v => v.Id == nPIQ.Id);

            if (!FormCanBeEdited(visit.Status))
            {
                ModelState.AddModelError("FormStatus", "Form cannot be modified because packet is complete.");
                return View(nPIQ);
            }

            nPIQ.Visit = visit;

            var participantIdentity = await _participantsService.GetParticipantAsync(nPIQ.Visit.Participant.Id);
            nPIQ.Visit.Participant.Profile = participantIdentity;


            ViewBag.CoParticipant = _coParticipant;
            ViewBag.SymptomPresent = _symptomPresent;
            ViewBag.SymptomSeverity = _symptomSeverity;

            if (!String.IsNullOrEmpty(save))
            {
                nPIQ.FormStatus = FormStatus.Incomplete;
            }
            else if (!String.IsNullOrEmpty(complete))
            {
                nPIQ.FormStatus = FormStatus.Complete;
                if (!TryValidateModel(nPIQ))
                {
                    return View(nPIQ);
                }
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(nPIQ);
                    await _context.SaveChangesAsync(HttpContext.User.Identity.Name);
                    await _checklistService.ValidateAndUpdateChecklistStatus(visit, typeof(NPIQ));
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!NPIQExists(nPIQ.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction("Details","Visit", new { id = nPIQ.Id });
            }
            
            return View(nPIQ);
        }

        // GET: NPIQ/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var nPIQ = await _context.NPIQs
                .Include(n => n.Visit)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (nPIQ == null)
            {
                return NotFound();
            }

            return View(nPIQ);
        }

        // POST: NPIQ/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var nPIQ = await _context.NPIQs.FindAsync(id);
            _context.NPIQs.Remove(nPIQ);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool NPIQExists(int id)
        {
            return _context.NPIQs.Any(e => e.Id == id);
        }
    }
}
