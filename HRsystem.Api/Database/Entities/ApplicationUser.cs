using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;


namespace HRsystem.Api.Database.Entities
{
    public class ApplicationUser : IdentityUser<int>
    {
        public Guid RowGuid { get; set; } = Guid.NewGuid();

        [MaxLength(80)] 
        public string UserFullName { get; set; }

        public int CompanyId { get; set; }

        public bool IsActive { get; set; }
        public bool IsToChangePassword { get; set; }

        public DateTime? LastPasswordChangedAt { get; set; }

        public DateTime? LastLoginAt { get; set; }
        public DateTime? LastFailedLoginAt { get; set; }
        public bool ForceLogout { get; set; }

        public int FailedLoginCount { get; set; }
             
       
        [MaxLength(10)]
        public string PreferredLanguage { get; set; } // e.g. "en", "ar"

        // 🔑 New field
        public int PermissionVersion { get; set; } = 1;


        public int? CreatedBy { get; set; }
        public DateTime? CreatedAt { get; set; } = DateTime.Now;

    }
}
