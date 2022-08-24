using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using UDS.Net.Data.DataAnnotations;
using UDS.Net.Data.Enums;

namespace UDS.Net.Data.Entities
{
  [Table("tbl_C2")]
  public class NeuropsychologicalBatteryScores : FormBase
  {
    [Display(Name = "Was any part of MoCA administered?")]
    [Column("MOCACOMP")]
    [RequiredIf(nameof(FormStatus), FormStatus.Complete, ErrorMessage = "Value Required")]
    public bool? MoCAPartAdministered { get; set; }

    [Display(Name = "Was any part of MoCA administered? If No, enter reason code, 95–98")]
    [Range(95, 98, ErrorMessage = "Value outside of required range")]
    [Column("MOCAREAS")]
    [RequiredIf(nameof(MoCAPartAdministered), false, ErrorMessage = "Value Required")]
    public int? MoCAPartAdministeredCode { get; set; }

    [Display(Name = "MoCA was administered?")]
    [Range(1, 3, ErrorMessage = "Value outside of required range")]
    [Column("MOCALOC")]
    [RequiredIf(nameof(MoCAPartAdministered), true, ErrorMessage = "Value Required")]
    public int? MoCAAdministered { get; set; }

    [Display(Name = "Language of MoCA administration?")]
    [Range(1, 3, ErrorMessage = "Value outside of required range")]
    [Column("MOCALAN")]
    [RequiredIf(nameof(MoCAPartAdministered), true, ErrorMessage = "Value Required")]
    public int? MoCALanguage { get; set; }

    [Display(Name = "Language of MoCA administration — Other specify?")]
    [MaxLength(60)]
    [Column("MOCALANX")]
    [RequiredIf(nameof(MoCALanguage), 3, ErrorMessage = "Value Required")]
    public string MoCALanguageOther { get; set; }

    [Display(Name = "Subject was unable to complete one or more sections due to visual impairment")]
    [Column("MOCAVIS")]
    [RequiredIf(nameof(MoCAPartAdministered), true, ErrorMessage = "Value Required")]
    public bool? MoCAVisualImpairment { get; set; }

    [Display(Name = "Subject was unable to complete one or more sections due to hearing impairment")]
    [Column("MOCAHEAR")]
    [RequiredIf(nameof(MoCAPartAdministered), true, ErrorMessage = "Value Required")]
    public bool? MoCAHearingImpairment { get; set; }

    [Display(Name = "Total Raw Score - Uncorrected: (Not corrected for education or visual/hearing impairment)")]
    [Range(0, 88, ErrorMessage = "Value outside of required range")]
    [InvalidRange(nameof(TotalRawScore), 31, 87, ErrorMessage = "Value outside of required range")]
    [Column("MOCATOTS")]
    [RequiredIf(nameof(MoCAPartAdministered), true, ErrorMessage = "Value Required")]
    public int? TotalRawScore { get; set; }

    [Display(Name = "Visuospatial/executive — Trails")]
    [Range(0, 98, ErrorMessage = "Value outside of required range")]
    [InvalidRange(nameof(MoCATrials), 2, 94, ErrorMessage = "Value outside of required range")]
    [Column("MOCATRAI")]
    [RequiredIf(nameof(MoCAPartAdministered), true, ErrorMessage = "Value Required")]
    public int? MoCATrials { get; set; }

    [Display(Name = "Visuospatial/executive — Cube")]
    [Range(0, 98, ErrorMessage = "Value outside of required range")]
    [InvalidRange(nameof(MoCACube), 2, 94, ErrorMessage = "Value outside of required range")]
    [Column("MOCACUBE")]
    [RequiredIf(nameof(MoCAPartAdministered), true, ErrorMessage = "Value Required")]
    public int? MoCACube { get; set; }

    [Display(Name = "Visuospatial/executive — Clock contour")]
    [Range(0, 98, ErrorMessage = "Value outside of required range")]
    [InvalidRange(nameof(MoCAClockContour), 2, 94, ErrorMessage = "Value outside of required range")]
    [Column("MOCACLOC")]
    [RequiredIf(nameof(MoCAPartAdministered), true, ErrorMessage = "Value Required")]
    public int? MoCAClockContour { get; set; }

