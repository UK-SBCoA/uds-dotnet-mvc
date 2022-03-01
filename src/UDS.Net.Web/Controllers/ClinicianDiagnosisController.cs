using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;
using UDS.Net.Data;
using UDS.Net.Data.Entities;
using UDS.Net.Data.Enums;
using UDS.Net.Web.ViewModels;
using UDS.Net.Web.Services;

namespace UDS.Net.Web.Controllers
{
    public class ClinicianDiagnosisController : Controller
    {
        private readonly UdsContext _context;
        private ProtocolVariable _findings;
        private ProtocolVariable _findingsSubs;
        private ProtocolVariable _etiologic;
        private ProtocolVariable[] _protocolVariables;
        private readonly IParticipantsService _participantsService;

        public ClinicianDiagnosisController(UdsContext context, IParticipantsService participantsService)
        {
            _context = context;
            _participantsService = participantsService;


            GetVariablesCodes();

        }

        private async void GetVariablesCodes()
        {
            string jsonString = await System.IO.File.ReadAllTextAsync("App_Data/ClinicianDiagnosisVariableCodes.json");
            _protocolVariables = JsonSerializer.Deserialize<ProtocolVariable[]>(jsonString);

            _findings = _protocolVariables
                .Where(item => item.Name == "FINDINGS")
                .FirstOrDefault();

            _findingsSubs = _protocolVariables
                .Where(item => item.Name == "FINDINGSSUBS")
                .FirstOrDefault();

            _etiologic = _protocolVariables
                .Where(item => item.Name == "ETIOLOGIC")
                .FirstOrDefault();
        }

        // GET: ClinicianDiagnosis
        public async Task<IActionResult> Index()
        {
            var udsContext = _context.ClinicianDiagnoses.Include(c => c.Visit);
            return View(await udsContext.ToListAsync());
        }

        // GET: ClinicianDiagnosis/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var clinicianDiagnosis = await _context.ClinicianDiagnoses
                .Include(c => c.Visit)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (clinicianDiagnosis == null)
            {
                return NotFound();
            }

            return View(clinicianDiagnosis);
        }

        // GET: ClinicianDiagnosis/Create
        public async Task<IActionResult> Create(int id)
        {
            var clinicianDiagnosis = await _context.ClinicianDiagnoses.FindAsync(id);
            if (clinicianDiagnosis == null)
            {
                clinicianDiagnosis = new ClinicianDiagnosis
                {
                    Id = id,
                    FormStatus = FormStatus.Incomplete
                };
                _context.Add(clinicianDiagnosis);
                await _context.SaveChangesAsync(HttpContext.User.Identity.Name);
            }
            return RedirectToAction("Edit", new { id = clinicianDiagnosis.Id });
        }

