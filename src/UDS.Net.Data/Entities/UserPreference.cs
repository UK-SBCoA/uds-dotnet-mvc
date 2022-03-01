using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using UDS.Net.Data.Enums;

namespace UDS.Net.Data.Entities
{
    [Table("tbl_UserPreferences")]
    public class UserPreference
    {
        [Key]
        public int Id { get; set; }
        public string Username { get; set; }
        public UserPreferenceOptions Preference { get; set; }
        public string Value { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public DateTime? UpdatedAt { get; set; }
    }
}
// dotnet ef migrations add InitializeUserDatabase --startup-project ../UDS.Net.Web/UDS.Net.Web.csproj --Context UserContext