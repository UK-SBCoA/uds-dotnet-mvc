using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using UDS.Net.Data.DataAnnotations;
using UDS.Net.Data.Enums;

namespace UDS.Net.Data.Entities
{
    [Table("tbl_A5")]
    public class SubjectHealthHistory: FormBase
    {
        [Display(Name = "1a. Has subject smoked within the last 30 days?", GroupName = "Cigarette Smoking")]
        [Column("TOBAC30")]
        [RequiredIf(nameof(FormStatus), FormStatus.Complete, "A value is required for this field")]
        public int? Tobac30 { get; set; }
        [Display(Name = "1b. Has subject smoked more than 100 cigarettes in his/her life? (If No or Unknown, SKIP TO QUESTION 1F)", GroupName = "Cigarette Smoking")]
        [Column("TOBAC100")]
        [RequiredIf(nameof(FormStatus), FormStatus.Complete, "A value is required for this field")]
        public int? Tobac100 { get; set; }
        [Display(Name = "1c. Total years smoked (99 = unknown)", GroupName = "Cigarette Smoking")]
        [Column("SMOKYRS")]
        [Range(0, 99, ErrorMessage = "Value outside of required range")]
        [RequiredIf(nameof(Tobac100), "1", ErrorMessage = "Please indicate total years smoked")]
        [InvalidRange(nameof(SmokingYears), 88, 98, ErrorMessage = "Value outside of required range")]
        public int? SmokingYears { get; set; }
        [Display(Name = "1d. Average number of packs smoked per day", GroupName = "Cigarette Smoking")]
        [Column("PACKSPER")]
        [RequiredIf(nameof(Tobac100), "1", ErrorMessage = "A value is required for this question")]
        public int? PackerPerDay { get; set; }
        [Display(Name = "1e. If the subject quit smoking, specify the age at which he/she last smoked (i.e., quit)   (888=N/A, 999=unknown)", GroupName = "Cigarette Smoking")]
        [Column("QUITSMOK")]
        [RequiredIf(nameof(Tobac100), "1", ErrorMessage = "A value is required for this question")]
        [Range(8,999, ErrorMessage = "Please indicate a valid age between 8 and 110 at which the participant quit smoking")]
        [InvalidRange(nameof(QuitSmoking), 110, 887, ErrorMessage = "Please indicate a valid age between 8 and 110 at which the participant quit smoking")]
        [InvalidRange(nameof(QuitSmoking), 889, 998, ErrorMessage = "Please indicate a valid age between 8 and 110 at which the participant quit smoking")]
        public int? QuitSmoking { get; set; }
        [Display(Name = "1f. In the past three months, has the subject consumed any alcohol? (If No or Unknown, SKIP TO QUESTION 2a)", GroupName = "Alcohol Use")]
        [Column("ALCOCCAS")]
        [RequiredIf(nameof(FormStatus), FormStatus.Complete, "Please indicate if the subject has consumed alcohol in the last 3 months")]
        public int? AlcoholConsumption { get; set; }
        [Display(Name = "1g. During the past three months, how often did the subject have at least one drink of any alcoholic beverage such as wine, beer, malt liquor, or spirits?", GroupName = "Alcohol Use")]
        [Column("ALCFREQ")]
        [RequiredIf(nameof(AlcoholConsumption), "1", ErrorMessage = "Please indicate the frequency of alcohol consumption over the last 3 months")]
        public int? AlcoholFrequency { get; set; }
        [Display(Name = "2a.  Heart attack / cardiac arrest (If absent or unknown, SKIP TO QUESTION 2b)", GroupName = "Cardiovascular disease")]
        [Column("CVHATT")]
        [RequiredIf(nameof(FormStatus), FormStatus.Complete, "A value is required for this field")]
        public int? HeartAttack { get; set; }
        [Display(Name = "2a1. More than one heart attack?", GroupName = "Cardiovascular disease")]
        [Column("HATTMULT")]
        [RequiredIf(nameof(HeartAttack), "1", ErrorMessage = "A value is required for this question")]
        [RequiredIf(nameof(HeartAttack), "2", ErrorMessage = "A value is required for this question")]
        public int? HeartAttackFreq { get; set; }
        [Display(Name = "2a2. Year of most recent heart attack (9999 = unknown)", GroupName = "Cardiovascular disease")]
        [Column("HATTYEAR")]
        [RequiredIf(nameof(HeartAttack), "1", ErrorMessage = "A value is required for this question")]
        [RequiredIf(nameof(HeartAttack), "2", ErrorMessage = "A value is required for this question")]
        [Range(1900, 9999)]
        public int? HeartAttackYear { get; set; }
        [Display(Name = "2b. Atrial fibrillation", GroupName = "Cardiovascular disease")]
        [Column("CVAFIB")]
        [RequiredIf(nameof(FormStatus), FormStatus.Complete, "A value is required for this field")]
        public int? AtrialFibrillation { get; set; }
        [Display(Name = "2c. Angioplasty/endarterectomy/stent", GroupName = "Cardiovascular disease")]
        [Column("CVANGIO")]
        [RequiredIf(nameof(FormStatus), FormStatus.Complete, "A value is required for this field")]
        public int? AngioplastyEndarterectomyStent { get; set; }
        [Display(Name = "2d. Cardiac bypass procedure", GroupName = "Cardiovascular disease")]
        [Column("CVBYPASS")]
        [RequiredIf(nameof(FormStatus), FormStatus.Complete, "A value is required for this field")]
        public int? CardiacBypassProcedure { get; set; }
        [Display(Name = "2e. Pacemaker and/or defibrillator", GroupName = "Cardiovascular disease")]
        [Column("CVPACDEF")]
        [RequiredIf(nameof(FormStatus), FormStatus.Complete, "A value is required for this field")]
        public int? PacemakerAndOrDefibrillator { get; set; }
        [Display(Name = "2f. Congestive heart failure", GroupName = "Cardiovascular disease")]
        [Column("CVCHF")]
        [RequiredIf(nameof(FormStatus), FormStatus.Complete, "A value is required for this field")]
        public int? CongestiveHeartFailure { get; set; }
        [Display(Name = "2g. Angina", GroupName = "Cardiovascular disease")]
        [Column("CVANGINA")]
        [RequiredIf(nameof(FormStatus), FormStatus.Complete, "A value is required for this field")]
        public int? Angina { get; set; }
        [Display(Name = "2h. Heart valve replacement or repair", GroupName = "Cardiovascular disease")]
        [Column("CVHVALVE")]
        [RequiredIf(nameof(FormStatus), FormStatus.Complete, "A value is required for this field")]
        public int? HeartValveReplacementOrRepair { get; set; }
        [Display(Name = "2i. Other cardiovascular disease", GroupName = "Cardiovascular disease")]
        [Column("CVOTHR")]
        [RequiredIf(nameof(FormStatus), FormStatus.Complete, "A value is required for this field")]
        public int? OtherCardiovascularDisease { get; set; }
        [Display(Name = "2i1. Other cardiovascular disease (specify)", GroupName = "Cardiovascular disease")]
        [Column("CVOTHRX")]
        [RequiredIf(nameof(OtherCardiovascularDisease), "1", ErrorMessage = "Please indicate a value")]
        [RequiredIf(nameof(OtherCardiovascularDisease), "2", ErrorMessage = "Please indicate a value")]
        [StringLength(60)]
        public string OtherCardiovascularDiseaseSpecify { get; set; }
        [Display(Name = "3a. Stroke — by history, not exam (imaging is not required) (If absent or unknown, SKIP TO QUESTION 3b)", GroupName = "Cerebrovascular disease")]
        [Column("CBSTROKE")]
        [RequiredIf(nameof(FormStatus), FormStatus.Complete, "A value is required for this field")]
        public int? StrokeByHistory { get; set; }
        [Display(Name = "3a1. More than one stroke?", GroupName = "Cerebrovascular disease")]
        [Column("STROKMUL")]
        [RequiredIf(nameof(StrokeByHistory), "1", ErrorMessage = "A value is required for this question")]
        [RequiredIf(nameof(StrokeByHistory), "2", ErrorMessage = "A value is required for this question")]
        public int? MoreThanOneStroke { get; set; }
        [Display(Name = "3a2. Year of most recent stroke (9999 = unknown)", GroupName = "Cerebrovascular disease")]
        [Column("STROKYR")]
        [RequiredIf(nameof(StrokeByHistory), "1", ErrorMessage = "Please specify a year")]
        [RequiredIf(nameof(StrokeByHistory), "2", ErrorMessage = "Please specify a year")]
        [Range(1900, 9999)]
        public int? MostRecentStroke { get; set; }
        [Display(Name = "3b. Transient ischemic attack (TIA)", GroupName = "Cerebrovascular disease")]
        [Column("CBTIA")]
        [RequiredIf(nameof(FormStatus), FormStatus.Complete, "A value is required for this field")]
        public int? TransientIschemicAttack { get; set; }
        [Display(Name = "3b1. More than one TIA", GroupName = "Cerebrovascular disease")]
        [Column("TIAMULT")]
        [RequiredIf(nameof(TransientIschemicAttack), "1", ErrorMessage = "A value is required for this question")]
        [RequiredIf(nameof(TransientIschemicAttack), "2", ErrorMessage = "A value is required for this question")]
        public int? MoreThanOneTIA { get; set; }
        [Display(Name = "3b2. Year of most recent TIA (9999 = unknown)", GroupName = "Cerebrovascular disease")]
        [Column("TIAYEAR")]
        [RequiredIf(nameof(TransientIschemicAttack), "1", ErrorMessage = "Please specify a year")]
        [RequiredIf(nameof(TransientIschemicAttack), "2", ErrorMessage = "Please specify a year")]
        [Range(1900, 9999)]
        public int? YearOfMostRecentTIA { get; set; }
        [Display(Name = "4a. Parkinson’s disease (PD) (If absent or unknown, SKIP TO QUESTION 4b)", GroupName = "Neurologic conditions")]
        [Column("PD")]
        [RequiredIf(nameof(FormStatus), FormStatus.Complete, "A value is required for this field")]
        public int? ParkinsonsDisease { get; set; }
        [Display(Name = "4a1. Year of PD diagnosis (9999 = unknown)", GroupName = "Neurologic conditions")]
        [Column("PDYR")]
        [RequiredIf(nameof(ParkinsonsDisease), "1", ErrorMessage = "Please specify a year")]
        [Range(1900, 9999)]
        public int? YearOfPDDiagnosis { get; set; }
        [Display(Name = "4b. Other parkinsonian disorder (If absent or unknown, SKIP TO QUESTION 4c)", GroupName = "Neurologic conditions")]
        [Column("PDOTHR")]
        [RequiredIf(nameof(FormStatus), FormStatus.Complete, "A value is required for this field")]
        public int? OtherParkinsonianDisorder { get; set; }
        [Display(Name = "4b1. Year of parkinsonian disorder diagnosis (9999 = unknown)", GroupName = "Neurologic conditions")]
        [Column("PDOTHRYR")]
        [RequiredIf(nameof(OtherParkinsonianDisorder), "1", ErrorMessage = "A value is required for this question")]
        [Range(1900, 9999)]
        public int? YearOfParkinsonianDisorder { get; set; }
        [Display(Name = "4c. Seizures", GroupName = "Neurologic conditions")]
        [Column("SEIZURES")]
        [RequiredIf(nameof(FormStatus), FormStatus.Complete, "A value is required for this field")]
        public int? Seizures { get; set; }
        [Display(Name = "4d. Traumatic brain injury (TBI) (If absent or unknown, SKIP TO QUESTION 5a)", GroupName = "Neurologic conditions")]
        [Column("TBI")]
        [RequiredIf(nameof(FormStatus), FormStatus.Complete, "A value is required for this field")]
        public int? TraumaticBrainInjury { get; set; }
        [Display(Name = "4d1. TBI with brief loss of consciousness (< 5 minutes)", GroupName = "Neurologic conditions")]
        [Column("TBIBRIEF")]
        [RequiredIf(nameof(TraumaticBrainInjury), "1", ErrorMessage = "A value is required for this question")]
        [RequiredIf(nameof(TraumaticBrainInjury), "2", ErrorMessage = "A value is required for this question")]
        public int? TBIBreif { get; set; }
        [Display(Name = "4d2. TBI with extended loss of consciousness (5 minutes or longer)", GroupName = "Neurologic conditions")]
        [Column("TBIEXTEN")]
        [RequiredIf(nameof(TraumaticBrainInjury), "1", ErrorMessage = "A value is required for this question")]
        [RequiredIf(nameof(TraumaticBrainInjury), "2", ErrorMessage = "A value is required for this question")]
        public int? TBIExtend { get; set; }
        [Display(Name = "4d3. TBI without loss of consciousness (as might result from military detonations or sports injuries)?", GroupName = "Neurologic conditions")]
        [Column("TBIWOLOS")]
        [RequiredIf(nameof(TraumaticBrainInjury), "1", ErrorMessage = "A value is required for this question")]
        [RequiredIf(nameof(TraumaticBrainInjury), "2", ErrorMessage = "A value is required for this question")]
        public int? TBIWoLos { get; set; }
        [Display(Name = "4d4. Year of most recent TBI (9999 = unknown)", GroupName = "Neurologic conditions")]
        [Column("TBIYEAR")]
        [RequiredIf(nameof(TraumaticBrainInjury), "1", ErrorMessage = "A value is required for this question")]
        [RequiredIf(nameof(TraumaticBrainInjury), "2", ErrorMessage = "A value is required for this question")]
        [Range(1900, 9999)]
        public int? TBIYear { get; set; }
        [Display(Name = "5a. Diabetes (If absent or unknown, SKIP TO QUESTION 5b)", GroupName = "Medical conditions")]
        [Column("DIABETES")]
        [RequiredIf(nameof(FormStatus), FormStatus.Complete, "A value is required for this field")]
        public int? Diabetes { get; set; }
        [Display(Name = "5a1. If Recent/active or Remote/inactive, which type?", GroupName = "Medical conditions")]
        [Column("DIABTYPE")]
        [RequiredIf(nameof(Diabetes), "1", ErrorMessage = "A value is required for this question")]
        [RequiredIf(nameof(Diabetes), "2", ErrorMessage = "A value is required for this question")]
        public int? DiabetesType { get; set; }
        [Display(Name = "5b. Hypertension", GroupName = "Medical conditions")]
        [Column("HYPERTEN")]
        [RequiredIf(nameof(FormStatus), FormStatus.Complete, "A value is required for this field")]
        public int? Hypertension { get; set; }
        [Display(Name = "5c. Hypercholesterolemia", GroupName = "Medical conditions")]
        [Column("HYPERCHO")]
        [RequiredIf(nameof(FormStatus), FormStatus.Complete, "A value is required for this field")]
        public int? Hypercholesterolemia { get; set; }
        [Display(Name = "5d. B12 deficiency", GroupName = "Medical conditions")]
        [Column("B12DEF")]
        [RequiredIf(nameof(FormStatus), FormStatus.Complete, "A value is required for this field")]
        public int? B12deficiency { get; set; }
        [Display(Name = "5e. Thyroid disease", GroupName = "Medical conditions")]
        [Column("THYROID")]
        [RequiredIf(nameof(FormStatus), FormStatus.Complete, "A value is required for this field")]
        public int? ThyroidDisease { get; set; }
        [Display(Name = "5f. Arthritis (If absent or unknown, SKIP TO QUESTION 5g)", GroupName = "Medical conditions")]
        [Column("ARTHRIT")]
        [RequiredIf(nameof(FormStatus), FormStatus.Complete, "A value is required for this field")]
        public int? Arthritis { get; set; }
        [Display(Name = "5f1. Type of arthritis", GroupName = "Medical conditions")]
        [Column("ARTHTYPE")]
        [RequiredIf(nameof(Arthritis), "1", ErrorMessage = "A value is required for this question")]
        [RequiredIf(nameof(Arthritis), "2", ErrorMessage = "A value is required for this question")]
        public int? ArthritisType { get; set; }
        [Display(Name = "Other arthritis (specify)", GroupName = "Medical conditions")]
        [Column("ARTHTYPX")]
        [RequiredIf(nameof(ArthritisType), "3", ErrorMessage = "A value is required for this question")]
        [StringLength(60)]
        public string ArthritisTypeOther { get; set; }
        [Display(Name = "Upper extremity", GroupName = "Medical conditions")]
        [Column("ARTHUPEX")]
        [RequiredIf(nameof(FormStatus), FormStatus.Complete, "A value is required for this field")]
        public bool? ArthUpperExtremity { get; set; }
        [Display(Name = "Lower extremity", GroupName = "Medical conditions")]
        [Column("ARTHLOEX")]
        public bool? ArthLowerExtremity { get; set; }
        [Display(Name = "Spine", GroupName = "Medical conditions")]
        [Column("ARTHSPIN")]
        public bool? ArthSpine { get; set; }
        [Display(Name = "Unknown", GroupName = "Medical conditions")]
        [Column("ARTHUNK")]
        public bool? ArthUnknown { get; set; }
        [Display(Name = "5g. Incontinence -— Urinary", GroupName = "Medical conditions")]
        [Column("INCONTU")]
        [RequiredIf(nameof(FormStatus), FormStatus.Complete, "A value is required for this field")]
        public int? IncontinenceUrinary { get; set; }
        [Display(Name = "5h. Incontinence -— Bowel", GroupName = "Medical conditions")]
        [Column("INCONTF")]
        [RequiredIf(nameof(FormStatus), FormStatus.Complete, "A value is required for this field")]
        public int? IncontinenceBowel { get; set; }
        [Display(Name = "5i. Sleep Apnea", GroupName = "Medical conditions")]
        [Column("APNEA")]
        [RequiredIf(nameof(FormStatus), FormStatus.Complete, "A value is required for this field")]
        public int? Apnea { get; set; }
        [Display(Name = "5j. REM sleep behavior disorder (RBD)", GroupName = "Medical conditions")]
        [Column("RBD")]
        [RequiredIf(nameof(FormStatus), FormStatus.Complete, "A value is required for this field")]
        public int? RBD { get; set; }
        [Display(Name = "5k. Hyposomnia/insomnia", GroupName = "Medical conditions")]
        [Column("INSOMN")]
        [RequiredIf(nameof(FormStatus), FormStatus.Complete, "A value is required for this field")]
        public int? HyposomniaInsomnia { get; set; }
        [Display(Name = "5l. Other sleep disorder", GroupName = "Medical conditions")]
        [Column("OTHSLEEP")]
        [RequiredIf(nameof(FormStatus), FormStatus.Complete, "A value is required for this field")]
        public int? OtherSleepDisorder { get; set; }
        [Display(Name = "Other sleep disorder (specify)", GroupName = "Medical conditions")]
        [Column("OTHSLEEX")]
        [RequiredIf(nameof(OtherSleepDisorder), "1", ErrorMessage = "A value is required for this question")]
        [RequiredIf(nameof(OtherSleepDisorder), "2", ErrorMessage = "A value is required for this question")]
        [StringLength(60)]
        public string OtherSleepDisorderSpecify { get; set; }
        [Display(Name = "6a. Alcohol abuse: Clinically significant impairment occurring over a 12-month period manifested in one of the following areas: work, driving, legal, or social", GroupName = "Medical conditions")]
        [Column("ALCOHOL")]
        [RequiredIf(nameof(FormStatus), FormStatus.Complete, "A value is required for this field")]
        public int? AlcoholAbuse { get; set; }
        [Display(Name = "6b. Other abused substances: clinically significant impairment occurring over a 12-month period manifested in one of the following areas: work, driving, legal, or social. (If absent or unknown, SKIP TO QUESTION 7a)", GroupName = "Medical conditions")]
        [Column("ABUSOTHR")]
        [RequiredIf(nameof(FormStatus), FormStatus.Complete, "A value is required for this field")]
        public int? AbuseOther { get; set; }
        [Display(Name = "6b1. If recent/active or remote/inactive, specify abused substance:", GroupName = "Medical conditions")]
        [Column("ABUSX")]
        [RequiredIf(nameof(AbuseOther), "1", ErrorMessage = "A value is required for this question")]
        [RequiredIf(nameof(AbuseOther), "2", ErrorMessage = "A value is required for this question")]
        [StringLength(60)]
        public string AbuseOtherSpecify { get; set; }
        [Display(Name = "7a. Post-traumatic stress disorder (PTSD)", GroupName = "Psychiatric conditions, diagnosed or treated by a physician")]
        [Column("PTSD")]
        [RequiredIf(nameof(FormStatus), FormStatus.Complete, "A value is required for this field")]
        public int? PTSD { get; set; }
        [Display(Name = "7b. Bipolar Disorder", GroupName = "Psychiatric conditions, diagnosed or treated by a physician")]
        [Column("BIPOLAR")]
        [RequiredIf(nameof(FormStatus), FormStatus.Complete, "A value is required for this field")]
        public int? BipolarDisorder { get; set; }
        [Display(Name = "7c. Schizophrenia", GroupName = "Psychiatric conditions, diagnosed or treated by a physician")]
        [Column("SCHIZ")]
        [RequiredIf(nameof(FormStatus), FormStatus.Complete, "A value is required for this field")]
        public int? Schizophrenia { get; set; }
        [Display(Name = "7d1. Active depression in the last two years", GroupName = "Psychiatric conditions, diagnosed or treated by a physician")]
        [Column("DEP2YRS")]
        [RequiredIf(nameof(FormStatus), FormStatus.Complete, "A value is required for this field")]
        public int? DepressionLastTwoYears { get; set; }
        [Display(Name = "7d2. 3 Depression episodes more than two years ago", GroupName = "Psychiatric conditions, diagnosed or treated by a physician")]
        [Column("DEPOTHR")]
        [RequiredIf(nameof(FormStatus), FormStatus.Complete, "A value is required for this field")]
        public int? DepressionPriorTwoYears { get; set; }
        [Display(Name = "7e. Anxiety", GroupName = "Psychiatric conditions, diagnosed or treated by a physician")]
        [Column("ANXIETY")]
        [RequiredIf(nameof(FormStatus), FormStatus.Complete, "A value is required for this field")]
        public int? Anxiety { get; set; }
        [Display(Name = "7f. Obsessive-compulsive disorder (OCD)", GroupName = "Psychiatric conditions, diagnosed or treated by a physician")]
        [Column("OCD")]
        [RequiredIf(nameof(FormStatus), FormStatus.Complete, "A value is required for this field")]
        public int? OCD { get; set; }
        [Display(Name = "7g. Developmental neuropsychiatric disorders (e.g., autism spectrum disorder [ASD], attention-deficit hyperactivity disorder [ADHD], dyslexia)", GroupName = "Psychiatric conditions, diagnosed or treated by a physician")]
        [Column("NPSYDEV")]
        [RequiredIf(nameof(FormStatus), FormStatus.Complete, "A value is required for this field")]
        public int? NpsyDev { get; set; }
        [Display(Name = "7h. Other psychiatric disorders (If absent or unknown, END FORM HERE)", GroupName = "Psychiatric conditions, diagnosed or treated by a physician")]
        [Column("PSYCDIS")]
        [RequiredIf(nameof(FormStatus), FormStatus.Complete, "A value is required for this field")]
        public int? PsychOther { get; set; }
        [Display(Name = "7h1. If recent/active or remote/inactive, specify disorder", GroupName = "Psychiatric conditions, diagnosed or treated by a physician")]
        [Column("PSYCDISX")]
        [RequiredIf(nameof(PsychOther), "1", ErrorMessage = "A value is required for this question")]
        [RequiredIf(nameof(PsychOther), "2", ErrorMessage = "A value is required for this question")]
        [StringLength(60)]
        public string PsychOtherSpecify { get; set; }
        public SubjectHealthHistory()
        {
            Version = "3.0.0";
        }
    }
} 