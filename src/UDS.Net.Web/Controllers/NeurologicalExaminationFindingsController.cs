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
    public class NeurologicalExaminationFindingsController : PacketFormController
    {
        public NeurologicalExaminationFindingsController(UdsContext context, IParticipantsService participantsService, IChecklistService checklistService) : base(context, participantsService, checklistService)
        {
        }

        // GET: NeurologicalExaminationFindings
        public async Task<IActionResult> Index()
        {
            var udsContext = _context.NeurologicalExaminationFindings.Include(n => n.Visit);
            return View(await udsContext.ToListAsync());
        }

        // GET: NeurologicalExaminationFindings/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var neurologicalExaminationFindings = await _context.NeurologicalExaminationFindings
                .Include(n => n.Visit)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (neurologicalExaminationFindings == null)
            {
                return NotFound();
            }

            return View(neurologicalExaminationFindings);
        }

        // GET: NeurologicalExaminationFindings/Create
        public async Task<IActionResult> Create(int id)
        {
            var neurologicalExaminationFindings = await _context.NeurologicalExaminationFindings.FindAsync(id);

            if(neurologicalExaminationFindings  == null)
            {
                neurologicalExaminationFindings = new NeurologicalExaminationFindings();
                neurologicalExaminationFindings.InitializeForm(id);
                _context.Add(neurologicalExaminationFindings);
                await _context.SaveChangesAsync(HttpContext.User.Identity.Name);
            }

            return RedirectToAction("Edit", new { id = neurologicalExaminationFindings.Id });
        }

        // POST: NeurologicalExaminationFindings/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("AbnormalNeurologicalExamFindings,ParkinsonianSigns,ParkinsonianLeftRestingTremorArm,ParkinsonianRightRestingTremorArm,ParkinsonianRightSlowingOfFineMotorMovements,ParkinsonianLeftSlowingOfFineMotorMovements,ParkinsonianRightRigidityArm,ParkinsonianLeftRigidityArm,ParkinsonianBradykinesia,ParkinsonianGaitDisorder,ParkinsonianPosturalInstability,CerebrovascularDiseaseSigns,CorticalCognitiveDeficit,CorticalSIVD,CorticalMotorLeft,CorticalMotorRight,CorticalVisualFieldLossLeft,CorticalVisualFieldLossRight,CorticalSomatosensoryLossLeft,CorticalSomatosensoryLossRight,HigherCorticalVisual,PSP,PSP_EyeMovement,PSP_Dysarthria,PSP_Axial,PSP_Gait,PSP_Apraxia,CBS_ApraxiaLeft,CBS_ApraxiaRight,CBS_CorticalSensoryDeficitsLeft,CBS_CorticalSensoryDeficitsRight,CBS_AtaxiaLeft,CBS_AtaxiaRight,CBS_AlienLimbLeft,CBS_AlienLimbRight,CBS_DystoniaLeft,CBS_DystoniaRight,CBS_MyoclonusLeft,CBS_MyoclonusRight,ALS_Findings,GaitApraxia,OtherFindings,OtherFindingsSpeicify,Id,ExaminerInitials,FormStatus,Version")] NeurologicalExaminationFindings neurologicalExaminationFindings)
        {
            if (ModelState.IsValid)
            {
                _context.Add(neurologicalExaminationFindings);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            

            return View(neurologicalExaminationFindings);
        }
        [HttpGet]
        // GET: NeurologicalExaminationFindings/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var neurologicalExaminationFindings = await _context.NeurologicalExaminationFindings
                .Include(c => c.Visit)
                .ThenInclude(c => c.Participant)
                .AsNoTracking()
                .FirstOrDefaultAsync(c => c.Id == id);

            if (neurologicalExaminationFindings == null)
            {
                return NotFound();
            }
            else if (!FormCanBeEdited(neurologicalExaminationFindings.Visit.Status))
            {
                return View("Details", neurologicalExaminationFindings);
            }

            var participantIdentity = await _participantsService.GetParticipantAsync(neurologicalExaminationFindings.Visit.Participant.Id);
            neurologicalExaminationFindings.Visit.Participant.Profile = participantIdentity;
            return View(neurologicalExaminationFindings);
        }

        // POST: NeurologicalExaminationFindings/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, NeurologicalExaminationFindings neurologicalExaminationFindings, string save, string complete)
        {
            if (id != neurologicalExaminationFindings.Id)
            {
                return NotFound();
            }
            var visit = await _context.Visits
                .AsNoTracking()
                .Include("Participant")
                .FirstOrDefaultAsync(v => v.Id == neurologicalExaminationFindings.Id);

            if (!FormCanBeEdited(visit.Status))
            {
                ModelState.AddModelError("FormStatus", "Form cannot be modified because packet is complete.");
                return View(neurologicalExaminationFindings);
            }

            neurologicalExaminationFindings.Visit = visit;

            var participantIdentity = await _participantsService.GetParticipantAsync(neurologicalExaminationFindings.Visit.Participant.Id);
            neurologicalExaminationFindings.Visit.Participant.Profile = participantIdentity;

            if (!String.IsNullOrEmpty(save))
            {
                neurologicalExaminationFindings.FormStatus = FormStatus.Incomplete;
            }
            else if (!String.IsNullOrEmpty(complete))
            {
                neurologicalExaminationFindings.FormStatus = FormStatus.Complete;
                if (!TryValidateModel(neurologicalExaminationFindings))
                {
                    return View(neurologicalExaminationFindings);
                }
            }
            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(neurologicalExaminationFindings);
                    await _context.SaveChangesAsync(HttpContext.User.Identity.Name);
                    await _checklistService.ValidateAndUpdateChecklistStatus(visit, typeof(NeurologicalExaminationFindings));
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!NeurologicalExaminationFindingsExists(neurologicalExaminationFindings.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction("Details", "Visit", new { id = neurologicalExaminationFindings.Id });
            }
            return View(neurologicalExaminationFindings);
        }

        // GET: NeurologicalExaminationFindings/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var neurologicalExaminationFindings = await _context.NeurologicalExaminationFindings
                .Include(n => n.Visit)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (neurologicalExaminationFindings == null)
            {
                return NotFound();
            }

            return View(neurologicalExaminationFindings);
        }

        // POST: NeurologicalExaminationFindings/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var neurologicalExaminationFindings = await _context.NeurologicalExaminationFindings.FindAsync(id);
            _context.NeurologicalExaminationFindings.Remove(neurologicalExaminationFindings);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool NeurologicalExaminationFindingsExists(int id)
        {
            return _context.NeurologicalExaminationFindings.Any(e => e.Id == id);
        }
    }
}
