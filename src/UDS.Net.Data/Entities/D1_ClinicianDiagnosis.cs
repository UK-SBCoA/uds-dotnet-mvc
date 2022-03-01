using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using COA.Components.Web.DataAnnotations;
using UDS.Net.Data.Enums;

namespace UDS.Net.Data.Entities
{
    [Display(Name = "Clinician Diagnosis")]
    [Table("tbl_D1")]
    public class ClinicianDiagnosis : FormBase
    {
        [RequiredIf(nameof(FormStatus), FormStatus.Complete, ErrorMessage = "Please indicate diagnosis method.")]
        [Display(Name = "Diagnosis method — responses in this form are based on diagnosis by")]
        [Column("DXMETHOD")]
        public int? DiagnosisMethod { get; set; }

        /// <summary>
        /// 0 No (CONTINUE TO QUESTION 3)
        /// 1 Yes (SKIP TO QUESTION 6)
        /// </summary>
        [RequiredIf(nameof(FormStatus), FormStatus.Complete, ErrorMessage = "Please indicate cognitive status.")]
        [Display(Name = "Does the subject have normal cognition (global CDR=0 and/or neuropsychological testing within normal range) and normal behavior (i.e., the subject does not exhibit behavior sufficient to diagnose MCI or dementia due to FTLD or LBD)?")]
        [Column("NORMCOG")]
        public bool? HasNormalCognition { get; set; }

        /// <summary>
        ///  0 No  (SKIP TO QUESTION 5)
        ///  1 Yes  (CONTINUE TO QUESTION 4)
        /// </summary>
        [RequiredIfImpaired(nameof(HasNormalCognition), false, ErrorMessage = "Please indicate dementia status.")]
        [Display(Name = "Does the subject meet the criteria for dementia?")]
        [Column("DEMENTED")]
        public bool? MeetsCriteriaForDementia { get; set; }

        #region Question 4 DEMENTIA SYNDROMES
        /// <summary>
        /// Select one or more as Present; all others will default to Absent in the NACC database.
        /// 0 = Absent
        /// 1 = Present
        /// Sharepoint did not use 0 (although NACC defaults to 0), there were only 1s and nulls in Sharepoint
        /// </summary>
        [Display(Name = "Amnestic multidomain dementia syndrome")]
        [Column("AMNDEM")]
        public bool AmnesticMultiDomainPresent { get; set; }

        [Display(Name = "Posterior cortical atrophy syndrome (or primary visual presentation)")]
        [Column("PCA")]
        public bool PosteriorCorticalAtrophyPresent { get; set; }

        [Display(Name = "Primary progressive aphasia (PPA) syndrome")]
        [Column("PPASYN")]
        public bool PrimaryProgressiveAphasiaPresent { get; set; }

        /// <summary>
        /// Further details for PrimaryProgressiveAphasia
        /// 1  Meets criteria for semantic PPA
        /// 2 Meets criteria for logopenic PPA
        /// 3 Meets criteria for nonfluent/agrammatic PPA
        /// 4 PPA other/not otherwise specified
        /// </summary>
        [RequiredIf(nameof(PrimaryProgressiveAphasiaPresent), true, ErrorMessage = "Please indicate PPA type.")]
        [Display(Name = "If PPA present")]
        [Column("PPASYNT")]
        public int? PrimaryProgressiveAphasiaType { get; set; }

        [Display(Name = "Behavioral variant FTD (bvFTD) syndrome")]
        [Column("FTDSYN")]
        public bool BehavioralVariantFTDPresent { get; set; }

        [Display(Name = "Lewy body dementia syndrome")]
        [Column("LBDSYN")]
        public bool LewyBodyPresent { get; set; }

        [Display(Name = "Non-amnestic multidomain dementia, not PCA, PPA, bvFTD, or DLB syndrome")]
        [Column("NAMNDEM")]
        public bool NonAmnesticMultiDomainPresent { get; set; }

        [RequiredIf(nameof(MeetsCriteriaForDementia), true, ErrorMessage = "Please select one or more cognitive/behavioral syndromes.")]
        [NotMapped]
        public bool? DementiaSyndromeIndicated
        {
            get
            {
                if (AmnesticMultiDomainPresent || PosteriorCorticalAtrophyPresent || PrimaryProgressiveAphasiaPresent || BehavioralVariantFTDPresent || LewyBodyPresent || NonAmnesticMultiDomainPresent)
                {
                    return true;
                }
                else return null;
            }
        }

        #endregion

        #region Question 5 COGNITIVE IMPAIRMENTS

        [Display(Name = "Amnestic MCI, single domain (aMCI SD)")]
        [Column("MCIAMEM")]
        public bool AmnesticMCISingleDomainPresent { get; set; }

        #region Question 5b

        [Display(Name = "Amnestic MCI, multiple domains (aMCI MD)")]
        [Column("MCIAPLUS")]
        public bool AmnesticMCIMultipleDomains { get; set; }

        /// <summary>
        /// CHECK YES for at least one additional domain (besides memory)
        /// </summary>
        [Display(Name = "Language")]
        [Column("MCIAPLAN")]
        public bool? AmnesticMCIMultipleDomainsLanguage { get; set; }

        [Display(Name = "Attention")]
        [Column("MCIAPATT")]
        public bool? AmnesticMCIMultipleDomainsAttention { get; set; }

