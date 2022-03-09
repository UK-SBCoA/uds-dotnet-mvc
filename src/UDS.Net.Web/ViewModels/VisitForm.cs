using System;
using UDS.Net.Data.Entities;
using UDS.Net.Data.Enums;

namespace UDS.Net.Web.ViewModels
{
    public class VisitForm
    {
        public int VisitId { get; set; }

        public VisitStatus VisitStatus { get; set; }

        public FormBase FormBase { get; set; }

        public string FormDisplayName { get; set; }

        public string ControllerName { get; set; }

        public string ReturnUrl { get; set; }

        public bool Disabled { get; set; } = false;

        public Checklist Checklist { get; set; }
    }
}
