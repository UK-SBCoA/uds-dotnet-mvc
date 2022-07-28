using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using UDS.Net.Data.DataAnnotations;
using UDS.Net.Data.Enums;

namespace UDS.Net.Data.Entities
{
  [Table("tbl_B7")]
  public class FunctionalActivitiesQuestionnaire: FormBase
  {
    [RequiredIf(nameof(FormStatus), FormStatus.Complete, ErrorMessage = "Please provide a value for this question")]
    [Display(Name = "1. Writing checks, paying bills, or balancing a checkbook")]
    [Column("BILLS")]
    public int? Bills { get; set; }
    [RequiredIf(nameof(FormStatus), FormStatus.Complete,ErrorMessage =  "Please provide a value for this question")]
    [Display(Name = "2. Assembling tax records, business affairs, or other papers")]
    [Column("TAXES")]
    public int? Taxes { get; set; }
    [RequiredIf(nameof(FormStatus), FormStatus.Complete,ErrorMessage =  "Please provide a value for this question")]
    [Display(Name = "3. Shopping alone for clothes, household necessities, or groceries")]
    [Column("SHOPPING")]
    public int? Shopping { get; set; }
    [RequiredIf(nameof(FormStatus), FormStatus.Complete, ErrorMessage = "Please provide a value for this question")]
    [Display(Name = "4. In the past four weeks, did the subject have any difficulty or need help with: Playing a game of skill such as bridge or chess, working on a hobby")]
    [Column("GAMES")]
    public int? Games { get; set; }
    [RequiredIf(nameof(FormStatus), FormStatus.Complete, ErrorMessage = "Please provide a value for this question")]
    [Display(Name = "5. In the past four weeks, did the subject have any difficulty or need help with: Heating water, making a cup of coffee, turning off the stove")]
    [Column("STOVE")]
    public int? Stove { get; set; }
    [RequiredIf(nameof(FormStatus), FormStatus.Complete, ErrorMessage = "Please provide a value for this question")]
    [Display(Name = "6. Preparing a balanced meal")]
    [Column("MEALPREP")]
    public int? MealPrep { get; set; }
    [RequiredIf(nameof(FormStatus), FormStatus.Complete, ErrorMessage = "Please provide a value for this question")]
    [Display(Name = "7. Keeping track of current events")]
    [Column("EVENTS")]
    public int? Events { get; set; }
    [RequiredIf(nameof(FormStatus), FormStatus.Complete,ErrorMessage =  "Please provide a value for this question")]
    [Display(Name = "8. Paying attention to and understanding a TV program, book, or magazine")]
    [Column("PAYATTN")]
    public int? PayAttention { get; set; }
    [RequiredIf(nameof(FormStatus), FormStatus.Complete,ErrorMessage =  "Please provide a value for this question")]
    [Display(Name = "9. Remembering appointments, family occasions, holidays, medications")]
    [Column("REMDATES")]
    public int? RememberDates { get; set; }
    [RequiredIf(nameof(FormStatus), FormStatus.Complete,ErrorMessage =  "Please provide a value for this question")]
    [Display(Name = "10. Traveling out of the neighborhood, driving, or arranging to take public transportation")]
    [Column("TRAVEL")]
    public int? Travel { get; set; }
  }
}