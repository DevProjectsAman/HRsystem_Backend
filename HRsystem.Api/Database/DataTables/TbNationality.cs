using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace HRsystem.Api.Database.DataTables
{
    [Table("Tb_Nationality")]
    public class TbNationality
    {
        [Key]
        public int NationalityId { get; set; }

        [MaxLength(100)]
        public string NameEn { get; set; } = null!;

        [MaxLength(100)]
        public string? NameAr { get; set; }   // Optional Arabic name
    }

}
