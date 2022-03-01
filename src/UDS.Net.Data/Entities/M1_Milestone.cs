using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using COA.Components.Web.DataAnnotations;
using UDS.Net.Data.Enums;

namespace UDS.Net.Data.Entities
{
    /// <summary>
    /// Paper form
    /// https://files.alz.washington.edu/documentation/uds3-milestone-form.pdf
    /// Data element dictionary
    /// https://files.alz.washington.edu/documentation/uds3-milestone-ded.pdf
    /// </summary>
    [Table("tbl_M1")]
    public class Milestone
    {
        [Key]
        [Column("MilestoneId")]
        public int Id { get; set; }

        [Display(Name = "PTID")]
        [Column("PTID")]
        public int FriendlyId { get; set; }

        [ForeignKey("FriendlyId")]
        public Participation Participant { get; set; }

        [MaxLength(6)]
        [Column("FORMVER")]
        public string Version { get; set; } = "3.0.0";

        [MaxLength(3)]
        [Column("INITIALS")]
        public string ExaminerInitials { get; set; }

        public FormStatus FormStatus { get; set; } = FormStatus.Incomplete;

        [Display(Name = "Packet has been submitted to NACC")]
        public bool IsSubmittedToNACC { get; set; }

        [MaxLength(30)]
        public string ModifiedBy { get; set; }

        public DateTime ModifiedAt { get; set; }

        /// <summary>
        /// A = Box A Continued Contact
        /// B = Box B No Further Contact
        /// </summary>
        [RequiredIf(nameof(FormStatus), FormStatus.Complete, ErrorMessage = "Please select milestone type")]
        public string MilestoneType { get; set; }

        /// <summary>
        /// 99 = unknown
        /// </summary>
        [RequiredIf(nameof(MilestoneType), "A", ErrorMessage = "Please provide status change month")]
        [Column("CHANGEMO")]
        [Range(1, 99, ErrorMessage = "Provide month or 99 for unknown")]
        [InvalidRange(nameof(StatusChangeMonth), 13, 98, ErrorMessage = "Provide month or 99 for unknown")]
        public int? StatusChangeMonth { get; set; }

        /// <summary>
        /// 99 = unknown
        /// </summary>
        [RequiredIf(nameof(MilestoneType), "A", ErrorMessage = "Please provide status change day")]
        [Column("CHANGEDY")]
        [Range(1, 99, ErrorMessage = "Provide day or 99 for unknown")]
        [InvalidRange(nameof(StatusChangeDay), 32, 98, ErrorMessage = "Provide day or 99 for unknown")]
        public int? StatusChangeDay { get; set; }

        /// <summary>
        /// Required
        /// </summary>
        [RequiredIf(nameof(MilestoneType), "A", ErrorMessage = "Please provide status change year")]
        [Column("CHANGEYR")]
        [Range(2015, 2999, ErrorMessage = "Provide a valid year between 2015-2999")]
        public int? StatusChangeYear { get; set; }

        /// <summary>
        /// 1 = Annual UDS follow-up by telephone
        /// 2 = Minimal contact
        /// 3 = Annual in-person UDS follow-up
        /// </summary>
        [Column("PROTOCOL")]
        [RequiredIf(nameof(MilestoneType), "A", ErrorMessage = "Required")]
        // If nothing else is selected then NewStatus should be 3
        public int? NewStatus { get; set; }

        [Column("ACONSENT")]
        [RequiredIf(nameof(NewStatus), 1, ErrorMessage = "Autopsy status required")]
        [RequiredIf(nameof(NewStatus), 2, ErrorMessage = "Autopsy status required")]
        public bool? AutopsyConsentOnFile { get; set; }

        /// <summary>
        /// TODO Some of these are related to T1 if the visit is TFP. Might be a place for
        /// global validation pilot.
        /// </summary>
        #region Reasons for change indicated in NewStatus
        [Display(Name = "Participant is too cognitively impaired.")]
        [Column("RECOGIM")]
        public bool? ReasonCognitivelyImpaired { get; set; }

