using System;
using System.ComponentModel.DataAnnotations;
using UDS.Net.Data.Entities;
using UDS.Net.Data.Enums;

namespace UDS.Net.Web.ViewModels
{
    public class VisitCompletionViewModel
    {
        /// <summary>
        /// A1 required
        /// Checks for existence and completion for all visit types: IVP, FVP, TFP
        /// </summary>
        [Required(ErrorMessage = "A1 must be completed")]
        public bool? A1_IsIncluded
        {
            get
            {
                if (this.Visit.ParticipantDemographics != null && this.Visit.ParticipantDemographics.FormStatus == FormStatus.Complete)
                    return true;
                return null;
            }
        }

        /// <summary>
        /// A2 is required for TFP
        /// </summary>
        [Required(ErrorMessage = "A2 must be completed")]
        public bool? A2_RequiredForTFP
        {
            get
            {
                if (this.Visit.VisitType == VisitType.TFP)
                {
                    if (this.Visit.CoParticipantDemographics == null || this.Visit.CoParticipantDemographics.FormStatus != FormStatus.Complete)
                        return null;
                }
                return true;
            }
        }

        [Required(ErrorMessage = "Please provide whether the A2 form should be included in submission")]
        public bool? A2_OptionalForIVPFVP
        {
            get
            {
                if (this.Visit.VisitType != VisitType.TFP)
                {
                    if (!this.Checklist.A2_IsIncluded.HasValue)
                        return null;
                }
                return true;
            }
        }

        /// <summary>
        /// A2 is optional for IVP, FVP
        /// </summary>
        [Required(ErrorMessage = "A2 must be completed if included")]
        public bool? A2_IfIncludedThenCompleted
        {
            get
            {
                if (this.Checklist.A2_IsIncluded.HasValue) // Being set as true for TFP in view
                {
                    if (this.Checklist.A2_IsIncluded.Value == true && (this.Visit.CoParticipantDemographics == null || this.Visit.CoParticipantDemographics.FormStatus != FormStatus.Complete))
                    {
                        return null;
                    }
                }
                return true; // if it doesn't have a value, then don't show this error message, the child property will control validation for it's value
            }
        }

        /// <summary>
        /// A3 is optional for all
        /// </summary>
        [Required(ErrorMessage = "A3 must be completed if included")]
        public bool? A3_IfIncludedThenCompleted
        {
            get
            {
                if (this.Checklist.A3_IsIncluded.HasValue)
                {
                    if (this.Checklist.A3_IsIncluded.Value == true && (this.Visit.SubjectFamilyHistory == null || this.Visit.SubjectFamilyHistory.FormStatus != FormStatus.Complete))
                        return null;
                }
                return true; // don't show error message if it's not answered yet
            }
        }

        /// <summary>
        /// A4 is optional for all
        /// </summary>
        [Required(ErrorMessage = "A4 must be completed if included")]
        public bool? A4_IfIncludedThenCompleted
        {
            get
            {
                if (this.Checklist.A4_IsIncluded.HasValue)
                {
                    if (this.Checklist.A4_IsIncluded.Value == true && (this.Visit.MedicationsReview == null || this.Visit.MedicationsReview.FormStatus != FormStatus.Complete))
                        return null;
                }
                return true;
            }
        }

        /// <summary>
        /// A5 is only required for IVP
        /// Checks for existence and completion for initial in-person visit: IVP
        /// </summary>
        [Required(ErrorMessage = "A5 must be completed")]
        public bool? A5_IsIncluded
        {
            get
            {
                if (this.Visit.VisitType != VisitType.IVP)
                    return true;
                else
                {
                    if (this.Visit.SubjectHealthHistory != null && this.Visit.SubjectHealthHistory.FormStatus == FormStatus.Complete)
                        return true;
                }
                return null;
            }
        }

        /// <summary>
        /// B1 is optional for in-person visits: IVP, FVP
        /// B1 is not show for telephone visit: TFP
        /// </summary>
        [Required(ErrorMessage = "B1 must be completed if included")]
        public bool? B1_IfIncludedThenCompleted
        {
            get
            {
                if (this.Visit.VisitType != VisitType.TFP)
                {
                    if (this.Checklist.B1_IsIncluded.HasValue)
                    {
                        if (this.Checklist.B1_IsIncluded.Value == true && (this.Visit.PhysicalEvaluation == null || this.Visit.PhysicalEvaluation.FormStatus != FormStatus.Complete))
                            return null;
                    }
                }
                return true;
            }
        }