        // POST: ClinicianDiagnosis/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("DiagnosisMethod,HasNormalCognition,MeetsCriteriaForDementia,AmnesticMultiDomainPresent,PosteriorCorticalAtrophyPresent,PrimaryProgressiveAphasiaPresent,PrimaryProgressiveAphasiaType,BehavioralVariantFTDPresent,LewyBodyPresent,NonAmnesticMultiDomainPresent,AmnesticMCISingleDomainPresent,AmnesticMCIMultipleDomains,AmnesticMCIMultipleDomainsLanguage,AmnesticMCIMultipleDomainsAttention,AmnesticMCIMultipleDomainsExecutive,AmnesticMCIMultipleDomainsVisuospatial,NonAmnesticMCISingleDomain,NonAmnesticMCISingleDomainLanguage,NonAmnesticMCISingleDomainAttention,NonAmnesticMCISingleDomainExecutive,NonAmnesticMCISingleDomainVisuospatial,NonAmnesticMCIMultipleDomains,NonAmnesticMCIMultipleDomainsLanguage,NonAmnesticMCIMultipleDomainsAttention,NonAmnesticMCIMultipleDomainsExecutive,NonAmnesticMCIMultipleDomainsVisuospatial,CognitivelyImpairedNotMCIPresent,AbnormallyElevatedAmyloidInPET,AbnormallyLowAmyloidInCSF,FDGPETPattern,HippocampalAtrophy,TauPETEvidenceForAD,AbnormallyElevatedCSFTauOrPtau,FDGPETEvidenceForFrontalOrAnteriorTemporalHypometabolism,TauPETEvidence,StructuralMREvidence,DopamineTransporterScanEvidence,OtherEvidenceExists,OtherEvidenceSpecified,LargeVesselInfarcts,LacunarInfarcts,Macrohemorrhages,Microhemorrhages,ModerateWhiteMatterHyperintensity,ExtensiveWhiteMatterHyperintensity,DominantlyInheritedADMutation,HereditaryFTLDMutation,HereditaryMutationOther,HereditaryMutationOtherSpecified,AlzheimersDiseasePresent,AlzheimersDiseaseDiagnosis,LewyBodyDiseasePresent,LewyBodyDiseaseDiagnosis,ParkinsonsDiseasePresent,MultipleSystemAtrophyPresent,MultipleSystemAtrophyDiagnosis,ProgressiveSupranuclearPaslyPresent,ProgressiveSupranuclearPaslyDiagnosis,CorticobasalDegenerationPresent,CorticobasalDegenerationDiagnosis,FTLDWithMotorNeuronDiseasePresent,FTLDWithMotorNeuronDiseaseDiagnosis,FTLDNOSPresent,FTLDNOSDiagnosis,FTLDSubtype,FTLDSubtypeOtherSpecified,VascularBrainInjuryPresent,VascularBrainInjuryDiagnosis,PreviousSymptomaticStroke,TemporalRelationshipBetweenStrokeAndCognitive,ConfirmationOfStrokeByNeuroImaging,ImagingEvidenceOfCysticInfarctionInCognitiveNetwork,ImagingEvidenceOfCysticInfarctionExtensiveWhiteMatterHyperintensistyAndExecutiveImpairment,EssentialTremorPresent,EssentialTremorDiagnosis,DownSyndromePresent,DownSyndromeDiagnosis,HuntingtonsDiseasePresent,HuntingtonsDiseaseDiagnosis,PrionDiseasePresent,PrionDiseaseDiagnosis,TraumaticBrainInjuryPresent,TraumaticBrainInjuryDiagnosis,ChronicTraumaticEncephalopathySymptom,NormalPressureHydrocephalusPresent,NormalPressureHydropcephalusDiagnosis,EpilepsyPresent,EpilepsyDiagnosis,CNSNeoplasmPresent,CNSNeoplasmDiagnosis,CNSNeoplasmType,HumanImmunodeficiencyVirusPresent,HumanImmunodeficiencyVirusDiagnosis,OtherCognitiveImpairmentPresent,OtherCognitiveImpairmentDiagnosis,OtherCognitiveImpairmentSpecified,ActiveDepressionPresent,ActiveDepressionDiagnosis,ActiveDepressionTreatment,BipolarDisorderPresent,BipolarDisorderDiagnosis,SchizophreniaPresent,SchizophreniaDiagnosis,AnxietyDisorderPresent,AnxietyDiagnosis,DeliriumPresent,DeliriumDiagnosis,PostTraumaticStressDisorderPresent,PostTraumaticStressDisorderDiagnosis,OtherPsychiatricDiseasePresent,OtherPsychiatricDiseaseDiagnosis,OtherPsychiatricDiseaseSpecified,AlcoholAbuse,AlcoholAbuseDiagnosis,CurrentAlcoholAbuse,SubstanceAbuse,SubstanceAbuseDiagnosis,SystemicDisease,SystemicDiseaseDiagnosis,Medications,MedicationsDiagnosis,NOS1,NOS1Diagnosis,NOS1Specified,NOS2,NOS2Diagnosis,NOS2Specified,NOS3,NOS3Diagnosis,NOS3Specified,Id,ExaminerInitials,FormStatus")] ClinicianDiagnosis clinicianDiagnosis)
        {
            if (ModelState.IsValid)
            {
                _context.Add(clinicianDiagnosis);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(clinicianDiagnosis);
        }

        // GET: ClinicianDiagnosis/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var clinicianDiagnosis = await _context.ClinicianDiagnoses
                .Include(c => c.Visit)
                    .ThenInclude(v => v.Participant)
                .AsNoTracking()
                .FirstOrDefaultAsync(c => c.Id == id);

            if (clinicianDiagnosis == null)
            {
                return NotFound();
            }

            var participantIdentity = await _participantsService.GetParticipantAsync(clinicianDiagnosis.Visit.Participant.Id);
            clinicianDiagnosis.Visit.Participant.Profile = participantIdentity;

            ViewBag.Findings = _findings;
            ViewBag.FindingsSubs = _findingsSubs;
            ViewBag.Etiologic = _etiologic;

            return View(clinicianDiagnosis);
        }

