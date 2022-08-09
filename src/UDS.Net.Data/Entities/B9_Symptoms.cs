using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using UDS.Net.Data.DataAnnotations;
using Microsoft.EntityFrameworkCore;
using UDS.Net.Data.Enums;

namespace UDS.Net.Data.Entities
{
    [Display(Name = "Clinician Judgment of Symptoms")]
    [Table("tbl_B9")]
    public class Symptoms: FormBase
    {
        //declines in memory
        [Column("DECSUB")]
        [Display(Name = "Does the subject report a decline in memory (relative to previously attained abilities)?")]
        [Range(0, 8, ErrorMessage = "Value outside of required range")]
        [RequiredIf(nameof(FormStatus), FormStatus.Complete, ErrorMessage = "Please provide a value")]
        public int? SubjectMemory { get; set; }

        [Column("DECIN")]
        [RequiredIf(nameof(FormStatus), FormStatus.Complete, ErrorMessage = "Please provide a value")]
        [Display(Name = "Does the co-participant report a decline in the subject's memory (relative to previously attained abilities)?")]
        [Range(0, 8, ErrorMessage = "Value outside of required range")]
        public int? CoParticipantMemory { get; set; }

        //cognitive symptoms
        [Column("DECCLCOG")]
        [RequiredIf(nameof(FormStatus), FormStatus.Complete, ErrorMessage = "Please provide a value")]
        [Display(Name = "Based on the clinician's judgment, is the subject currently experiencing meaningful impairment in cognition?")]
        [Range(0, 1, ErrorMessage = "Value outside of required range")]
        public int? CognitionImpairment { get; set; }

        [Column("COGMEM")]
        [RequiredIf(nameof(CognitionImpairment), 1, ErrorMessage = "Please provide a value")]
        [Display(Name = "For example, does s/he forget conversations and/or dates, repeat questions and/or statements, misplace things more than usual, forget names of people s/he knows well?")]
        [Range(0, 9, ErrorMessage = "Value outside of required range")]
        public int? Memory { get; set; }

        [Column("COGORI")]
        [RequiredIf(nameof(CognitionImpairment), 1, ErrorMessage = "Please provide a value")]
        [Display(Name = "For example, does s/he have trouble knowing the day, month, and year, or not recognize familiar locations, or get lost in familiar locations?")]
        [Range(0, 9, ErrorMessage = "Value outside of required range")]
        public int? Orientation { get; set; }

        [Column("COGJUDG")]
        [RequiredIf(nameof(CognitionImpairment), 1, ErrorMessage = "Please provide a value")]
        [Display(Name = "Does s/he have trouble handling money (e.g., tips), paying bills, preparing meals, shopping, using appliances, handling medications, driving?")]
        [Range(0, 9, ErrorMessage = "Value outside of required range")]
        public int? ExecutiveFunction { get; set; }

        [Column("COGLANG")]
        [RequiredIf(nameof(CognitionImpairment), 1, ErrorMessage = "Please provide a value")]
        [Display(Name = "Does s/he have hesitant speech, have trouble finding words, use inappropriate words without self-correction?")]
        [Range(0, 9, ErrorMessage = "Value outside of required range")]
        public int? Language { get; set; }

        [Column("COGVIS")]
        [RequiredIf(nameof(CognitionImpairment), 1, ErrorMessage = "Please provide a value")]
        [Display(Name = "Does s/he have difficulty interpreting visual stimuli and finding his/her way around?")]
        [Range(0, 9, ErrorMessage = "Value outside of required range")]
        public int? VisuospatialFunction { get; set; }

        [Column("COGATTN")]
        [RequiredIf(nameof(CognitionImpairment), 1, ErrorMessage = "Please provide a value")]
        [Display(Name = "Does the subject have a short attention span or limited ability to concentrate? Is s/he easily distracted?")]
        [Range(0, 9, ErrorMessage = "Value outside of required range")]
        public int? AttentionConcentration { get; set; }

        [Column("COGFLUC")]
        [RequiredIf(nameof(CognitionImpairment), 1, ErrorMessage = "Please provide a value")]
        [Display(Name = "Does the subject exhibit pronounced variation in attention and alertness, noticeably over hours or days — for example, long lapses or periods of staring into space, or times when his/her ideas have a disorganized flow?")]
        [Range(0, 9, ErrorMessage = "Value outside of required range")]
        public int? FluctuatingCognition { get; set; }

