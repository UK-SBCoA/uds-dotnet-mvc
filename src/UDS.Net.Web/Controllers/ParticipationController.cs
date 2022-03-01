using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using UDS.Net.Data;
using UDS.Net.Data.Entities;
using UDS.Net.Data.Enums;
using UDS.Net.Web.ViewModels;
using UDS.Net.Web.Services;

namespace UDS.Net.Web.Controllers
{
    public class ParticipationController : Controller
    {
        private readonly UdsContext _context;
        private readonly IParticipantsService _participantsService;
        private readonly IUserPreferencesService _userPreferencesService;
        private readonly IMilestonesService _milestonesService;
        private readonly IConfiguration _config;

        public ParticipationController(IConfiguration config, UdsContext context, IParticipantsService participantsService, IUserPreferencesService userPreferencesService, IMilestonesService milestonesService)
        {
            _config = config;
            _context = context;
            _participantsService = participantsService;
            _userPreferencesService = userPreferencesService;
            _milestonesService = milestonesService;
        }

        // GET: Participation
        public async Task<IActionResult> Index()
        {
            return View(await _context.Participations.ToListAsync());
        }

        public async Task<IActionResult> Search(int? searchTerm)
        {
            if (searchTerm == null || !searchTerm.HasValue)
            {
                return View(null);
            }

            var examiners = await _context.Examiners
                .AsNoTracking()
                .ToListAsync();

            ViewBag.Examiners = examiners;

            var participation = await _context.Participations
                .Include(p => p.Visits)
                .Where(p => p.Id == searchTerm)
                .AsNoTracking()
                .Select(x => new Participation {
                    Id = x.Id,
                    Profile = x.Profile,
                    Status = x.Status,
                    Visits = x.Visits.OrderBy(v => v.VisitNumber)
                })
                .FirstOrDefaultAsync();


            var participantIdentity = await _participantsService.GetParticipantAsync(searchTerm.Value);

            // If participant does not yet have participation in UDS, but should
            if (participation == null && participantIdentity != null)
            {
                participation = new Participation
                {
                    Id = participantIdentity.Id,
                    Status = ParticipationStatus.Enrolled
                };

                _context.Add(participation);

                await _context.SaveChangesAsync(HttpContext.User.Identity.Name);
            }

            if (participation != null)
            {
                participation.Profile = participantIdentity;
            }

            var searchParticipant = new SearchParticipation
            {
                SearchTerm = searchTerm.Value,
                Result = participation
            };
            if (User.Identity.IsAuthenticated)
            {
                await _userPreferencesService.AddToParticipationSearchHistoryAsync(participation.Id, User.Identity.Name);
            }
            return View(searchParticipant);

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<JsonResult> Search([FromForm] SearchParticipation searchParticipant)
        {

            if (searchParticipant.SearchTerm > 0)
            {
                var participant = await _context.Participations.FindAsync(searchParticipant.SearchTerm);
                if (participant != null)
                {
                    return Json(participant);
                }
            }
            return Json(null);
        }

        // GET: Participation/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var examiners = await _context.Examiners
                .AsNoTracking()
                .ToListAsync();

            ViewBag.Examiners = examiners;
            ViewBag.participantPortalUrl = _config.GetValue<string>("ParticipantPortal:Url");

            var participation = await _context.Participations
                .AsNoTracking()
                .Include("Visits")
                .FirstOrDefaultAsync(m => m.Id == id);

            if (participation == null)
            {
                return NotFound();
            }

            var participant = await _participantsService.GetParticipantAsync(participation.Id);

            participation.Profile = participant;

            var milestones = await _milestonesService.GetMilestonesForParticipant(participation.Id);

            participation.Milestones = milestones;

            return View(participation);
        }

        // GET: Participation/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Participation/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,COAID,FriendlyID,Status")] Participation participation)
        {
            if (ModelState.IsValid)
            {
                _context.Add(participation);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(participation);
        }

        // GET: Participation/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var participation = await _context.Participations.FindAsync(id);
            if (participation == null)
            {
                return NotFound();
            }
            return View(participation);
        }

        // POST: Participation/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,COAID,FriendlyID,Status")] Participation participation)
        {
            if (id != participation.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(participation);
                    await _context.SaveChangesAsync(HttpContext.User.Identity.Name);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ParticipationExists(participation.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(participation);
        }

        // GET: Participation/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var participation = await _context.Participations
                .FirstOrDefaultAsync(m => m.Id == id);
            if (participation == null)
            {
                return NotFound();
            }

            return View(participation);
        }

        // POST: Participation/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var participation = await _context.Participations.FindAsync(id);
            _context.Participations.Remove(participation);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ParticipationExists(int id)
        {
            return _context.Participations.Any(e => e.Id == id);
        }
    }
}