    [Display(Name = "Visuospatial/executive — Clock numbers")]
    [Range(0, 98, ErrorMessage = "Value outside of required range")]
    [InvalidRange(nameof(MoCAClockNumbers), 2, 94, ErrorMessage = "Value outside of required range")]
    [Column("MOCACLON")]
    [RequiredIf(nameof(MoCAPartAdministered), true, ErrorMessage = "Value Required")]
    public int? MoCAClockNumbers { get; set; }

    [Display(Name = "Visuospatial/executive — Clock hands")]
    [Range(0, 98, ErrorMessage = "Value outside of required range")]
    [InvalidRange(nameof(MoCAHands), 2, 94, ErrorMessage = "Value outside of required range")]
    [Column("MOCACLOH")]
    [RequiredIf(nameof(MoCAPartAdministered), true, ErrorMessage = "Value Required")]
    public int? MoCAHands { get; set; }

    [Display(Name = "Language —  Naming")]
    [Range(0, 98, ErrorMessage = "Value outside of required range")]
    [InvalidRange(nameof(MoCANaming), 4, 94, ErrorMessage = "Value outside of required range")]
    [Column("MOCANAMI")]
    [RequiredIf(nameof(MoCAPartAdministered), true, ErrorMessage = "Value Required")]
    public int? MoCANaming { get; set; }

    [Display(Name = "Memory —  Registration (two trials)")]
    [Range(0, 98, ErrorMessage = "Value outside of required range")]
    [InvalidRange(nameof(MoCARegistration), 11, 94, ErrorMessage = "Value outside of required range")]
    [Column("MOCAREGI")]
    [RequiredIf(nameof(MoCAPartAdministered), true, ErrorMessage = "Value Required")]
    public int? MoCARegistration { get; set; }

    [Display(Name = "Attention —  Digits")]
    [Range(0, 98, ErrorMessage = "Value outside of required range")]
    [InvalidRange(nameof(MoCADigits), 3, 94, ErrorMessage = "Value outside of required range")]
    [Column("MOCADIGI")]
    [RequiredIf(nameof(MoCAPartAdministered), true, ErrorMessage = "Value Required")]
    public int? MoCADigits { get; set; }

    [Display(Name = "Attention —  Letter A")]
    [Range(0, 98, ErrorMessage = "Value outside of required range")]
    [InvalidRange(nameof(MoCALetter), 2, 94, ErrorMessage = "Value outside of required range")]
    [Column("MOCALETT")]
    [RequiredIf(nameof(MoCAPartAdministered), true, ErrorMessage = "Value Required")]
    public int? MoCALetter { get; set; }

    [Display(Name = "Attention —  Serial 7s")]
    [Range(0, 98, ErrorMessage = "Value outside of required range")]
    [InvalidRange(nameof(MoCASerial), 4, 94, ErrorMessage = "Value outside of required range")]
    [Column("MOCASER7")]
    [RequiredIf(nameof(MoCAPartAdministered), true, ErrorMessage = "Value Required")]
    public int? MoCASerial { get; set; }

    [Display(Name = "Language —  Repetition")]
    [Range(0, 98, ErrorMessage = "Value outside of required range")]
    [InvalidRange(nameof(MoCARepetition), 3, 94, ErrorMessage = "Value outside of required range")]
    [Column("MOCAREPE")]
    [RequiredIf(nameof(MoCAPartAdministered), true, ErrorMessage = "Value Required")]
    public int? MoCARepetition { get; set; }

    [Display(Name = "Language —  Fluency")]
    [Range(0, 98, ErrorMessage = "Value outside of required range")]
    [InvalidRange(nameof(MoCAFluency), 2, 94, ErrorMessage = "Value outside of required range")]
    [Column("MOCAFLUE")]
    [RequiredIf(nameof(MoCAPartAdministered), true, ErrorMessage = "Value Required")]
    public int? MoCAFluency { get; set; }

    [Display(Name = "Abstraction")]
    [Range(0, 98, ErrorMessage = "Value outside of required range")]
    [InvalidRange(nameof(MoCAAbstraction), 3, 94, ErrorMessage = "Value outside of required range")]
    [Column("MOCAABST")]
    [RequiredIf(nameof(MoCAPartAdministered), true, ErrorMessage = "Value Required")]
    public int? MoCAAbstraction { get; set; }

    [Display(Name = "Delayed recall — No cue")]
    [Range(0, 98, ErrorMessage = "Value outside of required range")]
    [InvalidRange(nameof(MoCANoCue), 6, 94, ErrorMessage = "Value outside of required range")]
    [Column("MOCARECN")]
    [RequiredIf(nameof(MoCAPartAdministered), true, ErrorMessage = "Value Required")]
    public int? MoCANoCue { get; set; }

