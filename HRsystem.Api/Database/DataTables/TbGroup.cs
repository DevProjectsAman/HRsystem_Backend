using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HRsystem.Api.Database.DataTables;
[Table("Tb_Group")]
public partial class TbGroup
{
    [Key]
    public int GroupId { get; set; }

    public string GroupName { get; set; } = null!;

    public virtual ICollection<TbCompany> TbCompanies { get; set; } = new List<TbCompany>();
}
