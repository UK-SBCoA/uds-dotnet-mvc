using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using UDS.Net.Data;
using UDS.Net.Data.Entities;
using UDS.Net.Data.Enums;
using UDS.Net.Web.Services;
using UDS.Net.Web.ViewModels;

namespace UDS.Net.Web.Controllers
{
    public class VisitController : Controller
    {
        private readonly UdsContext _context;
        private readonly IParticipantsService _participantsService;
        private readonly IVisitService _visitService;

        public VisitController(UdsContext context, IParticipantsService participantsService, IVisitService visitService)
        {
            _context = context;
            _participantsService = participantsService;
            _visitService = visitService;
        }

        // GET: Visit5
        [HttpGet]
        public async Task<IActionResult> Index(int? friendlyId, bool? isSubmittedToNACC, VisitStatus? visitStatus, int pageSize = 20, int pageNumber = 1)
        {
            if (pageNumber <= 0)
                pageNumber = 1;
            var baseQuery = GetFilteredVisits(friendlyId, isSubmittedToNACC, visitStatus);
            var visitCount = await GetVisitCountAsync(baseQuery);
            int totalPages = CalculateTotalPages(visitCount, pageSize);
            bool hasPreviousPage = (pageNumber > 1);
            bool hasNextPage = (pageNumber < totalPages);
            var visitsPageResults = await
                    baseQuery      
                    .Skip((pageNumber - 1) * pageSize)
                    .Take(pageSize)
                    .AsNoTracking()
                    .ToListAsync();
            
            var adminVisitViewModel = new AdminVisitViewModel
            {
                FriendlyId = friendlyId,
                IsSubmittedToNACC = isSubmittedToNACC,
                PageNumber = pageNumber,
                HasNextPage = hasNextPage,
                HasPreviousPage = hasPreviousPage,
                Visits = visitsPageResults,
                VisitStatus = visitStatus,
                PageSize = pageSize
            };
            return View(adminVisitViewModel);
        }
        private int CalculateTotalPages(int visitCount, int pageSize)
        {
            return (int)Math.Ceiling(visitCount / (double)pageSize);
        }
        public IQueryable<Visit> GetFilteredVisits(int? friendlyId, bool? isSubmittedToNACC, VisitStatus? visitStatus)
        {
            return _context.Visits
                   .Where(x =>
                       (!String.IsNullOrWhiteSpace(visitStatus.ToString()) ? x.Status == visitStatus : true) &&
                       (friendlyId.HasValue ? x.FriendlyId == friendlyId : true) &&
                       (isSubmittedToNACC.HasValue ? x.IsSubmittedToNACC == isSubmittedToNACC : true));
        }
        private async Task<int> GetVisitCountAsync(IQueryable<Visit> visitQuery) 
        {
            return await visitQuery.CountAsync();
        }
        public async Task<IActionResult> InProgress()
        {
            return View(await _context.Visits.Where(v => v.IsSubmittedToNACC == false).AsNoTracking().ToListAsync());
        }

        // GET: Participant/{friendlyId}/Visit
        public async Task<IActionResult> ByParticipant(int friendlyId)
        {
            ViewData["FriendlyId"] = friendlyId;
            return View(await _context.Visits.Where(v => v.FriendlyId == friendlyId).ToListAsync());
        }

        // GET: Visit/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var visit = await _visitService.GetVisitWithParticipantAndFormBases(id.Value);

            if (visit == null)
            {
                return NotFound();
            }

            var examiners = await _context.Examiners
                .AsNoTracking()
                .ToListAsync();

            ViewBag.Examiners = examiners;

            var participantIdentity = await _participantsService.GetParticipantAsync(visit.Participant.Id);
            visit.Participant.Profile = participantIdentity;

            if (visit.VisitType == VisitType.IVP)
            {
                return View("IVP", visit);
            }
            else if (visit.VisitType == VisitType.FVP)
            {
                return View("FVP", visit);
            }
            else if (visit.VisitType == VisitType.TFP)
            {
                return View("TFP", visit);
            }

            return View(visit);
        }

