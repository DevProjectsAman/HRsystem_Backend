using Microsoft.AspNetCore.Mvc.ModelBinding;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using static HRsystem.Api.Enums.EnumsList;

namespace HRsystem.Api.Database.DataTables;

[Table("Tb_Vacation_Rule")]
public partial class TbVacationRule
{
    [Key]
    public int RuleId { get; set; }

    [MaxLength(100)]
    public string RuleName { get; set; }

    public int VacationTypeId { get; set; }

    public int? MinAge { get; set; }

    public int? MaxAge { get; set; }

    public int? MinServiceYears { get; set; }

    public int? MaxServiceYears { get; set; }

    public EnumGenderType Gender { get; set; } = EnumGenderType.Male;

    public EnumReligionType Religion { get; set; } = EnumReligionType.All;

    public int YearlyBalance { get; set; }

    public bool? Prorate { get; set; }

    public virtual TbVacationType VacationType { get; set; } = null!;
}
