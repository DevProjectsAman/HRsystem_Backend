using HRsystem.Api.Database;
using HRsystem.Api.Database.DataTables;
using MediatR;

namespace HRsystem.Api.Features.Groups.Create
{
    public class CreateGroupHandler : IRequestHandler<CreateGroupCommand, int>
    {
        private readonly DBContextHRsystem _context;

        public CreateGroupHandler(DBContextHRsystem context)
        {
            _context = context;
        }

        public async Task<int> Handle(CreateGroupCommand request, CancellationToken cancellationToken)
        {
            var group = new TbGroup
            {
                GroupName = request.GroupName
            };

            _context.TbGroups.Add(group);
            await _context.SaveChangesAsync(cancellationToken);

            return group.GroupId; // نرجع Id الجديد
        }
    }
}
