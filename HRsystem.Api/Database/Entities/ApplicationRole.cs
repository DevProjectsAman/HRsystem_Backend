using Microsoft.AspNetCore.Identity;


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
        //public DateTime? CreatedDate { get; set; } = DateTime.Now;
    }
}