        /// <summary>
        /// B4 required for all
        /// Checks for existence and completion for all visit types: IVP, FVP, TFP
        /// </summary>
        [Required(ErrorMessage = "B4 must be completed")]
        public bool? B4_IsIncluded
        {
            get
            {
                if (this.Visit.CDRPlusNACCFTLD != null && this.Visit.CDRPlusNACCFTLD.FormStatus == FormStatus.Complete)
                    return true;
                return null;
            }
        }

        /// <summary>
        /// Optional for all
        /// </summary>
        [Required(ErrorMessage = "B5 must be completed if included")]
        public bool? B5_IfIncludedThenCompleted
        {
            get
            {
                if (this.Checklist.B5_IsIncluded.HasValue)
                {
                    if (this.Checklist.B5_IsIncluded.Value == true && (this.Visit.NPIQ == null || this.Visit.NPIQ.FormStatus != FormStatus.Complete))
                        return null;
                }
                return true;
            }
        }

        /// <summary>
        /// Optional for in-person visits
        /// Not listed for telephone
        /// </summary>
        [Required(ErrorMessage = "B6 must be completed if included")]
        public bool? B6_IfIncludedThenCompleted
        {
            get
            {
                if (this.Visit.VisitType != VisitType.TFP)
                {
                    if (this.Checklist.B6_IsIncluded.HasValue)
                    {
                        if (this.Checklist.B6_IsIncluded.Value == true && (this.Visit.GeriatricDepressionScale == null || this.Visit.GeriatricDepressionScale.FormStatus != FormStatus.Complete))
                            return null;
                    }
                }
                return true;
            }
        }

        /// <summary>
        /// Optional for all
        /// </summary>
        [Required(ErrorMessage = "B7 must be completed if included")]
        public bool? B7_IfIncludedThenCompleted
        {
            get
            {
                if (this.Checklist.B7_IsIncluded.HasValue)
                {
                    if (this.Checklist.B7_IsIncluded.Value == true && (this.Visit.FunctionalActivitiesQuestionnaire == null || this.Visit.FunctionalActivitiesQuestionnaire.FormStatus != FormStatus.Complete))
                        return null;
                }
                return true;
            }
        }

        /// <summary>
        /// B8 required for in-person
        /// Checks for existence and completion for in-person visits: IVP, FVP
        /// </summary>
        [Required(ErrorMessage = "B8 must be completed")]
        public bool? B8_IsIncluded
        {
            get
            {
                if (this.Visit.VisitType == VisitType.TFP)
                    return true;
                else
                {
                    if (this.Visit.NeurologicalExaminationFindings != null && this.Visit.NeurologicalExaminationFindings.FormStatus == FormStatus.Complete)
                        return true;
                }
                return null;
            }
        }

        /// B9
        /// IVP, FVP, TFP required
        [Required(ErrorMessage = "B9 must be completed")]
        public bool? B9_IsIncluded
        {
            get
            {
                if (this.Visit.Symptoms != null && this.Visit.Symptoms.FormStatus == FormStatus.Complete)
                    return true;
                return null;
            }
        }

        /// <summary>
        /// C2 required for in-person
        /// Checks for existence and completion for in-person visits: IVP, FVP
        /// Not listed on ours for telephone: TFP
        /// </summary>
        [Required(ErrorMessage = "C2 must be completed")]
        public bool? C2_IsIncluded
        {
            get
            {
                if (this.Visit.VisitType == VisitType.TFP)
                    return true;
                else
                {
                    if (this.Visit.NeuropsychologicalBatteryScores != null && this.Visit.NeuropsychologicalBatteryScores.FormStatus == FormStatus.Complete)
                        return true;
                }
                return null;
            }
        }

        /// <summary>
        /// D1 required for all visits
        /// Checks for existence and completion for in-person visits: IVP, FVP, TFP
        /// </summary>
        [Required(ErrorMessage = "D1 must be completed")]
        public bool? D1_IsIncluded
        {
            get
            {
                if (this.Visit.ClinicianDiagnosis != null && this.Visit.ClinicianDiagnosis.FormStatus == FormStatus.Complete)
                    return true;
                return null;
            }
        }

        /// <summary>
        /// D2 required for all visits
        /// Checks for existence and completion for in-person visits: IVP, FVP, TFP
        /// </summary>
        [Required(ErrorMessage = "D2 must be completed")]
        public bool? D2_IsIncluded
        {
            get
            {
                if (this.Visit.MedicalConditions != null && this.Visit.MedicalConditions.FormStatus == FormStatus.Complete)
                    return true;
                return null;
            }
        }