        [Display(Name = "Executive")]
        [Column("MCIAPEX")]
        public bool? AmnesticMCIMultipleDomainsExecutive { get; set; }

        [Display(Name = "Visuospatial")]
        [Column("MCIAPVIS")]
        public bool? AmnesticMCIMultipleDomainsVisuospatial { get; set; }

        [RequiredIf(nameof(AmnesticMCIMultipleDomains), true, ErrorMessage = "Please at least one additional affected domain.")]
        [NotMapped]
        public bool? AmnesticMCIMultipleDomainsIndicated
        {
            get
            {
                if (AmnesticMCIMultipleDomainsLanguage.HasValue && AmnesticMCIMultipleDomainsAttention.HasValue && AmnesticMCIMultipleDomainsExecutive.HasValue && AmnesticMCIMultipleDomainsVisuospatial.HasValue)
                {
                    if (AmnesticMCIMultipleDomainsLanguage.Value || AmnesticMCIMultipleDomainsAttention.Value || AmnesticMCIMultipleDomainsExecutive.Value || AmnesticMCIMultipleDomainsVisuospatial.Value)
                    {
                        return true;
                    }
                }
                return null;
            }
        }

        #endregion

        #region Question 5c

        [Display(Name = "Non-amnestic MCI, single domain (naMCI SD)")]
        [Column("MCINON1")]
        public bool NonAmnesticMCISingleDomain { get; set; }

        /// <summary>
        /// CHECK YES to indicate the affected domain
        /// </summary>
        [Display(Name = "Language")]
        [Column("MCIN1LAN")]
        public bool? NonAmnesticMCISingleDomainLanguage { get; set; }

        [Display(Name = "Attention")]
        [Column("MCIN1ATT")]
        public bool? NonAmnesticMCISingleDomainAttention { get; set; }

        [Display(Name = "Executive")]
        [Column("MCIN1EX")]
        public bool? NonAmnesticMCISingleDomainExecutive { get; set; }

        [Display(Name = "Visuospatial")]
        [Column("MCIN1VIS")]
        public bool? NonAmnesticMCISingleDomainVisuospatial { get; set; }

        [RequiredIf(nameof(NonAmnesticMCISingleDomain), true, ErrorMessage = "Please select ONE affected domain.")]
        [NotMapped]
        public bool? NonAmnesticMCISingleDomainIndicated
        {
            get
            {
                if (NonAmnesticMCISingleDomainLanguage.HasValue && NonAmnesticMCISingleDomainAttention.HasValue && NonAmnesticMCISingleDomainExecutive.HasValue && NonAmnesticMCISingleDomainVisuospatial.HasValue)
                {
                    int counter = 0;
                    if (NonAmnesticMCISingleDomainLanguage.Value)
                    {
                        counter++;
                    }
                    if (NonAmnesticMCISingleDomainAttention.Value)
                    {
                        counter++;
                    }
                    if (NonAmnesticMCISingleDomainExecutive.Value)
                    {
                        counter++;
                    }
                    if (NonAmnesticMCISingleDomainVisuospatial.Value)
                    {
                        counter++;
                    }
                    if (counter == 1)
                    {
                        return true;
                    }
                }
                return null;
            }
        }

        #endregion

        #region Question 5d

        [Display(Name = "Non-amnestic MCI, multiple domains (naMCI MD)")]
        [Column("MCINON2")]
        public bool NonAmnesticMCIMultipleDomains { get; set; }

        /// <summary>
        /// CHECK YES for at least two domains
        /// </summary>
        [Display(Name = "Language")]
        [Column("MCIN2LAN")]
        public bool? NonAmnesticMCIMultipleDomainsLanguage { get; set; }

        [Display(Name = "Attention")]
        [Column("MCIN2ATT")]
        public bool? NonAmnesticMCIMultipleDomainsAttention { get; set; }

        [Display(Name = "Executive")]
        [Column("MCIN2EX")]
        public bool? NonAmnesticMCIMultipleDomainsExecutive { get; set; }

        [Display(Name = "Visuospatial")]
        [Column("MCIN2VIS")]
        public bool? NonAmnesticMCIMultipleDomainsVisuospatial { get; set; }

        [RequiredIf(nameof(NonAmnesticMCIMultipleDomains), true, ErrorMessage = "Please select at least two affected domains.")]
        [NotMapped]
        public bool? NonAmnesticMCIMultipleDomainsIndicated
        {
            get
            {
                if (NonAmnesticMCIMultipleDomainsLanguage.HasValue && NonAmnesticMCIMultipleDomainsAttention.HasValue && NonAmnesticMCIMultipleDomainsExecutive.HasValue && NonAmnesticMCIMultipleDomainsVisuospatial.HasValue)
                {
                    int counter = 0;
                    if (NonAmnesticMCIMultipleDomainsLanguage.Value)
                    {
                        counter++;
                    }
                    if (NonAmnesticMCIMultipleDomainsAttention.Value)
                    {
                        counter++;
                    }
                    if (NonAmnesticMCIMultipleDomainsExecutive.Value)
                    {
                        counter++;
                    }
                    if (NonAmnesticMCIMultipleDomainsVisuospatial.Value)
                    {
                        counter++;
                    }
                    if (counter >= 2)
                    {
                        return true;
                    }
                }
                return null;
            }
        }

