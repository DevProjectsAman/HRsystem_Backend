using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HRsystem.Api.Database.Entities
{
    [Table("Tb_User_Login_History")]
    public class TbUserLoginHistory
    {
        [Key]
        public long Id { get; set; }

         
        public int UserId { get; set; }

        [Required]
        [MaxLength(20)]
        public string ClientType { get; set; } = null!; // web / mobile

        [MaxLength(100)]
        public string? DeviceId { get; set; }

        [Required]
        public DateTime LoggedInAt { get; set; } = DateTime.UtcNow;

        public DateTime? LoggedOutAt { get; set; }

        [MaxLength(50)]
        public string? IPAddress { get; set; }

        public bool Success { get; set; } = true;

        [MaxLength(50)]
        public string? FailureReason { get; set; }
        [MaxLength(50)]
        public string? UserNameAttempt { get; set; }

        [ForeignKey(nameof(UserId))]
        public ApplicationUser? User { get; set; }
    }

}
