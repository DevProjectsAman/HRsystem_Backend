namespace HRsystem.Api.Features.Groups.GetALL
{
    public class GetGroupCommand
    {
        public int GroupId { get; set; }
        public string GroupName { get; set; } = default!;
    }
}
