using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using UDS.Net.Data.Enums;

namespace UDS.Net.Data.Entities
{
    [Table("tbl_Relative")]
    public class Relative
    {
        [Key]
        public int Id { get; set; }
        public int SubjectFamilyHistoryId { get; set; }
        public int RelationshipNumber { get; set; }
        public FamilyRelationship Relation { get; set; }
        public int? BirthMonth { get; set; }
        public int? BirthYear { get; set; }
        public int? AgeAtDeath { get; set; }
        public int? PrimaryNeurologicalProblemPsychiatricCondition { get; set; }
        public int? PrimaryDx { get; set; }
        public int? MethodOfEvaluation { get; set; }
        public int? AgeOfOnSet { get; set; }
        public SubjectFamilyHistory SubjectFamilyHistory { get; set; }
    }
}