    [Display(Name = "Delayed recall — Category cue")]
    [Range(0, 88, ErrorMessage = "Value outside of required range")]
    [InvalidRange(nameof(MoCACategoryCue), 6, 87, ErrorMessage = "Value outside of required range")]
    [Column("MOCARECC")]
    [RequiredIf(nameof(MoCAPartAdministered), true, ErrorMessage = "Value Required")]
    public int? MoCACategoryCue { get; set; }

    [Display(Name = "Delayed recall  — Recognition")]
    [Range(0, 88, ErrorMessage = "Value outside of required range")]
    [InvalidRange(nameof(MoCARecognition), 6, 87, ErrorMessage = "Value outside of required range")]
    [Column("MOCARECR")]
    [RequiredIf(nameof(MoCAPartAdministered), true, ErrorMessage = "Value Required")]
    public int? MoCARecognition { get; set; }

    // Rules for MoCANoCue, MoCACategoryCue, MoCARecognition
    // if no reason codes or N/As then MoCANoCue + MoCACategoryCue + MoCARecognition <= 5.
    [NotMapped]
    [RequiredIf(nameof(FormStatus), FormStatus.Complete, ErrorMessage = "The sum of the Delayed recall questions, excluding reason variable codes, must be equal to or less than 5")]
    public bool? DelayedRecallIsValid { 
      get {
        int delayedRecallTotal = 0;

        if(MoCANoCue.HasValue && MoCACategoryCue.HasValue && MoCARecognition.HasValue) {
          int moCANoCueValue = MoCANoCue.Value >= 95 && MoCANoCue.Value <= 98 ? 0 : MoCANoCue.Value;
          int moCACategoryCueValue = MoCACategoryCue.Value != 88 ? MoCACategoryCue.Value : 0;
          int moCARecognitionValue = MoCARecognition.Value != 88 ? MoCARecognition.Value : 0;

          delayedRecallTotal += moCANoCueValue + moCACategoryCueValue + moCARecognitionValue;
        }

        return delayedRecallTotal <= 5 ? true : null;
      }
    }

    [Display(Name = "Orientation — Date")]
    [Range(0, 98, ErrorMessage = "Value outside of required range")]
    [InvalidRange(nameof(MoCADate), 2, 94, ErrorMessage = "Value outside of required range")]
    [Column("MOCAORDT")]
    [RequiredIf(nameof(MoCAPartAdministered), true, ErrorMessage = "Value Required")]
    public int? MoCADate { get; set; }

    [Display(Name = "Orientation — Month")]
    [Range(0, 98, ErrorMessage = "Value outside of required range")]
    [InvalidRange(nameof(MoCAMonth), 2, 94, ErrorMessage = "Value outside of required range")]
    [Column("MOCAORMO")]
    [RequiredIf(nameof(MoCAPartAdministered), true, ErrorMessage = "Value Required")]
    public int? MoCAMonth { get; set; }

    [Display(Name = "Orientation — Year")]
    [Range(0, 98, ErrorMessage = "Value outside of required range")]
    [InvalidRange(nameof(MoCAYear), 2, 94, ErrorMessage = "Value outside of required range")]
    [Column("MOCAORYR")]
    [RequiredIf(nameof(MoCAPartAdministered), true, ErrorMessage = "Value Required")]
    public int? MoCAYear { get; set; }

    [Display(Name = "Orientation — Day")]
    [Range(0, 98, ErrorMessage = "Value outside of required range")]
    [InvalidRange(nameof(MoCADay), 2, 94, ErrorMessage = "Value outside of required range")]
    [Column("MOCAORDY")]
    [RequiredIf(nameof(MoCAPartAdministered), true, ErrorMessage = "Value Required")]
    public int? MoCADay { get; set; }

    [Display(Name = "Orientation — Place")]
    [Range(0, 98, ErrorMessage = "Value outside of required range")]
    [InvalidRange(nameof(MoCAPlace), 2, 94, ErrorMessage = "Value outside of required range")]
    [Column("MOCAORPL")]
    [RequiredIf(nameof(MoCAPartAdministered), true, ErrorMessage = "Value Required")]
    public int? MoCAPlace { get; set; }

