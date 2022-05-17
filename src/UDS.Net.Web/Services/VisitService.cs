using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using UDS.Net.Data;
using UDS.Net.Data.Entities;
using UDS.Net.Data.Enums;
using UDS.Net.Web.ViewModels;

namespace UDS.Net.Web.Services
{
    public class VisitService : IVisitService
    {
        private readonly UdsContext _context;

        public async Task<string> GetVisitFormsInStatusAsync(int id, VisitType visitType, FormStatus formStatus)
        {
            var visit = await GetVisitWithParticipantAndFormBases(id);

            var counter = GetTotalNACCFormCount(visit, formStatus);

            return counter.ToString();
        }

        private int GetTotalNACCFormCount(VisitOverviewViewModel visit, FormStatus formStatus)
        {
            int counter = 0;

            counter = GetRequiredNACCFormCount(visit, formStatus);

            // A2 (optional)
            if (visit.CoParticipantDemographics != null && visit.CoParticipantDemographics.FormStatus == formStatus)
            {
                counter++;
            }

            // A3 (optional)
            if (visit.SubjectFamilyHistory != null && visit.SubjectFamilyHistory.FormStatus == formStatus)
            {
                counter++;
            }

            // A4 (optional)
            if (visit.MedicationsReview != null && visit.MedicationsReview.FormStatus == formStatus)
            {
                counter++;
            }

            if (visit.VisitType != VisitType.IVP) // checking so it doens't get counted twice, in required count method for IVP
            {
                // A5 (only optional for FVP, TFP)
                if (visit.SubjectHealthHistory != null && visit.SubjectHealthHistory.FormStatus == formStatus)
                {
                    counter++;
                }
            }

            // B1
            if (visit.PhysicalEvaluation != null && visit.PhysicalEvaluation.FormStatus == formStatus)
            {
                counter++;
            }

            // B2
            if (visit.Hachinski != null && visit.Hachinski.FormStatus == formStatus)
            {
                counter++;
            }

            // B3
            if (visit.UnifiedParkinsonsDiseaseRatingScale != null && visit.UnifiedParkinsonsDiseaseRatingScale.FormStatus == formStatus)
            {
                counter++;
            }

            // B5
            if (visit.NPIQ != null && visit.NPIQ.FormStatus == formStatus)
            {
                counter++;
            }

            // B6
            if (visit.GeriatricDepressionScale != null && visit.GeriatricDepressionScale.FormStatus == formStatus)
            {
                counter++;
            }

            // B7
            if (visit.FunctionalActivitiesQuestionnaire != null && visit.FunctionalActivitiesQuestionnaire.FormStatus == formStatus)
            {
                counter++;
            }

            if (visit.VisitType == VisitType.TFP)
            {
                // B8
                if (visit.NeurologicalExaminationFindings != null && visit.NeurologicalExaminationFindings.FormStatus == formStatus)
                {
                    counter++;
                }
            }

            if (visit.VisitType == VisitType.TFP)
            {
                // C2
                if (visit.NeuropsychologicalBatteryScores != null && visit.NeuropsychologicalBatteryScores.FormStatus == formStatus)
                {
                    counter++;
                }
            }

            return counter;
        }


        /// <summary>
        /// IVP visit required form count = 9 (A1, A5, B4, B8, B9, C2, D1, D2, Z1)
        /// FVP visit required form count = 8 (A1, B4, B8, B9, C2, D1, D2, Z1)
        /// TFP visit required form count = 8 (A1, A2, B4, B9, D1, D2, T1, Z1)
        /// TODO this should use the checklist service
        /// </summary>
        /// <param name="visit"></param>
        /// <param name="formStatus"></param>
        /// <returns>int</returns>
        private int GetRequiredNACCFormCount(VisitOverviewViewModel visit, FormStatus formStatus)
        {
            int counter = 0;

            // A1
            if (visit.ParticipantDemographics != null && visit.ParticipantDemographics.FormStatus == formStatus)
            {
                counter++;
            }

            if (visit.VisitType == VisitType.IVP)
            {
                // A5 (only required for IVP)
                if (visit.SubjectHealthHistory != null && visit.SubjectHealthHistory.FormStatus == formStatus)
                {
                    counter++;
                }
            }

            // B4
            if (visit.CDRPlusNACCFTLD != null && visit.CDRPlusNACCFTLD.FormStatus == formStatus)
            {
                counter++;
            }

            if (visit.VisitType != VisitType.TFP)
            {
                // B8
                if (visit.NeurologicalExaminationFindings != null && visit.NeurologicalExaminationFindings.FormStatus == formStatus)
                {
                    counter++;
                }
            }

            // B9
            if (visit.Symptoms != null && visit.Symptoms.FormStatus == formStatus)
            {
                counter++;
            }

            if (visit.VisitType != VisitType.TFP)
            {
                // C2
                if (visit.NeuropsychologicalBatteryScores != null && visit.NeuropsychologicalBatteryScores.FormStatus == formStatus)
                {
                    counter++;
                }
            }

            // D1
            if (visit.ClinicianDiagnosis != null && visit.ClinicianDiagnosis.FormStatus == formStatus)
            {
                counter++;
            }

            // D2
            if (visit.MedicalConditions != null && visit.MedicalConditions.FormStatus == formStatus)
            {
                counter++;
            }

            // Z1
            if (visit.Checklist != null && visit.Checklist.FormStatus == formStatus)
            {
                counter++;
            }

            /// T1
            if (visit.Inclusion != null && visit.Inclusion.FormStatus == formStatus)
            {
                counter++;
            }

            return counter;
        }