        // POST: ClinicianDiagnosis/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("DiagnosisMethod,HasNormalCognition,MeetsCriteriaForDementia,AmnesticMultiDomainPresent,PosteriorCorticalAtrophyPresent,PrimaryProgressiveAphasiaPresent,PrimaryProgressiveAphasiaType,BehavioralVariantFTDPresent,LewyBodyPresent,NonAmnesticMultiDomainPresent,AmnesticMCISingleDomainPresent,AmnesticMCIMultipleDomains,AmnesticMCIMultipleDomainsLanguage,AmnesticMCIMultipleDomainsAttention,AmnesticMCIMultipleDomainsExecutive,AmnesticMCIMultipleDomainsVisuospatial,NonAmnesticMCISingleDomain,NonAmnesticMCISingleDomainLanguage,NonAmnesticMCISingleDomainAttention,NonAmnesticMCISingleDomainExecutive,NonAmnesticMCISingleDomainVisuospatial,NonAmnesticMCIMultipleDomains,NonAmnesticMCIMultipleDomainsLanguage,NonAmnesticMCIMultipleDomainsAttention,NonAmnesticMCIMultipleDomainsExecutive,NonAmnesticMCIMultipleDomainsVisuospatial,CognitivelyImpairedNotMCIPresent,AbnormallyElevatedAmyloidInPET,AbnormallyLowAmyloidInCSF,FDGPETPattern,HippocampalAtrophy,TauPETEvidenceForAD,AbnormallyElevatedCSFTauOrPtau,FDGPETEvidenceForFrontalOrAnteriorTemporalHypometabolism,TauPETEvidence,StructuralMREvidence,DopamineTransporterScanEvidence,OtherEvidenceExists,OtherEvidenceSpecified,LargeVesselInfarcts,LacunarInfarcts,Macrohemorrhages,Microhemorrhages,ModerateWhiteMatterHyperintensity,ExtensiveWhiteMatterHyperintensity,DominantlyInheritedADMutation,HereditaryFTLDMutation,HereditaryMutationOther,HereditaryMutationOtherSpecified,AlzheimersDiseasePresent,AlzheimersDiseaseDiagnosis,LewyBodyDiseasePresent,LewyBodyDiseaseDiagnosis,ParkinsonsDiseasePresent,MultipleSystemAtrophyPresent,MultipleSystemAtrophyDiagnosis,ProgressiveSupranuclearPaslyPresent,ProgressiveSupranuclearPaslyDiagnosis,CorticobasalDegenerationPresent,CorticobasalDegenerationDiagnosis,FTLDWithMotorNeuronDiseasePresent,FTLDWithMotorNeuronDiseaseDiagnosis,FTLDNOSPresent,FTLDNOSDiagnosis,FTLDSubtype,FTLDSubtypeOtherSpecified,VascularBrainInjuryPresent,VascularBrainInjuryDiagnosis,PreviousSymptomaticStroke,TemporalRelationshipBetweenStrokeAndCognitive,ConfirmationOfStrokeByNeuroImaging,ImagingEvidenceOfCysticInfarctionInCognitiveNetwork,ImagingEvidenceOfCysticInfarctionExtensiveWhiteMatterHyperintensistyAndExecutiveImpairment,EssentialTremorPresent,EssentialTremorDiagnosis,DownSyndromePresent,DownSyndromeDiagnosis,HuntingtonsDiseasePresent,HuntingtonsDiseaseDiagnosis,PrionDiseasePresent,PrionDiseaseDiagnosis,TraumaticBrainInjuryPresent,TraumaticBrainInjuryDiagnosis,ChronicTraumaticEncephalopathySymptom,NormalPressureHydrocephalusPresent,NormalPressureHydropcephalusDiagnosis,EpilepsyPresent,EpilepsyDiagnosis,CNSNeoplasmPresent,CNSNeoplasmDiagnosis,CNSNeoplasmType,HumanImmunodeficiencyVirusPresent,HumanImmunodeficiencyVirusDiagnosis,OtherCognitiveImpairmentPresent,OtherCognitiveImpairmentDiagnosis,OtherCognitiveImpairmentSpecified,ActiveDepressionPresent,ActiveDepressionDiagnosis,ActiveDepressionTreatment,BipolarDisorderPresent,BipolarDisorderDiagnosis,SchizophreniaPresent,SchizophreniaDiagnosis,AnxietyDisorderPresent,AnxietyDiagnosis,DeliriumPresent,DeliriumDiagnosis,PostTraumaticStressDisorderPresent,PostTraumaticStressDisorderDiagnosis,OtherPsychiatricDiseasePresent,OtherPsychiatricDiseaseDiagnosis,OtherPsychiatricDiseaseSpecified,AlcoholAbuse,AlcoholAbuseDiagnosis,CurrentAlcoholAbuse,SubstanceAbuse,SubstanceAbuseDiagnosis,SystemicDisease,SystemicDiseaseDiagnosis,Medications,MedicationsDiagnosis,NOS1,NOS1Diagnosis,NOS1Specified,NOS2,NOS2Diagnosis,NOS2Specified,NOS3,NOS3Diagnosis,NOS3Specified,Id,ExaminerInitials,FormStatus")] ClinicianDiagnosis clinicianDiagnosis, string save, string complete)
        {
            if (id != clinicianDiagnosis.Id)
            {
                return NotFound();
            }

            var visit = await _context.Visits
                .AsNoTracking()
                .Include("Participant")
                .FirstOrDefaultAsync(v => v.Id == clinicianDiagnosis.Id);

            clinicianDiagnosis.Visit = visit;

            var participantIdentity = await _participantsService.GetParticipantAsync(clinicianDiagnosis.Visit.Participant.Id);
            clinicianDiagnosis.Visit.Participant.Profile = participantIdentity;

            ViewBag.Findings = _findings;
            ViewBag.FindingsSubs = _findingsSubs;
            ViewBag.Etiologic = _etiologic;

            if (!String.IsNullOrEmpty(save))
            {
                clinicianDiagnosis.FormStatus = FormStatus.Incomplete;
            }
            else if (!String.IsNullOrEmpty(complete))
            {
                clinicianDiagnosis.FormStatus = FormStatus.Complete;
                if (!TryValidateModel(clinicianDiagnosis))
                {
                    return View(clinicianDiagnosis);
                }
            }
            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(clinicianDiagnosis);
                    await _context.SaveChangesAsync(HttpContext.User.Identity.Name);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ClinicianDiagnosisExists(clinicianDiagnosis.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction("Details", "Visit", new { id = clinicianDiagnosis.Id });
            }
            return View(clinicianDiagnosis);
        }

        // GET: ClinicianDiagnosis/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var clinicianDiagnosis = await _context.ClinicianDiagnoses
                .Include(c => c.Visit)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (clinicianDiagnosis == null)
            {
                return NotFound();
            }

            return View(clinicianDiagnosis);
        }

        // POST: ClinicianDiagnosis/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var clinicianDiagnosis = await _context.ClinicianDiagnoses.FindAsync(id);
            _context.ClinicianDiagnoses.Remove(clinicianDiagnosis);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ClinicianDiagnosisExists(int id)
        {
            return _context.ClinicianDiagnoses.Any(e => e.Id == id);
        }
    }
}
