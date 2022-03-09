using System;
using UDS.Net.Data.Entities;

namespace UDS.Net.Web.ViewModels
{
    public class FormStatusViewModel
    {
        public FormBase Form { get; set; }

        public string ControllerName { get; set; }

        public Checklist Checklist { get; set; } // checklist will show whether the form is included or excluded
    }
}
