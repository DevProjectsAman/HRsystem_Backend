using HRsystem.Api.Shared.DTO;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace HRsystem.Api.Database.DataTables
{
    [Table("Tb_WorkDays")]
    public class TbWorkDays
    {
     
        [Key]
        public int WorkDaysId { get; set; }

        [MaxLength(100)]
        public string WorkDaysDescription { get; set; }

        [MaxLength(100)]
        public List<string> WorkDaysDescriptions {  get; set; } = [];


    }
}
