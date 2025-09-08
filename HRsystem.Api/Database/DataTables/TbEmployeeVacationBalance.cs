using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HRsystem.Api.Database.DataTables;

[Table("Tb_Employee_Vacation_Balance")]
public partial class TbEmployeeVacationBalance
{
    [Key]
    public int BalanceId { get; set; }

    public int EmployeeId { get; set; }

    public int VacationTypeId { get; set; }

    public int Year { get; set; }

    public decimal TotalDays { get; set; }

    public decimal? UsedDays { get; set; }

    public decimal? RemainingDays { get; set; }

    public virtual TbEmployee Employee { get; set; } = null!;

    public virtual TbVacationType VacationType { get; set; } = null!;
}
