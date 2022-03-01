using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using COA.Components.Web.DataAnnotations;
using UDS.Net.Data.Enums;

namespace UDS.Net.Data.Entities
{
    /// <summary>
    /// A5 for IVP not for FVP
    /// </summary>
    [Display(Name = "Checklist")]
    [Table("tbl_Z1")]
    public class Checklist : FormBase
    {
        /// TODO NACCReportable ??

        #region A2
        /// <summary>
        /// A2 is required for TFP
        /// The optional inclusion should be displayed for IVP, FVP
        /// </summary>
        [Display(Name = "A2 Participant Demographics")]
        [Column("A2SUB")]
        public bool? A2_IsIncluded { get; set; }

        [Display(Name = "If not submitted, specify reason (see KEY)")]
        [Column("A2NOT")]
        [RequiredIf(nameof(A2_IsIncluded), false, ErrorMessage = "Indicate why the form not be submitted")]
        [Range(95, 98, ErrorMessage = "Choose a reason from the available codes")]
        public int? A2_ReasonNotIncluded { get; set; }

        [Display(Name = "Comments (provide if form not included)")]
        [Column("A2COMM")]
        [RequiredIf(nameof(A2_IsIncluded), false, ErrorMessage = "Include a comment")]
        public string A2_Comments { get; set; }
        #endregion

        #region A3
        [Display(Name = "A3 Family History")]
        [Column("A3SUB")]
        [RequiredIf(nameof(FormStatus), FormStatus.Complete, ErrorMessage = "Please provide whether the A3 form should be included in submission")]
        public bool? A3_IsIncluded { get; set; }

        [Display(Name = "If not submitted, specify reason (see KEY)")]
        [Column("A3NOT")]
        [RequiredIf(nameof(A3_IsIncluded), false, ErrorMessage = "Indicate why the form not be submitted")]
        [Range(95, 98, ErrorMessage = "Choose a reason from the available codes")]
        public int? A3_ReasonNotIncluded { get; set; }

        [Display(Name = "Comments (provide if form not included)")]
        [Column("A3COMM")]
        [RequiredIf(nameof(A3_IsIncluded), false, ErrorMessage = "Include a comment")]
        public string A3_Comments { get; set; }
        #endregion

        #region A4
        [Display(Name = "A4 Medications")]
        [Column("A4SUB")]
        [RequiredIf(nameof(FormStatus), FormStatus.Complete, ErrorMessage = "Please provide whether the A4 form should be included in submission")]
        public bool? A4_IsIncluded { get; set; }

        [Display(Name = "If not submitted, specify reason (see KEY)")]
        [Column("A4NOT")]
        [RequiredIf(nameof(A4_IsIncluded), false, ErrorMessage = "Indicate why the form not be submitted")]
        [Range(95, 98, ErrorMessage = "Choose a reason from the available codes")]
        public int? A4_ReasonNotIncluded { get; set; }

        [Display(Name = "Comments (provide if form not included)")]
        [Column("A4COMM")]
        [RequiredIf(nameof(A4_IsIncluded), false, ErrorMessage = "Include a comment")]
        public string A4_Comments { get; set; }
        #endregion

        #region B1
        [Display(Name = "B1 Physical Evaluation")]
        [Column("B1SUB")]
        public bool? B1_IsIncluded { get; set; }

        [Display(Name = "If not submitted, specify reason (see KEY)")]
        [Column("B1NOT")]
        [RequiredIf(nameof(B1_IsIncluded), false, ErrorMessage = "Indicate why the form not be submitted")]
        [Range(95, 98, ErrorMessage = "Choose a reason from the available codes")]
        public int? B1_ReasonNotIncluded { get; set; }

        [Display(Name = "Comments (provide if form not included)")]
        [Column("B1COMM")]
        [RequiredIf(nameof(B1_IsIncluded), false, ErrorMessage = "Include a comment")]
        public string B1_Comments { get; set; }
        #endregion

        #region B5
        [Display(Name = "B5 NPIQ")]
        [Column("B5SUB")]
        [RequiredIf(nameof(FormStatus), FormStatus.Complete, ErrorMessage = "Please provide whether the B5 form should be included in submission")]
        public bool? B5_IsIncluded { get; set; }

        [Display(Name = "If not submitted, specify reason (see KEY)")]
        [Column("B5NOT")]
        [RequiredIf(nameof(B5_IsIncluded), false, ErrorMessage = "Indicate why the form not be submitted")]
        [Range(95, 98, ErrorMessage = "Choose a reason from the available codes")]
        public int? B5_ReasonNotIncluded { get; set; }

        [Display(Name = "Comments (provide if form not included)")]
        [Column("B5COMM")]
        [RequiredIf(nameof(B5_IsIncluded), false, ErrorMessage = "Include a comment")]
        public string B5_Comments { get; set; }
        #endregion

        #region B6
        [Display(Name = "B6 Geriatric Depression Scale (GDS)")]
        [Column("B6SUB")]
        public bool? B6_IsIncluded { get; set; }

        [Display(Name = "If not submitted, specify reason (see KEY)")]
        [Column("B6NOT")]
        [RequiredIf(nameof(B6_IsIncluded), false, ErrorMessage = "Indicate why the form not be submitted")]
        [Range(95, 98, ErrorMessage = "Choose a reason from the available codes")]
        public int? B6_ReasonNotIncluded { get; set; }

        [Display(Name = "Comments (provide if form not included)")]
        [Column("B6COMM")]
        [RequiredIf(nameof(B6_IsIncluded), false, ErrorMessage = "Include a comment")]
        public string B6_Comments { get; set; }
        #endregion

        #region B7
        [Display(Name = "B7 Functional Activities Questionnaire (FAQ)")]
        [Column("B7SUB")]
        [RequiredIf(nameof(FormStatus), FormStatus.Complete, ErrorMessage = "Please provide whether the B7 form should be included in submission")]
        public bool? B7_IsIncluded { get; set; }

        [Display(Name = "If not submitted, specify reason (see KEY)")]
        [Column("B7NOT")]
        [RequiredIf(nameof(B7_IsIncluded), false, ErrorMessage = "Indicate why the form not be submitted")]
        [Range(95, 98, ErrorMessage = "Choose a reason from the available codes")]
        public int? B7_ReasonNotIncluded { get; set; }

        [Display(Name = "Comments (provide if form not included)")]
        [Column("B7COMM")]
        [RequiredIf(nameof(B7_IsIncluded), false, ErrorMessage = "Include a comment")]
        public string B7_Comments { get; set; }
        #endregion

        /// TODO these are not in Sharepoint or UDS 3 DED
        /// D1SUB (D1 is required so it isn't an optional submission and this variable doesn't make sense)
        /// RSUB
        /// COGONLY
        /// PTIDDate
        /// ADCID
        /// Time_Stamp
    }
}