        #endregion

        [Display(Name = "Cognitively impaired, not MCI")]
        [Column("IMPNOMCI")]
        public bool CognitivelyImpairedNotMCIPresent { get; set; }

        [RequiredIfImpaired(nameof(MeetsCriteriaForDementia), false, ErrorMessage = "Please select one syndrome as being present.")]
        [NotMapped]
        public bool? MCIIndicated
        {
            get
            {
                if (AmnesticMCISingleDomainPresent || AmnesticMCIMultipleDomains || NonAmnesticMCISingleDomain || NonAmnesticMCIMultipleDomains || CognitivelyImpairedNotMCIPresent)
                {
                    return true;
                } else return null;
            }
        }


        #endregion

        #region BIOMARKER FINDINGS

        [RequiredIf(nameof(FormStatus), FormStatus.Complete, ErrorMessage = "Please indicate biomarker findings.")]
        [Display(Name = "Abnormally elevated amyloid on PET")]
        [Column("AMYLPET")]
        public int? AbnormallyElevatedAmyloidInPET{ get; set; }

        [RequiredIf(nameof(FormStatus), FormStatus.Complete, ErrorMessage = "Please indicate biomarker findings.")]
        [Display(Name = "Abnormally low amyloid in CSF")]
        [Column("AMYLCSF")]
        public int? AbnormallyLowAmyloidInCSF { get; set; }

        [RequiredIf(nameof(FormStatus), FormStatus.Complete, ErrorMessage = "Please indicate biomarker findings.")]
        [Display(Name = "FDG-PET pattern of AD")]
        [Column("FDGAD")]
        public int? FDGPETPattern { get; set; }

        [RequiredIf(nameof(FormStatus), FormStatus.Complete, ErrorMessage = "Please indicate biomarker findings.")]
        [Display(Name = "Hippocampal atrophy")]
        [Column("HIPPATR")]
        public int? HippocampalAtrophy { get; set; }

        [RequiredIf(nameof(FormStatus), FormStatus.Complete, ErrorMessage = "Please indicate biomarker findings.")]
        [Display(Name = "Tau PET evidence for AD")]
        [Column("TAUPETAD")]
        public int? TauPETEvidenceForAD { get; set; }

        [RequiredIf(nameof(FormStatus), FormStatus.Complete, ErrorMessage = "Please indicate biomarker findings.")]
        [Display(Name = "Abnormally elevated CSF tau or ptau")]
        [Column("CSFTAU")]
        public int? AbnormallyElevatedCSFTauOrPtau { get; set; }

        [RequiredIf(nameof(FormStatus), FormStatus.Complete, ErrorMessage = "Please indicate biomarker findings.")]
        [Display(Name = "FDG-PET evidence for frontal or anterior temporal hypometabolism for FTLD")]
        [Column("FDGFTLD")]
        public int? FDGPETEvidenceForFrontalOrAnteriorTemporalHypometabolism { get; set; }

        [RequiredIf(nameof(FormStatus), FormStatus.Complete, ErrorMessage = "Please indicate biomarker findings.")]
        [Display(Name = "Tau PET evidence for FTLD")]
        [Column("TPETFTLD")]
        public int? TauPETEvidence { get; set; }

        [RequiredIf(nameof(FormStatus), FormStatus.Complete, ErrorMessage = "Please indicate biomarker findings.")]
        [Display(Name = "Structural MR evidence for frontal or anterior temporal atrophy for FTLD")]
        [Column("MRFTLD")]
        public int? StructuralMREvidence { get; set; }

        [RequiredIf(nameof(FormStatus), FormStatus.Complete, ErrorMessage = "Please indicate biomarker findings.")]
        [Display(Name = "Dopamine transporter scan (DATscan) evidence for Lewy body disease")]
        [Column("DATSCAN")]
        public int? DopamineTransporterScanEvidence { get; set; }

        /// <summary>
        /// 0 = No
        /// 1 = Yes
        /// </summary>
        [Display(Name = "Other (SPECIFY)")]
        [Column("OTHBIOM")]
        public bool? OtherEvidenceExists { get; set; }

        [RequiredIf(nameof(OtherEvidenceExists), "1", ErrorMessage = "Please specify other biomarker findings.")]
        [Display(Name = "Other (SPECIFY)")]
        [Column("OTHBIOMX")]
        [MaxLength(60)]
        public string OtherEvidenceSpecified { get; set; }

        #endregion

        #region IMAGING FINDINGS

        [RequiredIf(nameof(FormStatus), FormStatus.Complete, ErrorMessage = "Please indicate imaging findings.")]
        [Display(Name = "Large vessel infarct(s)")]
        [Column("IMAGLINF")]
        public int? LargeVesselInfarcts{ get; set; }

        [RequiredIf(nameof(FormStatus), FormStatus.Complete, ErrorMessage = "Please indicate imaging findings.")]
        [Display(Name = "Lacunar infarct(s)")]
        [Column("IMAGLAC")]
        public int? LacunarInfarcts { get; set; }

        [RequiredIf(nameof(FormStatus), FormStatus.Complete, ErrorMessage = "Please indicate imaging findings.")]
        [Display(Name = "Macrohemorrhage(s)")]
        [Column("IMAGMACH")]
        public int? Macrohemorrhages { get; set; }

