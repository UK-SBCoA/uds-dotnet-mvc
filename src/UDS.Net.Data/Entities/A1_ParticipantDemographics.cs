using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using COA.Components.Web.DataAnnotations;
using Microsoft.EntityFrameworkCore;
using UDS.Net.Data.Enums;

namespace UDS.Net.Data.Entities
{
    [Display(Name = "Participant Demographics")]
    [Table("tbl_A1")]
    public class ParticipantDemographics: FormBase
    {
        [Display(Name = "Primary reason for coming to ADC")]
        [Column("REASON")]
        [Range(1,9)]
        public int? Reason { get; set; }

        [Display(Name = "Principal referral source")]
        [Range(1, 9)]
        [Column("REFERSC")]
        public int? ReferralSource { get; set; }

        [Display(Name = "If the referral source was self-referral or a non-professional contact, how did the referral source learn of the ADC?")]
        [Column("LEARNED")]
        [Range(1, 9)]
        [RequiredIf(nameof(ReferralSource), 1, ErrorMessage = "Please indicate how the referral source learned of the ADC")]
        [RequiredIf(nameof(ReferralSource), 2, ErrorMessage = "Please indicate how the referral source learned of the ADC")]
        public int? Learned { get; set; }

        [Display(Name = "Presumed disease status at enrollment")]
        [Column("PRESTAT")]
        [Range(1, 3)]
        public int? EnrollmentDiseaseStatus { get; set; }

        [Display(Name = "Presumed participation")]
        [Column("PRESPART")]
        [Range(1, 2)]
        public int? PresumedParticipation { get; set; }

        [Display(Name = "ADC enrollment type:")]
        [Column("SOURCENW")]
        [Range(1, 2)]
        public int? EnrollmentType { get; set; }

        [Display(Name = "Participant’s month of birth")]
        [Column("BIRTHMO")]
        [Range(01, 12)]
        [RequiredIf(nameof(FormStatus), FormStatus.Complete, ErrorMessage = "Please provide participant's birth month")]
        public int? BirthMonth { get; set; }

        [Display(Name = "Participant’s year of birth")]
        [Column("BIRTHYR")]
        //not about if we can do math here
        [Range(1875, 2006)]
        [RequiredIf(nameof(FormStatus), FormStatus.Complete, ErrorMessage = "Please provide participant's birth year")]
        public int? BirthYear { get; set; }

        [Display(Name = "Participant’s sex")]
        [Column("SEX")]
        [Range(1,2)]
        [RequiredIf(nameof(FormStatus), FormStatus.Complete, ErrorMessage = "Please provide participant's sex")]
        public int? Sex { get; set; }

        [Display(Name = "Does the participant report being of Hispanic/Latino ethnicity (i.e., having origins from a mainly Spanish-speaking Latin American country), regardless of race?")]
        [Column("HISPANIC")]
        [Range(0, 9)]
        public int? HispanicLatinoEthnicity { get; set; }

        [Display(Name = "If yes, what are the participant's reported origins?")]
        [Column("HISPOR")]
        [Range(1, 99)]
        [RequiredIf(nameof(HispanicLatinoEthnicity), 1, ErrorMessage = "Please provide subject's Hispanic/Latino origins")]
        public int? HispanicLatinoOrigins { get; set; }

        [Display(Name = "Other (specify)")]
        [Column("HISPORX")]
        [MaxLength(60)]
        [RequiredIf(nameof(HispanicLatinoOrigins), 50, ErrorMessage = "Please indicate participant's other origins")]
        public string HispanicLatinoOriginsOther { get; set; }

        [Display(Name = "What does the participant report as his or her race?")]
        [Column("RACE")]
        [Range(1,99)]
        public int? Race { get; set; }

        [Display(Name = "Other (specify)")]
        [Column("RACEX")]
        [MaxLength(60)]
        [RequiredIf(nameof(Race), 50, ErrorMessage = "Please indicate participant's other race")]
        public string RaceOther { get; set; }


        [Display(Name = "What additional race does participant report?")]
        [Column("RACESEC")]
        [Range(1, 99)]
        public int? SecondaryRace { get; set; }

        [Display(Name = "Other (specify)")]
        [Column("RACESECX")]
        [MaxLength(60)]
        [RequiredIf(nameof(SecondaryRace), 50, ErrorMessage = "Please provide participant's other additional race")]
        public string SecondaryRaceOther { get; set; }

        [Display(Name = "What additional race, beyond those reported in Questions 9 and 10, does participant report?")]
        [Column("RACETER")]
        [Range(1, 99)]
        public int? AdditionalRace { get; set; }

        [Display(Name = "Other (specify)")]
        [Column("RACETERX")]
        [MaxLength(60)]
        [RequiredIf(nameof(AdditionalRace), 50, ErrorMessage = "Please indicate participant's other additional race beyond questions 9 and 10")]
        public string AdditionalRaceOther { get; set; }

        [Display(Name = "Participant’s primary language")]
        [Column("PRIMLANG")]
        [Range(1,9)]
        public int? PrimaryLanguage { get; set; }

        [Display(Name = "Other (specify)")]
        [Column("PRIMLANX")]
        [MaxLength(60)]
        [RequiredIf(nameof(PrimaryLanguage), 8, ErrorMessage = "Please indicate participant's other primary race")]
        public string PrimaryLanguageOther { get; set; }

        [Display(Name = "Participant’s years of education, use the codes below to report the level achieved; if an attempted level is not completed, enter the number of years completed")]
        [Column("EDUC")]
        [Range(0, 99)]
        public int? Education { get; set; }

        [Display(Name = "Participant’s current marital status")]
        [Column("MARISTAT")]
        [Range(0, 9)]
        [RequiredIf(nameof(FormStatus), FormStatus.Complete, ErrorMessage = "Please indicate participant's marriage status")]
        public int? MarriageStatus { get; set; }

        [Display(Name = "What is the participant’s living situation?")]
        [Column("LIVSITUA")]
        [Range(0, 9)]
        [RequiredIf(nameof(FormStatus), FormStatus.Complete, ErrorMessage = "Please indicate participant's living situation")]
        public int? LivingSituation { get; set; }

        [Display(Name = "What is the participant’s level of independence?")]
        [Column("INDEPEND")]
        [Range(0, 9)]
        [RequiredIf(nameof(FormStatus), FormStatus.Complete, ErrorMessage = "Please indicate participant's level of independence")]
        public int? LevelOfIndependence { get; set; }

        [Display(Name = "What is the participant’s primary type of residence?")]
        [Column("RESIDENC")]
        [Range(0, 9)]
        [RequiredIf(nameof(FormStatus), FormStatus.Complete, ErrorMessage = "Please indicate participant's primary type of residence")]
        public int? Residence { get; set; }

        [Display(Name = "ZIP Code (first three digits) of participant’s primary residence")]
        [Column("ZIP")]
        [Range(006,999)]
        public int? Zip { get; set; }

        [Display(Name = "Is the participant left- or right-handed (for example, which hand would s/ he normally use to write or throw a ball)?")]
        [Column("HANDED")]
        [Range(1, 9)]
        public int? Handedness { get; set; }
    }
}