    [Display(Name = "Orientation —  City")]
    [Range(0, 98, ErrorMessage = "Value outside of required range")]
    [InvalidRange(nameof(MoCACity), 2, 94, ErrorMessage = "Value outside of required range")]
    [Column("MOCAORCT")]
    [RequiredIf(nameof(MoCAPartAdministered), true, ErrorMessage = "Value Required")]
    public int? MoCACity { get; set; }

    [Display(Name = "The tests following the MoCA were administered")]
    [Range(1, 3, ErrorMessage = "Value outside of required range")]
    [Column("NPSYCLOC")]
    [RequiredIf(nameof(FormStatus), FormStatus.Complete, ErrorMessage = "Value Required")]
    public string MoCATestsAdministered { get; set; }

    [Display(Name = "Language of test administration")]
    [Range(1, 3, ErrorMessage = "Value outside of required range")]
    [Column("NPSYLAN")]
    [RequiredIf(nameof(FormStatus), FormStatus.Complete, ErrorMessage = "Value Required")]
    public string MoCATestLanguage { get; set; }

    [Display(Name = "following MoCA: Language of test administration  — Other specify")]
    [MaxLength(60)]
    [Column("NPSYLANX")]
    [RequiredIf(nameof(MoCATestLanguage), 3, ErrorMessage = "Value Required")]
    public string MoCATestLanguageOther { get; set; }

    [Display(Name = "Total story units recalled, verbatim scoring")]
    [Range(0, 98, ErrorMessage = "Value outside of required range")]
    [InvalidRange(nameof(CraftStoryVerbatim), 45, 94, ErrorMessage = "Value outside of required range")]
    [Column("CRAFTVRS")]
    [RequiredIf(nameof(FormStatus), FormStatus.Complete, ErrorMessage = "Value Required")]
    public string CraftStoryVerbatim { get; set; }

    [Display(Name = "Total story units recalled, paraphrase scoring")]
    [Range(0, 25, ErrorMessage = "Value outside of required range")]
    [Column("CRAFTURS")]
    [RequiredIfRange(nameof(CraftStoryVerbatim), 0, 44, ErrorMessage = "Value Required")]
    public string CraftStoryParaphrase { get; set; }

    [Display(Name = "Total Score for copy of Benson figure")]
    [Range(0, 98, ErrorMessage = "Value outside of required range")]
    [InvalidRange(nameof(BensonFigureScore), 18, 94, ErrorMessage = "Value outside of required range")]
    [Column("UDSBENTC")]
    [RequiredIf(nameof(FormStatus), FormStatus.Complete, ErrorMessage = "Value Required")]
    public string BensonFigureScore { get; set; }

    [Display(Name = "Number of correct trials")]
    [Range(0, 98, ErrorMessage = "Value outside of required range")]
    [InvalidRange(nameof(NumberSpanTestForwardCorrectTrials), 15, 94, ErrorMessage = "Value outside of required range")]
    [Column("DIGFORCT")]
    [RequiredIf(nameof(FormStatus), FormStatus.Complete, ErrorMessage = "Value Required")]
    public string NumberSpanTestForwardCorrectTrials { get; set; }

    [Display(Name = "Longest span forward")]
    [Range(0, 9, ErrorMessage = "Value outside of required range")]
    [InvalidRange(nameof(NumberSpanTestForwardLongestSpanForward), 1, 2, ErrorMessage = "Value outside of required range")]
    [Column("DIGFORSL")]
    public int? NumberSpanTestForwardLongestSpanForward { get; set; }

    [Display(Name = "Number of correct trials")]
    [Range(0, 98, ErrorMessage = "Value outside of required range")]
    [InvalidRange(nameof(NumberSpanTestBackwardCorrectTrials), 15, 94, ErrorMessage = "Value outside of required range")]
    [Column("DIGBACCT")]
    [RequiredIf(nameof(FormStatus), FormStatus.Complete, ErrorMessage = "Value Required")]
    public int? NumberSpanTestBackwardCorrectTrials { get; set; }

    [Display(Name = "Longest span backward")]
    [Range(0, 8, ErrorMessage = "Value outside of required range")]
    [InvalidRange(nameof(NumberSpanTestBackwardLongestSpanBackward), 1, 1, ErrorMessage = "Value outside of required range")]
    [Column("DIGBACLS")]
    [RequiredIfRange(nameof(NumberSpanTestBackwardCorrectTrials), 0, 77, ErrorMessage = "Value Required")]
    public int? NumberSpanTestBackwardLongestSpanBackward { get; set; }

