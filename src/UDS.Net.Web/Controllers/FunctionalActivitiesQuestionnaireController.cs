using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;
using Microsoft.Identity.Web;
using UDS.Net.Data;
using UDS.Net.Data.Entities;
using UDS.Net.Data.Enums;
using UDS.Net.Web.ViewModels;
using UDS.Net.Web.Services;

namespace UDS.Net.Web.Controllers
{
    public class FunctionalActivitiesQuestionnaireController : Controller
    {
        private ProtocolVariable[] _protocolVariables;
        private ProtocolVariable _basicResponse;
        private readonly UdsContext _context;
        private readonly IParticipantsService _participantsService;

        public FunctionalActivitiesQuestionnaireController(UdsContext context, IParticipantsService participantsService)
        {
            _context = context;
            _participantsService = participantsService;
            string jsonString = System.IO.File.ReadAllText("App_Data/FunctionalActivitiesVariableCodes.json");
            _protocolVariables = JsonSerializer.Deserialize<ProtocolVariable[]>(jsonString);
            _basicResponse = _protocolVariables.Where(x => x.Name == "BasicResponse").Single();
        }

        // GET: FunctionalActivitiesQuestionnaire
        public async Task<IActionResult> Index()
        {
            var udsContext = _context.FunctionalActivitiesQuestionnaires.Include(f => f.Visit);
            return View(await udsContext.ToListAsync());
        }

        // GET: FunctionalActivitiesQuestionnaire/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var functionalActivitiesQuestionnaire = await _context.FunctionalActivitiesQuestionnaires
                .Include(f => f.Visit)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (functionalActivitiesQuestionnaire == null)
            {
                return NotFound();
            }

            return View(functionalActivitiesQuestionnaire);
        }

        // GET: FunctionalActivitiesQuestionnaire/Create
        public async Task<IActionResult> Create(int id)
        {
            var functionalActivitiesQuestionnaires = await _context.FunctionalActivitiesQuestionnaires.FindAsync(id);

            if(functionalActivitiesQuestionnaires  == null)
            {
                functionalActivitiesQuestionnaires = new FunctionalActivitiesQuestionnaire
                {
                    Id = id,
                    FormStatus = FormStatus.Incomplete
                };

                _context.Add(functionalActivitiesQuestionnaires);
                await _context.SaveChangesAsync(HttpContext.User.Identity.Name);
            }

            return RedirectToAction("Edit", new { id = functionalActivitiesQuestionnaires.Id });
        }

        // GET: FunctionalActivitiesQuestionnaire/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var functionalActivitiesQuestionnaire = await _context.FunctionalActivitiesQuestionnaires
                .Include(c => c.Visit)
                    .ThenInclude(v => v.Participant)
                .AsNoTracking()
                .FirstOrDefaultAsync(c => c.Id == id);

            if (functionalActivitiesQuestionnaire == null)
            {
                return NotFound();
            }

            ViewBag.BasicResponse = _basicResponse;

            var participantIdentity = await _participantsService.GetParticipantAsync(functionalActivitiesQuestionnaire.Visit.Participant.Id);
            functionalActivitiesQuestionnaire.Visit.Participant.Profile = participantIdentity;

            return View(functionalActivitiesQuestionnaire);
        }

        // POST: FunctionalActivitiesQuestionnaire/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, FunctionalActivitiesQuestionnaire functionalActivitiesQuestionnaire,string save, string complete)
        {
            if (id != functionalActivitiesQuestionnaire.Id)
            {
                return NotFound();
            }

            var visit = await _context.Visits
                .AsNoTracking()
                .Include("Participant")
                .FirstOrDefaultAsync(v => v.Id == functionalActivitiesQuestionnaire.Id);

            functionalActivitiesQuestionnaire.Visit = visit;

            var participantIdentity = await _participantsService.GetParticipantAsync(functionalActivitiesQuestionnaire.Visit.Participant.Id);
            functionalActivitiesQuestionnaire.Visit.Participant.Profile = participantIdentity;

            ViewBag.BasicResponse = _basicResponse;

            if (!String.IsNullOrEmpty(save))
            {
                functionalActivitiesQuestionnaire.FormStatus = FormStatus.Incomplete;
            }
            else if (!String.IsNullOrEmpty(complete))
            {
                functionalActivitiesQuestionnaire.FormStatus = FormStatus.Complete;
                if (!TryValidateModel(functionalActivitiesQuestionnaire))
                {
                    return View(functionalActivitiesQuestionnaire);
                }
            }
            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(functionalActivitiesQuestionnaire);
                    await _context.SaveChangesAsync(HttpContext.User.Identity.Name);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!FunctionalActivitiesQuestionnaireExists(functionalActivitiesQuestionnaire.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction("Details", "Visit", new { id = functionalActivitiesQuestionnaire.Id });
            }

            return View(functionalActivitiesQuestionnaire);
        }

        // GET: FunctionalActivitiesQuestionnaire/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var functionalActivitiesQuestionnaire = await _context.FunctionalActivitiesQuestionnaires
                .Include(f => f.Visit)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (functionalActivitiesQuestionnaire == null)
            {
                return NotFound();
            }

            return View(functionalActivitiesQuestionnaire);
        }

        // POST: FunctionalActivitiesQuestionnaire/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var functionalActivitiesQuestionnaire = await _context.FunctionalActivitiesQuestionnaires.FindAsync(id);
            _context.FunctionalActivitiesQuestionnaires.Remove(functionalActivitiesQuestionnaire);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool FunctionalActivitiesQuestionnaireExists(int id)
        {
            return _context.FunctionalActivitiesQuestionnaires.Any(e => e.Id == id);
        }
    }
}
