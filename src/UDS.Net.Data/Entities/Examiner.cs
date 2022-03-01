using System;
using System.ComponentModel.DataAnnotations;

namespace UDS.Net.Data.Entities
{
    public class Examiner
    {
        [Key]
        [MaxLength(3)]
        public string Initials { get; set; }

        /// <summary>
        /// First name and last name
        /// </summary>
        [MaxLength(200)]
        public string Name { get; set; }

        /// <summary>
        /// Official username (such as User.Identity.Name)
        /// </summary>
        [MaxLength(200)]
        public string Username { get; set; }

    }
}
