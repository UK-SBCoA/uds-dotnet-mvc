using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using UDS.Net.Data;
using UDS.Net.Data.Entities;
using UDS.Net.Data.Enums;
using UDS.Net.Web.Services;
using UDS.Net.Web.ViewModels;

namespace UDS.Net.Web.Controllers
{
    public class SubjectFamilyHistoryController : PacketFormController
    {
        private ProtocolVariable[] _protocolVariables;
        private ProtocolVariable _primaryDiagnosis;
        private ProtocolVariable _neurologicalProblems;
        private ProtocolVariable _methodOfEvaluation;
        private int _siblingNumber = 20;
        private int _childNumber = 15;

        public SubjectFamilyHistoryController(UdsContext context, IParticipantsService participantsService, IChecklistService checklistService) : base(context, participantsService, checklistService)
        {
            string jsonString = System.IO.File.ReadAllText("App_Data/SubjectFamilyHistoryVariableCodes.json");
            _protocolVariables = JsonSerializer.Deserialize<ProtocolVariable[]>(jsonString);
            _primaryDiagnosis = _protocolVariables.Where(p => p.Name == "PrimaryDiagnosis").FirstOrDefault();
            _neurologicalProblems = _protocolVariables.Where(p => p.Name == "NeurologicalPsychiatric").FirstOrDefault();
            _methodOfEvaluation = _protocolVariables.Where(p => p.Name == "EvaluationMethod").FirstOrDefault();
        }

