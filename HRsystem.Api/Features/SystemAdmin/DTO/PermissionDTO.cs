namespace HRsystem.Api.Features.SystemAdmin.DTO
{

    public class PermissionDTO
    {
     
        public Guid? PermissionId { get; set; }  // Example: Guid.NewGuid() or null if not set
        public required string PermissionCatagory { get; set; }  // Example: "CanEditUser", "CanDeleteItem"
        public required string PermissionName { get; set; }  // Example: "CanEditUser", "CanDeleteItem"
        public string? PermissionDescription { get; set; }
    }
}
