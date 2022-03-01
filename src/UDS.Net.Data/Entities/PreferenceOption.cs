using System.ComponentModel.DataAnnotations;

namespace UDS.Net.Data.Entities
{
    public class PreferenceOption
    {
        [Key]
        public int Id { get; set; }
        public string Option { get; set; }
    }
}