using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using UDS.Net.Data.DataAnnotations;
using UDS.Net.Data.Enums;

namespace UDS.Net.Data.Entities
{
    [Table("tbl_B8")]
    public class NeurologicalExaminationFindings: FormBase
    {
        [Display(Name = "1. Were there abnormal neurological exam findings?")]
        [Column("NORMEXAM")]
        [RequiredIf(nameof(FormStatus), FormStatus.Complete, "Please indicate if there were abnormal neurological exam findings")]
        public int? AbnormalNeurologicalExamFindings {get;set;}
        [Display(Name = "2. Parkinsonian signs")]
        [Column("PARKSIGN")]
        [RequiredIf(nameof(AbnormalNeurologicalExamFindings), 1, "A value is required you have indicated abnormal neurological exam findings")]
        public bool? ParkinsonianSigns {get;set;}
        [RequiredIf(nameof(ParkinsonianSigns), true, "A value is required you have indicated parkinsonian signs")]
        [Column("RESTTRL")]
        public int? ParkinsonianLeftRestingTremorArm {get;set;}
        [RequiredIf(nameof(ParkinsonianSigns), true, "A value is required you have indicated parkinsonian signs")]
        [Column("RESTTRR")]
        public int? ParkinsonianRightRestingTremorArm {get;set;}
        [RequiredIf(nameof(ParkinsonianSigns), true, "A value is required you have indicated parkinsonian signs")]
        [Column("SLOWINGL")]
        public int? ParkinsonianRightSlowingOfFineMotorMovements {get;set;}
        [RequiredIf(nameof(ParkinsonianSigns), true, "A value is required you have indicated parkinsonian signs")]
        [Column("SLOWINGR")]
        public int? ParkinsonianLeftSlowingOfFineMotorMovements {get;set;}
        [RequiredIf(nameof(ParkinsonianSigns), true, "A value is required you have indicated parkinsonian signs")]
        [Column("RIGIDL")]
        public int? ParkinsonianRightRigidityArm {get;set;}
        [RequiredIf(nameof(ParkinsonianSigns), true, "A value is required you have indicated parkinsonian signs")]
        [Column("RIGIDR")]
        public int? ParkinsonianLeftRigidityArm {get;set;}
        [RequiredIf(nameof(ParkinsonianSigns), true, "A value is required you have indicated parkinsonian signs")]
        [Display(Name = "2d. Bradykinesia")]
        [Column("BRADY")]
        public int? ParkinsonianBradykinesia {get;set;}
        [RequiredIf(nameof(ParkinsonianSigns), true, "A value is required you have indicated parkinsonian signs")]
        [Display(Name = "2e. Parkinsonian gait disorder")]
        [Column("PARKGAIT")]
        public int? ParkinsonianGaitDisorder {get;set;}
        [RequiredIf(nameof(ParkinsonianSigns), true, "A value is required you have indicated parkinsonian signs")]
        [Display(Name  = "2f. Postural instability")]
        [Column("POSTINST")]
        public int? ParkinsonianPosturalInstability {get;set;}
        [Display(Name = "3. Neurological signs considered by examiner to be most likely consistent with cerebrovascular disease")]
        [Column("CVDSIGNS")]
        [RequiredIf(nameof(AbnormalNeurologicalExamFindings), 1, "A value is required you have indicated abnormal neurological exam findings")]
        public bool? CerebrovascularDiseaseSigns {get;set;}
        [RequiredIf(nameof(CerebrovascularDiseaseSigns), true, "A value is required you have indicated cerebrovascular disease signs")]
        [Display(Name = "3a. Cortical cognitive deficit (e.g., aphasia, apraxia, neglect)")]
        [Column("CORTDEF")]
        public int? CorticalCognitiveDeficit {get;set;}
        [RequiredIf(nameof(CerebrovascularDiseaseSigns), true, "A value is required you have indicated cerebrovascular disease signs")]
        [Display(Name = "3b. Focal or other neurological findings consistent with SIVD (subcortical ischemic vascular dementia)")]
        [Column("SIVDFIND")]
        public int? CorticalSIVD {get;set;}
        [RequiredIf(nameof(CerebrovascularDiseaseSigns), true, "A value is required you have indicated cerebrovascular disease signs")]
        [Column("CVDMOTL")]
        public int? CorticalMotorLeft {get;set;}
        [RequiredIf(nameof(CerebrovascularDiseaseSigns), true, "A value is required you have indicated cerebrovascular disease signs")]
        [Column("CVDMOTR")]
        public int? CorticalMotorRight {get;set;}
        [RequiredIf(nameof(CerebrovascularDiseaseSigns), true, "A value is required you have indicated cerebrovascular disease signs")]
        [Column("CORTVISL")]
        public int? CorticalVisualFieldLossLeft {get;set;}
        [RequiredIf(nameof(CerebrovascularDiseaseSigns), true, "A value is required you have indicated cerebrovascular disease signs")]
        [Column("CORTVISR")]
        public int? CorticalVisualFieldLossRight {get;set;}
        [RequiredIf(nameof(CerebrovascularDiseaseSigns), true, "A value is required you have indicated cerebrovascular disease signs")]
        [Column("SOMATL")]
        public int? CorticalSomatosensoryLossLeft {get;set;}
        [RequiredIf(nameof(CerebrovascularDiseaseSigns), true, "A value is required you have indicated cerebrovascular disease signs")]
        [Column("SOMATR")]
        public int? CorticalSomatosensoryLossRight {get;set;}
        [Display(Name = "4. Higher cortical visual problem suggesting posterior cortical atrophy (e.g., prosopagnosia, simultagnosia, Balint's syndrome) or apraxia of gaze")]
        [Column("POSTCORT")]
        [RequiredIf(nameof(AbnormalNeurologicalExamFindings), 1, "A value is required you have indicated abnormal neurological exam findings")]
        public bool? HigherCorticalVisual {get;set;}
        [Display(Name = "5. Findings suggestive of progressive supranuclear palsy (PSP), corticobasal syndrome, or other related disorders")]
        [Column("PSPCBS")]
        [RequiredIf(nameof(AbnormalNeurologicalExamFindings), 1, "A value is required you have indicated abnormal neurological exam findings")]
        public bool? PSP {get;set;}
        [RequiredIf(nameof(PSP), true, "A value is required you have indicated fingings suggestive of progressive supranuclear palsy (PSP), corticobasal syndrome, or other related disorders")]
        [Column("EYEPSP")]
        public int? PSP_EyeMovement {get;set;}
        [RequiredIf(nameof(PSP), true, "A value is required you have indicated fingings suggestive of progressive supranuclear palsy (PSP), corticobasal syndrome, or other related disorders")]
        [Column("DYSPSP")]
        public int? PSP_Dysarthria {get;set;}
        [RequiredIf(nameof(PSP), true, "A value is required you have indicated fingings suggestive of progressive supranuclear palsy (PSP), corticobasal syndrome, or other related disorders")]
        [Column("AXIALPSP")]
        public int? PSP_Axial {get;set;}
        [RequiredIf(nameof(PSP), true, "A value is required you have indicated fingings suggestive of progressive supranuclear palsy (PSP), corticobasal syndrome, or other related disorders")]
        [Column("GAITPSP")]
        public int? PSP_Gait {get;set;}
        [RequiredIf(nameof(PSP), true, "A value is required you have indicated fingings suggestive of progressive supranuclear palsy (PSP), corticobasal syndrome, or other related disorders")]
        [Column("APRAXSP")]
        public int? PSP_Apraxia {get;set;}
        [RequiredIf(nameof(PSP), true, "A value is required you have indicated fingings suggestive of progressive supranuclear palsy (PSP), corticobasal syndrome, or other related disorders")]
        [Column("APRAXL")]
        public int? CBS_ApraxiaLeft {get;set;}
        [RequiredIf(nameof(PSP), true, "A value is required you have indicated fingings suggestive of progressive supranuclear palsy (PSP), corticobasal syndrome, or other related disorders")]
        [Column("APRAXR")]
        public int? CBS_ApraxiaRight {get;set;}
        [RequiredIf(nameof(PSP), true, "A value is required you have indicated fingings suggestive of progressive supranuclear palsy (PSP), corticobasal syndrome, or other related disorders")]
        [Column("CORTSENL")]
        public int? CBS_CorticalSensoryDeficitsLeft {get;set;}
        [RequiredIf(nameof(PSP), true, "A value is required you have indicated fingings suggestive of progressive supranuclear palsy (PSP), corticobasal syndrome, or other related disorders")]
        [Column("CORTSENR")]
        public int? CBS_CorticalSensoryDeficitsRight {get;set;}
        [RequiredIf(nameof(PSP), true, "A value is required you have indicated fingings suggestive of progressive supranuclear palsy (PSP), corticobasal syndrome, or other related disorders")]
        [Column("ATAXL")]
        public int? CBS_AtaxiaLeft {get;set;}
        [RequiredIf(nameof(PSP), true, "A value is required you have indicated fingings suggestive of progressive supranuclear palsy (PSP), corticobasal syndrome, or other related disorders")]
        [Column("ATAXR")]
        public int? CBS_AtaxiaRight {get;set;}
        [RequiredIf(nameof(PSP), true, "A value is required you have indicated fingings suggestive of progressive supranuclear palsy (PSP), corticobasal syndrome, or other related disorders")]
        [Column("ALIENLML")]
        public int? CBS_AlienLimbLeft {get;set;}
        [RequiredIf(nameof(PSP), true, "A value is required you have indicated fingings suggestive of progressive supranuclear palsy (PSP), corticobasal syndrome, or other related disorders")]
        [Column("ALIENLMR")]
        public int? CBS_AlienLimbRight {get;set;}
        [RequiredIf(nameof(PSP), true, "A value is required you have indicated fingings suggestive of progressive supranuclear palsy (PSP), corticobasal syndrome, or other related disorders")]
        [Column("DYSTONL")]
        public int? CBS_DystoniaLeft {get;set;}    
        [RequiredIf(nameof(PSP), true, "A value is required you have indicated fingings suggestive of progressive supranuclear palsy (PSP), corticobasal syndrome, or other related disorders")]
        [Column("DYSTONR")] 
        public int? CBS_DystoniaRight {get;set;}
        [RequiredIf(nameof(PSP), true, "A value is required you have indicated fingings suggestive of progressive supranuclear palsy (PSP), corticobasal syndrome, or other related disorders")]
        [Column("MYOCLLT")]
        public int? CBS_MyoclonusLeft {get;set;}
        [RequiredIf(nameof(PSP), true, "A value is required you have indicated fingings suggestive of progressive supranuclear palsy (PSP), corticobasal syndrome, or other related disorders")]
        [Column("MYOCLRT")]
        public int? CBS_MyoclonusRight {get;set;}
        [Display(Name = "6. Findings suggesting ALS (e.g., muscle wasting, fasciculations, upper motor neuron and/or lower motor neuron signs)")]
        [Column("ALSFIND")]
        [RequiredIf(nameof(AbnormalNeurologicalExamFindings), 1, "A value is required you have indicated abnormal neurological exam findings")]
        public bool? ALS_Findings {get;set;}
        [Display(Name ="7. Normal-pressure hydrocephalus: gait apraxia")]
        [Column("GAITNPH")]
        [RequiredIf(nameof(AbnormalNeurologicalExamFindings), 1, "A value is required you have indicated abnormal neurological exam findings")]
        public bool? GaitApraxia {get;set;}
        [Display(Name = "8. Other findings (e.g., cerebellar ataxia, chorea, myoclonus) (NOTE: For this question, do not specify symptoms that have already been checked above)")]
        [Column("OTHNEUR")]
        [RequiredIf(nameof(AbnormalNeurologicalExamFindings), 1, "A value is required you have indicated abnormal neurological exam findings")]
        [RequiredIf(nameof(AbnormalNeurologicalExamFindings), 2, "A value is required you have indicated abnormal neurological exam findings")]
        public bool? OtherFindings {get;set;}
        [Column("OTHNEURX")]
        [RequiredIf(nameof(OtherFindings), true, "A value is required you have indicated abnormal neurological exam findings")]
        [MaxLength(60)]
        public string OtherFindingsSpeicify {get;set;}

    }
}