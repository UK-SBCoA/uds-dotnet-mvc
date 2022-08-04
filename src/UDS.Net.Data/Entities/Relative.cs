using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using UDS.Net.Data.DataAnnotations;
using UDS.Net.Data.Enums;

namespace UDS.Net.Data.Entities
{
    [Table("tbl_Relative")]
    public class Relative
    {
        [Key]
        public int Id { get; set; }
        [Column("PacketId")]
        public int SubjectFamilyHistoryId { get; set; }
        public int RelationshipNumber { get; set; }
        public FamilyRelationship Relation { get; set; }
        [Range(1, 99, ErrorMessage = "Value must be within the valid range of 1 - 12 or 99")]
        [InvalidRange(nameof(BirthMonth), 13, 98, ErrorMessage = "Value must be within the valid range of 1 - 12 or 99")]
        public int? BirthMonth { get; set; }        [CurrentYearRange(nameof(BirthYear), 1875, 9999, ErrorMessage = "Value must be within the valid range of 1875 - current year or 9999")]
        public int? BirthYear { get; set; }
        [Range(0, 999, ErrorMessage = "Value must be within the valid range of 0 - 110, 888, or 999")]
        [InvalidRange(nameof(AgeAtDeath), 111, 887, ErrorMessage = "Value must be within the valid range of 0 - 110, 888, or 999")]
        [InvalidRange(nameof(AgeAtDeath), 889, 998, ErrorMessage = "Value must be within the valid range of 0 - 110, 888, or 999")]
        public int? AgeAtDeath { get; set; }
        public int? PrimaryNeurologicalProblemPsychiatricCondition { get; set; }
        public int? PrimaryDx { get; set; }
        public int? MethodOfEvaluation { get; set; }
        [InvalidRange(nameof(AgeOfOnSet), 111, 998, ErrorMessage = "Value must be within the valid range of 0 - 110 or 999")]
        public int? AgeOfOnSet { get; set; }
        public SubjectFamilyHistory SubjectFamilyHistory { get; set; }
    }
}
