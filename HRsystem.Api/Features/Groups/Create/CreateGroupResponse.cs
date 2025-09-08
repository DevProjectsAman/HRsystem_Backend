namespace HRsystem.Api.Features.Groups.Create
{
    public class CreateGroupResponse
    {
        public int GroupId { get; set; }
        public string GroupName { get; set; } = string.Empty;
        public string Message { get; set; } = "Group created successfully";

    }
}