        /// <summary>
        /// T1 required for telephone visits (TFP)
        /// Checks for existence and completion for telephone visit: TFP
        /// </summary>
        [Required(ErrorMessage = "T1 form is required")]
        public bool? T1_IsIncluded
        {
            get
            {
                if (Visit.VisitType == VisitType.TFP)
                {
                    if (this.Visit.Inclusion != null && this.Visit.Inclusion.FormStatus == FormStatus.Complete)
                        return true;
                    return null;
                }
                return true;
            }
        }

        /// <summary>
        /// Checklist child property for Z1 data
        /// </summary>
        public Checklist Checklist { get; set; } // IVP, FVP, TFP required

        /// <summary>
        /// Checks all required and optional forms for different visit types.
        /// </summary>
        [Required(ErrorMessage = "All included and required forms must be completed before checklist can be completed")]
        public bool? ChecklistCanBeCompleted
        {
            get
            {
                if (Visit.VisitType == VisitType.PRE)
                    return true;
                else if (Visit.VisitType == VisitType.IVP)
                {
                    // https://files.alz.washington.edu/documentation/uds3-ivp-z1x.pdf
                    // all child properties of the checklist are optional
                    // all properties within this form check their own rules
                    if (this.A1_IsIncluded.HasValue // required
                        && this.Checklist.A2_IsIncluded.HasValue
                        && this.Checklist.A3_IsIncluded.HasValue
                        && this.Checklist.A4_IsIncluded.HasValue
                        && this.A5_IsIncluded.HasValue
                        && this.Checklist.B1_IsIncluded.HasValue
                        && this.B4_IsIncluded.HasValue
                        && this.Checklist.B5_IsIncluded.HasValue
                        && this.Checklist.B6_IsIncluded.HasValue
                        && this.Checklist.B7_IsIncluded.HasValue
                        && this.B8_IsIncluded.HasValue
                        && this.B9_IsIncluded.HasValue
                        && this.C2_IsIncluded.HasValue
                        && this.D1_IsIncluded.HasValue
                        && this.D2_IsIncluded.HasValue)
                    {
                        return true;
                    }
                }
                else if (Visit.VisitType == VisitType.FVP)
                {
                    // https://files.alz.washington.edu/documentation/uds3-fvp-z1x.pdf
                    // No A5
                    if (this.A1_IsIncluded.HasValue
                        && this.Checklist.A2_IsIncluded.HasValue
                        && this.Checklist.A3_IsIncluded.HasValue
                        && this.Checklist.A4_IsIncluded.HasValue
                        && this.Checklist.B1_IsIncluded.HasValue
                        && this.B4_IsIncluded.HasValue
                        && this.Checklist.B5_IsIncluded.HasValue
                        && this.Checklist.B6_IsIncluded.HasValue
                        && this.Checklist.B7_IsIncluded.HasValue
                        && this.B8_IsIncluded.HasValue
                        && this.B9_IsIncluded.HasValue
                        && this.C2_IsIncluded.HasValue
                        && this.D1_IsIncluded.HasValue
                        && this.D2_IsIncluded.HasValue)
                    {
                        return true;
                    }
                }
                else if (Visit.VisitType == VisitType.TFP)
                {
                    // https://files.alz.washington.edu/documentation/uds3-tfp-z1x.pdf
                    // No A5, B1, B8
                    if (this.T1_IsIncluded.HasValue
                        && this.A1_IsIncluded.HasValue
                        && (this.Checklist.A2_IsIncluded.HasValue && this.Visit.CoParticipantDemographics != null && this.Visit.CoParticipantDemographics.FormStatus == FormStatus.Complete) // A2 must be completed
                        && this.Checklist.A3_IsIncluded.HasValue
                        && this.Checklist.A4_IsIncluded.HasValue
                        && this.B4_IsIncluded.HasValue
                        && this.Checklist.B5_IsIncluded.HasValue
                        && this.Checklist.B7_IsIncluded.HasValue
                        && this.B9_IsIncluded.HasValue
                        && this.D1_IsIncluded.HasValue
                        && this.D2_IsIncluded.HasValue)
                    {
                        return true;
                    }
                }
                return null;
            }
        }

        public VisitOverviewViewModel Visit { get; set; } // for statuses 

    }
}