    [Display(Name = "Animals: Total number of animals named in 60 seconds")]
    [Range(0, 98, ErrorMessage = "Value outside of required range")]
    [InvalidRange(nameof(CategoryFluencyAnimals), 78, 94, ErrorMessage = "Value outside of required range")]
    [Column("ANIMALS")]
    [RequiredIf(nameof(FormStatus), FormStatus.Complete, ErrorMessage = "Value Required")]
    public int? CategoryFluencyAnimals { get; set; }

    [Display(Name = "Vegetables: Total number of vegtables named in 60 seconds")]
    [Range(0, 98, ErrorMessage = "Value outside of required range")]
    [InvalidRange(nameof(CategoryFluencyVegetables), 78, 94, ErrorMessage = "Value outside of required range")]
    [Column("VEG")]
    [RequiredIf(nameof(FormStatus), FormStatus.Complete, ErrorMessage = "Value Required")]
    public int? CategoryFluencyVegetables { get; set; }

    [Display(Name = "PART A: Total number of seconds to complete")]
    [Range(0, 998, ErrorMessage = "Value outside of required range")]
    [InvalidRange(nameof(TrailMakingTestASecondsToComplete), 151, 994, ErrorMessage = "Value outside of required range")]
    [Column("TRAILA")]
    [RequiredIf(nameof(FormStatus), FormStatus.Complete, ErrorMessage = "Value Required")]
    public int? TrailMakingTestASecondsToComplete { get; set; }

    [Display(Name = "Number of commission errors")]
    [Range(0, 40, ErrorMessage = "Value outside of required range")]
    [Column("TRAILARR")]
    [RequiredIfRange(nameof(TrailMakingTestASecondsToComplete), 0, 40, ErrorMessage = "Value Required")]
    public int? TrailMakingTestANumberOfErrors { get; set; }

    [Display(Name = "Number of correct lines")]
    [Range(0, 24, ErrorMessage = "Value outside of required range")]
    [Column("TRAILALI")]
    [RequiredIfRange(nameof(TrailMakingTestASecondsToComplete), 0, 24, ErrorMessage = "Value Required")]
    public int? TrailMakingTestANumberOfCorrect { get; set; }

    [Display(Name = "PART B: Total number of seconds to complete")]
    [Range(0, 998, ErrorMessage = "Value outside of required range")]
    [InvalidRange(nameof(TrailMakingTestBSecondsToComplete), 301, 994, ErrorMessage = "Value outside of required range")]
    [Column("TRAILB")]
    [RequiredIf(nameof(FormStatus), FormStatus.Complete, ErrorMessage = "Value Required")]
    public int? TrailMakingTestBSecondsToComplete { get; set; }

    [Display(Name = "Number of commission errors")]
    [Range(0, 40, ErrorMessage = "Value outside of required range")]
    [Column("TRAILBRR")]
    [RequiredIfRange(nameof(TrailMakingTestBSecondsToComplete), 0, 40, ErrorMessage = "Value Required")]
    public int? TrailMakingTestBNumberOfErrors { get; set; }

    [Display(Name = "Number of correct lines")]
    [Range(0, 24, ErrorMessage = "Value outside of required range")]
    [Column("TRAILBLI")]
    [RequiredIfRange(nameof(TrailMakingTestBSecondsToComplete), 0, 24, ErrorMessage = "Value Required")]
    public int? TrailMakingTestBNumberOfCorrect { get; set; }

    [Display(Name = "Total story units recalled, verbatim scoring")]
    [Range(0, 98, ErrorMessage = "Value outside of required range")]
    [InvalidRange(nameof(CraftStoryRecallUnitsRecallVerbatim), 45, 94, ErrorMessage = "Value outside of required range")]
    [Column("CRAFTDVR")]
    [RequiredIf(nameof(FormStatus), FormStatus.Complete, ErrorMessage = "Value Required")]

    public int? CraftStoryRecallUnitsRecallVerbatim { get; set; }

    [Display(Name = "Total story units recalled, paraphrase scoring")]
    [Range(0, 25, ErrorMessage = "Value outside of required range")]
    [Column("CRAFTDRE")]
    [RequiredIfRange(nameof(CraftStoryRecallUnitsRecallVerbatim), 0, 44, ErrorMessage = "Value Required")]
    public int? CraftStoryRecallUnitsRecallParaphrase { get; set; }