        [RequiredIf(nameof(FormStatus), FormStatus.Complete, ErrorMessage = "Please indicate imaging findings.")]
        [Display(Name = "Microhemorrhage(s)")]
        [Column("IMAGMICH")]
        public int? Microhemorrhages { get; set; }

        [RequiredIf(nameof(FormStatus), FormStatus.Complete, ErrorMessage = "Please indicate imaging findings.")]
        [Display(Name = "Moderate white-matter hyperintensity (CHS score 5–6)")]
        [Column("IMAGMWMH")]
        public int? ModerateWhiteMatterHyperintensity { get; set; }

        [RequiredIf(nameof(FormStatus), FormStatus.Complete, ErrorMessage = "Please indicate imaging findings.")]
        [Display(Name = "Extensive white-matter hyperintensity (CHS score 7–8+)")]
        [Column("IMAGEWMH")]
        public int? ExtensiveWhiteMatterHyperintensity { get; set; }

        #endregion

        [RequiredIf(nameof(FormStatus), FormStatus.Complete, ErrorMessage = "Please indicate imaging findings.")]
        [Display(Name = "Does the subject have a dominantly inherited AD mutation (PSEN1, PSEN2, APP)?")]
        [Column("ADMUT")]
        public int? DominantlyInheritedADMutation { get; set; }

        [RequiredIf(nameof(FormStatus), FormStatus.Complete, ErrorMessage = "Please indicate imaging findings.")]
        [Display(Name = "Does the subject have a hereditary FTLD mutation (e.g., GRN, VCP, TARBP, FUS, C9orf72, CHMP2B, MAPT)?")]
        [Column("FTLDMUT")]
        public int? HereditaryFTLDMutation { get; set; }

        [RequiredIf(nameof(FormStatus), FormStatus.Complete, ErrorMessage = "Please indicate imaging findings.")]
        [Display(Name = "Does the subject have a hereditary mutation other than an AD or FTLD mutation?")]
        [Column("OTHMUT")]
        public int? HereditaryMutationOther { get; set; }

        [RequiredIf(nameof(HereditaryMutationOther), "1", ErrorMessage = "Please specify other imaging findings.")]
        [Column("OTHMUTX")]
        [MaxLength(60)]
        public string HereditaryMutationOtherSpecified { get; set; }

        #region ETIOLOGIC DIAGNOSES

        /// <summary>
        /// All diagnoses that are present should be selected;
        /// all others will default to Absent in the NACC database.
        ///
        /// For subjects with normal cognition, presence should be indicated, but whether the
        /// diagnosis was primary, contributing, or non-contributing should not be indicated.
        /// </summary>
        [RequiredIf(nameof(FormStatus), FormStatus.Complete, ErrorMessage = "In Section 3, ONE diagnosis should be indicated as primary.")]
        [NotMapped]
        public bool? OnlyOnePrimaryDiagnosisAllowed {
            get
            {
                int counter = 0;
                
                if (AlzheimersDiseaseDiagnosis.HasValue && AlzheimersDiseaseDiagnosis.Value == 1)
                {
                    counter++;
                }
                if (LewyBodyDiseaseDiagnosis.HasValue && LewyBodyDiseaseDiagnosis.Value == 1)
                {
                    counter++;
                }
                if (MultipleSystemAtrophyDiagnosis.HasValue && MultipleSystemAtrophyDiagnosis.Value == 1)
                {
                    counter++;
                }
                if (ProgressiveSupranuclearPaslyDiagnosis.HasValue && ProgressiveSupranuclearPaslyDiagnosis.Value == 1)
                {
                    counter++;
                }
                if (CorticobasalDegenerationDiagnosis.HasValue && CorticobasalDegenerationDiagnosis.Value == 1)
                {
                    counter++;
                }
                if (FTLDWithMotorNeuronDiseaseDiagnosis.HasValue && FTLDWithMotorNeuronDiseaseDiagnosis.Value == 1)
                {
                    counter++;
                }
                if (FTLDNOSDiagnosis.HasValue && FTLDNOSDiagnosis.Value == 1)
                {
                    counter++;
                }
                if (VascularBrainInjuryDiagnosis.HasValue && VascularBrainInjuryDiagnosis.Value == 1)
                {
                    counter++;
                }
                if (EssentialTremorDiagnosis.HasValue && EssentialTremorDiagnosis.Value == 1)
                {
                    counter++;
                }
                if (DownSyndromeDiagnosis.HasValue && DownSyndromeDiagnosis.Value == 1)
                {
                    counter++;
                }
                if (HuntingtonsDiseaseDiagnosis.HasValue && HuntingtonsDiseaseDiagnosis.Value == 1)
                {
                    counter++;
                }
                if (PrionDiseaseDiagnosis.HasValue && PrionDiseaseDiagnosis.Value == 1)
                {
                    counter++;
                }
                if (TraumaticBrainInjuryDiagnosis.HasValue && TraumaticBrainInjuryDiagnosis.Value == 1)
                {
                    counter++;
                }
                if (NormalPressureHydropcephalusDiagnosis.HasValue && NormalPressureHydropcephalusDiagnosis.Value == 1)
                {
                    counter++;
                }
                if (EpilepsyDiagnosis.HasValue && EpilepsyDiagnosis.Value == 1)
                {
                    counter++;
                }
                if (CNSNeoplasmDiagnosis.HasValue && CNSNeoplasmDiagnosis.Value == 1)
                {
                    counter++;
                }
                if (HumanImmunodeficiencyVirusDiagnosis.HasValue && HumanImmunodeficiencyVirusDiagnosis.Value == 1)
                {
                    counter++;
                }
                if (OtherCognitiveImpairmentDiagnosis.HasValue && OtherCognitiveImpairmentDiagnosis.Value == 1)
                {
                    counter++;
                }
                if (ActiveDepressionDiagnosis.HasValue && ActiveDepressionDiagnosis.Value == 1)
                {
                    counter++;
                }
                if (BipolarDisorderDiagnosis.HasValue && BipolarDisorderDiagnosis.Value == 1)
                {
                    counter++;
                }
                if (SchizophreniaDiagnosis.HasValue && SchizophreniaDiagnosis.Value == 1)
                {
                    counter++;
                }
                if (AnxietyDiagnosis.HasValue && AnxietyDiagnosis.Value == 1)
                {
                    counter++;
                }
                if (DeliriumDiagnosis.HasValue && DeliriumDiagnosis.Value == 1)
                {
                    counter++;
                }
                if (PostTraumaticStressDisorderDiagnosis.HasValue && PostTraumaticStressDisorderDiagnosis.Value == 1)
                {
                    counter++;
                }
                if (OtherPsychiatricDiseaseDiagnosis.HasValue && OtherPsychiatricDiseaseDiagnosis.Value == 1)
                {
                    counter++;
                }
                if (AlcoholAbuseDiagnosis.HasValue && AlcoholAbuseDiagnosis.Value == 1)
                {
                    counter++;
                }
                if (SubstanceAbuseDiagnosis.HasValue && SubstanceAbuseDiagnosis.Value == 1)
                {
                    counter++;
                }
                if (SystemicDiseaseDiagnosis.HasValue && SystemicDiseaseDiagnosis.Value == 1)
                {
                    counter++;
                }
                if (MedicationsDiagnosis.HasValue && MedicationsDiagnosis.Value == 1)
                {
                    counter++;
                }
                if (NOS1Diagnosis.HasValue && NOS1Diagnosis.Value == 1)
                {
                    counter++;
                }
                if (NOS2Diagnosis.HasValue && NOS2Diagnosis.Value == 1)
                {
                    counter++;
                }
                if (NOS3Diagnosis.HasValue && NOS3Diagnosis.Value == 1)
                {
                    counter++;
                }
                
                if (counter == 1)
                {
                    return true;
                }
                else
                {
                    // For normal cognition, etiologies should be marked as present, but the diagnosis indicators can be ignored.
                    if (HasNormalCognition.HasValue && HasNormalCognition.Value == true)
                    {
                        return true;
                    }
                    return null;
                }
            }
        }