        [Column("COGFLAGO")]
        [RequiredIf(nameof(FluctuatingCognition), 1, ErrorMessage = "Please provide a value")]
        [Display(Name = "If yes, at what age did the fluctuating cognition begin?")]
        [Range(15, 777, ErrorMessage = "Value must be 15 to 110 or 777")]
        [InvalidRange(nameof(FluctuatingCognitionAge), 111, 776, ErrorMessage = "Value must be 15 to 110 or 777")]
        public int? FluctuatingCognitionAge { get; set; }

        [Column("COGOTHR")]
        [RequiredIf(nameof(CognitionImpairment), 1, ErrorMessage = "Please provide a value")]
        [Range(0, 1, ErrorMessage = "Value outside of required range")]
        public int? ImpairedOther { get; set; }

        [Column("COGOTHRX")]
        [RequiredIf(nameof(ImpairedOther), 1, ErrorMessage = "Please provide a value")]
        [MaxLength(60)]
        public string ImpairedOtherSpecify { get; set; }

        [Column("COGFPRED")]
        [RequiredIf(nameof(CognitionImpairment), 1, ErrorMessage = "Please provide a value")]
        [Display(Name = "Indicate the predominant symptom that was first recognized as a decline in the subject’s cognition")]
        [Range(0, 99, ErrorMessage = "Value outside of required range")]
        [InvalidRange(nameof(PredominantSymptom), 9, 98, ErrorMessage = "Value outside of required range")]
        public int? PredominantSymptom { get; set; }

        [Column("COGFPREX")]
        [RequiredIf(nameof(PredominantSymptom), 8, ErrorMessage = "Please provide other value")]
        [MaxLength(60)]
        public string PredominantSymptomOther { get; set; }

        [Column("COGMODE")]
        [RequiredIf(nameof(CognitionImpairment), 1, ErrorMessage = "Please provide a value")]
        [Display(Name = "Mode of onset of cognitive symptoms")]
        [Range(1, 99, ErrorMessage = "Value outside of required range")]
        [InvalidRange(nameof(CognitiveSymptoms), 5, 98, ErrorMessage = "Value outside of required range")]
        public int? CognitiveSymptoms { get; set; }

        [Column("COGMODEX")]
        [RequiredIf(nameof(CognitiveSymptoms), 4, ErrorMessage = "Please provide other value")]
        [MaxLength(60)]
        public string CognitiveSymptomsOther { get; set; }

        [Column("DECAGE")]
        [Display(Name = "Based on the clinician’s assessment, at what age did the cognitive decline begin?")]
        [RequiredIf(nameof(CognitionImpairment), 1, ErrorMessage = "Please provide a value")]
        [Range(15, 777, ErrorMessage = "Value must be 15 to 110 or 777")]
        [InvalidRange(nameof(AgeOfDecline), 111, 776, ErrorMessage = "Value must be 15 to 110 or 777")]
        public int? AgeOfDecline { get; set; }


        //Behavioral symptoms - IF NO SKIP TO QUESTION 13 FROM HERE
        [Column("DECCLBE")]
        [RequiredIf(nameof(FormStatus), FormStatus.Complete, ErrorMessage = "Please provide a value")]
        [Display(Name = "Based on the clinician’s judgment, is the subject currently experiencing any kind of behavioral symptoms?")]
        [Range(0, 1, ErrorMessage = "Value outside of required range")]
        public int? BehavioralSymptoms { get; set; }

        [NotMapped]
        [RequiredIf(nameof(FormStatus), FormStatus.Complete, ErrorMessage = "Please indicate at least one behavioral symptom (9a-j)")]
        public bool? AtLeastOneIndicatedIfHasBehavioralSymptom
        {
            get
            {
                if (BehavioralSymptoms.HasValue)
                {
                    if (BehavioralSymptoms.Value == 0)
                        return true;
                    else
                    {
                        int count = 0;

                        if (ApathyWithdrawal.HasValue && ApathyWithdrawal.Value == 1)
                            count++;

                        if (DepressedMood.HasValue && DepressedMood.Value == 1)
                            count++;

                        if (VisualHallucinations.HasValue && VisualHallucinations.Value == 1)
                            count++;

                        if (AuditoryHallucinations.HasValue && AuditoryHallucinations.Value == 1)
                            count++;

                        if (AbnormalBeliefs.HasValue && AbnormalBeliefs.Value == 1)
                            count++;

                        if (Disinhibition.HasValue && Disinhibition.Value == 1)
                            count++;

                        if (Irritability.HasValue && Irritability.Value == 1)
                            count++;

                        if (Agitation.HasValue && Agitation.Value == 1)
                            count++;

                        if (PersonalityChange.HasValue && PersonalityChange.Value == 1)
                            count++;

                        if (RemDisorder.HasValue && RemDisorder.Value == 1)
                            count++;

                        if (Anxiety.HasValue && Anxiety.Value == 1)
                            count++;

                        if (ChangeInBehavior.HasValue && ChangeInBehavior.Value == 1)
                            count++;

                        if (count > 0)
                            return true;

                        return null;
                    }
                }
                else
                {
                    return true;
                }
            }
        }

