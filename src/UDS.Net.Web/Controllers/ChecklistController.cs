using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using UDS.Net.Data;
using UDS.Net.Data.Entities;
using UDS.Net.Data.Enums;
using UDS.Net.Web.Services;
using UDS.Net.Web.ViewModels;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace UDS.Net.Web.Controllers
{
    public class ChecklistController : Controller
    {
        private readonly UdsContext _context;
        private readonly IVisitService _visitService;
        private readonly IParticipantsService _participantsService;

        public ChecklistController(UdsContext context, IVisitService visitService, IParticipantsService participantsService)
        {
            _context = context;
            _visitService = visitService;
            _participantsService = participantsService;
        }

        // GET: /<controller>/
        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            return View();
        }

        public async Task<IActionResult> Create(int id)
        {
            var visit = await _context.Visits.FindAsync(id);

            if (visit == null)
            {
                return NotFound();
            }

            var checklist = await _context.Checklists.FindAsync(id);

            if (checklist == null)
            {
                // create a new checklist
                checklist = new Checklist
                {
                    Id = id,
                    FormStatus = FormStatus.Incomplete
                };
                _context.Checklists.Add(checklist);
                await _context.SaveChangesAsync(HttpContext.User.Identity.Name);
            }

            return RedirectToAction("Edit", new { id = checklist.Id });
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var checklist = await _context.Checklists
                .Include(c => c.Visit)
                .ThenInclude(c => c.Participant)
                .AsNoTracking()
                .FirstOrDefaultAsync(c => c.Id == id.Value);

            var participantIdentity = await _participantsService.GetParticipantAsync(checklist.Visit.Participant.Id);
            checklist.Visit.Participant.Profile = participantIdentity;

            var visitBase = await _visitService.GetVisitWithParticipantAndFormBases(id.Value);

            var vm = new VisitCompletionViewModel
            {
                Checklist = checklist,
                Visit = visitBase
            };
            
            return View(vm); 
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Checklist checklist, string save, string complete)
        {
            if (id != checklist.Id)
            {
                return NotFound();
            }

            var visit = await _context.Visits
                .AsNoTracking()
                .Include("Participant")
                .FirstOrDefaultAsync(v => v.Id == checklist.Id);

            checklist.Visit = visit;

            var participantIdentity = await _participantsService.GetParticipantAsync(checklist.Visit.Participant.Id);
            checklist.Visit.Participant.Profile = participantIdentity;

            var visitBase = await _visitService.GetVisitWithParticipantAndFormBases(id);

            var vm = new VisitCompletionViewModel
            {
                Checklist = checklist,
                Visit = visitBase
            };

            if (!String.IsNullOrEmpty(save))
            {
                checklist.FormStatus = FormStatus.Incomplete;
            }
            else if (!String.IsNullOrEmpty(complete))
            {
                checklist.FormStatus = FormStatus.Complete;
                if (!TryValidateModel(vm))
                {
                    return View(vm);
                }
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(checklist);
                    await _context.SaveChangesAsync(HttpContext.User.Identity.Name);
                }
                catch (DbUpdateConcurrencyException)
                {
                    throw;
                }
                return RedirectToAction("Details", "Visit", new { id = checklist.Id });
            }

            return View(vm);
        }
    }
}
