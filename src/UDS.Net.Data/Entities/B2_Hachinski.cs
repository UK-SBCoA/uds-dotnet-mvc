using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using UDS.Net.Data.DataAnnotations;
using UDS.Net.Data.Enums;

namespace UDS.Net.Data.Entities
{
    [Display(Name = "B2: Hachinski")]
    [Table("tbl_B2")]
    public class Hachinski: FormBase
    {
      [Display(Name = "Abrupt onset (re: cognitive status)")]
      [Range(0, 2, ErrorMessage = "Please provide a valid score")]
      [Column("ABRUPT")]
      [RequiredIf(nameof(FormStatus), FormStatus.Complete, ErrorMessage="Please provide a value")]
      public int? AbruptOnset {get; set;}

      [Display(Name = "Stepwise deterioration (re: cognitive status)")]
      [Range(0, 1, ErrorMessage = "Please provide a value")]
      [Column("STEPWISE")]
      [RequiredIf(nameof(FormStatus), FormStatus.Complete, ErrorMessage="Please provide a value")]
      public int? StepwiseDeterioration {get; set;}

      [Display(Name = "Somatic complaints")]
      [Range(0, 1, ErrorMessage = "Please provide a value")]
      [Column("SOMATIC")]
      [RequiredIf(nameof(FormStatus), FormStatus.Complete, ErrorMessage="Please provide a value")]
      public int? SomaticComplaints {get; set;}
   
      [Display(Name = "Emotional incontinence")]
      [Range(0, 1, ErrorMessage = "Please provide a value")]
      [Column("EMOT")]
      [RequiredIf(nameof(FormStatus), FormStatus.Complete, ErrorMessage="Please provide a value")]
      public int? EmotionalIncontinence {get; set;}
      
      [Display(Name = "History or presence of hypertension")]
      [Range(0, 1, ErrorMessage = "Please provide a value")]
      [Column("HXHYPER")]
      [RequiredIf(nameof(FormStatus), FormStatus.Complete, ErrorMessage="Please provide a value")]
      public int? Hypertension {get; set;}
      
      [Display(Name = "History of stroke")]
      [Range(0, 2, ErrorMessage = "Please provide a value")]
      [Column("HXSTROKE")]
      [RequiredIf(nameof(FormStatus), FormStatus.Complete, ErrorMessage="Please provide a value")]
      public int? Stroke {get; set;}
      
      [Display(Name = "Focal neurological symptoms")]
      [Range(0, 2, ErrorMessage = "Please provide a value")]
      [Column("FOCLSYM")]
      [RequiredIf(nameof(FormStatus), FormStatus.Complete, ErrorMessage="Please provide a value")]
      public int? Symptoms {get; set;}
      
      [Display(Name = "Focal neurological signs")]
      [Range(0, 2, ErrorMessage = "Please provide a value")]
      [Column("FOCLSIGN")]
      [RequiredIf(nameof(FormStatus), FormStatus.Complete, ErrorMessage="Please provide a value")]
      public int? Signs {get; set;}
      
      [Display(Name = "Sum all checked answers for a Total Score:")]
      [Column("HACHIN")]
      [Range(0, 12, ErrorMessage = "Please provide a valid score")]
      [RequiredIf(nameof(FormStatus), FormStatus.Complete, ErrorMessage="Please provide a score")]
      public int? HachinskiTotal {get; set;}

      [Column("COMMENTS")]
      [Display(Name = "Comments")]
      public string Comment { get; set; }
    }


}