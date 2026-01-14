using MediatR;

namespace HRsystem.Api.Features.Organization.Groups.Create
{
    public record CreateGroupCommand(string GroupName) : IRequest<int>;
}
