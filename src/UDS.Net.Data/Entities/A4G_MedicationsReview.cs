using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using COA.Components.Web.DataAnnotations;
using UDS.Net.Data.Enums;

namespace UDS.Net.Data.Entities
{
    [Table("tbl_A4G")]
    public class MedicationsReview : FormBase
    {
        [Display(Name = "Is the subject currently taking any medications?")]
        [RequiredIf(nameof(FormStatus), FormStatus.Complete, ErrorMessage = "Please indicate if the subject has taken any medications.")]
        [Column("ANYMEDS")]
        public bool? CurrentlyTakingMedications { get; set; }

        [Display(Name = "Notes")]
        [MaxLength(500)]
        [Column("UKNotes")]
        public string Comments { get; set; }

        public IList<MedicationCurrent> CurrentMedications { get; set; }

    }
}
