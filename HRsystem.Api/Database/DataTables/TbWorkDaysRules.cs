using HRsystem.Api.Shared.DTO;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace HRsystem.Api.Database.DataTables
{
    [Table("Tb_WorkDays_Rules")]
    public class TbWorkDaysRules
    {

        [Key]
        public int WorkDaysRuleId { get; set; }
        public int? GovID { get; set; }
        public int? CityID { get; set; }

        public int? JobTitleId { get; set; }

        public int? WorkingLocationId { get; set; }

        public int? ProjectId { get; set; }

        public int   WorkDaysId { get; set; }
        public int? Priority { get; set; }

        public int CompanyId { get; set; }

        public int? CreatedBy { get; set; }

        public DateTime? CreatedAt { get; set; }

    }
}
