using HRsystem.Api.Database;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace HRsystem.Api.Features.Organization.Groups.GetALlGroup
{
    public record GetAllGroupCommand() : IRequest<List<GetAllGroupResponse>>;

    public class GetAllGroupResponse
    {

        public int group_Id { get; set; }

        public string group_name { get; set; }
    }


    public class GetAllGroupHandler : IRequestHandler<GetAllGroupCommand, List<GetAllGroupResponse>>
    {

        private readonly DBContextHRsystem _db;

        public GetAllGroupHandler(DBContextHRsystem db)
        {
            _db = db;
        }

        public async Task<List<GetAllGroupResponse>> Handle(GetAllGroupCommand request, CancellationToken cancellationToken)
        {
            var groups = await _db.TbGroups
             .Select(g => new GetAllGroupResponse
             {
                 group_Id = g.GroupId,
                 group_name = g.GroupName,
             })
             .ToListAsync(cancellationToken);

            return groups;

        }

    }
}