        [Display(Name = "Alzheimer's disease")]
        [Column("ALZDIS")]
        public bool AlzheimersDiseasePresent { get; set; }

        /// <summary>
        /// Diagnosis of observed impairment:
        /// 1 = Primary
        /// 2 = Contributing
        /// 3 = Non-contributing
        /// </summary>
        [RequiredIfImpaired(nameof(AlzheimersDiseasePresent), true, ErrorMessage = "Please indicate diagnosis. Only one should be selected as 1=Primary.")]
        [Column("ALZDISIF")]
        public int? AlzheimersDiseaseDiagnosis { get; set; }



        [Display(Name = "Lewy body disease")]
        [Column("LBDIS")]
        public bool LewyBodyDiseasePresent { get; set; }

        [RequiredIfImpaired(nameof(LewyBodyDiseasePresent), true, ErrorMessage = "Please indicate diagnosis. Only one should be selected as 1=Primary.")]
        [Column("LBDIF")]
        public int? LewyBodyDiseaseDiagnosis { get; set; }

        [Display(Name = "Parkinson's disease")]
        [Column("PARK")]
        public bool ParkinsonsDiseasePresent { get; set; }

        [Display(Name = "Multiple system atrophy")]
        [Column("MSA")]
        public bool MultipleSystemAtrophyPresent { get; set; }

        [RequiredIfImpaired(nameof(MultipleSystemAtrophyPresent), true, ErrorMessage = "Please indicate diagnosis. Only one should be selected as 1=Primary.")]
        [Column("MSAIF")]
        public int? MultipleSystemAtrophyDiagnosis { get; set; }

        [Display(Name = "Progressive supranuclear palsy (PSP)")]
        [Column("PSP")]
        public bool ProgressiveSupranuclearPaslyPresent { get; set; }

        [RequiredIfImpaired(nameof(ProgressiveSupranuclearPaslyPresent), true, ErrorMessage = "Please indicate diagnosis. Only one should be selected as 1=Primary.")]
        [Column("PSPIF")]
        public int? ProgressiveSupranuclearPaslyDiagnosis { get; set; }

        [Display(Name = "Corticobasal degeneration (CBD)")]
        [Column("CORT")]
        public bool CorticobasalDegenerationPresent { get; set; }

        [RequiredIfImpaired(nameof(CorticobasalDegenerationPresent), true, ErrorMessage = "Please indicate diagnosis. Only one should be selected as 1=Primary.")]
        [Column("CORTIF")]
        public int? CorticobasalDegenerationDiagnosis { get; set; }