    [Display(Name = "Delay time (minutes)")]
    [Range(0, 99, ErrorMessage = "Value outside of required range")]
    [Column("CRAFTDTI")]
    [RequiredIfRange(nameof(CraftStoryRecallUnitsRecallVerbatim), 0, 44, ErrorMessage = "Value Required")]
    public int? CraftStoryRecallUnitsRecallDelay { get; set; }

    [Display(Name = "Cue (boy) needed")]
    [Column("CRAFTCUE")]
    [RequiredIfRange(nameof(CraftStoryRecallUnitsRecallVerbatim), 0, 44, ErrorMessage = "Value Required")]
    public bool? CraftStoryRecallCueNeeded { get; set; }

    [Display(Name = "Total score for drawing of Benson figure following 10- to 15-minute delay")]
    [Range(0, 98, ErrorMessage = "Value outside of required range")]
    [InvalidRange(nameof(BensonComplexFigureRecallTotalDrawingScore), 18, 94, ErrorMessage = "Value outside of required range")]
    [Column("UDSBENTD")]
    [RequiredIf(nameof(FormStatus), FormStatus.Complete, ErrorMessage = "Value Required")]
    public int? BensonComplexFigureRecallTotalDrawingScore { get; set; }

    [Display(Name = "Recognized original stimulus among four options")]
    [Column("UDSBENRS")]
    [RequiredIfRange(nameof(BensonComplexFigureRecallTotalDrawingScore), 0, 32, ErrorMessage = "Value Required")]
    public bool? BensonComplexFigureRecallRecognizedOriginalStiumulus { get; set; }

    [Display(Name = "Total score")]
    [Range(0, 98, ErrorMessage = "Value outside of required range")]
    [InvalidRange(nameof(MINTTotalScore), 33, 94, ErrorMessage = "Value outside of required range")]
    [Column("MINTTOTS")]
    [RequiredIf(nameof(FormStatus), FormStatus.Complete, ErrorMessage = "Value Required")]
    public int? MINTTotalScore { get; set; }

    [Display(Name = "Total correct without semantic cue")]
    [Range(0, 32, ErrorMessage = "Value outside of required range")]
    [Column("MINTTOTW")]
    [RequiredIfRange(nameof(MINTTotalScore), 0, 32, ErrorMessage = "Value Required")]
    public int? MINTTotalCorrectWithoutSemanticCue { get; set; }

    [Display(Name = "Semantic cues: Number given")]
    [Range(0, 32, ErrorMessage = "Value outside of required range")]
    [Column("MINTSCNG")]
    [RequiredIfRange(nameof(MINTTotalScore), 0, 32, ErrorMessage = "Value Required")]
    public int? MINTSemanticCuesNumberGiven { get; set; }

    [Display(Name = "Semantic cues: Number correct with cue")]
    [Range(0, 88, ErrorMessage = "Value outside of required range")]
    [InvalidRange(nameof(MINTSemanticCuesNumberCorrectWithoutCue), 33, 87, ErrorMessage = "Value outside of required range")]
    [Column("MINTSCNC")]
    [RequiredIfRange(nameof(MINTTotalScore), 0, 32, ErrorMessage = "Value Required")]
    public int? MINTSemanticCuesNumberCorrectWithoutCue { get; set; }

    [Display(Name = "Phonemic cues: Number given")]
    [Range(0, 32, ErrorMessage = "Value outside of required range")]
    [Column("MINTPCNG")]
    [RequiredIfRange(nameof(MINTTotalScore), 0, 32, ErrorMessage = "Value Required")]
    public int? MINTPhonemicCuesNumberGiven { get; set; }

    [Display(Name = "Phonemic cues: Number correct with cue")]
    [Range(0, 88, ErrorMessage = "Value outside of required range")]
    [InvalidRange(nameof(MINTPhonemicCuesNumberCorrectWithCue), 33, 87, ErrorMessage = "Value outside of required range")]
    [Column("MINTPCNC")]
    [RequiredIfRange(nameof(MINTTotalScore), 0, 32, ErrorMessage = "Value Required")]
    public int? MINTPhonemicCuesNumberCorrectWithCue { get; set; }

