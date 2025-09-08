using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HRsystem.Api.Database.DataTables;

[Table("Tb_Employee_Vacation")]
public partial class TbEmployeeVacation
{
    [Key]
    public long VacationId { get; set; }

    public long ActivityId { get; set; }

    public DateOnly StartDate { get; set; }

    public DateOnly EndDate { get; set; }

    public int VacationTypeId { get; set; }

    public string? Notes { get; set; }

    public int? DaysCount { get; set; }

    public virtual TbEmployeeActivity Activity { get; set; } = null!;

    public virtual TbVacationType VacationType { get; set; } = null!;
}
