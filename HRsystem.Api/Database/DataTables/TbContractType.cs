using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HRsystem.Api.Database.DataTables;

[Table("Tb_Contract_Type")]
public partial class TbContractType
{
    [Key]
    public int ContractTypeId { get; set; }

    [MaxLength(25)]
    public string? ContractTypeCode { get; set; }
    [MaxLength(60)]
    public string ContractTypeName { get; set; }
    
}
