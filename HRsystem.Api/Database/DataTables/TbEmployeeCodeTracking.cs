using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HRsystem.Api.Database.DataTables
{
    // Domain/Entities/EmployeeCodeTracking.cs

    [Table("Tb_EmployeeCode_Tracking")]
    public class TbEmployeeCodeTracking
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(50)]
        public string UniqueEmployeeCode { get; set; } = string.Empty;

        public bool IsUsed { get; set; } = false;

      

        public DateTime? UsedAt { get; set; }

        public int? EmployeeId { get; set; } // Reference to employee if used

        [MaxLength(500)]
        public string? FolderPath { get; set; } // Store the physical folder path

        [MaxLength(500)]
        public string DocFullPath { get; set; } = string.Empty; // Store the FULL document path
        public DateTime GeneratedAt { get; set; } = DateTime.UtcNow;
        public int GeneratedById { get; set; }  // Reference to user who generated the code

 
    }
}
