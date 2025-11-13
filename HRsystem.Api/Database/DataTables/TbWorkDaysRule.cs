using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HRsystem.Api.Database.DataTables
{
    [Table("Tb_WorkDays_Rules")]
    public class TbWorkDaysRule
    {
        [Key]
        public int WorkDaysRuleId { get; set; }

        [MaxLength(100)]
        public string? WorkDaysRuleName { get; set; } = string.Empty;

        public int? GovID { get; set; }
        public int? CityID { get; set; }

        public int? DepartmentId { get; set; }
        public int? JobLevelId { get; set; }
        public int? JobTitleId { get; set; }

        public int? WorkingLocationId { get; set; }

        public int? ProjectId { get; set; }

        public int WorkDaysId { get; set; }
        public int? Priority { get; set; }

        public int CompanyId { get; set; }

        public int? CreatedBy { get; set; }

        public DateTime? CreatedAt { get; set; }
        

        public virtual TbJobLevel? JobLevel { get; set; }

        public virtual TbJobTitle? JobTitle { get; set; }

        public virtual TbProject? Project { get; set; }
        public virtual TbWorkDays? WorkDays { get; set; }

     

        public virtual TbWorkLocation? WorkingLocation { get; set; }


        // ✅ Add these
        [ForeignKey("GovID")]
        public virtual TbGov? Gov { get; set; }

        [ForeignKey("CityID")]
        public virtual TbCity? City { get; set; }


        [ForeignKey("DepartmentId")]
        public virtual TbDepartment? Department { get; set; }
    }
}