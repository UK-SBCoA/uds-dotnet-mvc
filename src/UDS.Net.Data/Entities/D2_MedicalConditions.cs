using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using UDS.Net.Data.DataAnnotations;
using UDS.Net.Data.Enums;

namespace UDS.Net.Data.Entities
{
    [Table("tbl_D2")]
    public class MedicalConditions : FormBase
    {
        [Display(Name = "Cancer (excluding non-melanoma skin cancer), primary or metastatic")]
        [RequiredIf(nameof(FormStatus), FormStatus.Complete, ErrorMessage = "Please indicate presence")]
        [Column("CANCER")]
        public int? Cancer { get; set; }

        [Display(Name = "If yes, specify primary site")]
        [RequiredIf(nameof(Cancer), 1, ErrorMessage = "Please indicate presence")]
        [RequiredIf(nameof(Cancer), 2, ErrorMessage = "Please indicate presence")]
        [Column("CANCSITE")]
        [MaxLength(60)]
        public string CancerSite { get; set; }

        [Display(Name = "Diabetes")]
        [RequiredIf(nameof(FormStatus), FormStatus.Complete, ErrorMessage = "Please indicate presence")]
        [Column("DIABET")]
        public int? Diabetes { get; set; }

        [Display(Name = "Myocardial infarct")]
        [RequiredIf(nameof(FormStatus), FormStatus.Complete, ErrorMessage = "Please indicate presence")]
        [Column("MYOINF")]
        public int? MyocardialInfarct { get; set; }

        [Display(Name = "Congestive heart failure")]
        [RequiredIf(nameof(FormStatus), FormStatus.Complete, ErrorMessage = "Please indicate presence")]
        [Column("CONGHRT")]
        public int? CongestiveHeartFailure { get; set; }

        [Display(Name = "Atrial fibrillation")]
        [RequiredIf(nameof(FormStatus), FormStatus.Complete, ErrorMessage = "Please indicate presence")]
        [Column("AFIBRILL")]
        public int? AtrialFibrillation { get; set; }

        [Display(Name = "Hypertension")]
        [RequiredIf(nameof(FormStatus), FormStatus.Complete, ErrorMessage = "Please indicate presence")]
        [Column("HYPERT")]
        public int? Hypertension { get; set; }

        [Display(Name = "Angina")]
        [RequiredIf(nameof(FormStatus), FormStatus.Complete, ErrorMessage = "Please indicate presence")]
        [Column("ANGINA")]
        public int? Angina { get; set; }

        [Display(Name = "Hypercholesterolemia")]
        [RequiredIf(nameof(FormStatus), FormStatus.Complete, ErrorMessage = "Please indicate presence")]
        [Column("HYPCHOL")]
        public int? Hypercholesterolemia { get; set; }

        [Display(Name = "B12 deficiency")]
        [RequiredIf(nameof(FormStatus), FormStatus.Complete, ErrorMessage = "Please indicate presence")]
        [Column("VB12DEF")]
        public int? B12Deficiency { get; set; }

        [Display(Name = "Thyroid disease")]
        [RequiredIf(nameof(FormStatus), FormStatus.Complete, ErrorMessage = "Please indicate presence")]
        [Column("THYDIS")]
        public int? ThyroidDisease { get; set; }

        [Display(Name = "Arthritis")]
        [RequiredIf(nameof(FormStatus), FormStatus.Complete, ErrorMessage = "Please indicate presence")]
        [Column("ARTH")]
        public int? Arthritis { get; set; }

        [Display(Name = "If yes, what type?")]
        [RequiredIf(nameof(Arthritis), 1, ErrorMessage = "Please indicate presence")]
        [Column("ARTYPE")]
        public int? ArthritisType { get; set; }

        [Display(Name = "Other (Specify)")]
        [RequiredIf(nameof(ArthritisType), 3, ErrorMessage = "Please indicate presence")]
        [MaxLength(60)]
        [Column("ARTYPEX")]
        public string ArthritisTypeSpecified { get; set; }

        /// <summary>
        /// If yes, regions affected (check at least one)
        /// </summary>

        [Display(Name = "Upper extremity")]
        [Column("ARTUPEX")]
        public bool? ArthritisRegionUpperExtremity { get; set; }

        [Display(Name = "Lower extremity")]
        [Column("ARTLOEX")]
        public bool? ArthritisRegionLowerExtremity { get; set; }

        [Display(Name = "Spine")]
        [Column("ARTSPIN")]
        public bool? ArthritisRegionSpine { get; set; }

        [Display(Name = "Unknown")]
        [Column("ARTUNKN")]
        public bool? ArthritisRegionUnknown { get; set; }

