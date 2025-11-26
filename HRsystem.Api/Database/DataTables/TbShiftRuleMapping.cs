using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HRsystem.Api.Database.DataTables;

[Table("Tb_Shift_Rule_Mapping")]
public partial class TbShiftRuleMappng
{
    [Key]
    public int RuleMappingId { get; set; }

    public int ShiftRuleId { get; set; }
    public int ShiftId { get; set; }

    public virtual TbShift Shift { get; set; } = null!;
    public virtual TbShiftRule ShiftRule { get; set; } = null!;
       
     

}