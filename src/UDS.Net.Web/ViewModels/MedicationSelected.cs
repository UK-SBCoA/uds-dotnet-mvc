using System;
using UDS.Net.Data.Entities;

namespace UDS.Net.Web.ViewModels
{
    public class MedicationSelected
    {
        public int VisitId { get; set; }

        public MedicationReference MedicationReference { get; set; }

        public MedicationCurrent MedicationCurrent { get; set; }

        public bool Selected { get; set; }
    }
}
