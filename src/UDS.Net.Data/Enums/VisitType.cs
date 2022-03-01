using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace UDS.Net.Data.Enums
{
    public enum VisitType
    {
        [Display(Name = "In-person initial")]
        IVP,
        [Display(Name = "In-person follow-up")]
        FVP,
        [Display(Name = "Telephone follow-up")]
        TFP,
        [Display(Name = "Preliminary visit")]
        PRE
    }
}
