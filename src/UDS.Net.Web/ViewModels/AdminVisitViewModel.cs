using System;
using System.Collections.Generic;
using UDS.Net.Data.Entities;
using UDS.Net.Data.Enums;

namespace UDS.Net.Web.ViewModels
{
    public class AdminVisitViewModel
    {
        public VisitStatus? VisitStatus { get; set; }
        public int? FriendlyId { get; set; }
        public bool? IsSubmittedToNACC { get; set; }
        public List<Visit> Visits { get; set; }
        public bool HasNextPage { get; set; }
        public bool HasPreviousPage { get; set; }
        public int PageSize { get; set; }
        public int PageNumber { get; set; }
    }
}
