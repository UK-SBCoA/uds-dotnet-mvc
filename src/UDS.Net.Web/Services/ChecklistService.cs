using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UDS.Net.Data;
using UDS.Net.Data.Entities;
using UDS.Net.Data.Enums;
using UDS.Net.Web.ViewModels;

namespace UDS.Net.Web.Services
{
    /// <summary>
    /// Currently all the business logic for Checklists is in VisitCompletionViewModel.
    /// The controller is lean as it should be, but now we are checking for form saves after the checklist has been completed.
    /// Consequently, TODO All the logic should be moved here for ease of maintenance.
    /// </summary>
    public class ChecklistService : IChecklistService
    {
        private readonly UdsContext _context;
        // Initializing for most common form: FVP
        private readonly Dictionary<string, Type> _requiredForms = new Dictionary<string, Type>()
        {
            { "A1", typeof(ParticipantDemographics) },
            { "B4", typeof(CDRPlusNACCFTLD) },
            { "B8", typeof(NeurologicalExaminationFindings) },
            { "B9", typeof(Symptoms) },
            { "C2", typeof(NeuropsychologicalBatteryScores) },
            { "D1", typeof(ClinicianDiagnosis) },
            { "D2", typeof(MedicalConditions) },
            { "Z1", typeof(Checklist) }
        };
        // Initializing for most common form: FVP
        private readonly Dictionary<string, Type> _optionalForms = new Dictionary<string, Type>()
        {
            { "A2", typeof(CoParticipantDemographics) },
            { "A3", typeof(SubjectFamilyHistory) },
            { "A4", typeof(MedicationsReview) },
            { "A5", typeof(SubjectHealthHistory) },
            { "B1", typeof(PhysicalEvaluation) },
            { "B5", typeof(NPIQ) },
            { "B6", typeof(GeriatricDepressionScale) },
            { "B7", typeof(FunctionalActivitiesQuestionnaire) }
        };

        private void SetRequiredAndOptionalFormsForIVP()
        {
            //Required: A1, A5, B4, B8, B9, C2, D1, D2, Z1
            _requiredForms.Add("A5", typeof(SubjectHealthHistory));

            //Optional
            _optionalForms.Remove("A5");
        }

        private void SetRequiredAndOptionalFormsForTFP()
        {
            //Required: A1, A2, B4, B9, D1, D2, T1, Z1
            _requiredForms.Remove("B8"); // No B8 for telephone
            _requiredForms.Remove("C2"); // No C2 for telephone
            _requiredForms.Add("A2", typeof(CoParticipantDemographics)); // A2 is required for telephone
            _requiredForms.Add("T1", typeof(Inclusion)); // T1 is required for telephone

            //Optional
            _optionalForms.Remove("A2"); // A2 is required for telephone
            _optionalForms.Remove("A5"); // No A5 listed for telephone
            _optionalForms.Remove("B1"); // No B1 listed for telephone
            _optionalForms.Remove("B6"); // No B6 listed for telephone
        }

        public async Task ValidateAndUpdateChecklistStatus(Visit visit, Type formType)
        {
            if (visit == null || visit.Id <= 0)
            {
                return;
            }

            var checklist = visit.Checklist; // if we already have the checklist, use it
            if (checklist == null)
            {
                checklist = await _context.Checklists.FindAsync(visit.Id);
                if (checklist == null)
                    return; // if checklist is still null it hasn't been created yet and therefore doesn't need to be modified
            }

            if (visit.VisitType == VisitType.IVP)
            {
                SetRequiredAndOptionalFormsForIVP();
            }
            else if (visit.VisitType == VisitType.TFP)
            {
                SetRequiredAndOptionalFormsForTFP();
            }

            // First, check if the form is required for the type of visit
            // if so, change the status of the checklist
            foreach (KeyValuePair<string, Type> requiredForm in _requiredForms)
            {
                if (requiredForm.Value == formType)
                    checklist.FormStatus = FormStatus.Incomplete;
            }

            // Then, if the form is optional, make sure it is set with a reason code
            // otherwise, change the status of the checklist
            foreach (KeyValuePair<string, Type> optionalForm in _optionalForms)
            {
                if (optionalForm.Value == formType)
                {
                    // if the form is optional and has not been indicated to be included, or is indicated to be included then change the status
                    if ((formType == typeof(CoParticipantDemographics)) && (!checklist.A2_IsIncluded.HasValue || (checklist.A2_IsIncluded.HasValue && checklist.A2_IsIncluded.Value)))
                        checklist.FormStatus = FormStatus.Incomplete;
                    if ((formType == typeof(SubjectFamilyHistory)) && (!checklist.A3_IsIncluded.HasValue || (checklist.A3_IsIncluded.HasValue && checklist.A3_IsIncluded.Value)))
                        checklist.FormStatus = FormStatus.Incomplete;
                    if ((formType == typeof(MedicationsReview)) && (!checklist.A4_IsIncluded.HasValue || (checklist.A4_IsIncluded.HasValue && checklist.A4_IsIncluded.Value)))
                        checklist.FormStatus = FormStatus.Incomplete;
                    if ((formType == typeof(PhysicalEvaluation)) && (!checklist.B1_IsIncluded.HasValue || (checklist.B1_IsIncluded.HasValue && checklist.B1_IsIncluded.Value)))
                        checklist.FormStatus = FormStatus.Incomplete;
                    if ((formType == typeof(NPIQ)) && (!checklist.B5_IsIncluded.HasValue || (checklist.B5_IsIncluded.HasValue && checklist.B5_IsIncluded.Value)))
                        checklist.FormStatus = FormStatus.Incomplete;
                    if ((formType == typeof(GeriatricDepressionScale)) && (!checklist.B6_IsIncluded.HasValue || (checklist.B6_IsIncluded.HasValue && checklist.B6_IsIncluded.Value)))
                        checklist.FormStatus = FormStatus.Incomplete;
                    if ((formType == typeof(FunctionalActivitiesQuestionnaire)) && (!checklist.B7_IsIncluded.HasValue || (checklist.B7_IsIncluded.HasValue && checklist.B7_IsIncluded.Value)))
                        checklist.FormStatus = FormStatus.Incomplete;
                }
            }


            _context.Update(checklist);
            await _context.SaveChangesAsync();

            return;
        }

        public string GetRequiredFormsDisplay(Visit visit)
        {
            if (visit.VisitType == VisitType.IVP)
            {
                SetRequiredAndOptionalFormsForIVP();
            }
            else if (visit.VisitType == VisitType.TFP)
            {
                SetRequiredAndOptionalFormsForTFP();
            }

            var sortedAbbreviations = _requiredForms.Keys.ToList();
            sortedAbbreviations.Sort();

            return string.Join(", ", sortedAbbreviations);
        }

        public ChecklistService(UdsContext context)
        {
            _context = context;
        }
    }
}
