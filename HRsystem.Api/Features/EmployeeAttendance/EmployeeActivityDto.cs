using HRsystem.Api.Features.EmployeeDashboard.EmployeeApp;

namespace HRsystem.Api.Features.EmployeeActivityDt
{

    // EmployeeActivityDto.cs
    public class EmployeeActivityDto
    {
        public long ActivityId { get; set; }
        public long EmployeeId { get; set; }
        public int ActivityTypeId { get; set; }
        public int StatusId { get; set; }
        public long RequestBy { get; set; }
        public DateTime RequestDate { get; set; }
        public long CompanyId { get; set; }
    }

    // EmployeeAttendanceDto.cs
    public class EmployeeAttendanceDto
    {
        public long AttendanceId { get; set; }
        public long EmployeeId { get; set; }
        public long ActivityId { get; set; }
        public DateTime? FirstPunchIn { get; set; }
        public DateTime? LastPunchOut { get; set; }
        public decimal? TotalHours { get; set; }

        public statues Statues { get; set; }
        public decimal? ActualWorkingHours { get; set; }

    }

    // AttendancePunchDto.cs
    public class AttendancePunchDto
    {
        public long PunchId { get; set; }
        public long AttendanceId { get; set; }
        public DateTime? PunchIn { get; set; }
        public DateTime? PunchOut { get; set; }
        public double? Latitude { get; set; }
        public double? Longitude { get; set; }
        public string DeviceToken { get; set; }
    }




    //    //public class RecordAttendanceHandler : IRequestHandler<RecordAttendanceCommand , RecordAttendanceResponse>
    //    //{
    //    //    private readonly DBContextHRsystem _db;
    //    //    private readonly ICurrentUserService _currentUserService;

    //    //     public RecordAttendanceHandler(DBContextHRsystem db , ICurrentUserService currentUserService)
    //    //    {
    //    //        _db = db;
    //    //        _currentUserService = currentUserService;

    //    //    }
    //    //    public async Task<RecordAttendanceResponse> Handle(RecordAttendanceCommand request, CancellationToken ct)
    //    //    {
    //    //        var statues = await _db.TbEmployeeActivities.ToListAsync(ct);

    //    //        statues.EmployeeId = _currentUserService.UserId;

    //    //         statues.Select(s => new RecordAttendanceResponse
    //    //        {
    //    //            EmployeeId = s.EmployeeId,
    //    //            ActionId = s.ActivityId,

    //    //            CompanyId = // ✅ translated here
    //    //             = s.ActivityDescription
    //    //        }).ToList();

    //    //        if (employeeId == null)
    //    //            throw new UnauthorizedAccessException("Invalid user token");

    //    //        var companyId = await _db.TbEmployees
    //    //            .Where(e => e.EmployeeId == employeeId)
    //    //            .Select(e => e.CompanyId)
    //    //            .FirstOrDefaultAsync(ct);

    //    //        if (companyId == 0)
    //    //            throw new InvalidOperationException("Company not found for this employee");


    //    //        var now = DateTime.Now;
    //    //        var date = now.Date;

    //    //        var statuses = await _db.TbEmployeeActivities.ToListAsync(ct);
    //    //   }
    //    public record StartEmployeeActivityCommand(int UserId, int ActionId)
    //        : IRequest<EmployeeActivityCommand>;

    //    public class StartEmployeeActivityHandler
    //        : IRequestHandler<StartEmployeeActivityCommand, EmployeeActivityCommand>
    //    {
    //        private readonly DBContextHRsystem _db;
    //        public StartEmployeeActivityHandler(DBContextHRsystem db) => _db = db;

    //        public async Task<EmployeeActivityCommand> Handle(StartEmployeeActivityCommand request, CancellationToken ct)
    //        {
    //            // 1️⃣ Get Employee
    //            var employee = await _db.TbEmployees
    //                .FirstOrDefaultAsync(e => e.EmployeeId == request.UserId, ct);
    //            if (employee == null) throw new Exception("Employee not found");

    //            // 2️⃣ Create Activity
    //            var activity = new TbEmployeeActivity
    //            {
    //                EmployeeId = employee.EmployeeId,
    //                ActivityTypeId = request.ActionId, // ActionId → ActivityTypeId
    //                StatusId = 1, // Pending
    //                RequestBy = employee.EmployeeId,
    //                RequestDate = DateTime.Now,
    //                CompanyId = employee.CompanyId
    //            };

    //            _db.TbEmployeeActivities.Add(activity);
    //            await _db.SaveChangesAsync(ct);

    //            // 3️⃣ Return DTO
    //            return new EmployeeActivityCommand
    //            {
    //                ActivityId = activity.ActivityId,
    //                EmployeeId = activity.EmployeeId,
    //                CompanyId = activity.CompanyId,
    //                ActivityTypeId = activity.ActivityTypeId,
    //                StatusId = activity.StatusId,
    //                RequestBy = activity.RequestBy,
    //                RequestDate = activity.RequestDate
    //            };
    //        }
    //    }
    //    public record EndEmployeeActivityCommand(
    //    long ActivityId,         
    //    string? EndReason = null  
    //) : IRequest<EmployeeActivityCommand>; 
    //    public class EndEmployeeActivityHandler
    //    : IRequestHandler<EndEmployeeActivityCommand, EmployeeActivityCommand>
    //    {
    //        private readonly DBContextHRsystem _db;
    //        public EndEmployeeActivityHandler(DBContextHRsystem db) => _db = db;

    //        public async Task<EmployeeActivityCommand> Handle(EndEmployeeActivityCommand request, CancellationToken ct)
    //        {
    //            var activity = await _db.TbEmployeeActivities
    //                .FirstOrDefaultAsync(a => a.ActivityId == request.ActivityId, ct);
    //            if (activity == null) throw new Exception("Activity not found");

    //            activity.StatusId = 2; // Completed
    //            //activity.EndDate = DateTime.Now;

    //            await _db.SaveChangesAsync(ct);

    //            return new EmployeeActivityCommand
    //            {
    //                ActivityId = activity.ActivityId,
    //                EmployeeId = activity.EmployeeId,
    //                CompanyId = activity.CompanyId,
    //                ActivityTypeId = activity.ActivityTypeId,
    //                StatusId = activity.StatusId,
    //                RequestBy = activity.RequestBy,
    //                RequestDate = activity.RequestDate
    //            };
    //        }
    //    }




}
