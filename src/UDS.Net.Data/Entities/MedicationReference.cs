using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using UDS.Net.Data.DataAnnotations;

namespace UDS.Net.Data.Entities
{
    /// <summary>
    /// This table stores all default medications from NACC.
    /// Seed data is in App_Data:
    /// MedicationsReference.json
    /// MedicationsOTCReference.json
    /// </summary>
    [Table("tbl_MedicationReference")]
    public class MedicationReference
    {
        [Key]
        [Display(Name = "NACC drugID")]
        [Required(ErrorMessage ="Please use the Lookup Tool on the NACC website to find the drugID.")]
        [MaxLength(6)]
        public string DrugId { get; set; }

        [Display(Name = "Generic and brand name(s)")]
        [RequiredIf(nameof(FromNACCDefaultReference), false, ErrorMessage = "Provide the generic name and brand names for the medication.")]
        [MaxLength(100)]
        public string DisplayName { get; set; }

        public bool FromNACCDefaultReference { get; set; }

        [Display(Name = "Is this medication available over the counter?")]
        public bool IsOverTheCounter { get; set; }
    }
}