        [Column("BEAPATHY")]
        [RequiredIf(nameof(BehavioralSymptoms), 1, ErrorMessage = "Please provide a value")]
        [Display(Name = "Has the subject lost interest in or displayed a reduced ability to initiate usual activities and social interaction, such as conversing with family and/or friends?")]
        public int? ApathyWithdrawal { get; set; }

        [Column("BEDEP")]
        [RequiredIf(nameof(BehavioralSymptoms), 1, ErrorMessage = "Please provide a value")]
        [Display(Name = "Has the subject seemed depressed for more than two weeks at a time, e.g., shown loss of interest or pleasure in nearly all activities, sadness, hopelessness, loss of appetite, fatigue?")]
        public int? DepressedMood { get; set; }

        [Column("BEVHALL")]
        [RequiredIf(nameof(BehavioralSymptoms), 1, ErrorMessage = "Please provide a value")]
        [Display(Name = "Visual hallucinations")]
        public int? VisualHallucinations { get; set; }

        [Column("BEVWELL")]
        [RequiredIf(nameof(VisualHallucinations), 1, ErrorMessage = "Please provide a value")]
        [Display(Name = "If Yes, are the hallucinations well formed and detailed?")]
        public int? DetailedHallucinations { get; set; }

        [Column("BEVHAGO")]
        [RequiredIf(nameof(DetailedHallucinations), 1, ErrorMessage = "Please provide a value")]
        [Display(Name = "If well formed, clear-cut visual hallucinations, at what age did these visual hallucinations begin?")]
        [Range(15, 888, ErrorMessage = "Value must be 15 to 110 or 777 or 888")]
        [InvalidRange(nameof(HallucinationsAge), 111, 776, ErrorMessage = "Value must be 15 to 110 or 777 or 888")]
        [InvalidRange(nameof(HallucinationsAge), 778, 887, ErrorMessage = "Value must be 15 to 110 or 777 or 888")]
        public int? HallucinationsAge { get; set; }

        [Column("BEAHALL")]
        [RequiredIf(nameof(DetailedHallucinations), 1, ErrorMessage = "Please provide a value")]
        [Display(Name = "Auditory hallucinations")]
        public int? AuditoryHallucinations { get; set; }

        [Column("BEDEL")]
        [RequiredIf(nameof(DetailedHallucinations), 1, ErrorMessage = "Please provide a value")]
        [Display(Name = "Abnormal, false, or delusional beliefs")]
        public int? AbnormalBeliefs { get; set; }

        [Column("BEDISIN")]
        [RequiredIf(nameof(BehavioralSymptoms), 1, ErrorMessage = "Please provide a value")]
        [Display(Name = "Does the subject use inappropriate coarse language or exhibit inappropriate speech or behaviors in public or in the home? Does s/he talk personally to strangers or have disregard for personal hygiene?")]
        public int? Disinhibition { get; set; }

        [Column("BEIRRIT")]
        [RequiredIf(nameof(BehavioralSymptoms), 1, ErrorMessage = "Please provide a value")]
        [Display(Name = "Does the subject overreact, e.g., by shouting at family members or others?")]
        public int? Irritability { get; set; }

        [Column("BEAGIT")]
        [RequiredIf(nameof(BehavioralSymptoms), 1, ErrorMessage = "Please provide a value")]
        [Display(Name = "Does the subject have trouble sitting still? Does s/he shout, hit, and/or kick?")]
        public int? Agitation { get; set; }

        [Column("BEPERCH")]
        [RequiredIf(nameof(BehavioralSymptoms), 1, ErrorMessage = "Please provide a value")]
        [Display(Name = "Does the subject exhibit bizarre behavior 3 or behavior uncharacteristic of the subject, such as unusual collecting, suspiciousness (without delusions), unusual dress, or dietary changes? Does the subject fail to take others’ feelings into account?")]
        public int? PersonalityChange { get; set; }

        [Column("BEREM")]
        [RequiredIf(nameof(BehavioralSymptoms), 1, ErrorMessage = "Please provide a value")]
        [Display(Name = "While sleeping, does the subject appear to act out his/her dreams (e.g., punch or flail their arms, shout, or scream)?")]
        public int? RemDisorder { get; set; }

