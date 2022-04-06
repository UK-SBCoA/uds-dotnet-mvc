using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using UDS.Net.Data;
using UDS.Net.Data.Entities;
using UDS.Net.Data.Enums;
using UDS.Net.Web.Services;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace UDS.Net.Web.Controllers
{
    public class MilestoneController : Controller
    {
        private readonly IMilestonesService _milestonesService;
        private readonly IParticipantsService _participantsService;
        private readonly UdsContext _context;

        public MilestoneController(IMilestonesService milestonesService,IParticipantsService participantsService, UdsContext context)
        {
            _milestonesService = milestonesService;
            _participantsService = participantsService;
            _context = context;
        }

        // GET: /<controller>/
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Details()
        {
            return View();
        }

        [HttpGet("Milestone/Create/{friendlyId}")]
        public async Task<IActionResult> Create(int friendlyId)
        {
            var participation = await _context.Participations
                .AsNoTracking()
                .FirstOrDefaultAsync(m => m.Id == friendlyId);

            var participantIdentity = await _participantsService.GetParticipantAsync(friendlyId);
            participation.Profile = participantIdentity;


            var milestone = new Milestone
            {
                FriendlyId = friendlyId,
                Participant = participation,
                FormStatus = FormStatus.Incomplete
            };

            return View(milestone);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Milestone milestone, string save, string complete)
        {
            if (!String.IsNullOrEmpty(save))
            {
                milestone.FormStatus = FormStatus.Incomplete;
            }
            else if (!String.IsNullOrEmpty(complete))
            {
                milestone.FormStatus = FormStatus.Complete;
                if (!TryValidateModel(milestone))
                {
                    var participation = await _context.Participations
                        .AsNoTracking()
                        .FirstOrDefaultAsync(m => m.Id == milestone.FriendlyId);

                    var participantIdentity = await _participantsService.GetParticipantAsync(milestone.FriendlyId);
                    participation.Profile = participantIdentity;

                    milestone.Participant = participation;

                    return View(milestone);
                }
            }
            if (ModelState.IsValid)
            {
                milestone.ModifiedBy = User.Identity.Name;
                milestone.ModifiedAt = DateTime.Now;

                var newMilestone = await _milestonesService.CreateMilestone(milestone);

                return RedirectToAction("Details", "Participation", new { Id = milestone.FriendlyId });
            }
            return View(milestone);
        }

        [HttpGet("Milestone/Edit/{id}")]
        public async Task<IActionResult> Edit(int? id)
        {
            var milestone = await _milestonesService.GetMilestone(id.Value);

            if (milestone != null)
            {
                var participation = await _context.Participations
                    .AsNoTracking()
                    .FirstOrDefaultAsync(m => m.Id == milestone.FriendlyId);

                var participantIdentity = await _participantsService.GetParticipantAsync(milestone.FriendlyId);
                participation.Profile = participantIdentity;

                milestone.Participant = participation;
            }
            return View(milestone);
        }

        [HttpPost("Milestone/Edit/{id}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Milestone milestone, string save, string complete)
        {
            if (id != milestone.Id)
            {
                return NotFound();
            }

            if (!String.IsNullOrEmpty(save))
            {
                milestone.FormStatus = FormStatus.Incomplete;
            }
            else if (!String.IsNullOrEmpty(complete))
            {
                milestone.FormStatus = FormStatus.Complete;
                if (!TryValidateModel(milestone))
                {
                    var participation = await _context.Participations
                        .AsNoTracking()
                        .FirstOrDefaultAsync(m => m.Id == milestone.FriendlyId);

                    var participantIdentity = await _participantsService.GetParticipantAsync(milestone.FriendlyId);
                    participation.Profile = participantIdentity;

                    milestone.Participant = participation;

                    return View(milestone);
                }
            }
            if (ModelState.IsValid)
            {
                milestone.ModifiedBy = User.Identity.Name;
                milestone.ModifiedAt = DateTime.Now;

                var newMilestone = await _milestonesService.UpdateMilestone(id, milestone);

                return RedirectToAction("Details", "Participation", new { Id = milestone.FriendlyId });
            }
            return View(milestone);
        }
    }

}
