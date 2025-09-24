using HRsystem.Api.Shared.DTO;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HRsystem.Api.Database.DataTables;

[Table("Tb_Shift")]
public partial class TbShift
{
    [Key]
    public int ShiftId { get; set; }

    //[MaxLength(55)]
    public LocalizedData ShiftName { get; set; } = null!;

    public TimeOnly StartTime { get; set; }

    public TimeOnly EndTime { get; set; }

    public bool IsFlexible { get; set; }

    public TimeOnly? MinStartTime { get; set; }

    public TimeOnly? MaxStartTime { get; set; }

    public int GracePeriodMinutes { get; set; }

    [Precision(5, 2)]  // EF Core 6+
    public decimal? RequiredWorkingHours { get; set; }

    [MaxLength(255)]
    public string? Notes { get; set; }

    public int CompanyId { get; set; }

    public int? CreatedBy { get; set; }

    public DateTime? CreatedAt { get; set; }

    public int? UpdatedBy { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public virtual TbCompany Company { get; set; } = null!;

    public virtual ICollection<TbEmployeeShift> TbEmployeeShifts { get; set; } = new List<TbEmployeeShift>();

    public virtual ICollection<TbShiftRule> TbShiftRules { get; set; } = new List<TbShiftRule>();
}