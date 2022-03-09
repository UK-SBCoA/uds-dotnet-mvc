using System;
using System.Collections.Generic;
using UDS.Net.Data.Entities;
using UDS.Net.Data.Enums;

namespace UDS.Net.Web.ViewModels
{
    public class VisitOverviewViewModel
    {
        public int Id { get; set; }

        public Participation Participant { get; set; } // needs Id and Profile

        public int FriendlyId { get; set; }

        public DateTime VisitDate { get; set; }

        public int VisitNumber { get; set; }

        public VisitType VisitType { get; set; }

        public VisitStatus Status { get; set; }

        public string CoordinatorInitials { get; set; }

        public string ClinicianInitials { get; set; }

        public string SocialWorkerInitials { get; set; }

        public bool IsSubmittedtoNACC { get; set; }

        // A1
        public FormBase ParticipantDemographics { get; set; }

        // A5
        public FormBase SubjectHealthHistory { get; set; }

        // D2
        public FormBase MedicalConditions { get; set; }

        // A4G
        public FormBase MedicationsReview { get; set; }

        // B2
        public FormBase Hachinski { get; set; }

        // B3
        public FormBase UnifiedParkinsonsDiseaseRatingScale { get; set; }

        // B8
        public FormBase NeurologicalExaminationFindings { get; set; }

        // C2
        public FormBase NeuropsychologicalBatteryScores { get; set; }

        // A2
        public FormBase CoParticipantDemographics { get; set; }

        // B5
        public FormBase NPIQ { get; set; }

        // B7
        public FormBase FunctionalActivitiesQuestionnaire { get; set; }

        // D1
        public FormBase ClinicianDiagnosis { get; set; }

        // B1
        public FormBase PhysicalEvaluation { get; set; }

        // B4
        public FormBase CDRPlusNACCFTLD { get; set; }

        // B6
        public FormBase GeriatricDepressionScale { get; set; }

        // A3
        public FormBase SubjectFamilyHistory { get; set; }

        // Z1
        public Checklist Checklist { get; set; }

        // B9
        public FormBase Symptoms { get; set; }

        // T1
        public FormBase Inclusion { get; set; }

    }
}
