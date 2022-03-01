using System;
using Microsoft.EntityFrameworkCore;
using UDS.Net.Data.Entities;
using UDS.Net.Data.Enums;
using EntityFrameworkCore.TemporalTables.Extensions;
using System.Threading.Tasks;
using System.Linq;

namespace UDS.Net.Data
{
    //// In EF Core 5, behavior changes. We have to explicitly tell
    //// EF Core to materialize its child instances even when all columns are null
    //// Currently, haven't found a way to do this, so we are staying with EF Core 3.1
    public class UdsContext : DbContext
    {
        public UdsContext(DbContextOptions<UdsContext> options) : base(options) { }

        public DbSet<Participation> Participations { get; set; }
        public DbSet<Visit> Visits { get; set; }
        public DbSet<Examiner> Examiners { get; set; }
        public DbSet<ParticipantDemographics> ParticipantDemographics{ get; set; } // A1
        public DbSet<CoParticipantDemographics> CoParticipantDemographics { get; set; } // A2
        public DbSet<MedicationCurrent> MedicationCurrent { get; set; } // A4D
        public DbSet<MedicationsReview> MedicationsReviews { get; set; } //A4G
        public DbSet<MedicationReference> MedicationReferences { get; set; }
        public DbSet<SubjectHealthHistory> SubjectHealthHistories { get; set; } // A5
        public DbSet<PhysicalEvaluation> PhysicalEvaluation { get; set; } // B1
        public DbSet<UnifiedParkinsonsDiseaseRatingScale> UnifiedParkinsonsDiseaseRatingScale { get; set; } // B3
        public DbSet<CDRPlusNACCFTLD> CDRPlusNACCFTDLs { get; set; } // B4
        public DbSet<NPIQ> NPIQs { get; set; } // B5
        public DbSet<GeriatricDepressionScale> GeriatricDepressionScales { get; set; } // B6
        public DbSet<FunctionalActivitiesQuestionnaire> FunctionalActivitiesQuestionnaires { get; set; } // B7
        public DbSet<NeurologicalExaminationFindings> NeurologicalExaminationFindings { get; set; } // B8
        public DbSet<Hachinski> HachinskiScores { get; set; } // B9
        public DbSet<NeuropsychologicalBatteryScores> NeuropsychologicalBatteryScores { get; set; } // C2
        public DbSet<ClinicianDiagnosis> ClinicianDiagnoses { get; set; } // D1
        public DbSet<MedicalConditions> MedicalConditions { get; set; } // D2
        public DbSet<SubjectFamilyHistory> SubjectFamilyHistories { get; set; }
        public DbSet<Relative> Relatives { get; set; }
        public DbSet<Checklist> Checklists { get; set; } // Z1
        public DbSet<Milestone> Milestones { get; set; } // M1
        public DbSet<Inclusion> Inclusions { get; set; } // T1
        public DbSet<Symptoms> Symptoms{ get; set; } // B9

        protected override void OnModelCreating(ModelBuilder builder)
        {
            // Register Temporal Tables
            builder.UseTemporalTables();

            // Enum conversion for participation status
            builder.Entity<Participation>()
                .Property(p => p.Status)
                .HasConversion(
                    x => x.ToString(),
                    x => (ParticipationStatus)Enum.Parse(typeof(ParticipationStatus), x)
                );

            builder.Entity<Participation>()
                .Ignore(p => p.Profile);

            // Enum conversion for visit type
            builder.Entity<Visit>()
                .Property(v => v.VisitType)
                .HasConversion(
                    x => x.ToString(),
                    x => (VisitType)Enum.Parse(typeof(VisitType), x)
                );

            // Enum conversion for visit status
            builder.Entity<Visit>()
                .Property(v => v.Status)
                .HasConversion(
                    x => x.ToString(),
                    x => (VisitStatus)Enum.Parse(typeof(VisitStatus), x)
                );

            builder.Entity<Relative>()
                .Property(x => x.Relation)
                .HasConversion(
                    x => x.ToString(),
                    x => (FamilyRelationship)Enum.Parse(typeof(FamilyRelationship), x)
                );



        }
        public virtual async Task<int> SaveChangesAsync(string username)
        {
            if(String.IsNullOrEmpty(username)) 
            {
                throw new InvalidOperationException("A username must be provided");
            }
            foreach(var entry in ChangeTracker.Entries())
            {
                var modifiedByProperty = entry.Properties.Where(x => x.Metadata.Name == "ModifiedBy");
                if (modifiedByProperty.Any())
                {
                    entry.Property("ModifiedBy").CurrentValue = username;
                }
            }
            var result = await base.SaveChangesAsync();
            return result;
        }

    }
}