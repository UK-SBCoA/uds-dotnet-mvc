using System;
using UDS.Net.Data.Entities;

namespace UDS.Net.Web.ViewModels
{
    public class SearchParticipation
    {
        public int SearchTerm { get; set; }
        public Participation Result { get; set; }
    }
}
