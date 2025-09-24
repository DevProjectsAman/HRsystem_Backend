using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HRsystem.Api.Database.DataTables
{
    [Table("Tb_Remote_Work_Days")]
    public class TbRemoteWorkDays
    {
        [Key]
        public int RemoteWorkDaysId { get; set; }

        [Column(TypeName = "json")]
        public List<string>? RemoteWorkDaysNames { get; set; } = [];

        public int? CreatedBy { get; set; }

        public DateTime? CreatedAt { get; set; }
    }
}