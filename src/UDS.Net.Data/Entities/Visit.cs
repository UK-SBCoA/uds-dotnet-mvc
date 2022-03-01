using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using UDS.Net.Data.Enums;

namespace UDS.Net.Data.Entities
{
    [Table("tbl_Packet")]
    public class Visit
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("PacketId")]
        public int Id { get; set; }

        [Display(Name = "Date of visit")]
        [DataType(DataType.Date)]
        [Column("VISITDATE")]
        public DateTime VisitDate { get; set; }

        /// <summary>
        /// Jacob will calculate these variables:
        /// VISITMO
        /// VISITDAY
        /// VISITYR
        /// PACKET
        /// UDSVER
        /// ADCID = 19
        /// INNACC
        /// </summary>
        [Display(Name = "PTID")]
        [Column("PTID")]
        public int FriendlyId { get; set; }

        [ForeignKey("FriendlyId")]
        public Participation Participant { get; set; }

        [Display(Name = "Visit number")]
        [Column("VISITNUM")]
        public int VisitNumber { get; set; }

        [Display(Name = "Which type of visit?")]
        public VisitType VisitType { get; set; }


        [NotMapped]
        public int Urgency
        {
            get
            {
                int dayDifference = (int)(DateTime.Now - VisitDate).TotalDays;
                if (dayDifference <= 28)
                {
                    return 28;
                } else if (dayDifference <= 40)
                {
                    return 40;
                }
                return 41;
            }
        }

        /// <summary>
        /// This indicates the packet's status: InProgress, Prioritized, Tabled, Completed
        /// </summary>
        public VisitStatus Status { get; set; }

        /// <summary>
        /// Sort order for priority
        /// </summary>
        public int PriorityWeight { get; set; }

        /// <summary>
        /// This indicator will be used by Josh to flag which packets have been sent
        /// to NACC for submission. Sometimes packets are still sent back due to validation errors.
        /// 
        /// Should not be set unless Status = Completed
        /// </summary>
        [Display(Name = "Packet has been submitted to NACC")]
        public bool IsSubmittedToNACC { get; set; }

        [Display(Name = "Coordinator initials")]
        public string CoordinatorInitials { get; set; }

        [ForeignKey("CoordinatorInitials")]
        public Examiner Coordinator { get; set; }

        [Display(Name = "Clinician initials")]
        public string ClinicianInitials { get; set; }

        [ForeignKey("ClinicianInitials")]
        public Examiner Clinician { get; set; }

        [Display(Name = "Social worker initials")]
        public string SocialWorkerInitials { get; set; }

        [ForeignKey("SocialWorkerInitials")]
        [Display(Name = "Social Worker")]
        public Examiner SocialWorker { get; set; }

        /// <summary>
        /// A1
        /// <remarks>
        /// Participant demographics
        /// </remarks>
        /// </summary>
        [Display(Name = "Participant Demographics")]
        public virtual ParticipantDemographics ParticipantDemographics { get; set; }

        /// <summary>
        /// Z1
        /// <remarks>
        /// Checklist
        /// </remarks>
        /// </summary>
        [Display(Name = "Checklist")]
        public virtual Checklist Checklist { get; set; }

        /// <summary>
        /// <c>A2</c> 
        /// </summary>
        /// <remarks>
        /// Co-participant demographics
        /// </remarks>
        [Display(Name = "Co-participant Demographics")]
        public virtual CoParticipantDemographics CoParticipantDemographics { get; set; }

        /// <summary>
        /// <c>B4</c>
        /// </summary>
        /// <remarks>
        /// Forms: All
        /// </remarks>
        [Display(Name = "CDR® Plus NACC FTLD")]
        public virtual CDRPlusNACCFTLD CDRPlusNACCFTLD {get;set;}

        /// <summary>
        /// <c>D1</c>
        /// </summary>
        [Display(Name = "Clinician Diagnosis")]
        public virtual ClinicianDiagnosis ClinicianDiagnosis { get; set; }

        /// <summary>
        /// <c>B7</c>
        /// </summary>
        [Display(Name = "Functional Activities Questionnaire")]
        public virtual FunctionalActivitiesQuestionnaire FunctionalActivitiesQuestionnaire { get; set; }

        /// <summary>
        /// <c>B6</c> Geriatric Depresion Scale
        /// </summary>
        [Display(Name = "Geriatric Depression Scale")]
        public virtual GeriatricDepressionScale GeriatricDepressionScale {get;set;}

        /// <summary>
        /// <c>B2</c>
        /// <remarks>Hachinski</remarks>
        /// </summary>
        [Display(Name = "Hachinski")]
        public virtual Hachinski Hachinski { get; set; }

        /// <summary>
        /// <c>T1</c>
        /// <remarks>Telephone Inclusion</remarks>
        /// </summary>
        [Display(Name = "Inclusion")]
        public virtual Inclusion Inclusion { get; set; }

        /// <summary>
        /// <c>A4G and A4D</c>
        /// </summary>
        [Display(Name = "Current Medications")]
        public virtual MedicationsReview MedicationsReview { get; set; }

        [Display(Name = "Medical Conditions")]
        public virtual MedicalConditions MedicalConditions { get; set; }

        /// <summary>
        /// <c>B8</c>
        /// </summary>
        public virtual NeurologicalExaminationFindings NeurologicalExaminationFindings { get; set; }

        /// <summary>
        /// <c>B5</c>
        /// </summary>
        [Display(Name = "NPI-Q")]
        public virtual NPIQ NPIQ { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [Display(Name = "Subject Health History")]
        public virtual SubjectHealthHistory SubjectHealthHistory {get;set;}

        /// <summary>
        /// Unified Parkinsons Disease Rating Scale
        /// </summary>
        [Display(Name = "Unified Parkinsons Disease Rating Scale")]
        public virtual UnifiedParkinsonsDiseaseRatingScale UnifiedParkinsonsDiseaseRatingScale { get; set; }



        /// <summary>
        /// Physical Evaluation
        /// </summary>
        /// <value></value>
        public virtual PhysicalEvaluation PhysicalEvaluation {get;set;}

        public virtual NeuropsychologicalBatteryScores NeuropsychologicalBatteryScores { get; set; }
        /// <summary>
        /// Subject Family History
        /// </summary>
        public virtual SubjectFamilyHistory SubjectFamilyHistory { get; set; }
        /// User who modified visit
        /// </summary>
        /// <value></value>
        public string ModifiedBy {get;set;}

        /// <summary>
        /// B9
        /// </summary>
        [Display(Name = "Symptoms")]
        public virtual Symptoms Symptoms { get; set; }
    }
}