        [Column("BEREMAGO")]
        [RequiredIf(nameof(RemDisorder), 1, ErrorMessage = "Please provide a value")]
        [Display(Name = "If yes, at what age did the REM sleep behavior disorder begin?")]
        [Range(15, 777, ErrorMessage = "Value must be 15 to 110 or 777")]
        [InvalidRange(nameof(RemDisorderAge), 111, 776, ErrorMessage = "Value must be 15 to 110 or 777")]
        public int? RemDisorderAge { get; set; }

        [Column("BEANX")]
        [RequiredIf(nameof(BehavioralSymptoms), 1, ErrorMessage = "Please provide a value")]
        [Display(Name = "For example, does s/he show signs of nervousness (e.g., frequent sighing, anxious facial expressions, or hand-wringing) and/or excessive worrying?")]
        public int? Anxiety { get; set; }

        [Column("BEOTHR")]
        [RequiredIf(nameof(BehavioralSymptoms), 1, ErrorMessage = "Please provide a value")]
        public int? ChangeInBehavior { get; set; }

        [Column("BEOTHRX")]
        [RequiredIf(nameof(ChangeInBehavior), 1, ErrorMessage = "Please provide other value")]
        [MaxLength(60)]
        public string ChangeInBehaviorOther { get; set; }

        [Column("BEFPRED")]
        [RequiredIf(nameof(BehavioralSymptoms), 1, ErrorMessage = "Please provide a value")]
        [Display(Name = "Indicate the predominant symptom that was first recognized as a decline in the subject's behavior:")]
        public int? BehaviorDecline { get; set; }

        [Column("BEFPREDX")]
        [RequiredIf(nameof(BehaviorDecline), 10, ErrorMessage = "Please provide other value")]
        [MaxLength(60)]
        public string BehaviorDeclineOther { get; set; }


        [Column("BEMODE")]
        [RequiredIf(nameof(BehavioralSymptoms), 1, ErrorMessage = "Please provide a value")]
        [Display(Name = "Mode of onset of behavioral symptoms:")]
        public int? OnsetBehavioral { get; set; }

        [Column("BEMODEX")]
        [RequiredIf(nameof(OnsetBehavioral), 4, ErrorMessage = "Please provide other value")]
        [MaxLength(60)]
        public string OnsetBehavioralOther { get; set; }

        [Column("BEAGE")]
        [Range(15, 777, ErrorMessage = "Value must be 15 to 110 or 777")]
        [InvalidRange(nameof(BehaviorSymptomsAge), 111, 776, ErrorMessage = "Value must be 15 to 110 or 777")]
        [RequiredIf(nameof(BehavioralSymptoms), 1, ErrorMessage = "Please provide a value")]
        [Display(Name = "Based on the clinician's assessment, at what age did the behavioral symptoms begin?")]
        public int? BehaviorSymptomsAge { get; set; }

        //motor symptoms
        [Column("DECCLMOT")]
        [RequiredIf(nameof(FormStatus), FormStatus.Complete, ErrorMessage = "Please provide a value")]
        [Display(Name = "Based on the clinician's judgment, is the subject currently experiencing any motor symptoms?")]
        public int? MotorSymptoms { get; set; }

        [Column("MOGAIT")]
        [RequiredIf(nameof(MotorSymptoms), 1, ErrorMessage = "Please provide a value")]
        [Display(Name = "Has the subject's walking changed, not specifically due to arthritis or an injury? Is s/he unsteady, or does s/he shuffle when walking, have little or no arm-swing, or drag a foot?")]
        public int? GaitDisorder { get; set; }

        [Column("MOFALLS")]
        [RequiredIf(nameof(MotorSymptoms), 1, ErrorMessage = "Please provide a value")]
        [Display(Name = "Does the subject fall more than usual?")]
        public int? Falls { get; set; }

        [Column("MOTREM")]
        [RequiredIf(nameof(MotorSymptoms), 1, ErrorMessage = "Please provide a value")]
        [Display(Name = "Has the subject had rhythmic shaking, especially in the hands, arms, legs, head, mouth, or tongue?")]
        public int? Tremor { get; set; }

        [Column("MOSLOW")]
        [RequiredIf(nameof(MotorSymptoms), 1, ErrorMessage = "Please provide a value")]
        [Display(Name = "Has the subject noticeably slowed down in walking, moving, or writing by hand, other than due to an injury or illness? Has his/her facial expression changed or become more \"wooden, \" or masked and unexpressive?")]
        public int? Slowness { get; set; }