        [Display(Name = "Participant is too ill or physically impaired.")]
        [Column("REPHYILL")]
        public bool? ReasonPhysicallyImpaired { get; set; }

        [Display(Name = "Participant refuses neuropsychological testing or clinical exam.")]
        [Column("REREFUSE")]
        public bool? ReasonRefused { get; set; }

        [Display(Name = "Participant or co-participant unreachable, not available, or moved away.")]
        [Column("RENAVAIL")]
        public bool? ReasonUnavailable { get; set; }

        [Display(Name = "Participant has permanently entered nursing home.")]
        [Column("RENURSE")]
        public bool? ReasonPermanentAssistedLiving { get; set; }

        [NotMapped]
        [RequiredIf(nameof(MilestoneType), "A", ErrorMessage = "Please provide a reason")]
        public bool? ReasonProvided
        {
            get
            {
                bool? reasonProvided = null;
                if (ReasonCognitivelyImpaired.HasValue && ReasonCognitivelyImpaired.Value == true)
                    reasonProvided = true;
                if (ReasonPhysicallyImpaired.HasValue && ReasonPhysicallyImpaired.Value == true)
                    reasonProvided = true;
                if (ReasonRefused.HasValue && ReasonRefused.Value == true)
                    reasonProvided = true;
                if (ReasonUnavailable.HasValue && ReasonUnavailable.Value == true)
                    reasonProvided = true;
                if (ReasonPermanentAssistedLiving.HasValue && ReasonPermanentAssistedLiving.Value == true)
                    reasonProvided = true;

                return reasonProvided;
            }
        }

        /// <summary>
        /// 99 = unknown
        /// </summary>
        [Column("NURSEMO")]
        [RequiredIf(nameof(ReasonPermanentAssistedLiving), true, ErrorMessage = "Please provide a month or 99 = unknown")]
        [Range(1, 99, ErrorMessage = "Provide month or 99 for unknown")]
        [InvalidRange(nameof(PermanentAssistedLivingMonth), 13, 98, ErrorMessage = "Provide month or 99 for unknown")]
        public int? PermanentAssistedLivingMonth { get; set; }

        /// <summary>
        /// 99 = unknown
        /// </summary>
        [Column("NURSEDY")]
        [RequiredIf(nameof(ReasonPermanentAssistedLiving), true, ErrorMessage = "Please provide a day or 99 = unknown")]
        [Range(1, 99, ErrorMessage = "Provide day or 99 for unknown")]
        [InvalidRange(nameof(PermanentAssistedLivingDay), 32, 98, ErrorMessage = "Provide day or 99 for unknown")]
        public int? PermanentAssistedLivingDay { get; set; }

        /// <summary>
        /// Required
        /// </summary>
        [Column("NURSEYR")]
        [RequiredIf(nameof(ReasonPermanentAssistedLiving), true, ErrorMessage = "Please provide a year")]
        [Range(2015, 2999, ErrorMessage = "Provide a valid year between 2015-2999")]
        public int? PermanentAssistedLivingYear { get; set; }

        [Display(Name = "Participant is REJOINING ADC.")]
        [Column("REJOIN")]
        public bool? ReasonRejoiningADC { get; set; }
        #endregion

        [Display(Name = "Participant will no longer receive FTLD Module follow-up, but annual in-person UDS visits will continue")]
        [Column("FTLDDISC")]
        public bool? FTLDStatus { get; set; }

        /// <summary>
        /// 1 = ADC decision
        /// 2 = Subject/informant refused
        /// 3 = Informant not available
        /// 4 = Other, specify below
        /// </summary>
        [Display(Name = "FTLD Reason")]
        [Column("FTLDREAS")]
        [RequiredIf(nameof(FTLDStatus), true, ErrorMessage = "Please provide a reason for FTLD change")]
        public int? FTLDReason { get; set; }

        [MaxLength(60)]
        [Display(Name = "FTLD Reason Other")]
        [Column("FTLDREAX")]
        [RequiredIf(nameof(FTLDReason), 4, ErrorMessage = "Please specify the other reason")]
        public string FTLDReasonOther { get; set; }

