using Azure.Core;
using HRsystem.Api.Database;
using HRsystem.Api.Database.DataTables;
using HRsystem.Api.Services.CurrentUser;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.Runtime.InteropServices;
using static Google.Rpc.Context.AttributeContext.Types;

namespace HRsystem.Api.Features.EmployeeDashboard.EmployeeApp
{
   public class ChangeActivityRequestStatuesDto
    {
        public long ActivityId { get; set; }
        public int NewStatusId { get; set; }
    }

    public record ChangeActivityRequestStatues
        (ChangeActivityRequestStatuesDto REQUEST) : IRequest<bool>;

    public class ChangeActivityRequestStatuesHandler : IRequestHandler<ChangeActivityRequestStatues, bool>
    {
        private readonly DBContextHRsystem _db;
        private readonly ICurrentUserService _currentUser;

        public ChangeActivityRequestStatuesHandler(DBContextHRsystem db, ICurrentUserService currentUser)
        {
            _db = db;
            _currentUser = currentUser;
        }

        public async Task<bool> Handle(ChangeActivityRequestStatues request, CancellationToken ct)
        {

            var activity = await _db.TbEmployeeActivities.FirstOrDefaultAsync(e => e.ActivityId == request.REQUEST.ActivityId, ct);
            if (activity == null)
            {
                return false;
            }
            else
            {
                activity.StatusId = request.REQUEST.NewStatusId;
                await _db.SaveChangesAsync(ct);
                return true;
            }

        }

    }
 }
