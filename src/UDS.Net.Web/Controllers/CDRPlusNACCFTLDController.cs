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

namespace UDS.Net.Web.Controllers
{
    public class CDRPlusNACCFTLDController : Controller
    {
        private readonly UdsContext _context;
        private readonly IParticipantsService _participantsService;

        public CDRPlusNACCFTLDController(UdsContext context, IParticipantsService participantsService)
        {
            _context = context;
            _participantsService = participantsService;
        }

        // GET: CDRPlusNACCFTLD
        public async Task<IActionResult> Index()
        {
            var udsContext = _context.CDRPlusNACCFTDLs.Include(c => c.Visit);
            return View(await udsContext.ToListAsync());
        }

        // GET: CDRPlusNACCFTLD/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var cDRPlusNACCFTLD = await _context.CDRPlusNACCFTDLs
                .Include(c => c.Visit)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (cDRPlusNACCFTLD == null)
            {
                return NotFound();
            }

            return View(cDRPlusNACCFTLD);
        }

        // GET: CDRPlusNACCFTLD/Create
        public async Task<IActionResult> Create(int id)
        {
            var cdrpPlus = await _context.CDRPlusNACCFTDLs.FindAsync(id);
            if (cdrpPlus == null)
            {
                cdrpPlus = new CDRPlusNACCFTLD
                {
                    Id = id,
                    FormStatus = FormStatus.Incomplete
                };
                _context.Add(cdrpPlus);
                await _context.SaveChangesAsync(HttpContext.User.Identity.Name);
            }

            return RedirectToAction("Edit", new { id = cdrpPlus.Id });
        }

        // POST: CDRPlusNACCFTLD/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Memory,Orientation,JudgmentAndProblemSolving,CommunityAffairs,HomesAndHobbies,PersonalCare,StandardCDRSumOfBoxes,StandardGlobalCDR,BehaviorComportmentAndPersonality,Language,SupplementalCDRSumOfBoxes,SupplementalGlobalCDR,Id,ExaminerInitials,FormStatus")] CDRPlusNACCFTLD cDRPlusNACCFTLD)
        {
            if (ModelState.IsValid)
            {
                _context.Add(cDRPlusNACCFTLD);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["Id"] = new SelectList(_context.Visits, "Id", "Id", cDRPlusNACCFTLD.Id);
            return View(cDRPlusNACCFTLD);
        }

        // GET: CDRPlusNACCFTLD/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var cDRPlusNACCFTLD = await _context.CDRPlusNACCFTDLs
                .Include(c => c.Visit)
                    .ThenInclude(v => v.Participant)
                .AsNoTracking()
                .FirstOrDefaultAsync(c => c.Id == id);

            if (cDRPlusNACCFTLD == null)
            {
                return NotFound();
            }


            var participantIdentity = await _participantsService.GetParticipantAsync(cDRPlusNACCFTLD.Visit.Participant.Id);
            cDRPlusNACCFTLD.Visit.Participant.Profile = participantIdentity;

            return View(cDRPlusNACCFTLD);
        }

        // POST: CDRPlusNACCFTLD/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Memory,Orientation,JudgmentAndProblemSolving,CommunityAffairs,HomesAndHobbies,PersonalCare,StandardCDRSumOfBoxes,StandardGlobalCDR,BehaviorComportmentAndPersonality,Language,SupplementalCDRSumOfBoxes,SupplementalGlobalCDR,Id,ExaminerInitials,FormStatus")] CDRPlusNACCFTLD cDRPlusNACCFTLD, string save, string complete)
        {
            if (id != cDRPlusNACCFTLD.Id)
            {
                return NotFound();
            }

            var visit = await _context.Visits
                .AsNoTracking()
                .Include("Participant")
                .FirstOrDefaultAsync(v => v.Id == cDRPlusNACCFTLD.Id);

            cDRPlusNACCFTLD.Visit = visit;

            var participantIdentity = await _participantsService.GetParticipantAsync(cDRPlusNACCFTLD.Visit.Participant.Id);
            cDRPlusNACCFTLD.Visit.Participant.Profile = participantIdentity;

            if (!String.IsNullOrEmpty(save))
            {
                cDRPlusNACCFTLD.FormStatus = FormStatus.Incomplete;
            }
            else if (!String.IsNullOrEmpty(complete))
            {
                cDRPlusNACCFTLD.FormStatus = FormStatus.Complete;
                if (!TryValidateModel(cDRPlusNACCFTLD))
                {
                    return View(cDRPlusNACCFTLD);
                }
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(cDRPlusNACCFTLD);
                    await _context.SaveChangesAsync(HttpContext.User.Identity.Name);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CDRPlusNACCFTLDExists(cDRPlusNACCFTLD.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction("Details", "Visit", new { id = cDRPlusNACCFTLD.Id });
            }

            return View(cDRPlusNACCFTLD);
        }

        // GET: CDRPlusNACCFTLD/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var cDRPlusNACCFTLD = await _context.CDRPlusNACCFTDLs
                .Include(c => c.Visit)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (cDRPlusNACCFTLD == null)
            {
                return NotFound();
            }

            return View(cDRPlusNACCFTLD);
        }

        // POST: CDRPlusNACCFTLD/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var cDRPlusNACCFTLD = await _context.CDRPlusNACCFTDLs.FindAsync(id);
            _context.CDRPlusNACCFTDLs.Remove(cDRPlusNACCFTLD);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CDRPlusNACCFTLDExists(int id)
        {
            return _context.CDRPlusNACCFTDLs.Any(e => e.Id == id);
        }
    }
}
