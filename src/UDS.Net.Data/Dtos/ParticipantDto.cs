using System;

namespace UDS.Net.Data.Dtos
{
    public class ParticipantDto
    {
        public int Id { get; set; }
        public string PreferredName { get; set; }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public string DisplayName
        {
            get
            {
                return String.IsNullOrWhiteSpace(PreferredName) ? FirstName + " " + LastName : PreferredName + " " + LastName;
            }
        }
        public DateTime? DateOfBirth { get; set; }
    }
}
