namespace HRsystem.Api.Database.Entities
{
    public class AspRolePermissions : Entity
    {
        public int RoleId { get; set; }
        public int PermissionId { get; set; }
        public ApplicationRole Role { get; set; }
        public AspPermission Permission { get; set; }
    }
}
