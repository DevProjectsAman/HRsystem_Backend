using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;


namespace HRsystem.Api.Database.Entities
{
    public class ApplicationUser : IdentityUser<Guid>
    {
        [MaxLength(80)] 
        public string UserFullName { get; set; }

        public int? CreatedBy { get; set; }
        public DateTime? CreatedAt { get; set; } = DateTime.Now;

    }
}
