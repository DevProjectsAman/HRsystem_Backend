using FluentValidation;
using HRsystem.Api.Database.DataTables;
using HRsystem.Api.Database;
using HRsystem.Api.Services.CurrentUser;
using HRsystem.Api.Shared.ExceptionHandling;
using MediatR;
using Microsoft.EntityFrameworkCore;
using HRsystem.Api.Services.LookupCashing;

namespace HRsystem.Api.Features.EmployeeRequest.Execuse.CreateExecuse
{
    public record CreateExcuseCommand(
        int StatusId,
        DateTime ExcuseDate,
        TimeSpan StartTime,
        TimeSpan EndTime,
        string? ExcuseReason
    ) : IRequest<CreateExcuseResponse>;

    public record CreateExcuseResponse(
        long ActivityId,
        long ExcuseId,
        int EmployeeId,
        int ActivityTypeId,
       // int StatusId,
        DateTime ExcuseDate,
        TimeSpan StartTime,
        TimeSpan EndTime,
        string? ExcuseReason
    );

    public class Handler : IRequestHandler<CreateExcuseCommand, CreateExcuseResponse>
    {
        private readonly DBContextHRsystem _db;
        private readonly ICurrentUserService _currentUser;
        private readonly IActivityTypeLookupCache _activityTypeCache;
        private readonly IActivityStatusLookupCache _activityStatusLookupCache;

        public Handler(DBContextHRsystem db, ICurrentUserService currentUser, IActivityTypeLookupCache activityTypeCache, IActivityStatusLookupCache activityStatusLookupCache  )
        {
            _db = db;
            _currentUser = currentUser;
            _activityTypeCache = activityTypeCache;
            _activityStatusLookupCache = activityStatusLookupCache;
        }
        public async Task<CreateExcuseResponse> Handle(CreateExcuseCommand request, CancellationToken ct)
        {
            var employeeId = _currentUser.EmployeeID ?? 0;
            var companyId = _currentUser.CompanyID ?? 0;

            // 1️⃣ Get the ActivityType for Excuse
            var activityType = await _db.TbActivityTypes
                .FirstOrDefaultAsync(x => x.ActivityCode == "REQ_EXCUSE", ct);

            if (activityType == null)
                throw new NotFoundException("Invalid ActivityType code:", "REQ_EXCUSE");

            // 2️⃣ Create Employee Activity first
            var activity = new TbEmployeeActivity
            {
                EmployeeId = employeeId,
                // ActivityTypeId = activityType.ActivityTypeId,
                // StatusId = 10,// when create status automate to pending status
                ActivityTypeId = _activityTypeCache.GetIdByCode(ActivityCodes.ExcuseRequest),

                //  StatusId = 7, // Pending

                StatusId = _activityStatusLookupCache.GetIdByCode(ActivityStatusCodes.Pending),
                RequestBy = employeeId,
                RequestDate = DateTime.UtcNow,
                CompanyId = companyId
            };

            _db.TbEmployeeActivities.Add(activity);
            await _db.SaveChangesAsync(ct); // ✅ نحصل على ActivityId

            // 3️⃣ Create Excuse linked to the Activity
            var excuse = new TbEmployeeExcuse
            {
                ActivityId = activity.ActivityId,
                // تحويل DateTime -> DateOnly
                ExcuseDate = DateOnly.FromDateTime(request.ExcuseDate),

                // تحويل TimeSpan -> TimeOnly
                StartTime = TimeOnly.FromTimeSpan(request.StartTime),
                EndTime = TimeOnly.FromTimeSpan(request.EndTime),

                ExcuseReason = request.ExcuseReason

            };

            _db.TbEmployeeExcuses.Add(excuse);
            await _db.SaveChangesAsync(ct);

            return new CreateExcuseResponse(
                activity.ActivityId,
                excuse.ExcuseId,
                activity.EmployeeId,
                activity.ActivityTypeId,
               // activity.StatusId,
                excuse.ExcuseDate.ToDateTime(TimeOnly.MinValue),
                // ✅ نحول TimeOnly -> TimeSpan
                excuse.StartTime.ToTimeSpan(),
                excuse.EndTime.ToTimeSpan(),
                excuse.ExcuseReason
            );
        }
    }

    public class CreateExcuseValidator : AbstractValidator<CreateExcuseCommand>
    {
        public CreateExcuseValidator()
        {
            //RuleFor(x => x.StatusId)
            //    .GreaterThan(0)
            //    .WithMessage("StatusId must be greater than 0");

            RuleFor(x => x.ExcuseDate)
                    .GreaterThanOrEqualTo(DateTime.UtcNow.Date)
                    .WithMessage("ExcuseDate cannot be in the past");

            RuleFor(x => x.StartTime)
                .LessThan(x => x.EndTime)
                .WithMessage("StartTime must be before EndTime");

            RuleFor(x => x.ExcuseReason)
                .MaximumLength(150)
                .WithMessage("ExcuseReason cannot exceed 150 characters");
        }
    }
}



