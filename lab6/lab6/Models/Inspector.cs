using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace lab6.Models
{
    public class Inspector
    {
        [Key]
        [Display(Name = "Код Инспектора")]
        public int InspectorId { get; set; }

        
        [Display(Name = "Инспектор")]
        public string FullName { get; set; } = null!;

        
        [Display(Name = "Департамент")]
        public string Department { get; set; } = null!;

        [JsonIgnore]
        public virtual ICollection<Inspection> Inspections { get; set; } = new List<Inspection>();
    }
}