        /// <summary>
        /// This is a heavy query, only use if you must have all the data.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<Visit> GetVisitWithForms(int id)
        {
            var visit = await _context.Visits
                .Include("CoParticipantDemographics") // A2
                .Include("CDRPlusNACCFTLD") // B4
                .Include("Checklist") // Z1
                .Include("ClinicianDiagnosis") // D1
                .Include("FunctionalActivitiesQuestionnaire") // B7
                .Include("GeriatricDepressionScale") // B6
                .Include("Hachinski") // B2
                .Include("MedicationsReview") // A4G
                .Include("MedicalConditions") // D2
                .Include("NeurologicalExaminationFindings") // B8
                .Include("NPIQ") // B5
                .Include("ParticipantDemographics") // A1
                .Include("SubjectHealthHistory") // A5
                .Include("UnifiedParkinsonsDiseaseRatingScale") // B3
                .Include("PhysicalEvaluation") // B1
                .Include("NeuropsychologicalBatteryScores") // C2
                .Include("SubjectFamilyHistory") // A3
                .Include("Symptoms") // B9
                .Include("Inclusions") // T1
                .AsNoTracking()
                .Where(v => v.Id == id)
                .FirstOrDefaultAsync();

            return visit;
        }

        /// <summary>
        /// This is a heavy query, only use if you must have all the data.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<Visit> GetVisitWithParticipantAndForms(int id)
        {
            var visit = await _context.Visits
                  .Include("Participant")
                  .Include("CoParticipantDemographics") // A2
                  .Include("CDRPlusNACCFTLD") // B4
                  .Include("Checklist") // Z1
                  .Include("ClinicianDiagnosis") // D1
                  .Include("FunctionalActivitiesQuestionnaire") // B7
                  .Include("GeriatricDepressionScale") // B6
                  .Include("Hachinski") // B2
                  .Include("MedicationsReview") // A4G
                  .Include("MedicalConditions") // D2
                  .Include("NeurologicalExaminationFindings") // B8
                  .Include("NPIQ") // B5
                  .Include("ParticipantDemographics") // A1
                  .Include("SubjectHealthHistory") // A5
                  .Include("UnifiedParkinsonsDiseaseRatingScale") // B3
                  .Include("PhysicalEvaluation") // B1
                  .Include("NeuropsychologicalBatteryScores") // C2
                  .Include("SubjectFamilyHistory") // A3
                  .Include("Symptoms") // B9
                  .Include("Inclusions") // T1
                  .AsNoTracking()
                  .FirstOrDefaultAsync(m => m.Id == id);

            return visit;
        }

