using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HRsystem.Api.Database.DataTables
{
    [Table("tb_activity_status_workflow")]
    public class TbActivityStatusWorkflow
    {
        [Key]
        public int WorkflowId { get; set; }

        [Required]
        public int ActivityTypeId { get; set; }

        public int? VacationTypeId { get; set; }

        [Required]
        public int StepOrder { get; set; }

        [Required]
        [MaxLength(50)]
        public string ApproverRole { get; set; } = string.Empty;

        [Required]
        public int NextStatusId { get; set; }

        [Required]
        public bool IsFinalStep { get; set; } = false;

        [Required]
        public int CompanyId { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public int? CreatedBy { get; set; }
    }
}
