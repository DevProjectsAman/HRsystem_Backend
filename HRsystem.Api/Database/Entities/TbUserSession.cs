 
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    namespace HRsystem.Api.Database.Entities
    {
        [Table("Tb_User_Sessions")]
        public class TbUserSession
        {
            [Key]
            public long Id { get; set; }

            [Required]
            public int UserId { get; set; }

            [Required]
            [MaxLength(20)]
            public string ClientType { get; set; } = null!; // web / mobile

            [MaxLength(100)]
            public string? DeviceId { get; set; }

            [Required]
            [MaxLength(100)]
            public string Jti { get; set; } = null!;

            public bool IsActive { get; set; } = true;

            public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

            public DateTime LastSeenAt { get; set; } = DateTime.UtcNow;

            // 🔗 Optional navigation (good practice)
            [ForeignKey(nameof(UserId))]
            public ApplicationUser? User { get; set; }
        }
    }

