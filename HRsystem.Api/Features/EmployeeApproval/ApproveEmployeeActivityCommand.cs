using HRsystem.Api.Database;
using HRsystem.Api.Database.DataTables;
using HRsystem.Api.Services.CurrentUser;
using HRsystem.Api.Shared.ExceptionHandling;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace HRsystem.Api.Features.EmployeeApproval
{
    public class ActivityApprovalDto
    {
        public long ApprovalId { get; set; }
        public long ActivityId { get; set; }
        public int StatusId { get; set; }
        public int? ChangedBy { get; set; }
        public DateTime ChangedDate { get; set; }
        public string? Notes { get; set; }
    }
    // ✅ Command
    public record ApproveEmployeeActivityCommand(long ActivityId, int StatusId, string? Notes) : IRequest<ActivityApprovalDto>;

    // ✅ Handler
    public class ApproveEmployeeActivityHandler : IRequestHandler<ApproveEmployeeActivityCommand, ActivityApprovalDto>
    {
        private readonly DBContextHRsystem _db;
        private readonly ICurrentUserService _currentUser;

        public ApproveEmployeeActivityHandler(DBContextHRsystem db, ICurrentUserService currentUser)
        {
            _db = db;
            _currentUser = currentUser;
        }

        public async Task<ActivityApprovalDto> Handle(ApproveEmployeeActivityCommand request, CancellationToken ct)
        {
            // ✅ Step 1: Activity
            var activity = await _db.TbEmployeeActivities
                .FirstOrDefaultAsync(a => a.ActivityId == request.ActivityId, ct);

            if (activity == null)
                throw new NotFoundException("Activity Not Found", request.ActivityId);

            // ✅ Step 2: Record Approval 
            var approval = new TbEmployeeActivityApproval
            {
                ActivityId = activity.ActivityId,
                StatusId = request.StatusId,
                ChangedBy = _currentUser.EmployeeID,
                ChangedDate = DateTime.UtcNow,
                Notes = request.Notes
            };
            _db.TbEmployeeActivityApprovals.Add(approval);

            // ✅ Step 3: Update Activity StatuesID 
            activity.StatusId = request.StatusId;

            await _db.SaveChangesAsync(ct);

            // ✅ Step 4: Return DTO
            return new ActivityApprovalDto
            {
                ApprovalId = approval.ApprovalId,
                ActivityId = approval.ActivityId,
                StatusId = approval.StatusId,
                ChangedBy = approval.ChangedBy,
                ChangedDate = approval.ChangedDate,
                Notes = approval.Notes
            };
        }
    }
}