        // GET: Visit/Create
        public async Task<IActionResult> Create(int friendlyId)
        {
            var currentVisitNumber = 0;
            if (friendlyId > 0)
            {
                var lastVisit = await _context.Visits
                    .Where(v => v.FriendlyId == friendlyId)
                    .OrderByDescending(v => v.VisitNumber)
                    .FirstOrDefaultAsync();

                if (lastVisit != null)
                {
                    currentVisitNumber = lastVisit.VisitNumber;
                }
            }

            ViewData["NextVisitNumber"] = currentVisitNumber + 1;
            ViewData["FriendlyId"] = friendlyId;
            return View();
        }

        // POST: Visit/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,VisitDate,FriendlyId,VisitNumber,VisitType,Status,IsSubmittedToNACC,CoordinatorInitials,ClinicianInitials,SocialWorkerInitials")] Visit visit)
        {
            if (ModelState.IsValid)
            {
                int visitRedirectId;
                // Prevents Race Conditions and Create of Multiple In-Progress Visits
                var visitsInProgress = ParticipantsVisitsInProgress(visit.FriendlyId);
                if(visitsInProgress.Any()) {
                    visitRedirectId = visitsInProgress.First().Id;
                } else {
                    _context.Add(visit);
                    await _context.SaveChangesAsync(HttpContext.User.Identity.Name); 
                    visitRedirectId = visit.Id;
                }
                return RedirectToAction("Details", "Visit", new { id = visitRedirectId });
            }
            return View(visit);
        }

        // GET: Visit/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var examiners = await _context.Examiners
                .AsNoTracking()
                .ToListAsync();

            ViewBag.Examiners = examiners;

            var visit = await _context.Visits
                .Include(v => v.Checklist)
                .FirstOrDefaultAsync(v => v.Id == id);

            if (visit == null)
            {
                return NotFound();
            }
            return View(visit);
        }

        // POST: Visit/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,VisitDate,FriendlyId,VisitNumber,VisitType,CoordinatorInitials,ClinicianInitials,SocialWorkerInitials,Status,PriorityWeight,IsSubmittedToNACC")] Visit visit, string redirectToAction)
        {
            if (id != visit.Id)
            {
                return NotFound();
            }

            var existingVisit = await _context.Visits
                .Include(v => v.Checklist)
                .FirstOrDefaultAsync(v => v.Id == id);

            if (existingVisit == null)
            {
                return NotFound();
            }

            var examiners = await _context.Examiners
                .AsNoTracking()
                .ToListAsync();

            ViewBag.Examiners = examiners;

            if (ModelState.IsValid)
            {
                try
                {
                    existingVisit.VisitType = visit.VisitType;
                    existingVisit.CoordinatorInitials = visit.CoordinatorInitials;
                    existingVisit.ClinicianInitials = visit.ClinicianInitials;
                    existingVisit.SocialWorkerInitials = visit.SocialWorkerInitials;
                    existingVisit.Status = visit.Status;
                    existingVisit.PriorityWeight = visit.PriorityWeight;
                    existingVisit.IsSubmittedToNACC = visit.IsSubmittedToNACC;

                    _context.Update(existingVisit);
                    await _context.SaveChangesAsync(HttpContext.User.Identity.Name);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!VisitExists(existingVisit.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }

                if (String.IsNullOrWhiteSpace(redirectToAction))
                {
                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    if (redirectToAction == "Details")
                        return RedirectToAction(nameof(Details), new { Id = visit.Id });
                }
            }
            return View(visit);
        }

        // GET: Visit/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var visit = await _context.Visits
                .FirstOrDefaultAsync(m => m.Id == id);
            if (visit == null)
            {
                return NotFound();
            }

            return View(visit);
        }

        // POST: Visit/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var visit = await _context.Visits.FindAsync(id);
            _context.Visits.Remove(visit);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        /// <summary>
        /// Checks if user currently has an In-Progress Visit 
        /// </summary>
        /// <param name="friendlyId">Friendly ID user to identify the user</param>
        /// <returns>One or more in-progress visits in decending order or null</returns>
        private IEnumerable<Visit> ParticipantsVisitsInProgress(int friendlyId) {
            return _context.Visits.Where(x => x.FriendlyId == friendlyId && x.Status == VisitStatus.InProgress).OrderByDescending(x => x.VisitNumber).ToList();
        }
        private bool VisitExists(int id)
        {
            return _context.Visits.Any(e => e.Id == id);
        }
    }
}
