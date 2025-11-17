using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HRsystem.Api.Database.DataTables
{
 
    [Table("Tb_Vacation_Rules_Group")]
    public class TbVacationRulesGroup
    {
        [Key]
        public int GroupId { get; set; }

        [Required]
        public int CompanyId { get; set; }

        [MaxLength(150)]
        public string? GroupName { get; set; } // e.g. "Senior Employee Rules"

        // 🧍 Employee Filters
        public int? MinAge { get; set; }
        public int? MaxAge { get; set; }
        public int? MinServiceYears { get; set; }
        public int? MaxServiceYears { get; set; }
        public int? WorkingYearsAtCompany { get; set; }

        // Navigation
        public virtual ICollection<TbVacationRulesGroupDetail> VacationRuleDetails { get; set; } = new List<TbVacationRulesGroupDetail>();
    }



}
