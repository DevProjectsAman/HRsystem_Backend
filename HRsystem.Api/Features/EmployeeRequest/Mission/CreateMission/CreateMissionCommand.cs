using FluentValidation;
using HRsystem.Api.Database;
using HRsystem.Api.Database.DataTables;
using HRsystem.Api.Services.CurrentUser;
using HRsystem.Api.Services.LookupCashing;
using HRsystem.Api.Shared.ExceptionHandling;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace HRsystem.Api.Features.Mission.CreateMission
{
    public record CreateMissionCommand(
           // int EmployeeId,
            //int ActivityTypeId,
            int StatusId,
           // long RequestBy,
           // long ApprovedBy,
            DateTime RequestDate,
           // DateTime ApprovedDate,
           // long CompanyId,
            DateTime StartDatetime,
            DateTime EndDatetime,
            string MissionLocation,
            string MissionReason
        ) : IRequest<CreateMissionResponse>;

    public record CreateMissionResponse(
        long ActivityId,
        long MissionId,
        int EmployeeId,
        int ActivityTypeId,
        //int StatusId,
        DateTime StartDatetime,
        DateTime EndDatetime,
        string MissionLocation,
        string MissionReason
    );



    public class Handler : IRequestHandler<CreateMissionCommand, CreateMissionResponse>
    {

        private readonly DBContextHRsystem _db;
        private readonly ICurrentUserService _currentUser;
        private readonly IActivityTypeLookupCache _activityTypeCache;
        private readonly IActivityStatusLookupCache _activityStatusLookupCache;

        public Handler(DBContextHRsystem db, ICurrentUserService currentUser, IActivityTypeLookupCache activityTypeCache, IActivityStatusLookupCache activityStatusLookupCache)
        {
            _db = db;
            _currentUser = currentUser;
            _activityTypeCache = activityTypeCache;
            _activityStatusLookupCache = activityStatusLookupCache;
        }


        public async Task<CreateMissionResponse> Handle(CreateMissionCommand request, CancellationToken ct)
        {
            var employeeId = _currentUser.EmployeeID ?? 0;
            var companyId = _currentUser.CompanyID ?? 0;

            var activityType = await _db.TbActivityTypes
            .FirstOrDefaultAsync(x => x.ActivityCode == "REQ_MISSION", ct);

            if (activityType == null)
                throw new NotFoundException("Invalid ActivityType code:", "REQ_MISSION");
            

            // 1️⃣ Create the Activity first
            var activity = new TbEmployeeActivity
            {
                EmployeeId = employeeId,
                //ActivityTypeId = activityType.ActivityTypeId,
                //StatusId = 10, //asign automate to pending status when just created
                ActivityTypeId = _activityTypeCache.GetIdByCode(ActivityCodes.MissionRequest),

                //  StatusId = 7, // Pending

                StatusId = _activityStatusLookupCache.GetIdByCode(ActivityStatusCodes.Pending),
                RequestBy = employeeId,
              //  ApprovedBy = request.ApprovedBy,
                RequestDate = request.RequestDate,
               // ApprovedDate = request.ApprovedDate,
                CompanyId = companyId
            };

            _db.TbEmployeeActivities.Add(activity);
            await _db.SaveChangesAsync(ct); // Get ActivityId after save

            // 2️⃣ Create the Mission linked to the activity
            var mission = new TbEmployeeMission
            {
                ActivityId = activity.ActivityId,
                StartDatetime = request.StartDatetime,
                EndDatetime = request.EndDatetime,
                MissionLocation = request.MissionLocation,
                MissionReason = request.MissionReason
            };

            _db.TbEmployeeMissions.Add(mission);
            await _db.SaveChangesAsync(ct);

            return new CreateMissionResponse(
                activity.ActivityId,
                mission.MissionId,
                activity.EmployeeId,
                activity.ActivityTypeId,
               // activity.StatusId,
                mission.StartDatetime,
                mission.EndDatetime,
                mission.MissionLocation,
                mission.MissionReason
            );
        }
    }

    public class CreateMissionValidator : AbstractValidator<CreateMissionCommand>
    {
        public CreateMissionValidator()
        {
            // Activity validation
            //RuleFor(x => x.EmployeeId).GreaterThan(0);
            //RuleFor(x => x.ActivityTypeId).GreaterThan(0);
            //RuleFor(x => x.StatusId).GreaterThan(0);
         //   RuleFor(x => x.RequestBy).GreaterThan(0);
            //RuleFor(x => x.ApprovedBy).GreaterThan(0);
           // RuleFor(x => x.CompanyId).GreaterThan(0);

            //RuleFor(x => x.RequestDate)
            // .LessThanOrEqualTo(DateTime.UtcNow)
            // .WithMessage("RequestDate cannot be in the future");

            // Mission validation
            RuleFor(x => x.StartDatetime)
                .LessThan(x => x.EndDatetime)
                .WithMessage("StartDatetime must be before EndDatetime");

            RuleFor(x => x.MissionLocation)
                .NotEmpty()
                .MaximumLength(200);

            RuleFor(x => x.MissionReason)
                .NotEmpty()
                .MaximumLength(500);
        }
    }


}