using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HRsystem.Api.Database.DataTables;

[Table("Tb_Gov")]
public partial class TbGov
{
    [Key]
    public int GovId { get; set; }

    [MaxLength(25)]
    public string? GoveCode { get; set; }
    [MaxLength(60)]
    public string? GovName { get; set; }
    [MaxLength(100)]
    public string? GovArea { get; set; }

    public virtual ICollection<TbCity> TbCities { get; set; } = new List<TbCity>();
}