        /// <summary>
        /// For pages with overviews where you only need the form's status and other meta data and not the actual data entry data.
        /// <code>
        /// It doesn't seem possible to do this in EF Core 5, so it might be a blocker to upgrade.
        /// </code>
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<VisitOverviewViewModel> GetVisitWithParticipantAndFormBases(int id)
        {
            if (_context != null)
            {
                var visitOverview = await _context.Visits
                    .Select(visit => new VisitOverviewViewModel
                    {
                        Id = visit.Id,
                        Participant = visit.Participant,
                        FriendlyId = visit.FriendlyId,
                        VisitNumber = visit.VisitNumber,
                        VisitType = visit.VisitType,
                        VisitDate = visit.VisitDate,
                        Status = visit.Status,
                        CoordinatorInitials = visit.CoordinatorInitials,
                        ClinicianInitials = visit.ClinicianInitials,
                        SocialWorkerInitials = visit.SocialWorkerInitials,
                        IsSubmittedtoNACC = visit.IsSubmittedToNACC,
                        ParticipantDemographics = new FormBase // A1
                        {
                            Id = visit.ParticipantDemographics.Id,
                            ExaminerInitials = visit.ParticipantDemographics.ExaminerInitials,
                            FormStatus = visit.ParticipantDemographics.FormStatus,
                            Version = visit.ParticipantDemographics.Version
                        },
                        CoParticipantDemographics = new FormBase
                        {
                            Id = visit.CoParticipantDemographics.Id,
                            ExaminerInitials = visit.CoParticipantDemographics.ExaminerInitials,
                            FormStatus = visit.CoParticipantDemographics.FormStatus,
                            Version = visit.CoParticipantDemographics.Version
                        },
                        CDRPlusNACCFTLD = new FormBase
                        {
                            Id = visit.CDRPlusNACCFTLD.Id,
                            ExaminerInitials = visit.CDRPlusNACCFTLD.ExaminerInitials,
                            FormStatus = visit.CDRPlusNACCFTLD.FormStatus,
                            Version = visit.CDRPlusNACCFTLD.Version
                        },
                        Checklist = visit.Checklist, // include the whole thing
                        ClinicianDiagnosis = new FormBase
                        {
                            Id = visit.ClinicianDiagnosis.Id,
                            ExaminerInitials = visit.ClinicianDiagnosis.ExaminerInitials,
                            FormStatus = visit.ClinicianDiagnosis.FormStatus,
                            Version = visit.ClinicianDiagnosis.Version
                        },
                        FunctionalActivitiesQuestionnaire = new FormBase
                        {
                            Id = visit.FunctionalActivitiesQuestionnaire.Id,
                            ExaminerInitials = visit.FunctionalActivitiesQuestionnaire.ExaminerInitials,
                            FormStatus = visit.FunctionalActivitiesQuestionnaire.FormStatus,
                            Version = visit.FunctionalActivitiesQuestionnaire.Version
                        },
                        GeriatricDepressionScale = new FormBase
                        {
                            Id = visit.GeriatricDepressionScale.Id,
                            ExaminerInitials = visit.GeriatricDepressionScale.ExaminerInitials,
                            FormStatus = visit.GeriatricDepressionScale.FormStatus,
                            Version = visit.GeriatricDepressionScale.Version
                        },
                        Hachinski = new FormBase
                        {
                            Id = visit.Hachinski.Id,
                            ExaminerInitials = visit.Hachinski.ExaminerInitials,
                            FormStatus = visit.Hachinski.FormStatus,
                            Version = visit.Hachinski.Version
                        },
                        MedicationsReview = new FormBase
                        {
                            Id = visit.MedicationsReview.Id,
                            ExaminerInitials = visit.MedicationsReview.ExaminerInitials,
                            FormStatus = visit.MedicationsReview.FormStatus,
                            Version = visit.MedicationsReview.Version
                        },
                        MedicalConditions = new FormBase
                        {
                            Id = visit.MedicalConditions.Id,
                            ExaminerInitials = visit.MedicalConditions.ExaminerInitials,
                            FormStatus = visit.MedicalConditions.FormStatus,
                            Version = visit.MedicalConditions.Version
                        },
                        NeurologicalExaminationFindings = new FormBase
                        {
                            Id = visit.NeurologicalExaminationFindings.Id,
                            ExaminerInitials = visit.NeurologicalExaminationFindings.ExaminerInitials,
                            FormStatus = visit.NeurologicalExaminationFindings.FormStatus,
                            Version = visit.NeurologicalExaminationFindings.Version
                        },
                        NeuropsychologicalBatteryScores = new FormBase
                        {
                            Id = visit.NeuropsychologicalBatteryScores.Id,
                            ExaminerInitials = visit.NeuropsychologicalBatteryScores.ExaminerInitials,
                            FormStatus = visit.NeuropsychologicalBatteryScores.FormStatus,
                            Version = visit.NeuropsychologicalBatteryScores.Version
                        },
                        NPIQ = new FormBase
                        {
                            Id = visit.NPIQ.Id,
                            ExaminerInitials = visit.NPIQ.ExaminerInitials,
                            FormStatus = visit.NPIQ.FormStatus,
                            Version = visit.NPIQ.Version
                        },
                        SubjectHealthHistory = new FormBase
                        {
                            Id = visit.SubjectHealthHistory.Id,
                            ExaminerInitials = visit.SubjectHealthHistory.ExaminerInitials,
                            FormStatus = visit.SubjectHealthHistory.FormStatus,
                            Version = visit.SubjectHealthHistory.Version
                        },
                        UnifiedParkinsonsDiseaseRatingScale = new FormBase
                        {
                            Id = visit.UnifiedParkinsonsDiseaseRatingScale.Id,
                            ExaminerInitials = visit.UnifiedParkinsonsDiseaseRatingScale.ExaminerInitials,
                            FormStatus = visit.UnifiedParkinsonsDiseaseRatingScale.FormStatus,
                            Version = visit.UnifiedParkinsonsDiseaseRatingScale.Version
                        },
                        PhysicalEvaluation = new FormBase
                        {
                            Id = visit.PhysicalEvaluation.Id,
                            ExaminerInitials = visit.PhysicalEvaluation.ExaminerInitials,
                            FormStatus = visit.PhysicalEvaluation.FormStatus,
                            Version = visit.PhysicalEvaluation.Version
                        },
                        SubjectFamilyHistory = new FormBase {
                            Id = visit.SubjectFamilyHistory.Id,
                            ExaminerInitials = visit.SubjectFamilyHistory.ExaminerInitials,
                            FormStatus = visit.SubjectFamilyHistory.FormStatus,
                            Version = visit.SubjectFamilyHistory.Version
                        },
                        Symptoms = new FormBase
                        {
                            Id = visit.Symptoms.Id,
                            ExaminerInitials = visit.Symptoms.ExaminerInitials,
                            FormStatus = visit.Symptoms.FormStatus,
                            Version = visit.Symptoms.Version
                        },
                        Inclusion = new FormBase
                        {
                            Id = visit.Inclusion.Id,
                            ExaminerInitials = visit.Inclusion.ExaminerInitials,
                            FormStatus = visit.Inclusion.FormStatus,
                            Version = visit.Inclusion.Version
                        }
                    })
                    .AsNoTracking()
                    .FirstOrDefaultAsync(v => v.Id == id);

                return visitOverview;
            
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// Performs a patch update on visit. Only some properties can be updated.
        /// </summary>
        /// <param name="updatedVisit"></param>
        /// <returns></returns>
        public async Task<Visit> UpdateVisit(Visit updatedVisit, bool currentUserIsAdmin = false, bool currentUserIsSuperUser = false)
        {
            var visit = await _context.Visits.FindAsync(updatedVisit.Id);

            if (!visit.IsSubmittedToNACC || (visit.IsSubmittedToNACC && currentUserIsAdmin))
            {
                // updates to date can't happen if completed
                if (visit.Status != VisitStatus.Complete)
                {
                    visit.VisitDate = updatedVisit.VisitDate;
                }

                // updates to type of visit can't happen if completed
                if (visit.Status != VisitStatus.Complete)
                {
                    visit.VisitType = updatedVisit.VisitType;
                }

                // updates to status
                // anyone can move a packet ot awaiting consensus, prioritized, tabled
                // anyone can move a packet with all completed forms to complete
                // only admins or super users can move a status from complete backwards
                if (visit.Status != updatedVisit.Status)
                {
                    // TODO check if checklist is completed
                    if (updatedVisit.Status == VisitStatus.Complete)
                    {
                        // All forms required by the checklist need to be completed to complete the visit
                        // for submission to NACC
                    }
                    else
                    {
                        if (visit.Status == VisitStatus.InProgress)
                        {
                            visit.Status = updatedVisit.Status; // if status is being changed from in-progress to anything but completed

                        }
                    }
                }

                visit.PriorityWeight = updatedVisit.PriorityWeight;
                visit.IsSubmittedToNACC = updatedVisit.IsSubmittedToNACC;
                visit.CoordinatorInitials = updatedVisit.CoordinatorInitials;
                visit.ClinicianInitials = updatedVisit.ClinicianInitials;
                visit.SocialWorkerInitials = updatedVisit.SocialWorkerInitials;
            }
            if (visit.Status == VisitStatus.Complete)
            {

            }


            return visit;
        }

        public VisitService(UdsContext context)
        {
            _context = context;
        }
    }
}
