using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;


namespace HRsystem.Api.Database.Entities
{
    public class ApplicationRole : IdentityRole<int>
    {
        public ApplicationRole() : base()
        {
        }

        public ApplicationRole(string roleName) : base(roleName)
        {
        }
        //public Guid? AddUserID { get; set; }
        //public DateTime? CreatedDate { get; set; } = DateTime.UtcNow;

        [MaxLength(50)]
        public string Category { get; set; } = "General";
        // Examples: HR, Finance, Attendance, System

        [MaxLength(100)]
        public string DisplayName { get; set; } = "";
        // Example: "Edit Attendance Records"

        [MaxLength(250)]
        public string? Description { get; set; }="";
        // Example: "Allows the user to edit employee attendance entries"
    }

}
