namespace HRsystem.Api.Database.Entities
{
    public class AspRolePermissions : Entity
    {
        public Guid RoleId { get; set; }
        public Guid PermissionId { get; set; }
        public ApplicationRole Role { get; set; }
        public AspPermission Permission { get; set; }
    }
}
