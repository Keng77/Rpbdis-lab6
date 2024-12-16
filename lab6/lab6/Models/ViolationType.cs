using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace lab6.Models
{
    public partial class ViolationType
    {
        [Key]
        [Display(Name = "Код Нарушения")]
        public int ViolationTypeId { get; set; }

        
        [Display(Name = "Тип нарушения")]
        public string Name { get; set; } = null!;

        
        [Display(Name = "Задолженность")]
        public decimal PenaltyAmount { get; set; }

        
        [Display(Name = "Дедлайн исправления")]
        public int CorrectionPeriodDays { get; set; }

        [JsonIgnore]
        public virtual ICollection<Inspection> Inspections { get; set; } = new List<Inspection>();
    }
}
