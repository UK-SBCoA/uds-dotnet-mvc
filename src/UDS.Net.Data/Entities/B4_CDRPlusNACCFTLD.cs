using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using COA.Components.Web.DataAnnotations;
using UDS.Net.Data.Enums;

namespace UDS.Net.Data.Entities
{
    [Display(Name = "B4: Clinical Dementia Rating (CDR) STANDARD AND SUPPLEMENTAL")]
    [Table("tbl_B4")]
    public class CDRPlusNACCFTLD: FormBase
    {
        [Display(Name = "1. Memory", GroupName ="Standard CDR")]
        [Range(0, 3, ErrorMessage = "Please provide a valid score")]
        [Column("MEMORY")]
        [RequiredIf(nameof(FormStatus), FormStatus.Complete, ErrorMessage="Please provide a score")]
        public double? Memory {get;set;}
        [Display(Name = "2. Orientation", GroupName ="Standard CDR")]
        [Range(0, 3, ErrorMessage = "Please provide a valid score")]
        [Column("ORIENT")]
        [RequiredIf(nameof(FormStatus), FormStatus.Complete, ErrorMessage="Please provide a score")]
        public double? Orientation {get;set;}
        [Display(Name = "3. Judgment and problem solving", GroupName ="Standard CDR")]
        [Range(0, 3, ErrorMessage = "Please provide a valid score")]
        [Column("JUDGMENT")]
        [RequiredIf(nameof(FormStatus), FormStatus.Complete, ErrorMessage="Please provide a score")]
        public double? JudgmentAndProblemSolving {get;set;}
        [Display(Name = "4. Community affairs", GroupName ="Standard CDR")]
        [Range(0, 3, ErrorMessage = "Please provide a valid score")]
        [Column("COMMUN")]
        [RequiredIf(nameof(FormStatus), FormStatus.Complete, ErrorMessage="Please provide a score")]
        public double? CommunityAffairs {get;set;}
        [Display(Name = "5. Homes and hobbies", GroupName ="Standard CDR")]
        [Range(0, 3, ErrorMessage = "Please provide a valid score")]
        [Column("HOMEHOBB")]
        [RequiredIf(nameof(FormStatus), FormStatus.Complete, ErrorMessage="Please provide a score")]
        public double? HomesAndHobbies {get;set;}
        [Display(Name = "6. Personal care", GroupName ="Standard CDR")]
        [Range(0, 3, ErrorMessage = "Please provide a valid score")]
        [Column("PERSCARE")]
        [RequiredIf(nameof(FormStatus), FormStatus.Complete, ErrorMessage="Please provide a score")]
        public double? PersonalCare {get;set;}
        [Display(Name = "Standard CDR sum of boxes", GroupName ="Standard CDR")]
        [Range(0, 18, ErrorMessage = "Please provide a valid score")]
        // New column CDRSUM
        [Column("CDRSUM")]
        public double? StandardCDRSumOfBoxes {get;set;}
        [Display(Name = "Standard Global CDR", GroupName ="Standard CDR")]
        [Range(0, 3, ErrorMessage = "Please provide a valid score")]
        [Column("CDRGLOB")]
        public double? StandardGlobalCDR {get;set;}
        [Display(Name = "9. Behavior comportment and personality", GroupName ="Supplimental CDR")]
        [Range(0, 3, ErrorMessage = "Please provide a valid score")]
        [Column("COMPORT")]
        [RequiredIf(nameof(FormStatus), FormStatus.Complete, ErrorMessage="Please provide a score")]
        public double? BehaviorComportmentAndPersonality {get;set;}
        [Display(Name = "10. Language", GroupName ="Supplimental CDR")]
        [Range(0, 3, ErrorMessage = "Please provide a valid score")]
        [Column("CDRLANG")]
        [RequiredIf(nameof(FormStatus), FormStatus.Complete, ErrorMessage="Please provide a score")]
        public double? Language {get;set;}
        [Display(Name = "Supplemental CDR sum of boxes", GroupName ="Standard CDR")]
        [Column("CDRSUPP")]
        [Range(0, 6, ErrorMessage = "Please provide a valid score")]
        public double? SupplementalCDRSumOfBoxes {get;set;}
        [Display(Name = "Standard & supplemental CDR sum of boxes", GroupName ="Standard CDR")]
        [Range(0, 24, ErrorMessage = "Please provide a valid score")]
        [Column("CDRTOT")]
        public double? SupplementalGlobalCDR {get;set;}
    }
}