        [Column("MOFRST")]
        [RequiredIf(nameof(MotorSymptoms), 1, ErrorMessage = "Please provide a value")]
        [Display(Name = "Indicate the predominant symptom that was first recognized as a decline in the subject's motor function")]
        public int? PredominantMotorDecline { get; set; }

        [Column("MOMODE")]
        [RequiredIf(nameof(MotorSymptoms), 1, ErrorMessage = "Please provide a value")]
        [Display(Name = "Mode of onset of motor symptoms")]
        public int? ModeOfMotorSymptoms { get; set; }

        [Column("MOMODEX")]
        [RequiredIf(nameof(ModeOfMotorSymptoms), 4, ErrorMessage = "Please provide other value")]
        [MaxLength(60)]
        public string ModeOfMotorSymptomsOther { get; set; }

        [Column("MOMOPARK")]
        [RequiredIf(nameof(MotorSymptoms), 1, ErrorMessage = "Please provide a value")]
        [Display(Name = "Were changes in motor function suggestive of parkinsonism?")]
        public int? SuggestiveOfParkinsonism { get; set; }

        [Column("PARKAGE")]
        [RequiredIf(nameof(SuggestiveOfParkinsonism), 1, ErrorMessage = "Please provide a value")]
        [Display(Name = "If Yes, at what age did the motor symptoms suggestive of parkinsonism begin?")]
        [Range(15, 777, ErrorMessage = "Value must be 15 to 110 or 777")]
        [InvalidRange(nameof(SuggestiveOfParkinsonismAge), 111, 776, ErrorMessage = "Value must be 15 to 110 or 777")]
        public int? SuggestiveOfParkinsonismAge { get; set; }

        [Column("MOMOALS")]
        [RequiredIf(nameof(MotorSymptoms), 1, ErrorMessage = "Please provide a value")]
        [Display(Name = "Were changes in motor function suggestive of amyotrophic lateral sclerosis?")]
        public int? SuggestiveOfSclerosis { get; set; }

        [Column("ALSAGE")]
        [RequiredIf(nameof(SuggestiveOfSclerosis), 1, ErrorMessage = "Please provide a value")]
        [Display(Name = "If Yes, at what age did the motor symptoms suggestive of ALS begin?")]
        [Range(15, 777, ErrorMessage = "Value must be 15 to 110 or 777")]
        [InvalidRange(nameof(SuggestiveOfSclerosisAge), 111, 776, ErrorMessage = "Value must be 15 to 110 or 777")]
        public int? SuggestiveOfSclerosisAge { get; set; }

        [Column("MOAGE")]
        [RequiredIf(nameof(MotorSymptoms), 1, ErrorMessage = "Please provide a value")]
        [Display(Name = "Based on the clinician's assessment, at what age did the motor changes begin?")]
        [Range(15, 777, ErrorMessage = "Value must be 15 to 110 or 777")]
        [InvalidRange(nameof(AssessmentOfSclerosisAge), 111, 776, ErrorMessage = "Value must be 15 to 110 or 777")]
        public int? AssessmentOfSclerosisAge { get; set; }

        //Overall course of decline and predominant domain
        [Column("COURSE")]
        [RequiredIf(nameof(FormStatus), FormStatus.Complete, ErrorMessage = "Please provide a value")]
        [Display(Name = "Overall course of decline of cognitive / behavorial / motor syndrome")]
        public int? OverallDecline { get; set; }

        [Column("FRSTCHG")]
        [RequiredIf(nameof(FormStatus), FormStatus.Complete, ErrorMessage = "Please provide a value")]
        [Display(Name = "Indicate the predominant domain that was first recognized as changed in the subject")]
        public int? PredominantDomain { get; set; }

        //Candidate for further evaluation for Lewy body disease or frontotemporal lobar degeneration
        [Column("LBDEVAL")]
        [RequiredIf(nameof(FormStatus), FormStatus.Complete, ErrorMessage = "Please provide a value")]
        [Display(Name = "Is the subject a potential candidate for further evaluation for Lewy body disease?")]
        public int? LewyBodyDisease { get; set; }

        [Column("FTLDEVAL")]
        [RequiredIf(nameof(FormStatus), FormStatus.Complete, ErrorMessage = "Please provide a value")]
        [Display(Name = "Is the subject a potential candidate for further evaluation for frontotemporal lobar degeneration?")]
        public int? FrontotemporalLobarDegeneration { get; set; }
    }
}
