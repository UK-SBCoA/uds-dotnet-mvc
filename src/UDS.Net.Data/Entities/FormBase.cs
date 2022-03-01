using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using UDS.Net.Data.Enums;

namespace UDS.Net.Data.Entities
{
    public class FormBase
    {
        [ForeignKey("Id")]
        public virtual Visit Visit { get; set; }

        [Key]
        [Column("PacketId")]
        public int Id { get; set; }

        [MaxLength(3)]
        [Column("INITIALS")]
        public string ExaminerInitials { get; set; }

        public FormStatus FormStatus { get; set; } = FormStatus.Incomplete;

        [MaxLength(6)]
        [Column("FORMVER")]
        public string Version { get; set; } = "3.0.0";
        /// <summary>
        /// User who modified form
        /// </summary>
        public string ModifiedBy {get;set;}

        /// <summary>
        /// Initilize form with visit ID, will set form status to In Progress
        /// </summary>
        /// <param name="id"></param>
        public void InitializeForm(int id) {
            this.Id = id;
            this.FormStatus = FormStatus.Incomplete;
        }

    }


}
