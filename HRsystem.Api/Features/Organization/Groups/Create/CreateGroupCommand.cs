using MediatR;

namespace HRsystem.Api.Features.Groups.Create
{
    public record CreateGroupCommand(string GroupName) : IRequest<int>;
}
