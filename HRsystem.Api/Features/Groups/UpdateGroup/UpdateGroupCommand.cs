using HRsystem.Api.Database;
using HRsystem.Api.Features.Groups.GetALlGroup;
using MediatR;
using System.Security.Cryptography.X509Certificates;

namespace HRsystem.Api.Features.Groups.UpdateGroup
{
    public record UpdateGroupCommand ( int groupid , string new_groupname) : IRequest<UpdateGroupRsponse>;

    public class UpdateGroupRsponse
    {
        public int group_id {get; set;}
        public string groupname {get; set;}

    }

    public class UpdateGroupHandler : IRequestHandler<UpdateGroupCommand , UpdateGroupRsponse>
    {
        private readonly DBContextHRsystem _db;

        public UpdateGroupHandler (DBContextHRsystem db)
        {
            _db = db;
        }

        public async Task<UpdateGroupRsponse> Handle(UpdateGroupCommand request, CancellationToken cancellationToken)
        {
            var group = await _db.TbGroups.FindAsync(new object[] { request.groupid }, cancellationToken);

            if (group == null)
                throw new KeyNotFoundException($"Group with Id {request.groupid} not found");

            group.GroupName = request.new_groupname;

            await _db.SaveChangesAsync(cancellationToken);

            return new UpdateGroupRsponse
            {
                group_id = group.GroupId,
                groupname = group.GroupName,
            };
        }
    }
}
