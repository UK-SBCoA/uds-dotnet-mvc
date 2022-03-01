using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using UDS.Net.Data.Dtos;
using UDS.Net.Data.Enums;

namespace UDS.Net.Data.Entities
{
    [Table("tbl_Participation")]
    public class Participation
    {
        /// <summary>
        /// This ID is assigned whereever participants are registered.
        /// This is not a SQL identity (self-incrementing) column. Unique values must already be assigned.
        /// Also know as Friendly Id / ADC ID / PT ID
        /// </summary>
        [Display(Name = "PTID")]
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        [Column("PTID")]
        public int Id { get; set; }

        public ParticipationStatus Status { get; set; }

        public virtual IEnumerable<Visit> Visits { get; set; }

        public virtual IEnumerable<Milestone> Milestones { get; set; }

        [NotMapped]
        public virtual ParticipantDto Profile { get; set; }
        public string ModifiedBy {get;set;}

    }
}