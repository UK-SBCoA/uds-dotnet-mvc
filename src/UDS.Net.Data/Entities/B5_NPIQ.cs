using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using COA.Components.Web.DataAnnotations;
using UDS.Net.Data.Enums;

namespace UDS.Net.Data.Entities
{
    [Table("tbl_B5")]
    public class NPIQ : FormBase
    {
        [Display(Name = "NPI CO-PARTICIPANT")]
        [RequiredIf(nameof(FormStatus), FormStatus.Complete, ErrorMessage = "Please indicate subject's co-participant.")]
        [Column("NPIQINF")]
        public int? CoParticipant { get; set; }

        /// <summary>
        /// NACC allows for 60 chars, Sharepoint only allows for 30
        /// </summary>
        [MaxLength(60)]
        [RequiredIf(nameof(CoParticipant), "3", ErrorMessage = "Indicate co-participant relationship.")]
        [Column("NPIQINFX")]
        public string CoParticipantOther { get; set; }

        [Display(Name = "Delusions", Description = "Does the patient have false beliefs, such as thinking that others are stealing from him/her or planning to harm him/her in some way?")]
        [RequiredIf(nameof(FormStatus), FormStatus.Complete, ErrorMessage = "Indicate if delusions have been present in the last month.")]
        [Column("DEL")]
        public int? Delusions { get; set; }

        [RequiredIf(nameof(Delusions), "1", ErrorMessage = "Please indicate severity of delusions.")]
        [Column("DELSEV")]
        public int? DelusionsSeverity { get; set; }

        [Display(Name = "Hallucinations", Description = "Does the patient have hallucinations such as false visions or voices Does he or she seem to hear and see things that are not present?")]
        [RequiredIf(nameof(FormStatus), FormStatus.Complete, ErrorMessage = "Indicate if hallucinations have been present in the last month.")]
        [Column("HALL")]
        public int? Hallucinations { get; set; }

        [RequiredIf(nameof(Hallucinations), "1", ErrorMessage = "Please indicate severity of hallucinations.")]
        [Column("HALLSEV")]
        public int? HallucinationsSeverity { get; set; }

        [Display(Name = "Agitation/aggression", Description = "Is the patient resistive to help from others at times, or hard to handle?")]
        [RequiredIf(nameof(FormStatus), FormStatus.Complete, ErrorMessage = "Indicate if agitation/aggression has been present in the last month.")]
        [Column("AGIT")]
        public int? Agitation { get; set; }

        [RequiredIf(nameof(Agitation), "1", ErrorMessage = "Please indicate severity of agitation.")]
        [Column("AGITSEV")]
        public int? AgitationSeverity { get; set; }

        [Display(Name = "Depression/dysphoria", Description = "Does the patient seem sad or say that he/she is depressed?")]
        [RequiredIf(nameof(FormStatus), FormStatus.Complete, ErrorMessage = "Indicate if depression/dysphoria has been present in the last month.")]
        [Column("DEPD")]
        public int? Depression { get; set; }

        [RequiredIf(nameof(Depression), "1", ErrorMessage = "Please indicate severity of depression.")]
        [Column("DEPDSEV")]
        public int? DepressionSeverity { get; set; }

        [Display(Name = "Anxiety", Description = "Does the patient become upset when separated from you Does he or she have any other signs of nervousness, such as shortness of breath, sighing, being unable to relax, or feeling excessively tense?")]
        [RequiredIf(nameof(FormStatus), FormStatus.Complete, ErrorMessage = "Indicate if anxiety has been present in the last month.")]
        [Column("ANX")]
        public int? Anxiety { get; set; }

        [RequiredIf(nameof(Anxiety), "1", ErrorMessage = "Please indicate severity of anxiety.")]
        [Column("ANXSEV")]
        public int? AnxietySeverity { get; set; }

