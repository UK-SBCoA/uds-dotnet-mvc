using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using UDS.Net.Data.DataAnnotations;
using UDS.Net.Data.Enums;

namespace UDS.Net.Data.Entities
{
    [Table("tbl_B3")]
    public class UnifiedParkinsonsDiseaseRatingScale: FormBase
    {
        [DisplayName("[Optional] If the clinician completes the UPDRS examination and determines all items are normal, check this box and end form here.")]
        [Column("PDNORMAL")]
        public bool? IsNormal {get;set;}
        [DisplayName("1. Speech")]
        [Column("SPEECH")]
        [RequiredIf(nameof(IsNormal), false, ErrorMessage = "Please provide a value for this question")]
        public int? Speech {get;set;}
        [Column("SPEECHX")]
        [RequiredIf(nameof(Speech), "8", ErrorMessage = "Untestable, please provide a reason")]
        [StringLength(60)]
        public string SpeechUntestable {get;set;}
        [DisplayName("2. Facial expression")]
        [Column("FACEXP")]
        [RequiredIf(nameof(IsNormal), false, ErrorMessage = "Please provide a value for this question")]
        public int? FacialExpression {get;set;}
        [Column("FACEXPX")]
        [RequiredIf(nameof(FacialExpression), "8", ErrorMessage = "Untestable, please provide a reason")]
        [StringLength(60)]
        public string FacialExpressionUntestable {get;set;}
        [DisplayName("3a. Face, lips, chin")]
        [Column("TRESTFAC")]
        [RequiredIf(nameof(IsNormal), false, ErrorMessage = "Please provide a value for this question")]
        public int? TremorRestFaceLipsChin {get;set;}
        [Column("TRESTFAX")]
        [RequiredIf(nameof(TremorRestFaceLipsChin), "8", ErrorMessage = "Untestable, please provide a reason")]
        [StringLength(60)]
        public string TremorRestFaceLipsChinUntestable {get;set;}
        [DisplayName("3b. Right hand")]
        [Column("TRESTRHD")]
        [RequiredIf(nameof(IsNormal), false, ErrorMessage = "Please provide a value for this question")]
        public int? TremorRestRightHand {get;set;}
        [Column("TRESTRHX")]
        [RequiredIf(nameof(TremorRestRightHand), "8", ErrorMessage = "Untestable, please provide a reason")]
        [StringLength(60)]
        public string TremorRestRightHandUntestable {get;set;}
        [DisplayName("3c. Left hand")]
        [Column("TRESTLHD")]
        [RequiredIf(nameof(IsNormal), false, ErrorMessage = "Please provide a value for this question")]
        public int? TremorRestLeftHand {get;set;}
        [Column("TRESTLHX")]
        [RequiredIf(nameof(TremorRestLeftHand), "8", ErrorMessage = "Untestable, please provide a reason")]
        [StringLength(60)]
        public string TremorRestLeftHandUntestable {get;set;}
        [DisplayName("3d. Right foot")]
        [Column("TRESTRFT")]
        [RequiredIf(nameof(IsNormal), false, ErrorMessage = "Please provide a value for this question")]
        public int? TremorRestRightFoot {get;set;}
        [Column("TRESTRFX")]
        [RequiredIf(nameof(TremorRestRightFoot), "8", ErrorMessage = "Untestable, please provide a reason")]
        [StringLength(60)]
        public string TremorRestRightFootUntestable {get;set;}
        [DisplayName("3e. Left foot")]
        [Column("TRESTLFT")]
        [RequiredIf(nameof(IsNormal), false, ErrorMessage = "Please provide a value for this question")]
        public int? TremorRestLeftFoot {get;set;}
        [Column("TRESTLFX")]
        [RequiredIf(nameof(TremorRestLeftFoot), "8", ErrorMessage = "Untestable, please provide a reason")]
        [StringLength(60)]
        public string TremorRestLeftFootOther {get;set;}
        [DisplayName("4a. Right hand")]
        [Column("TRACTRHD")]
        [RequiredIf(nameof(IsNormal), false, ErrorMessage = "Please provide a value for this question")]
        public int? TremorActionRightHand {get;set;}
        [Column("TRACTRHX")]
        [RequiredIf(nameof(TremorActionRightHand), "8", ErrorMessage = "Untestable, please provide a reason")]
        [StringLength(60)]
        public string TremorActionRightHandOther {get;set;}
        [DisplayName("4b. Left hand")]
        [Column("TRACTLHD")]
        [RequiredIf(nameof(IsNormal), false, ErrorMessage = "Please provide a value for this question")]
        public int? TremorActionLeftHand {get;set;}
        [Column("TRACTLHX")]
        [RequiredIf(nameof(TremorActionLeftHand), "8", ErrorMessage = "Untestable, please provide a reason")]
        [StringLength(60)]
        public string TremorActionLeftHandOther {get;set;}
        [DisplayName("5a. Neck")]
        [Column("RIGDNECK")]
        [RequiredIf(nameof(IsNormal), false, ErrorMessage = "Please provide a value for this question")]
        public int? RigidityNeck {get;set;}
        [Column("RIGDNEX")]
        [RequiredIf(nameof(RigidityNeck), "8", ErrorMessage = "Untestable, please provide a reason")]
        [StringLength(60)]
        public string RigidityNeckUntestable {get;set;}
        [DisplayName("5b. Right upper extremity")]
        [Column("RIGDUPRT")]
        [RequiredIf(nameof(IsNormal), false, ErrorMessage = "Please provide a value for this question")]
        public int? RigidityRightUpper {get;set;}
        [Column("RIGDUPRX")]
        [RequiredIf(nameof(RigidityRightUpper), "8", ErrorMessage = "Untestable, please provide a reason")]
        [StringLength(60)]
        public string RigidityRightUpperUntestable {get;set;}
        [DisplayName("5c. Left upper extremity")]
        [Column("RIGDUPLF")]
        [RequiredIf(nameof(IsNormal), false, ErrorMessage = "Please provide a value for this question")]
        public int? RigidityLeftUpper {get;set;}
        [Column("RIGDUPLX")]
        [RequiredIf(nameof(RigidityLeftUpper), "8", ErrorMessage = "Untestable, please provide a reason")]
        [StringLength(60)]
        public string RigidityLeftUpperUntestable {get;set;}
        [DisplayName("5d. Right lower extremity")]
        [Column("RIGDLORT")]
        [RequiredIf(nameof(IsNormal), false, ErrorMessage = "Please provide a value for this question")]
        public int? RigidityRightLower {get;set;}
        [Column("RIGDLORX")]
        [RequiredIf(nameof(RigidityRightLower), "8", ErrorMessage = "Untestable, please provide a reason")]
        [StringLength(60)]
        public string RigidityRightLowerUntestable {get;set;}
        [DisplayName("5e. Left lower extremity")]
        [Column("RIGDLOLF")]
        [RequiredIf(nameof(IsNormal), false, ErrorMessage = "Please provide a value for this question")]
        public int? RigidityLeftLower {get;set;}
        [Column("RIGDLOLX")]
        [RequiredIf(nameof(RigidityLeftLower), "8", ErrorMessage = "Untestable, please provide a reason")]
        [StringLength(60)]
        public string RigidityLeftLowerUntestable {get;set;}
        [DisplayName("6a. Right hand")]
        [Column("TAPSRT")]
        [RequiredIf(nameof(IsNormal), false, ErrorMessage = "Please provide a value for this question")]
        public int? FingerTapsRightHand {get;set;}
        [Column("TAPSRTX")]
        [RequiredIf(nameof(FingerTapsRightHand), "8", ErrorMessage = "Untestable, please provide a reason")]
        [StringLength(60)]
        public string FingerTapsRightHandUntestable {get;set;}
        [DisplayName("6b. Left hand")]
        [Column("TAPSLF")]
        [RequiredIf(nameof(IsNormal), false, ErrorMessage = "Please provide a value for this question")]
        public int? FingerTapsLeftHand {get;set;}
        [Column("TAPSLFX")]
        [RequiredIf(nameof(FingerTapsLeftHand), "8", ErrorMessage = "Untestable, please provide a reason")]
        [StringLength(60)]
        public string FingerTapsLeftHandUntestable {get;set;}
        [DisplayName("7a. Right hand")]
        [Column("HANDMOVR")]
        [RequiredIf(nameof(IsNormal), false, ErrorMessage = "Please provide a value for this question")]
        public int? HandMovementsRightHand {get;set;}
        [Column("HANDMVRX")]
        [RequiredIf(nameof(HandMovementsRightHand), "8", ErrorMessage = "Untestable, please provide a reason")]
        [StringLength(60)]
        public string HandMovementsRightHandUntestable {get;set;}
        [DisplayName("7b. Left hand")]
        [Column("HANDMOVL")]
        [RequiredIf(nameof(IsNormal), false, ErrorMessage = "Please provide a value for this question")]
        public int? HandMovemonetsLeftHand {get;set;}
        [Column("HANDMVLX")]
        [RequiredIf(nameof(HandMovemonetsLeftHand), "8", ErrorMessage = "Untestable, please provide a reason")]
        [StringLength(60)]
        public string HandMovemonetsLeftHandUntestable {get;set;}
        [DisplayName("8a. Right hand")]
        [Column("HANDALTR")]
        [RequiredIf(nameof(IsNormal), false, ErrorMessage = "Please provide a value for this question")]
        public int? RapidRightHand {get;set;}
        [Column("HANDATRX")]
        [RequiredIf(nameof(RapidRightHand), "8", ErrorMessage = "Untestable, please provide a reason")]
        [StringLength(60)]
        public string RapidRightHandUntestable {get;set;}
        [DisplayName("8b. Left hand")]
        [Column("HANDALTL")]
        [RequiredIf(nameof(IsNormal), false, ErrorMessage = "Please provide a value for this question")]
        public int? RapidLeftHand {get;set;}
        [Column("HANDATLX")]
        [RequiredIf(nameof(RapidLeftHand), "8", ErrorMessage = "Untestable, please provide a reason")]
        [StringLength(60)]
        public string RapidLeftHandUntestable {get;set;}
        [DisplayName("9a. Right leg")]
        [Column("LEGRT")]
        [RequiredIf(nameof(IsNormal), false, ErrorMessage = "Please provide a value for this question")]
        public int? RightLegAgility {get;set;}
        [Column("LEGRTX")]
        [RequiredIf(nameof(RightLegAgility), "8", ErrorMessage = "Untestable, please provide a reason")]
        [StringLength(60)]
        public string RightLegAgilityUntestable {get;set;}
        [DisplayName("9b. Left leg")]
        [Column("LEGLF")]
        [RequiredIf(nameof(IsNormal), false, ErrorMessage = "Please provide a value for this question")]
        public int? LeftLegAgility {get;set;}
        [Column("LEGLFX")]
        [RequiredIf(nameof(LeftLegAgility), "8", ErrorMessage = "Untestable, please provide a reason")]
        [StringLength(60)]
        public string LeftLegAgilityUntestable {get;set;}
        [DisplayName("10. Arising from chair (patient attempts to rise from a straight-backed chair, with arms folded across chest)")]
        [Column("ARISING")]
        [RequiredIf(nameof(IsNormal), false, ErrorMessage = "Please provide a value for this question")]
        public int? ArisingFromChair {get;set;}
        [Column("ARISINGX")]
        [RequiredIf(nameof(ArisingFromChair), "8", ErrorMessage = "Untestable, please provide a reason")]
        [StringLength(60)]
        public string ArisingFromChairUntestable {get;set;}
        [DisplayName("11. Posture")]
        [Column("POSTURE")]
        [RequiredIf(nameof(IsNormal), false, ErrorMessage = "Please provide a value for this question")]
        public int? Posture {get;set;}
        [Column("POSTUREX")]
        [RequiredIf(nameof(Posture), "8", ErrorMessage = "Untestable, please provide a reason")]
        [StringLength(60)]
        public string PostureUntestable {get;set;}
        [DisplayName("12. Gait")]
        [Column("GAIT")]
        [RequiredIf(nameof(IsNormal), false, ErrorMessage = "Please provide a value for this question")]
        public int? Gait {get;set;}
        [Column("GAITX")]
        [RequiredIf(nameof(Gait), "8", ErrorMessage = "Untestable, please provide a reason")]
        [StringLength(60)]
        public string GaitUntestable {get;set;}
        [DisplayName("13. Posture stability (response to sudden, strong posterior displacement produced by pull on shoulders while patient erect with eyes open and feet slightly apart; patient is prepared)")]
        [Column("POSSTAB")]
        [RequiredIf(nameof(IsNormal), false, ErrorMessage = "Please provide a value for this question")]
        public int? PostureStability {get;set;}
        [Column("POSSTABX")]
        [RequiredIf(nameof(PostureStability), "8", ErrorMessage = "Untestable, please provide a reason")]
        [StringLength(60)]
        public string PostureStabilityUntestable {get;set;}
        [DisplayName("14. Body bradykinesia and hypokinesia (combining slowness, hesitancy, decreased arm swing, small amplitude, and poverty of movement in general)")]
        [Column("BRADYKIN")]
        [RequiredIf(nameof(IsNormal), false, ErrorMessage = "Please provide a value for this question")]
        public int? BradykinesiaAndHypokinesia {get;set;}
        [Column("BRADYKIX")]
        [RequiredIf(nameof(BradykinesiaAndHypokinesia), "8", ErrorMessage = "Untestable, please provide a reason")]
        [StringLength(60)]
        public string BradykinesiaAndHypokinesiaUntestable {get;set;}
    }
}