using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace UDS.Net.Data.Entities
{
    [Table("tbl_A4D")]
    public class MedicationCurrent
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [MaxLength(6)]
        [Column("DRUGID")]
        public string DrugId { get; set; }

        [MaxLength(255)]
        [Column("DRUGNOTES")]
        public string Notes { get; set; }

        [ForeignKey("MedicationsReviewId")]
        public MedicationsReview MedicationsReview { get; set; }

        [Column("PacketId")]
        public int MedicationsReviewId { get; set; }
        /// <summary>
        /// User who modified Medication
        /// </summary>
        public string ModifiedBy {get;set;}
    }
}