        public async Task<IActionResult> Create(int id)
        {
            /*
             * Creating a new form should do the following
             * Check if there was a previous visit
             * Check if that previous visit has any family members
             * Populate a new list or load in the previous one
             * Bonus: Update from a previous list
             */
            var subjectFamilyHistory = await _context.SubjectFamilyHistories.FindAsync(id);
            if(subjectFamilyHistory == null)
            {
                // Check for a previous visit
                subjectFamilyHistory = new SubjectFamilyHistory
                {
                    Id = id,
                    FormStatus = FormStatus.Incomplete
                };
                var currentVist = await _context.Visits
                    .AsNoTracking()
                    .Include("Participant")
                    .FirstOrDefaultAsync(v => v.Id == subjectFamilyHistory.Id);
                var newRelatives = new List<Relative>();
                var previousVisit = await _context.Visits.Include(x => x.SubjectFamilyHistory).ThenInclude(x => x.Relatives).Where(x => x.VisitNumber == currentVist.VisitNumber - 1 && x.FriendlyId == currentVist.FriendlyId).SingleOrDefaultAsync();
                // Check if there is both a previous subject history and there is a full list of relatives
                if(previousVisit != null) {
                    if(previousVisit.SubjectFamilyHistory != null) {
                        subjectFamilyHistory.SiblingNumber = previousVisit.SubjectFamilyHistory.SiblingNumber;
                        subjectFamilyHistory.ChildrenNumber = previousVisit.SubjectFamilyHistory.ChildrenNumber;
                        if (previousVisit.SubjectFamilyHistory.Relatives.Count == 37)
                        {
                            previousVisit.SubjectFamilyHistory.Relatives.ToList().ForEach(x =>
                            {
                                newRelatives.Add(new Relative
                                {
                                    SubjectFamilyHistoryId = id,
                                    Relation = x.Relation,
                                    RelationshipNumber = x.RelationshipNumber,
                                    BirthMonth = x.BirthMonth,
                                    BirthYear = x.BirthYear,
                                    AgeAtDeath = x.AgeAtDeath,
                                    PrimaryNeurologicalProblemPsychiatricCondition = x.PrimaryNeurologicalProblemPsychiatricCondition,
                                    PrimaryDx = x.PrimaryDx,
                                    MethodOfEvaluation = x.MethodOfEvaluation,
                                    AgeOfOnSet = x.AgeOfOnSet

                                });
                            });
                        }
                    }
                }
                // If there is a failure to copy from a previous visit then create a whole new list
                if(!newRelatives.Any())
                {
                    newRelatives.Add(new Relative()
                    {
                        RelationshipNumber = 1,
                        Relation = FamilyRelationship.Mother,
                        SubjectFamilyHistoryId = id
                    });
                    newRelatives.Add(new Relative()
                    {
                        RelationshipNumber = 2,
                        Relation = FamilyRelationship.Father,
                        SubjectFamilyHistoryId = id
                    });
                    for (var i = 1; i <= _siblingNumber; i++)
                    {
                        newRelatives.Add(new Relative
                        {
                            RelationshipNumber = i,
                            Relation = FamilyRelationship.Sibling,
                            SubjectFamilyHistoryId = id
                        });
                    }
                    for (var i = 1; i <= _childNumber; i++)
                    {
                        newRelatives.Add(new Relative
                        {
                            RelationshipNumber = i,
                            Relation = FamilyRelationship.Child,
                            SubjectFamilyHistoryId = id
                        });
                    }
                }
                subjectFamilyHistory.Relatives = newRelatives;
                await _context.SubjectFamilyHistories.AddAsync(subjectFamilyHistory);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction("Edit", new { id = subjectFamilyHistory.Id });
        }

        public async Task<IActionResult> Edit(int? id) {

            if (id == null)
            {
                return NotFound();
            }
            ViewBag.PrimaryDiagnosis = _primaryDiagnosis;
            ViewBag.NeurologicalProblems = _neurologicalProblems;
            ViewBag.MethodOfEvaluation = _methodOfEvaluation;

            var familyHistory = await _context.SubjectFamilyHistories
                .Include(x => x.Visit)
                    .ThenInclude(x => x.Participant)
                .Include(x => x.Relatives)
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Id == id);

            if (familyHistory == null)
            {
                return NotFound();
            }
            else if (!FormCanBeEdited(familyHistory.Visit.Status))
            {
                return View("Details", familyHistory);
            }

            var participantIdentity = await _participantsService.GetParticipantAsync(familyHistory.Visit.Participant.Id);
            familyHistory.Visit.Participant.Profile = participantIdentity;
            return View(familyHistory);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, SubjectFamilyHistory subjectFamilyHistory, string save, string complete)
        {
            if (id != subjectFamilyHistory.Id) {
                return NotFound();
            }
            var visit = await _context.Visits
                .AsNoTracking()
                .Include("Participant")
                .FirstOrDefaultAsync(v => v.Id == subjectFamilyHistory.Id);

            if (!FormCanBeEdited(visit.Status))
            {
                ModelState.AddModelError("FormStatus", "Form cannot be modified because packet is complete.");
                return View(subjectFamilyHistory);
            }

            subjectFamilyHistory.Visit = visit;

            var participantIdentity = await _participantsService.GetParticipantAsync(subjectFamilyHistory.Visit.Participant.Id);
            subjectFamilyHistory.Visit.Participant.Profile = participantIdentity;

            ViewBag.PrimaryDiagnosis = _primaryDiagnosis;
            ViewBag.NeurologicalProblems = _neurologicalProblems;
            ViewBag.MethodOfEvaluation = _methodOfEvaluation;

            if (!String.IsNullOrEmpty(save))
            {
                subjectFamilyHistory.FormStatus = FormStatus.Incomplete;
            }
            else if (!String.IsNullOrEmpty(complete))
            {
                subjectFamilyHistory.FormStatus = FormStatus.Complete;
                // Validate Relatives
                // Validate Mother
                var relatives = subjectFamilyHistory.Relatives.ToList();
                foreach (var relative in relatives) {
                    if ((relative.RelationshipNumber <= subjectFamilyHistory.SiblingNumber && relative.Relation == FamilyRelationship.Sibling) || (relative.RelationshipNumber <= subjectFamilyHistory.ChildrenNumber && relative.Relation == FamilyRelationship.Child) || relative.Relation == FamilyRelationship.Father || relative.Relation == FamilyRelationship.Mother) {
                        if ((relative.Relation == FamilyRelationship.Father && subjectFamilyHistory.ParentChange == 1) || (relative.Relation == FamilyRelationship.Mother && subjectFamilyHistory.ParentChange == 1) || (relative.Relation == FamilyRelationship.Father && subjectFamilyHistory.ParentChange == 1) || (relative.Relation == FamilyRelationship.Child && subjectFamilyHistory.ChildrenChange == 1) || (relative.Relation == FamilyRelationship.Sibling && subjectFamilyHistory.SiblingChange == 1) || subjectFamilyHistory.Visit.VisitType == VisitType.IVP)
                        {
                            int relativeIndex = relatives.IndexOf(relative);
                            if (!relative.BirthMonth.HasValue)
                            {
                                ModelState.AddModelError(String.Format("Relatives[{0}].BirthMonth", relativeIndex), "Please provide a Birth Month or indicate otherwise");
                            }
                            if (!relative.BirthYear.HasValue)
                            {
                                ModelState.AddModelError(String.Format("Relatives[{0}].BirthYear", relativeIndex), "Please provide a Birth Year indicate otherwise");
                            }
                            if (!relative.AgeAtDeath.HasValue)
                            {
                                ModelState.AddModelError(String.Format("Relatives[{0}].AgeAtDeath", relativeIndex), "Please provide an age at death or indicate otherwise");
                            }
                            if(relative.BirthMonth.HasValue || relative.BirthYear.HasValue || relative.AgeAtDeath.HasValue)
                            {
                                if(!relative.PrimaryNeurologicalProblemPsychiatricCondition.HasValue)
                                {
                                    ModelState.AddModelError(String.Format("Relatives[{0}].PrimaryNeurologicalProblemPsychiatricCondition", relativeIndex), "Please provide a value for Primary neurological problem/psychiatric condition");
                                }
                            }
                            if (relative.PrimaryNeurologicalProblemPsychiatricCondition.HasValue)
                            {
                                var codeExists = _neurologicalProblems.Codes.Where(x => int.Parse(x.Key) == relative.PrimaryNeurologicalProblemPsychiatricCondition).Any();
                                if(!codeExists) {
                                    ModelState.AddModelError(String.Format("Relatives[{0}].PrimaryNeurologicalProblemPsychiatricCondition", relativeIndex), "Please enter a valid code");
                                }                          
                            }
                            if (relative.PrimaryDx.HasValue)
                            {
                                var codeExists = _primaryDiagnosis.Codes.Where(x => int.Parse(x.Key) == relative.PrimaryDx).Any();
                                if (!codeExists)
                                {
                                    ModelState.AddModelError(String.Format("Relatives[{0}].PrimaryDx", relativeIndex), "Please enter a valid code, refer to APPENDIX 1");
                                }
                            }
                            if (relative.MethodOfEvaluation.HasValue)
                            {
                                var codeExists = _methodOfEvaluation.Codes.Where(x => int.Parse(x.Key) == relative.MethodOfEvaluation).Any();
                                if (!codeExists)
                                {
                                    ModelState.AddModelError(String.Format("Relatives[{0}].MethodOfEvaluation", relativeIndex), "Please enter a valid code");
                                }

                                if(relative.PrimaryDx.HasValue && relative.PrimaryDx.Value == 999) {
                                    int[] invalidMethodsOfEvaluation = {1,2,3};

                                    if(Array.Exists(invalidMethodsOfEvaluation, method => method == relative.MethodOfEvaluation.Value)) {
                                        ModelState.AddModelError(String.Format("Relatives[{0}].MethodOfEvaluation", relativeIndex), "If Primary Dx is 999, the method of evaluation cannot be 1, 2, or 3");
                                    }
                                }
                            }

                            bool hasNeurologicalProblemPsycyiatricCondition = relative.PrimaryNeurologicalProblemPsychiatricCondition.HasValue && relative.PrimaryNeurologicalProblemPsychiatricCondition.Value != 8 && relative.PrimaryNeurologicalProblemPsychiatricCondition.Value != 9;

                            if (!relative.PrimaryDx.HasValue && hasNeurologicalProblemPsycyiatricCondition)
                            {
                                ModelState.AddModelError(String.Format("Relatives[{0}].PrimaryDx", relativeIndex), "Please provide a Primary Dx, refer to the codes in APPENDIX 1");
                            }
                            if (!relative.MethodOfEvaluation.HasValue && hasNeurologicalProblemPsycyiatricCondition)
                            {
                                ModelState.AddModelError(String.Format("Relatives[{0}].MethodOfEvaluation", relativeIndex), "Please provide a method of evaluation, refer to the codes below");
                            }
                            if (!relative.AgeOfOnSet.HasValue && hasNeurologicalProblemPsycyiatricCondition)
                            {
                                ModelState.AddModelError(String.Format("Relatives[{0}].AgeOfOnSet", relativeIndex), "Please provide the age of onset");
                            }
                            relativeIndex++;
                        }
                    }
                }
                if (!TryValidateModel(subjectFamilyHistory))
                {
                    var error = ModelState.Values.SelectMany(v => v.Errors).ToList();
                    return View(subjectFamilyHistory);
                }
            }
            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(subjectFamilyHistory);
                    await _context.SaveChangesAsync(HttpContext.User.Identity.Name);
                    await _checklistService.ValidateAndUpdateChecklistStatus(visit, typeof(SubjectFamilyHistory));
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!SubjectHealthHistoryExists(subjectFamilyHistory.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction("Details", "Visit", new { id = subjectFamilyHistory.Id });
            }

            return View(subjectFamilyHistory);

        }
        [AcceptVerbs("GET", "POST")]
        public IActionResult PrimaryNeurologicalProblemPsychiatricCondition(string primaryNeurologicalProblemPsychiatricCondition)
        {
            bool validCode = _neurologicalProblems.Codes.Where(x => x.Key == primaryNeurologicalProblemPsychiatricCondition).Any();
            if (validCode)
                return Json(data: true);
            else
                return Json(data: false);
        }
        private bool SubjectHealthHistoryExists(int id) {
            return _context.SubjectHealthHistories.Where(x => x.Id == id).Any();
        }
    }
}
