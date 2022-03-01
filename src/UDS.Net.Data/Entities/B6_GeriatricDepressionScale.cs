using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using COA.Components.Web.DataAnnotations;
using UDS.Net.Data.Enums;

namespace UDS.Net.Data.Entities
{
    [Table("tbl_B6")]
    public class GeriatricDepressionScale: FormBase
    {
        [Display(Name ="Check this box and enter \"88\" below for the Total GDS Score if and only if the subject: 1.) does not attempt the GDS, or 2.) answers fewer than 12 questions.")]
        [Column("NOGDS")]
        public bool? NoGDS {get;set;}
        [Display(Name = "1. Are you basically satisfied with your life?")]
        [Column("SATIS")]
        public int? Satisfaction {get;set;}
        [Display(Name = "2. Have you dropped many of your activities and interests?")]
        [Column("DROPACT")]
        public int? ActivityInterests  {get;set;}
        [Display(Name = "3.	Do you feel that your life is empty?")]
        [Column("EMPTY")]
        public int? Empty {get;set;}
        [Display(Name = "4. Do you often get bored?")]
        [Column("BORED")]
        public int? Bored {get;set;}
        [Display(Name = "5. Are you in good spirits most of the time?")]
        [Column("SPIRITS")]
        public int? Spirits {get;set;}
        [Display(Name = "6. Are you afraid that something bad is going to happen to you?")]
        [Column("AFRAID")]
        public int? Afraid {get;set;}
        [Display(Name = "7. Do you feel happy most of the time?")]
        [Column("HAPPY")]
        public int? Happy {get;set;}
        [Display(Name =  "8. Do you often feel helpless?")]
        [Column("HELPLESS")]
        public int? Helpless {get;set;}
        [Display(Name = "9. Do you prefer to stay at home, rather than going out and doing new things?")]
        [Column("STAYHOME")]
        public int? StayHome {get;set;}
        [Display(Name = "10. Do you feel you have more problems with memory than most?")]
        [Column("MEMPROB")]
        public int? MemProb {get;set;}
        [Display(Name = "11. Do you think it is wonderful to be alive now?")]
        [Column("WONDRFUL")]
        public int? Wonderful {get;set;}
        [Display(Name = "12. Do you feel pretty worthless the way you are now?")]
        [Column("WRTHLESS")]
        public int? Worthless {get;set;}
        [Display(Name = "13. Do you feel full of energy?")]
        [Column("ENERGY")]
        public int? Energy {get;set;}
        [Display(Name = "14. Do you feel that your situation is hopeless?")]
        [Column("HOPELESS")]
        public int? Hopeless {get;set;}
        [Display(Name = "15. Do you think that most people are better off than you are?")]
        [Column("BETTER")]
        public int? Better {get;set;}
        [Display(Name = "16. Sum of all circled answers for a Total GDS Score")]
        [Column("GDS")]
        [RequiredIf(nameof(FormStatus), FormStatus.Complete, ErrorMessage="Please provide a score")]
        public int? GDS {get;set;}

    }
}