        #region NO FURTHER ADC CONTACT

        [NotMapped]
        [RequiredIf(nameof(MilestoneType), "B", ErrorMessage = "Please provide a reason")]
        public bool? NoFurtherContactReasonProvided
        {
            get
            {
                bool? reasonProvided = null;
                if (ParticipantIsDeceased.HasValue && ParticipantIsDeceased.Value == true)
                    reasonProvided = true;
                if (ParticipantHasWithdrawn.HasValue && ParticipantHasWithdrawn.Value == true)
                    reasonProvided = true;
                return reasonProvided;
            }
        }

        [Display(Name = "Participant has died")]
        [Column("DECEASED")]
        public bool? ParticipantIsDeceased { get; set; }

        /// <summary>
        /// 99 = unknown
        /// </summary>
        [RequiredIf(nameof(ParticipantIsDeceased), true, ErrorMessage = "Month required")]
        [Range(1, 99, ErrorMessage = "Provide month or 99 for unknown")]
        [Column("DEATHMO")]
        public int? DateOfDeathMonth { get; set; }

        /// <summary>
        /// 99 = unknown
        /// </summary>
        [RequiredIf(nameof(ParticipantIsDeceased), true, ErrorMessage = "Day required")]
        [Range(1, 99, ErrorMessage = "Provide day or 99 for unknown")]
        [Column("DEATHDY")]
        public int? DateOfDeathDay { get; set; }

        /// <summary>
        /// Required
        /// </summary>
        [RequiredIf(nameof(ParticipantIsDeceased), true, ErrorMessage = "Year required")]
        [Range(2015, 2999, ErrorMessage = "Provide a valid year between 2015-2999")]
        [Column("DEATHYR")]
        public int? DateOfDeathYear { get; set; }

        /// <summary>
        /// 0 = No ADC autopsy expected
        /// 1 = An ADC autopsy has been done; data submitted or pending
        /// </summary>
        [RequiredIf(nameof(ParticipantIsDeceased), true, ErrorMessage = "Required")]
        [Column("AUTOPSY")]
        public bool? ADCAutopsyStatus { get; set; }

        /// <summary>
        /// If chosen, no further contact should be made
        /// </summary>
        [Display(Name = "Participant has been dropped from ADC")]
        [Column("DISCONT")]
        public bool? ParticipantHasWithdrawn { get; set; }

        /// <summary>
        /// Optional
        /// 99 = unknown
        /// </summary>
        [RequiredIf(nameof(ParticipantHasWithdrawn), true, ErrorMessage = "Month required")]
        [Range(1, 99, ErrorMessage = "Provide month or 99 for unknown")]
        [Column("DISCMO")]
        public int? WithdrawnMonth { get; set; }

        /// <summary>
        /// Optional
        /// 99 = unknown
        /// </summary>
        [RequiredIf(nameof(ParticipantHasWithdrawn), true, ErrorMessage = "Day required")]
        [Range(1, 99, ErrorMessage = "Provide day or 99 for unknown")]
        [Column("DISCDY")]
        public int? WithdrawnDay { get; set; }

        /// <summary>
        /// Required
        /// </summary>
        [RequiredIf(nameof(ParticipantHasWithdrawn), true, ErrorMessage = "Year required")]
        [Range(2015, 2999, ErrorMessage = "Provide a valid year between 2015-2999")]
        [Column("DISCYR")]
        public int? WithdrawnYear { get; set; }

        /// <summary>
        ///  1 = ADC decision or protocol
        ///  2 = Subject or coparticipant asked to be dropped
        /// </summary>
        [RequiredIf(nameof(ParticipantHasWithdrawn), true, ErrorMessage = "Required")]
        [Column("DROPREAS")]
        public int? WithdrawnPrimaryReason { get; set; }

        #endregion

        public string Comments { get; set; }

        public string CommentsDisplay { get; set; }

        /// TODO There is no ReasonX in UDS 3 DED spec for M1
    }
}