        [Display(Name = "FTLD with motor neuron disease")]
        [Column("FTLDMO")]
        public bool FTLDWithMotorNeuronDiseasePresent { get; set; }

        [RequiredIfImpaired(nameof(FTLDWithMotorNeuronDiseasePresent), true, ErrorMessage = "Please indicate diagnosis. Only one should be selected as 1=Primary.")]
        [Column("FTLDMOIF")]
        public int? FTLDWithMotorNeuronDiseaseDiagnosis { get; set; }

        [Display(Name = "FTLD NOS")]
        [Column("FTLDNOS")]
        public bool FTLDNOSPresent { get; set; }

        [RequiredIfImpaired(nameof(FTLDNOSPresent), true, ErrorMessage = "Please indicate diagnosis. Only one should be selected as 1=Primary.")]
        [Column("FTLDNOIF")]
        public int? FTLDNOSDiagnosis { get; set; }

        [Display(Name = "If FTLD (Questions 14a – 14d) is Present, specify FTLD subtype")]
        [RequiredIf(nameof(ProgressiveSupranuclearPaslyPresent), "1", ErrorMessage = "Please indicate FTLD subtype")]
        [RequiredIf(nameof(CorticobasalDegenerationPresent), "1", ErrorMessage = "Please indicate FTLD subtype")]
        [RequiredIf(nameof(FTLDWithMotorNeuronDiseasePresent), "1", ErrorMessage = "Please indicate FTLD subtype")]
        [RequiredIf(nameof(FTLDNOSPresent), "1", ErrorMessage = "Please indicate FTLD subtype")]
        [Column("FTLDSUBT")]
        public int? FTLDSubtype { get; set; }

        [Display(Name = "Other (SPECIFY)")]
        [RequiredIf(nameof(FTLDSubtype), "3", ErrorMessage = "Please specify FTLD subtype")]
        [Column("FTLDSUBX")]
        [MaxLength(60)]
        public string FTLDSubtypeOtherSpecified { get; set; }

        [Display(Name = "Vascular brain injury (based on clinical or imaging evidence)")]
        [Column("CVD")]
        public bool VascularBrainInjuryPresent { get; set; }

        [RequiredIfImpaired(nameof(VascularBrainInjuryPresent), "1", ErrorMessage = "Please indicate")]
        [Column("CVDIF")]
        public int? VascularBrainInjuryDiagnosis { get; set; }

        [RequiredIf(nameof(VascularBrainInjuryPresent), "1", ErrorMessage = "Please indicate")]
        [Display(Name = "Previous symptomatic stroke?")]
        [Column("PREVSTK")]
        public bool? PreviousSymptomaticStroke { get; set; }

        [RequiredIf(nameof(PreviousSymptomaticStroke), true, ErrorMessage = "Please indicate")]
        [Display(Name = "Temporal relationship between stroke and cognitive decline?")]
        [Column("STROKDEC")]
        public bool? TemporalRelationshipBetweenStrokeAndCognitive { get; set; }

        [RequiredIf(nameof(PreviousSymptomaticStroke), true, ErrorMessage = "Please indicate")]
        [Display(Name = "Confirmation of stroke by neuroimaging?")]
        [Column("STKIMAG")]
        public int? ConfirmationOfStrokeByNeuroImaging { get; set; }

        [RequiredIf(nameof(VascularBrainInjuryPresent), "1", ErrorMessage = "Please indicate")]
        [Display(Name = "Is there imaging evidence of cystic infarction in cognitive network(s)?")]
        [Column("INFNETW")]
        public int? ImagingEvidenceOfCysticInfarctionInCognitiveNetwork { get; set; }

        [RequiredIf(nameof(VascularBrainInjuryPresent), "1", ErrorMessage = "Please indicate")]
        [Display(Name = "Is there imaging evidence of cystic infarction, imaging evidence of extensive white matter hyperintensity (CHS grade 7–8+), and impairment in executive function?")]
        [Column("INFWMH")]
        public int? ImagingEvidenceOfCysticInfarctionExtensiveWhiteMatterHyperintensistyAndExecutiveImpairment { get; set; }

        [Display(Name = "Essential tremor")]
        [Column("ESSTREM")]
        public bool EssentialTremorPresent { get; set; }

        [RequiredIfImpaired(nameof(EssentialTremorPresent), "1", ErrorMessage = "Please indicate")]
        [Column("ESSTREIF")]
        public int? EssentialTremorDiagnosis { get; set; }

        [Display(Name = "Down syndrome")]
        [Column("DOWNS")]
        public bool DownSyndromePresent { get; set; }

        [RequiredIfImpaired(nameof(DownSyndromePresent), "1", ErrorMessage = "Please indicate")]
        [Column("DOWNSIF")]
        public int? DownSyndromeDiagnosis { get; set; }

        [Display(Name = "Huntington's disease")]
        [Column("HUNT")]
        public bool HuntingtonsDiseasePresent { get; set; }

        [RequiredIfImpaired(nameof(HuntingtonsDiseasePresent), "1", ErrorMessage = "Please indicate")]
        [Column("HUNTIF")]
        public int? HuntingtonsDiseaseDiagnosis { get; set; }

