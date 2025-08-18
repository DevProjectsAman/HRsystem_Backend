using System.ComponentModel.DataAnnotations;

namespace HRsystem.Api.Database.Entities
{
    public class AspPermission : Entity
    {
        [Key]
        public Guid PermissionId { get; set; }
        [MaxLength(50)]
        public string PermissionCatagory { get; set; }  // Example: "CanEditUser", "CanDeleteItem"
        [MaxLength(80)] public string PermissionName { get; set; }  // Example: "CanEditUser", "CanDeleteItem"
        [MaxLength(100)] public string PermissionDescription { get; set; }


    }

}
