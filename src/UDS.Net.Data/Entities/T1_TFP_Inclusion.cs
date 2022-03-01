using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using COA.Components.Web.DataAnnotations;
using UDS.Net.Data.Enums;

namespace UDS.Net.Data.Entities
{
    [Table("tbl_T1")]
    public class Inclusion : FormBase
    {
        [Display(Name = "Participant is too cognitively impaired for an in-person UDS visit.")]
        [Column("TELCOG")]
        [RequiredIf(nameof(FormStatus), FormStatus.Complete, ErrorMessage = "Field required")]
        public bool? CognitivelyImpaired { get; set; }

        [Display(Name = "Participant is too physically impaired (medical illness or injury) to attend an in-person UDS visit.")]
        [Column("TELILL")]
        [RequiredIf(nameof(FormStatus), FormStatus.Complete, ErrorMessage = "Field required")]
        public bool? PhysicallyImpaired { get; set; }

        [Display(Name = "Subject is homebound or in a nursing home and cannot travel.")]
        [Column("TELHOME")]
        [RequiredIf(nameof(FormStatus), FormStatus.Complete, ErrorMessage = "Field required")]
        public bool? Homebound { get; set; }

        [Display(Name = "Subject or co-participant refused an in-person UDS visit.")]
        [Column("TELREFU")]
        [RequiredIf(nameof(FormStatus), FormStatus.Complete, ErrorMessage = "Field required")]
        public bool? Refused { get; set; }

        [Display(Name = "Other")]
        [Column("TELOTHR")]
        [RequiredIf(nameof(FormStatus), FormStatus.Complete, ErrorMessage = "Field required")]
        public bool? Other { get; set; }

        [Display(Name = "Other (SPECIFY)")]
        [MaxLength(60)]
        [Column("TELOTHRX")]
        [RequiredIf(nameof(Other), true, ErrorMessage = "Field required")]
        public string OtherSpecified { get; set; }

        /// <summary>
        /// If 0 or 9 continue
        /// If 1 end form
        /// </summary>
        [Display(Name = "Is the subject likely to resume in-person UDS follow-up evaluation?")]
        [Column("TELINPER")]
        [RequiredIf(nameof(FormStatus), FormStatus.Complete, ErrorMessage = "Field required")]
        public int? LikelyToResumeInPerson { get; set; }

        [Display(Name = "Has a Milestones Form documenting the change to telephone follow-up been completed? (If no, complete a Milestones Form now.)")]
        [Column("TELMILE")]
        [RequiredIf(nameof(LikelyToResumeInPerson), 0, ErrorMessage = "Field required")]
        [RequiredIf(nameof(LikelyToResumeInPerson), 9, ErrorMessage = "Field required")]
        public int? MilestoneFormCompleted { get; set; }

        /// <summary>
        /// TODO remove this because we're using temporal tables
        /// </summary>
        public DateTime ModifiedAt { get; set; }

    }
}