    [Display(Name = "Number of correct F-words generated in 1 minute")]
    [Range(0, 98, ErrorMessage = "Value outside of required range")]
    [InvalidRange(nameof(VerbalFluencyPhonemicTestGeneratedFWords), 41, 94, ErrorMessage = "Value outside of required range")]
    [Column("UDSVERFC")]
    [RequiredIf(nameof(FormStatus), FormStatus.Complete, ErrorMessage = "Value Required")]
    public int? VerbalFluencyPhonemicTestGeneratedFWords { get; set; }

    [Display(Name = "Number of correct F-words repeated in 1 minute")]
    [Range(0, 15, ErrorMessage = "Value outside of required range")]
    [Column("UDSVERFN")]
    [RequiredIfRange(nameof(VerbalFluencyPhonemicTestGeneratedFWords), 0, 40, ErrorMessage = "Value Required")]
    public int? VerbalFluencyPhonemicTestRepeatedFWords { get; set; }

    [Display(Name = "Number of non-Fwords and rule violation errors in 1 minute")]
    [Range(0, 15, ErrorMessage = "Value outside of required range")]
    [Column("UDSVERNF")]
    [RequiredIfRange(nameof(VerbalFluencyPhonemicTestGeneratedFWords), 0, 40, ErrorMessage = "Value Required")]
    public int? VerbalFluencyPhonemicTestFWordErrors { get; set; }

    [Display(Name = "Number of correct L-words generated in 1 minute")]
    [Range(0, 98, ErrorMessage = "Value outside of required range")]
    [InvalidRange(nameof(VerbalFluencyPhonemicTestGeneratedLWords), 41, 94, ErrorMessage = "Value outside of required range")]
    [Column("UDSVERLC")]
    [RequiredIfRange(nameof(VerbalFluencyPhonemicTestGeneratedFWords), 0, 40, ErrorMessage = "Value Required")]
    public int? VerbalFluencyPhonemicTestGeneratedLWords { get; set; }

    [Display(Name = "Number of correct L-words repeated in 1 minute")]
    [Range(0, 15, ErrorMessage = "Value outside of required range")]
    [Column("UDSVERLR")]
    [RequiredIfRange(nameof(VerbalFluencyPhonemicTestGeneratedLWords), 0, 40, ErrorMessage = "Value Required")]
    public int? VerbalFluencyPhonemicTestRepeatedLWords { get; set; }

    [Display(Name = "Number of non-L-words and rule violation errors in 1 minute")]
    [Range(0, 15, ErrorMessage = "Value outside of required range")]
    [Column("UDSVERLN")]
    [RequiredIfRange(nameof(VerbalFluencyPhonemicTestGeneratedLWords), 0, 40, ErrorMessage = "Value Required")]
    public int? VerbalFluencyPhonemicTestLWordErrors { get; set; }

    [Display(Name = "Total number of correct F-words and L-words")]
    [Range(0, 80, ErrorMessage = "Value outside of required range")]
    [Column("UDSVERTN")]
    [RequiredIfRange(nameof(VerbalFluencyPhonemicTestGeneratedLWords), 0, 40, ErrorMessage = "Value Required")]
    public int? VerbalFluencyPhonemicTestTotalCorrectFAndLWords { get; set; }

    [Display(Name = " Total number of F-word and L-words repetition errors")]
    [Range(0, 30, ErrorMessage = "Value outside of required range")]
    [Column("UDSVERTE")]
    [RequiredIfRange(nameof(VerbalFluencyPhonemicTestGeneratedLWords), 0, 40, ErrorMessage = "Value Required")]
    public int? VerbalFluencyPhonemicTestTotalFAndLRepetitionErrors { get; set; }

    [Display(Name = "Number of non-F/L-words and rule violation errors")]
    [Range(0, 30, ErrorMessage = "Value outside of required range")]
    [Column("UDSVERTI")]
    [RequiredIfRange(nameof(VerbalFluencyPhonemicTestGeneratedLWords), 0, 40, ErrorMessage = "Value Required")]
    public int? VerbalFluencyPhonemicTestTotalFAndLViolationErrors { get; set; }

    [Display(Name = "Per the clinician (e.g., neuropsychologist, behavioral neurologist, or other suitably qualified clinician), based on the UDS neuropsychological examination, the subjects cognitive status is deemed")]
    [Range(0, 4, ErrorMessage = "Value outside of required range")]
    [Column("COGSTAT")]
    [RequiredIf(nameof(FormStatus), FormStatus.Complete, ErrorMessage = "Value Required")]
    public int? OverallAppraisal { get; set; }

  }

}