        [Display(Name = "Prion disease (CJD, other)")]
        [Column("PRION")]
        public bool PrionDiseasePresent { get; set; }

        [RequiredIfImpaired(nameof(PrionDiseasePresent), "1", ErrorMessage = "Please indicate")]
        [Column("PRIONIF")]
        public int? PrionDiseaseDiagnosis { get; set; }

        [Display(Name = "Traumatic brain injury")]
        [Column("BRNINJ")]
        public bool TraumaticBrainInjuryPresent { get; set; }

        [RequiredIfImpaired(nameof(TraumaticBrainInjuryPresent), "1", ErrorMessage = "Please indicate")]
        [Column("BRNINJIF")]
        public int? TraumaticBrainInjuryDiagnosis { get; set; }

        [RequiredIf(nameof(TraumaticBrainInjuryPresent), "1", ErrorMessage = "Please indicate")]
        [Display(Name = "If Present, does the subject have symptoms consistent with chronic traumatic encephalopathy?")]
        [Column("BRNINCTE")]
        public int? ChronicTraumaticEncephalopathySymptom { get; set; }

        [Display(Name = "Normal-pressure hydrocephalus")]
        [Column("HYCEPH")]
        public bool NormalPressureHydrocephalusPresent { get; set; }

        [RequiredIfImpaired(nameof(NormalPressureHydrocephalusPresent), "1", ErrorMessage = "Please indicate")]
        [Column("HYCEPHIF")]
        public int? NormalPressureHydropcephalusDiagnosis { get; set; }

        [Display(Name = "Epilepsy")]
        [Column("EPILEP")]
        public bool EpilepsyPresent { get; set; }

        [RequiredIfImpaired(nameof(EpilepsyPresent), "1", ErrorMessage = "Please indicate")]
        [Column("EPILEPIF")]
        public int? EpilepsyDiagnosis { get; set; }

        [Display(Name = "CNS neoplasm")]
        [Column("NEOP")]
        public bool CNSNeoplasmPresent { get; set; }

        [RequiredIfImpaired(nameof(CNSNeoplasmPresent), "1", ErrorMessage = "Please indicate")]
        [Column("NEOPIF")]
        public int? CNSNeoplasmDiagnosis { get; set; }

        [RequiredIf(nameof(CNSNeoplasmPresent), "1", ErrorMessage = "Please indicate")]
        [Column("NEOPSTAT")]
        public int? CNSNeoplasmType { get; set; }

        [Display(Name = "Human immunodeficiency virus (HIV)")]
        [Column("HIV")]
        public bool HumanImmunodeficiencyVirusPresent { get; set; }

        [RequiredIfImpaired(nameof(HumanImmunodeficiencyVirusPresent), "1", ErrorMessage = "Please indicate")]
        [Column("HIVIF")]
        public int? HumanImmunodeficiencyVirusDiagnosis { get; set; }

        [Display(Name = "Cognitive impairment due to other neurologic, genetic, or infectious conditions not listed above")]
        [Column("OTHCOG")]
        public bool OtherCognitiveImpairmentPresent { get; set; }

        [RequiredIfImpaired(nameof(OtherCognitiveImpairmentPresent), "1", ErrorMessage = "Please indicate")]
        [Column("OTHCOGIF")]
        public int? OtherCognitiveImpairmentDiagnosis { get; set; }

        /// <summary>
        /// Any text or numbers with the exception of single quotes (‘), double quotes (“), ampersands (&), and percentage signs(%).
        /// </summary>
        [Display(Name = "If Present, specify")]
        [RequiredIf(nameof(OtherCognitiveImpairmentPresent), "1", ErrorMessage = "Please indicate")]
        [Column("OTHCOGX")]
        [MaxLength(60)]
        public string OtherCognitiveImpairmentSpecified { get; set; }

        #endregion

        #region CONDITION

        [Display(Name = "Active depression")]
        [Column("DEP")]
        public bool ActiveDepressionPresent { get; set; }

        [RequiredIfImpaired(nameof(ActiveDepressionPresent), "1", ErrorMessage = "Please indicate")]
        [Column("DEPIF")]
        public int? ActiveDepressionDiagnosis { get; set; }

        /// <summary>
        /// 0 Untreated
        /// 1 Treated with medication and/or counseling
        /// </summary>
        [RequiredIf(nameof(ActiveDepressionPresent), "1", ErrorMessage = "Please indicate")]
        [Display(Name = "If Present, select one")]
        [Column("DEPTREAT")]
        public bool? ActiveDepressionTreatment { get; set; }

        [Display(Name = "Bipolar disorder")]
        [Column("BIPOLDX")]
        public bool BipolarDisorderPresent { get; set; }

        [RequiredIfImpaired(nameof(BipolarDisorderPresent), "1", ErrorMessage = "Please indicate")]
        [Column("BIPOLDIF")]
        public int? BipolarDisorderDiagnosis { get; set; }

        [Display(Name = "Schizophrenia or other psychosis")]
        [Column("SCHIZOP")]
        public bool SchizophreniaPresent { get; set; }

        [RequiredIfImpaired(nameof(SchizophreniaPresent), "1", ErrorMessage = "Please indicate")]
        [Column("SCHIZOIF")]
        public int? SchizophreniaDiagnosis { get; set; }

