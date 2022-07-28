using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using UDS.Net.Data.DataAnnotations;
using Microsoft.EntityFrameworkCore;
using UDS.Net.Data.Enums;

namespace UDS.Net.Data.Entities
{
    [Table("tbl_A2")]
    public class CoParticipantDemographics: FormBase
    {
        [Display(Name = "Co-Participant's birthdate day")]
        [Range(01, 31, ErrorMessage = "Birth day must be within 01 and 31")]
        [Column("INBIRDAY")]
        [RequiredIf(nameof(FormStatus), FormStatus.Complete, ErrorMessage = "Please provide birth day")]
        public int? BirthDay { get; set; }

        [Range(01, 99, ErrorMessage = "Birth month must be within 01 and 99")]
        [Display(Name = "Co-participant's birth month")]
        [Column("INBIRMO")]
        [RequiredIf(nameof(FormStatus), FormStatus.Complete, ErrorMessage = "Please provide birth month")]
        public int? BirthMonth { get; set; }

        [Display(Name = "Co-participant's birth year")]
        [Range(1900, 9999, ErrorMessage = "Birth year must be within 1900 and 9999")]
        [Column("INBIRYR")]
        [RequiredIf(nameof(FormStatus), FormStatus.Complete, ErrorMessage = "Please provide birth year")]
        public int? BirthYear { get; set; }

        [Display(Name = "Co-participant's sex")]
        [Column("INSEX")]
        [RequiredIf(nameof(FormStatus), FormStatus.Complete, ErrorMessage = "Please indicate sex")]
        public int? Sex { get; set; }

        [Display(Name = "Is this a new co-participant—ie., one who was not a co-participant at any past UDS visit?")]
        [Column("NEWINF")]
        [RequiredIf(nameof(FollowUpIsComplete), true, ErrorMessage = "Please indicate if this is a new co-participant on follow-up visits")]
        public bool? IsNewCoParticipant { get; set; }

        [NotMapped]
        public bool FollowUpIsComplete
        {
            get
            {
                if (FormStatus == FormStatus.Complete && Visit.VisitType == VisitType.FVP)
                {
                    return true;
                }
                return false;

            }
        }
        [NotMapped]
        public bool InitialVisitIsComplete
        {
            get
            {
                if (FormStatus == FormStatus.Complete && Visit.VisitType == VisitType.IVP)
                {
                    return true;
                }
                return false;

            }
        }
        [NotMapped]
        public bool ShouldCompleteDemographics {
            get {
                if((IsNewCoParticipant == true && FollowUpIsComplete) || (InitialVisitIsComplete)) {
                    return true;
                } else {
                    return false;
                }
            }
        }

        [Display(Name = "Does the co-participant report being of Hispanic/Latino ethnicity  (i.e., having origins from a mainly Spanish-speaking Latin American country), regardless of race?")]
        [Column("INHISP")]
        [RequiredIf(nameof(ShouldCompleteDemographics), true, ErrorMessage = "Please indicate Hispanic/Latino ethnicity")]
        public int? HispanicLatinoEthnicity { get; set; }

        [Display(Name = "If yes, what are the co-participant's reported origins?")]
        [Column("INHISPOR")]
        [RequiredIf(nameof(HispanicLatinoEthnicity), 1, ErrorMessage = "Please indicate ethnic origins")]
        public int? EthnicOrigins { get; set; }

        [Display(Name = "Other (SPECIFY)")]
        [Column("INHISPOX")]
        [MaxLength(60)]
        [RequiredIf(nameof(EthnicOrigins), 50, ErrorMessage = "Please provide other")]
        public string EthnicOriginsOther { get; set; }

        [Display(Name = "What does the co-participant report as his or her race?")]
        [Column("INRACE")]
        [RequiredIf(nameof(ShouldCompleteDemographics), true, ErrorMessage = "Please indicate race")]
        public int? Race { get; set; }

        [Display(Name = "Other (SPECIFY)")]
        [Column("INRACEX")]
        [RequiredIf(nameof(Race), 50, ErrorMessage = "Please provide other")]
        [MaxLength(60)]
        public string RaceOther { get; set; }

        [Display(Name = "What additional race does the co-participant report?")]
        [Column("INRASEC")]
        [RequiredIf(nameof(ShouldCompleteDemographics), true, ErrorMessage = "Please indicate secondary race")]
        public int? SecondaryRace { get; set; }

        [Display(Name = "Other (SPECIFY)")]
        [Column("INRASECX")]
        [MaxLength(60)]
        [RequiredIf(nameof(SecondaryRace), 50, ErrorMessage = "Please provide other")]
        public string SecondaryRaceOther { get; set; }

        [Display(Name = "What additional race, beyond those reported in Questions 4 and 5, does the co-participant report?")]
        [Column("INRATER")]
        [RequiredIf(nameof(ShouldCompleteDemographics), true, ErrorMessage = "Please indicate additional race")]
        public int? AdditionalRace { get; set; }

        [Display(Name = "Other(SPECIFY)")]
        [Column("INRATERX")]
        [MaxLength(60)]
        [RequiredIf(nameof(AdditionalRace), 50, ErrorMessage = "Please provide other")]
        public string AdditionalRaceOther { get; set; }

        [Display(Name = "Co-participant's years of education — use the codes below to report the level achieved; if an attempted level is not completed, enter the number of years completed")]
        [Column("INEDUC")]
        [Range(0, 99, ErrorMessage = "Co-participants years of education must be within 0 and 99")]
        [RequiredIf(nameof(ShouldCompleteDemographics), true, ErrorMessage = "Please provide years of education")]
        public int? YearsOfEducation { get; set; }

        [Display(Name = "What is the co-participant's relationship to the subject?")]
        [Column("INRELTO")]
        [RequiredIf(nameof(FormStatus), FormStatus.Complete, ErrorMessage = "Please indicate relationship to subject")]
        public int? Relationship { get; set; }

        [Display(Name = "How long has the co-participant known the subject?")]
        [Column("INKNOWN")]
        [Range(0, 999, ErrorMessage = "Years known must be within 0 and 999")]
        [RequiredIf(nameof(FormStatus), FormStatus.Complete, ErrorMessage = "Please indicate how long the co-participant has known the subject")]
        public int? YearsKnown { get; set; }

        [Display(Name = "Does the co-participant live with the subject?")]
        [Column("INLIVWTH")]
        [RequiredIf(nameof(FormStatus), FormStatus.Complete, ErrorMessage = "Please indicate if the co-participant lives with the subject")]
        public bool? LivesWith { get; set; }

        [Display(Name = "If no, approximate frequency of in-person visits?")]
        [Column("INVISITS")]
        [RequiredIf(nameof(LivesWith), false, ErrorMessage = "Please indicate frequency of in-person visits")]
        public int? FrequencyOfVisit { get; set; }

        [Display(Name = "If no, approximate frequency of telephone contact?")]
        [RequiredIf(nameof(LivesWith), false, ErrorMessage = "Please indicate frequency of telephone contact")]
        [Column("INCALLS")]
        public int? FrequencyOfTele { get; set; }

        [Display(Name = "Is there a question about the co-participant's reliability?")]
        [Column("INRELY")]
        [RequiredIf(nameof(FormStatus), FormStatus.Complete, ErrorMessage = "Please indicate reliability")]
        public bool? Reliability { get; set; }
    }
}
