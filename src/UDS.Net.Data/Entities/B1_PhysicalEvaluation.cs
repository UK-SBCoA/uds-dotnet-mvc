using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using COA.Components.Web.DataAnnotations;
using UDS.Net.Data.Enums;

namespace UDS.Net.Data.Entities
{
  [Table("tbl_B1")]
  public class PhysicalEvaluation : FormBase
  {
    [Column("HEIGHT")]
    [Display(Name = "Subject height (inches)")]
    [RequiredIf(nameof(FormStatus), FormStatus.Complete, ErrorMessage = "Please indicate subject height (inches)")]
    [Range(24, 108, ErrorMessage = "Subject height must be between 24 and 108")]
    public int? HeightInches { get; set; }
    [Column("HEIGDEC")]
    [Range(0, 9, ErrorMessage = "Subject height decimal must be between 0 and 9")]
    [RequiredIf(nameof(FormStatus), FormStatus.Complete, ErrorMessage = "Please indicate subject decimal height")]
    public int? HeightInchesDecimal { get; set; }
    [Column("WEIGHT")]
    [Display(Name = "Subject weight (lbs.)")]
    [RequiredIf(nameof(FormStatus), FormStatus.Complete, ErrorMessage= "Please indicate subject weight (lbs.)")]
    [Range(50, 888, ErrorMessage = "Please enter a weight in the range: 50 to 400 or equal to 888")]
    [InvalidRange(nameof(Weight), 401, 887, ErrorMessage = "Please enter a weight in the range: 50 to 400 or equal to 888")]
    public int? Weight { get; set; }
    [Column("BPSYS")]
    [Display(Name = "Subject blood pressure at initial reading (sitting)")]
    [RequiredIf(nameof(FormStatus), FormStatus.Complete, ErrorMessage= "Please indicate subject systolic blood pressure")]
    [Range(70, 888, ErrorMessage = "Please enter a systolic pressure in the range: 70 to 230 or equal to 888")]
    [InvalidRange(nameof(BloodPressureSystolic), 231, 887, ErrorMessage = "Please enter a systolic pressure in the range: 70 to 230 or equal to 888")]
    public int? BloodPressureSystolic { get; set; }
    [Column("BPDIAS")]
    [RequiredIf(nameof(FormStatus), FormStatus.Complete, ErrorMessage= "Please indicate subject diastolic blood pressure")]
    [Range(30, 888, ErrorMessage = "Please enter a diastolic pressure in the range: 30 to 140 or equal to 888")]
    [InvalidRange(nameof(BloodPressureDiastolic), 141, 887, ErrorMessage = "Please enter a diastolic pressure in the range: 30 to 140 or equal to 888")]
    public int? BloodPressureDiastolic { get; set; }
    [Column("HRATE")]
    [Display(Name = "Subject resting heart rate (pulse)")]
    [RequiredIf(nameof(FormStatus), FormStatus.Complete, ErrorMessage= "Please indicate resting heart rate")]
    [Range(33, 888, ErrorMessage = "Please enter a resting heart rate in the range: 33 to 160 or equal to 888")]
    [InvalidRange(nameof(RestingHeartRate), 161, 887, ErrorMessage = "Please enter a resting heart rate in the range: 33 to 160 or equal to 888")]
    public int? RestingHeartRate { get; set; }
    [Column("VISION")]
    [Display(Name = "Without corrective lenses, is the subject's vision functionally normal?")]
    [RequiredIf(nameof(FormStatus), FormStatus.Complete, ErrorMessage= "Please indicate the subject's vision functionality")]
    public int? SubjectsVision { get; set; }
    [Column("VISCORR")]
    [Display(Name = "Does the subject usually wear corrective lenses?")]
    [RequiredIf(nameof(FormStatus), FormStatus.Complete, ErrorMessage= "Please indicate if corrective lenses are usually worn")]
    public int? CorrectiveLenses { get; set; }
    [Column("VISWCORR")]
    [RequiredIf(nameof(CorrectiveLenses), 1, ErrorMessage= "Please indicate the subject's vision functionality with corrective lenses")]
    public int? CorrectiveLensesNormal { get; set; }
    [Column("HEARING")]
    [Display(Name = "Without a hearing aid(s), is the subject's hearing functionally normal?")]
    [RequiredIf(nameof(FormStatus), FormStatus.Complete, ErrorMessage= "Please indicate subject's hearing functionality without hearing aid(s)")]
    public int? SubjectsHearing { get; set; }
    [Column("HEARAID")]
    [Display(Name = "Does the subject usually wear a hearing aid(s)?")]
    [RequiredIf(nameof(FormStatus), FormStatus.Complete, ErrorMessage= "Please indicate if the subject usually wears hearing aid(s)")]
    public int? HearingAids { get; set; }
    [Column("HEARWAID")]
    [RequiredIf(nameof(HearingAids), 1, ErrorMessage= "Please indicate if the subject's hearing is normal with hearing aid(s)")]
    public int? HearingAidsNormal { get; set; }
  }
}