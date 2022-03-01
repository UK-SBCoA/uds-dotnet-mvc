using System;
using System.ComponentModel.DataAnnotations;

namespace UDS.Net.Data.Enums
{
    public enum VisitStatus
    {
        [Display(Name = "In-progress")]
        InProgress,
        [Display(Name = "Awaiting consensus")]
        AwaitingConsensus,
        Prioritized,
        Tabled,
        Complete
    }
}
