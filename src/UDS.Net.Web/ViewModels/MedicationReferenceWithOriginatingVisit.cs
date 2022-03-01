using System;
using UDS.Net.Data.Entities;

namespace UDS.Net.Web.ViewModels
{
    public class MedicationReferenceWithOriginatingVisit
    {
        public MedicationReference MedicationReference { get; set; }
        public int? VisitId { get; set; }
    }
}