        [Display(Name = "Elation or euphoria", Description = "Does the patient appear to feel too good or act excessively happy?")]
        [RequiredIf(nameof(FormStatus), FormStatus.Complete, ErrorMessage = "Indicate if elation or euphoria has been present in the last month.")]
        [Column("ELAT")]
        public int? Elation { get; set; }

        [RequiredIf(nameof(Elation), "1", ErrorMessage = "Please indicate severity of elation.")]
        [Column("ELATSEV")]
        public int? ElationSeverity { get; set; }

        [Display(Name = "Apathy or indifference", Description = "Does the patient seem less int?erested in his or her usual activities and in the activities and plans of others?")]
        [RequiredIf(nameof(FormStatus), FormStatus.Complete, ErrorMessage = "Indicate if apathy or indifference has been present in the last month.")]
        [Column("APA")]
        public int? Apathy { get; set; }

        [RequiredIf(nameof(Apathy), "1", ErrorMessage = "Please indicate severity of apathy.")]
        [Column("APASEV")]
        public int? ApathySeverity { get; set; }

        [Display(Name = "Disinhibition", Description = "Does the patient seem to act impulsively For example, does the patient talk to strangers as if he or she knows them, or does the patient say things that may hurt people's feelings?")]
        [RequiredIf(nameof(FormStatus), FormStatus.Complete, ErrorMessage = "Indicate if disinhibition has been present in the last month.")]
        [Column("DISN")]
        public int? Disinhibition { get; set; }

        [RequiredIf(nameof(Disinhibition), "1", ErrorMessage = "Please indicate severity of disinhibition.")]
        [Column("DISNSEV")]
        public int? DisinhibitionSeverity { get; set; }

        [Display(Name = "Irritability or lability", Description = "Is the patient impatient or cranky Does he or she have difficulty coping with delays or waiting for planned activities?")]
        [RequiredIf(nameof(FormStatus), FormStatus.Complete, ErrorMessage = "Indicate if irritability or lability has been present in the last month.")]
        [Column("IRR")]
        public int? Irritability { get; set; }

        [RequiredIf(nameof(Irritability), "1", ErrorMessage = "Please indicate severity of irritability.")]
        [Column("IRRSEV")]
        public int? IrritabilitySeverity { get; set; }

        [Display(Name = "Motor disturbance", Description = "Does the patient engage in repetitive activities, such as pacing around the house, handling buttons, or wrapping string?")]
        [RequiredIf(nameof(FormStatus), FormStatus.Complete, ErrorMessage = "Indicate if motor disturbance has been present in the last month.")]
        [Column("MOT")]
        public int? MotorDisturbance { get; set; }

        [RequiredIf(nameof(MotorDisturbance), "1", ErrorMessage = "Please indicate severity of motor disturbance.")]
        [Column("MOTSEV")]
        public int? MotorDisturbanceSeverity { get; set; }

        [Display(Name = "Nighttime behaviors", Description = "Does the patient awaken you during the night, rise too early in the morning, or take excessive naps during the day?")]
        [RequiredIf(nameof(FormStatus), FormStatus.Complete, ErrorMessage = "Indicate if nighttime behaviors have been present in the last month.")]
        [Column("NITE")]
        public int? Nighttime { get; set; }

        [RequiredIf(nameof(Nighttime), "1", ErrorMessage = "Please indicate severity of nighttime behaviors.")]
        [Column("NITESEV")]
        public int? NighttimeSeverity { get; set; }

        [Display(Name = "Appetite and eating", Description = "Has the patient lost or gained weight, or had a change in the food he or she likes?")]
        [RequiredIf(nameof(FormStatus), FormStatus.Complete, ErrorMessage = "Indicate if appetite and eating has been present in the last month.")]
        [Column("APP")]
        public int? Appetite { get; set; }

        [RequiredIf(nameof(Appetite), "1", ErrorMessage = "Please indicate severity of appetite.")]
        [Column("APPSEV")]
        public int? AppetiteSeverity { get; set; }

    }
}
