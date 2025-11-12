using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HRsystem.Api.Database.DataTables;

[Table("Tb_Shift_Rule")]
public partial class TbShiftRule
{
    [Key]
    public int RuleId { get; set; }

    [MaxLength(100)]
    public string? ShiftRuleName { get; set; } = string.Empty;
    public int? GovID { get; set; }
    public int? CityID { get; set; }

    public int? JobLevelId { get; set; }

    public int? DepartmentId { get; set; }
    public int? JobTitleId { get; set; }

    public int? WorkingLocationId { get; set; }

    public int? ProjectId { get; set; }

    public int ShiftId { get; set; }

    public int? Priority { get; set; }

    public int CompanyId { get; set; }

    public int? CreatedBy { get; set; }

    public DateTime? CreatedAt { get; set; }

    public int? UpdatedBy { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public virtual TbCompany Company { get; set; } = null!;

    public virtual TbJobLevel? JobLevel { get; set; }
   
    public virtual TbJobTitle? JobTitle { get; set; }

    public virtual TbProject? Project { get; set; }

    public virtual TbShift Shift { get; set; } = null!;

    public virtual TbWorkLocation? WorkingLocation { get; set; }


    // ✅ Add these
    [ForeignKey("GovID")]
    public virtual TbGov? Gov { get; set; }

    [ForeignKey("CityID")]
    public virtual TbCity? City { get; set; }


    [ForeignKey("DepartmentId")]
    public virtual TbDepartment? Department { get; set; }

}