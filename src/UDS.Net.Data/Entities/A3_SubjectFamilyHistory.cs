using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using UDS.Net.Data.DataAnnotations;
using UDS.Net.Data.Enums;

namespace UDS.Net.Data.Entities
{
    [Table("tbl_A3")]
    public class SubjectFamilyHistory: FormBase
    {
        [Column("AFFFAMM")]
        [RequiredIf(nameof(IvpComplete), true, "Please indicate if there affected first-degree relatives")]
        public int? AffectedRelatives { get; set; }
        [Column("NWINFMUT")]
        [RequiredIf(nameof(FvpComplete), true, "Please indicate if there has been new information since the last visit")]
        public int? NewInfo {get;set;}
        [Column("FADMUT")]
        [RequiredIf(nameof(IvpComplete), true, "Please indicate if there is evidence of an AD Mutation")]
        [RequiredIf(nameof(NewInfo), true, "Please indicate if there is evidence of an AD Mutation")]
        public int? AD_Evidence { get; set; }
        [Column("FADMUTX")]
        [RequiredIf(nameof(AD_Evidence), 8, "Please specify evidence for the AD Mutation")]
        public string AD_Evidence_Specify {get;set;}
        [Column("FADMUSO")]
        [RequiredIf(nameof(AD_Evidence), 1, "Please indicate a value for the source of the AD mutation")]
        [RequiredIf(nameof(AD_Evidence), 2, "Please indicate a value for the source of the AD mutation")]
        [RequiredIf(nameof(AD_Evidence), 3, "Please indicate a value for the source of the AD mutation")]
        [RequiredIf(nameof(AD_Evidence), 8, "Please indicate a value for the source of the AD mutation")]
        public int? AD_Source { get; set; }
        [Column("FADMUSOX")]
        [RequiredIf(nameof(AD_Source), 8, "Please specify a source for the AD mutation")]
        public string AD_Source_Specify {get;set;}
        [Column("FFTDMUT")]
        [RequiredIf(nameof(IvpComplete), true, "Please indicate if there is evidence of an FTLD Mutation")]
        [RequiredIf(nameof(NewInfo), true, "Please indicate if there is evidence of an FTLD Mutation")]
        public int? FTLD_Evidence { get; set; }
        [Column("FFTDMUTX")]
        [RequiredIf(nameof(FTLD_Evidence), 8, "Please specify evidence for the FTLD Mutation")]
        public string FTLD_Evidence_Specify {get;set;}
        [Column("FFTDMUSO")]
        [RequiredIf(nameof(FTLD_Evidence), 1, "Please indicate a value for the source of the FTLD mutation")]
        [RequiredIf(nameof(FTLD_Evidence), 2, "Please indicate a value for the source of the FTLD mutation")]
        [RequiredIf(nameof(FTLD_Evidence), 3, "Please indicate a value for the source of the FTLD mutation")]
        [RequiredIf(nameof(FTLD_Evidence), 8, "Please indicate a value for the source of the FTLD mutation")]
        public int? FTLD_Source { get; set; }
        [Column("FFTDMUSX")]
        [RequiredIf(nameof(FTLD_Source), 8, "Please specify a source for the Other Mutation")]
        public string FTLD_Source_Specify {get;set;}
        [Column("FOTHMUT")]
        [RequiredIf(nameof(IvpComplete), true, "Please indicate if there is evidence of an Other Mutation")]
        [RequiredIf(nameof(NewInfo), true, "Please indicate if there is evidence of an Other Mutation")]
        public int? Other_Evidence { get; set; }
        [Column("FOTHMUTX")]
        [RequiredIf(nameof(Other_Evidence), 1, "Please specify evidence for the Other Mutation")]
        public string Other_Evidence_Specify {get;set;}
        [Column("FOTHMUSO")]
        [RequiredIf(nameof(Other_Evidence), 1, "Please indicate a value for the source of the Other mutation")]
        [RequiredIf(nameof(Other_Evidence), 2, "Please indicate a value for the source of the Other mutation")]
        [RequiredIf(nameof(Other_Evidence), 3, "Please indicate a value for the source of the Other mutation")]
        [RequiredIf(nameof(Other_Evidence), 8, "Please indicate a value for the source of the Other mutation")]
        public int? Other_Source { get; set; }
        [Column("FOTHMUSX")]
        [RequiredIf(nameof(Other_Source), 8, "Please specify a source for the Other Mutation")]
        public string Other_Source_Specify {get;set;}
        [Column("NWINFPAR")]
        [RequiredIf(nameof(FvpComplete), true, "Please indicate if there has been a change in mother or father information")]
        public int? ParentChange { get; set; }
        [Column("SIBS")]
        [RequiredIf(nameof(IvpComplete), true, "Please indicate number of siblings 0 - 20")]
        public int? SiblingNumber { get; set; }
        [Column("NWINFSIB")]
        [RequiredIf(nameof(FvpComplete), true, "Please indicate if there has been a change in sibling information")]
        public int? SiblingChange { get; set; }
        [Column("KIDS")]
        [RequiredIf(nameof(IvpComplete), true, "Please indicate number of children 0 - 15")]
        public int? ChildrenNumber { get; set; }
        [Column("NWINFKID")]
        [RequiredIf(nameof(FvpComplete), true, "Please indicate if there has been a change in child information")]
        [RequiredIfRange(nameof(ChildrenChange), 1, 15)]
        public int? ChildrenChange { get; set; }
        public ICollection<Relative> Relatives { get; set; }

        [NotMapped]
        public bool IvpComplete
        {
            get {
                if (FormStatus == FormStatus.Complete && Visit.VisitType == VisitType.IVP) {
                    return true;
                }
                return false;

            }

        }
        [NotMapped]
        public bool FvpComplete {
            get
            {
                if (FormStatus == FormStatus.Complete && Visit.VisitType == VisitType.FVP)
                {
                    return true;
                }
                return false;

            }
        }
    }
}
