using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HRsystem.Api.Database.DataTables
{
    [Table("tb_activity_type_status")]
    public class TbActivityTypeStatus
    {
        [Key]
        public int ActivityTypeStatusId { get; set; }

        [Required]
        public int ActivityTypeId { get; set; }

        [Required]
        public int StatusId { get; set; }

        public bool IsDefault { get; set; } = false;

        public bool IsActive { get; set; } = true;

        [Required]
        public int CompanyId { get; set; }

        // Navigation properties
        [ForeignKey(nameof(ActivityTypeId))]
        public virtual TbActivityType ActivityType { get; set; } = null!;

        [ForeignKey(nameof(StatusId))]
        public virtual TbActivityStatus ActivityStatus { get; set; } = null!;

        [ForeignKey(nameof(CompanyId))]
        public virtual TbCompany Company { get; set; } = null!;
    }
}
