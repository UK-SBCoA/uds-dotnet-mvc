using System;
using System.Collections.Generic;
using UDS.Net.Data.Entities;

namespace UDS.Net.Web.ViewModels
{
    public class CurrentVisits
    {
        public IEnumerable<Visit> InProgress { get; set; }
    }
}
