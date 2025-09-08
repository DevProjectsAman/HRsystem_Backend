using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HRsystem.Api.Database.DataTables;

[Table("Tb_Vacation_Type")]
public partial class TbVacationType
{
    [Key]
    public int VacationTypeId { get; set; }

    public string VacationName { get; set; } = null!;

    public string? Description { get; set; }

    public bool? IsPaid { get; set; }

    public bool? RequiresHrApproval { get; set; }

    public virtual ICollection<TbEmployeeVacationBalance> TbEmployeeVacationBalances { get; set; } = new List<TbEmployeeVacationBalance>();

    public virtual ICollection<TbEmployeeVacation> TbEmployeeVacations { get; set; } = new List<TbEmployeeVacation>();

    public virtual ICollection<TbVacationRule> TbVacationRules { get; set; } = new List<TbVacationRule>();
}
