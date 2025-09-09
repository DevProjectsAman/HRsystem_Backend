using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace HRsystem.Api.Database.DataTables
{
    [Table("Tb_Marital_Status")]
    public class TbMaritalStatus
    {
        [Key]
        public int MaritalStatusId { get; set; }

        [MaxLength(50)]
        public string NameEn { get; set; } = null!;

        [MaxLength(50)]
        public string? NameAr { get; set; }   // Optional Arabic name
    }

}