        [RequiredIf(nameof(Arthritis), 1, ErrorMessage = "Please indicate region(s) affected")]
        [NotMapped]
        public bool? ArthritisRegionIndicated
        {
            get
            {
                if ((ArthritisRegionUpperExtremity.HasValue && ArthritisRegionUpperExtremity.Value == true) ||
                    (ArthritisRegionLowerExtremity.HasValue && ArthritisRegionLowerExtremity.Value == true) ||
                    (ArthritisRegionSpine.HasValue && ArthritisRegionSpine.Value == true) ||
                    (ArthritisRegionUnknown.HasValue && ArthritisRegionUnknown.Value == true))
                {
                    return true;
                }
                return null;
            }
        }

        [Display(Name = "Incontinence — urinary")]
        [RequiredIf(nameof(FormStatus), FormStatus.Complete, ErrorMessage = "Please indicate presence")]
        [Column("URINEINC")]
        public int? IncontinenceUrinary { get; set; }

        [Display(Name = "Incontinence — bowel")]
        [RequiredIf(nameof(FormStatus), FormStatus.Complete, ErrorMessage = "Please indicate presence")]
        [Column("BOWLINC")]
        public int? IncontinenceBowel { get; set; }

        [Display(Name = "Sleep apnea")]
        [RequiredIf(nameof(FormStatus), FormStatus.Complete, ErrorMessage = "Please indicate presence")]
        [Column("SLEEPAP")]
        public int? SleepApnea { get; set; }

        [Display(Name = "REM sleep behavior disorder (RBD)")]
        [RequiredIf(nameof(FormStatus), FormStatus.Complete, ErrorMessage = "Please indicate presence")]
        [Column("REMDIS")]
        public int? REMSleepBehaviorDisorder { get; set; }

        [Display(Name = "Hyposomnia/ insomnia")]
        [RequiredIf(nameof(FormStatus), FormStatus.Complete, ErrorMessage = "Please indicate presence")]
        [Column("HYPOSOM")]
        public int? HyposomniaInsomnia { get; set; }

        [Display(Name = "Other sleep disorder")]
        [RequiredIf(nameof(FormStatus), FormStatus.Complete, ErrorMessage = "Please indicate presence")]
        [Column("SLEEPOTH")]
        public int? OtherSleepDisorder { get; set; }

        [Display(Name = "Other sleep disorder (Specified)")]
        [RequiredIf(nameof(OtherSleepDisorder), 1, ErrorMessage = "Please indicate presence")]
        [Column("SLEEPOTX")]
        [MaxLength(60)]
        public string OtherSleepDisorderSpecified { get; set; }

        [Display(Name = "Carotid procedure: angioplasty, endarterectomy, or stent")]
        [RequiredIf(nameof(FormStatus), FormStatus.Complete, ErrorMessage = "Please indicate presence")]
        [Column("ANGIOCP")]
        public int? CarotidProcedure { get; set; }

        [Display(Name = "Percutaneous coronary intervention: angioplasty and/or stent")]
        [RequiredIf(nameof(FormStatus), FormStatus.Complete, ErrorMessage = "Please indicate presence")]
        [Column("ANGIOPCI")]
        public int? PercutaneousCoronaryIntervention { get; set; }

        [Display(Name = "Procedure: pacemaker and/or defibrillator")]
        [RequiredIf(nameof(FormStatus), FormStatus.Complete, ErrorMessage = "Please indicate presence")]
        [Column("PACEMAKE")]
        public int? PacemakerDefibrillator { get; set; }

        [Display(Name = "Procedure: heart valve replacement or repair")]
        [RequiredIf(nameof(FormStatus), FormStatus.Complete, ErrorMessage = "Please indicate presence")]
        [Column("HVALVE")]
        public int? HeartValveReplacementRepair { get; set; }

        [Display(Name = "Antibody-mediated encephalopathy")]
        [RequiredIf(nameof(FormStatus), FormStatus.Complete, ErrorMessage = "Please indicate presence")]
        [Column("ANTIENC")]
        public int? AntibodyMediatedEncephalopathy { get; set; }

        [Display(Name = "SpecifyAntibody")]
        [RequiredIf(nameof(AntibodyMediatedEncephalopathy), 1, ErrorMessage = "Please indicate presence")]
        [Column("ANTIENCX")]
        [MaxLength(60)]
        public string AntibodyMediatedEncephalopathySpecified { get; set; }

        [Display(Name = "Other medical conditions or procedures not listed above")]
        [RequiredIf(nameof(FormStatus), FormStatus.Complete, ErrorMessage = "Please indicate presence")]
        [Column("OTHCOND")]
        public bool? OtherMedicalCondition { get; set; }

        [Display(Name = "(If yes, specify)")]
        [RequiredIf(nameof(OtherMedicalCondition), true, ErrorMessage = "Please indicate presence")]
        [Column("OTHCONDX")]
        [MaxLength(60)]
        public string OtherMedicalConditionSpecified { get; set; }

    }
}