        [Display(Name = "Anxiety disorder")]
        [Column("ANXIET")]
        public bool AnxietyDisorderPresent { get; set; }

        [RequiredIfImpaired(nameof(AnxietyDisorderPresent), "1", ErrorMessage = "Please indicate")]
        [Column("ANXIETIF")]
        public int? AnxietyDiagnosis { get; set; }

        [Display(Name = "Delirium")]
        [Column("DELIR")]
        public bool DeliriumPresent { get; set; }

        [RequiredIfImpaired(nameof(DeliriumPresent), "1", ErrorMessage = "Please indicate")]
        [Column("DELIRIF")]
        public int? DeliriumDiagnosis { get; set; }

        [Display(Name = "Post-traumatic stress disorder (PTSD)")]
        [Column("PTSDDX")]
        public bool PostTraumaticStressDisorderPresent { get; set; }

        [RequiredIfImpaired(nameof(PostTraumaticStressDisorderPresent), "1", ErrorMessage = "Please indicate")]
        [Column("PTSDDXIF")]
        public int? PostTraumaticStressDisorderDiagnosis { get; set; }

        [Display(Name = "Other psychiatric disease")]
        [Column("OTHPSY")]
        public bool OtherPsychiatricDiseasePresent { get; set; }

        [RequiredIfImpaired(nameof(OtherPsychiatricDiseasePresent), "1", ErrorMessage = "Please indicate")]
        [Column("OTHPSYIF")]
        public int? OtherPsychiatricDiseaseDiagnosis { get; set; }

        [RequiredIf(nameof(OtherPsychiatricDiseasePresent), "1", ErrorMessage = "Please indicate")]
        [Display(Name = "If Present, specify")]
        [Column("OTHPSYX")]
        [MaxLength(60)]
        public string OtherPsychiatricDiseaseSpecified { get; set; }

        [Display(Name = "Cognitive impairment due to alcohol abuse")]
        [Column("ALCDEM")]
        public bool AlcoholAbuse { get; set; }

        [RequiredIfImpaired(nameof(AlcoholAbuse), "1", ErrorMessage = "Please indicate")]
        [Column("ALCDEMIF")]
        public int? AlcoholAbuseDiagnosis { get; set; }

        [RequiredIf(nameof(AlcoholAbuse), "1", ErrorMessage = "Please indicate")]
        [Column("ALCABUSE")]
        public int? CurrentAlcoholAbuse { get; set; }

        [Display(Name = "Cognitive impairment due to other substance abuse")]
        [Column("IMPSUB")]
        public bool SubstanceAbuse { get; set; }

        [RequiredIfImpaired(nameof(SubstanceAbuse), "1", ErrorMessage = "Please indicate")]
        [Column("IMPSUBIF")]
        public int? SubstanceAbuseDiagnosis { get; set; }

        [Display(Name = "Cognitive impairment due to systemic disease/medical illness (as indicated on Form D2)")]
        [Column("DYSILL")]
        public bool SystemicDisease { get; set; }

        [RequiredIfImpaired(nameof(SystemicDisease), "1", ErrorMessage = "Please indicate")]
        [Column("DYSILLIF")]
        public int? SystemicDiseaseDiagnosis { get; set; }

        [Display(Name = "Cognitive impairment due to medications")]
        [Column("MEDS")]
        public bool Medications { get; set; }

        [RequiredIfImpaired(nameof(Medications), "1", ErrorMessage = "Please indicate")]
        [Column("MEDSIF")]
        public int? MedicationsDiagnosis { get; set; }

        [Display(Name = "Cognitive impairment NOS")]
        [Column("COGOTH")]
        public bool NOS1 { get; set; }

        [RequiredIfImpaired(nameof(NOS1), "1", ErrorMessage = "Please indicate")]
        [Column("COGOTHIF")]
        public int? NOS1Diagnosis { get; set; }

        [RequiredIf(nameof(NOS1), "1", ErrorMessage = "Please indicate")]
        [Display(Name = "If Present, specify")]
        [Column("COGOTHX")]
        [MaxLength(60)]
        public string NOS1Specified { get; set; }

        [Display(Name = "Cognitive impairment NOS")]
        [Column("COGOTH2")]
        public bool NOS2 { get; set; }

        [RequiredIfImpaired(nameof(NOS2), "1", ErrorMessage = "Please indicate")]
        [Column("COGOTH2F")]
        public int? NOS2Diagnosis { get; set; }

        [RequiredIf(nameof(NOS2), "1", ErrorMessage = "Please indicate")]
        [Display(Name = "If Present, specify")]
        [Column("COGOTH2X")]
        [MaxLength(60)]
        public string NOS2Specified { get; set; }

        [Display(Name = "Cognitive impairment NOS")]
        [Column("COGOTH3")]
        public bool NOS3 { get; set; }

        [RequiredIfImpaired(nameof(NOS3), "1", ErrorMessage = "Please indicate")]
        [Column("COGOTH3F")]
        public int? NOS3Diagnosis { get; set; }

        [RequiredIf(nameof(NOS3), "1", ErrorMessage = "Please indicate")]
        [Display(Name = "If Present, specify")]
        [Column("COGOTH3X")]
        [MaxLength(60)]
        public string NOS3Specified { get; set; }

        #endregion


    }
}
