using HRsystem.Api.Database;
using HRsystem.Api.Database.DataTables;
using HRsystem.Api.Features.Attendance;
using HRsystem.Api.Services.CurrentUser;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;

namespace HRsystem.Api.Features.EmployeeActivityDt.EmployeePunch
{
    // PunchInCommand.cs
    public record PunchInCommand(
        long ActivityId,
        int ActivityTypeId,
        int LocationId,
        double Latitude,
        double Longitude
    ) : IRequest<EmployeeAttendanceDto>;

    // PunchOutCommand.cs
    public record PunchOutCommand(
        long ActivityId,
        int ActivityTypeId,
        int LocationId,
        double Latitude,
        double Longitude
    ) : IRequest<EmployeeAttendanceDto>;

    public class PunchInHandler : IRequestHandler<PunchInCommand, EmployeeAttendanceDto>
    {
        private readonly DBContextHRsystem _db;
        private readonly ICurrentUserService _currentUserService;

        public PunchInHandler(DBContextHRsystem db, ICurrentUserService currentUserService)
        {
            _db = db;
            _currentUserService = currentUserService;
        }

        public async Task<EmployeeAttendanceDto> Handle(PunchInCommand request, CancellationToken ct)
        {
            var employeeId = _currentUserService.EmployeeID;
            var employee = await _db.TbEmployees.FirstOrDefaultAsync(e => e.EmployeeId == employeeId, ct);
            if (employee == null) throw new Exception("Employee not found");

            var location = await _db.TbWorkLocations.FirstOrDefaultAsync(l => l.WorkLocationId == request.LocationId, ct);
            if (location == null) throw new Exception("Location not found");

            var today = DateTime.UtcNow.Date;

            // Activity
            var activity = await _db.TbEmployeeActivities
                .FirstOrDefaultAsync(a => a.ActivityId == request.ActivityId &&
                                          a.EmployeeId == employee.EmployeeId &&
                                          a.RequestDate.Date == today, ct);

            if (activity == null)
            {
                activity = new TbEmployeeActivity
                {
                    EmployeeId = employee.EmployeeId,
                    ActivityTypeId = request.ActivityTypeId,
                    StatusId = 1,
                    RequestBy = employee.EmployeeId,
                    RequestDate = DateTime.UtcNow,
                    CompanyId = employee.CompanyId
                };
                _db.TbEmployeeActivities.Add(activity);
                await _db.SaveChangesAsync(ct);
            }

            // Attendance
            var attendance = await _db.TbEmployeeAttendances
                .FirstOrDefaultAsync(a => a.ActivityId == activity.ActivityId && a.AttendanceDate == today, ct);

            if (attendance == null)
            {
                attendance = new TbEmployeeAttendance
                {
                    ActivityId = activity.ActivityId,
                    AttendanceDate = today,
                    FirstPuchin = DateTime.UtcNow
                };
                _db.TbEmployeeAttendances.Add(attendance);
                await _db.SaveChangesAsync(ct);
            }

            // PunchIn (only if not already punched in)
            //var alreadyPunchedIn = await _db.TbEmployeeAttendancePunches
            //    .AnyAsync(p => p.AttendanceId == attendance.AttendanceId && p.PunchIn.HasValue, ct);

            //if (!alreadyPunchedIn)
            //{
                var punch = new TbEmployeeAttendancePunch
                {
                    AttendanceId = attendance.AttendanceId,
                    PunchIn = DateTime.UtcNow,
                    LocationId = request.LocationId,
                    DeviceId = employee.EmployeeId
                };
                _db.TbEmployeeAttendancePunches.Add(punch);

                await _db.SaveChangesAsync(ct);
            //}

            // AuditLog
            //var audit = new TbAuditLog
            //{
            //    CompanyId = employee.CompanyId,
            //    UserId = employee.EmployeeId,
            //    ActionDatetime = DateTime.UtcNow,
            //    TableName = "EmployeeAttendancePunch",
            //    ActionType = "PunchIn",
            //    RecordId = attendance.AttendanceId.ToString(),
            //    NewData = $"PunchIn Attempt at {DateTime.UtcNow}, LocationId={request.LocationId}"
            //};
            //_db.TbAuditLogs.Add(audit);

            //await _db.SaveChangesAsync(ct);

            return new EmployeeAttendanceDto
            {
                AttendanceId = attendance.AttendanceId,
                ActivityId = activity.ActivityId,
                FirstPunchIn = attendance.FirstPuchin
            };
        }
    }

    public class PunchOutHandler : IRequestHandler<PunchOutCommand, EmployeeAttendanceDto>
    {
        private readonly DBContextHRsystem _db;
        private readonly ICurrentUserService _currentUserService;

        public PunchOutHandler(DBContextHRsystem db, ICurrentUserService currentUserService)
        {
            _db = db;
            _currentUserService = currentUserService;
        }

        public async Task<EmployeeAttendanceDto> Handle(PunchOutCommand request, CancellationToken ct)
        {
            var employeeId = _currentUserService.UserId;
            var employee = await _db.TbEmployees.FirstOrDefaultAsync(e => e.EmployeeId == employeeId, ct);
            if (employee == null) throw new Exception("Employee not found");

            var location = await _db.TbWorkLocations.FirstOrDefaultAsync(l => l.WorkLocationId == request.LocationId, ct);
            if (location == null) throw new Exception("Location not found");

            var today = DateTime.UtcNow.Date;

            // Activity
            var activity = await _db.TbEmployeeActivities
                .FirstOrDefaultAsync(a => a.ActivityId == request.ActivityId &&
                                          a.EmployeeId == employee.EmployeeId &&
                                          a.RequestDate.Date == today, ct);

            if (activity == null)
            {
                activity = new TbEmployeeActivity
                {
                    EmployeeId = employee.EmployeeId,
                    ActivityTypeId = request.ActivityTypeId,
                    StatusId = 1,
                    RequestBy = employee.EmployeeId,
                    RequestDate = DateTime.UtcNow,
                    CompanyId = employee.CompanyId
                };
                _db.TbEmployeeActivities.Add(activity);
                await _db.SaveChangesAsync(ct);
            }

            // Attendance
            var attendance = await _db.TbEmployeeAttendances
                .FirstOrDefaultAsync(a => a.ActivityId == activity.ActivityId && a.AttendanceDate == today, ct);

            if (attendance == null)
            {
                var now = DateTime.UtcNow;
                // أول اليوم وبدأ بـ PunchOut
                attendance = new TbEmployeeAttendance
                {
                    ActivityId = activity.ActivityId,
                    AttendanceDate = today,
                    FirstPuchin = now.AddMinutes(-1),
                    LastPuchout = now,
                    TotalHours = (decimal)1 / 60m       // one min 
                };
                _db.TbEmployeeAttendances.Add(attendance);
                await _db.SaveChangesAsync(ct);

                var punch = new TbEmployeeAttendancePunch
                {
                    AttendanceId = attendance.AttendanceId,
                    PunchOut = DateTime.UtcNow,
                    LocationId = request.LocationId,
                    //DeviceId = employee.EmployeeId
                };
                _db.TbEmployeeAttendancePunches.Add(punch);
                await _db.SaveChangesAsync(ct);
            }
            else
            {
                // لو فيه Attendance موجودة
                //var alreadyPunchedOut = await _db.TbEmployeeAttendancePunches
                //    .AnyAsync(p => p.AttendanceId == attendance.AttendanceId && p.PunchOut.HasValue, ct);

                //if (!alreadyPunchedOut)
                //{
                    var punch = new TbEmployeeAttendancePunch
                    {
                        AttendanceId = attendance.AttendanceId,
                        PunchOut = DateTime.UtcNow,
                        LocationId = request.LocationId,
                        DeviceId = employee.EmployeeId
                    };
                    _db.TbEmployeeAttendancePunches.Add(punch);
                    await _db.SaveChangesAsync(ct);

                    attendance.LastPuchout = DateTime.UtcNow;
                    if (attendance.FirstPuchin.HasValue)
                    {
                        attendance.TotalHours =
                            ((decimal)(attendance.LastPuchout.Value - attendance.FirstPuchin.Value).TotalMinutes) / 60m;
                    }
                    else
                    {
                        attendance.TotalHours = 0;
                    }
                //}
            }

            // AuditLog
            //var audit = new TbAuditLog
            //{
            //    CompanyId = employee.CompanyId,
            //    UserId = employee.EmployeeId,
            //    ActionDatetime = DateTime.UtcNow,
            //    TableName = "EmployeeAttendancePunch",
            //    ActionType = "PunchOut",
            //    RecordId = attendance.AttendanceId.ToString(),
            //    NewData = $"PunchOut Attempt at {DateTime.UtcNow}, LocationId={request.LocationId}"
            //};
            //_db.TbAuditLogs.Add(audit);

            //await _db.SaveChangesAsync(ct);

            return new EmployeeAttendanceDto
            {
                AttendanceId = attendance.AttendanceId,
                ActivityId = activity.ActivityId,
                FirstPunchIn = attendance.FirstPuchin,
                LastPunchOut = attendance.LastPuchout,
                TotalHours = attendance.TotalHours
            };
        }
    }





















    //    public static class GeoHelper
    //    {
    //        public static double CalculateDistance(double lat1, double lon1, double lat2, double lon2)
    //        {
    //            const double R = 6371e3; // نصف قطر الأرض بالمتر
    //            var φ1 = lat1 * Math.PI / 180;
    //            var φ2 = lat2 * Math.PI / 180;
    //            var Δφ = (lat2 - lat1) * Math.PI / 180;
    //            var Δλ = (lon2 - lon1) * Math.PI / 180;

    //            var a = Math.Sin(Δφ / 2) * Math.Sin(Δφ / 2) +
    //                    Math.Cos(φ1) * Math.Cos(φ2) *
    //                    Math.Sin(Δλ / 2) * Math.Sin(Δλ / 2);

    //            var c = 2 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1 - a));

    //            return R * c; // المسافة بالمتر
    //        }
    //    }

    //    public class EmployeePunchDto
    //    {
    //        public long PunchId { get; set; }
    //        public long AttendanceId { get; set; }
    //        public long LocationId { get; set; }
    //        public DateTime PunchTime { get; set; }
    //        public bool IsPunchIn { get; set; }
    //    }

    //    public record PunchInCommand(long AttendanceId, double Latitude, double Longitude, int? DeviceId)
    //    : IRequest<long>;

    //    public class PunchInHandler : IRequestHandler<PunchInCommand, long>
    //    {
    //        private readonly DBContextHRsystem _db;
    //        public PunchInHandler(DBContextHRsystem db) => _db = db;

    //        public async Task<long> Handle(PunchInCommand request, CancellationToken ct)
    //        {
    //            var now = DateTime.UtcNow;

    //            // 1️⃣ Check WorkLocation
    //            var locations = await _db.TbWorkLocations.ToListAsync(ct);
    //            int? locationId = null;

    //            foreach (var loc in locations)
    //            {
    //                var distance = GeoHelper.CalculateDistance(request.Latitude, request.Longitude,
    //                                                           (double)loc.Latitude, (double)loc.Longitude);

    //                if (distance <= loc.AllowedRadiusM)
    //                {
    //                    locationId = loc.WorkLocationId;
    //                    break;
    //                }
    //            }

    //            if (locationId == null)
    //                throw new Exception("Employee is outside allowed work locations!");

    //            // 2️⃣ سجل PunchIn جديد
    //            var punch = new TbEmployeeAttendancePunch
    //            {
    //                AttendanceId = request.AttendanceId,
    //                PunchIn = now,
    //                LocationId = locationId,
    //                DeviceId = request.DeviceId
    //            };

    //            _db.TbEmployeeAttendancePunches.Add(punch);
    //            await _db.SaveChangesAsync(ct);

    //            return punch.PunchId;
    //        }
    //    }


    //    public record PunchOutCommand(long AttendanceId, double Latitude, double Longitude, int? DeviceId)
    //     : IRequest<long>;

    //    public class PunchOutHandler : IRequestHandler<PunchOutCommand, long>
    //    {
    //        private readonly DBContextHRsystem _db;
    //        public PunchOutHandler(DBContextHRsystem db) => _db = db;

    //        public async Task<long> Handle(PunchOutCommand request, CancellationToken ct)
    //        {
    //            var now = DateTime.UtcNow;

    //            // 1️⃣ Check WorkLocation
    //            var locations = await _db.TbWorkLocations.ToListAsync(ct);
    //            int? locationId = null;

    //            foreach (var loc in locations)
    //            {
    //                var distance = GeoHelper.CalculateDistance(request.Latitude, request.Longitude,
    //                                                          (double)loc.Latitude, (double)loc.Longitude);

    //                if (distance <= loc.AllowedRadiusM)
    //                {
    //                    locationId = loc.WorkLocationId;
    //                    break;
    //                }
    //            }

    //            if (locationId == null)
    //                throw new Exception("Employee is outside allowed work locations!");

    //            // 2️⃣ هات آخر PunchIn المفتوح
    //            var punch = await _db.TbEmployeeAttendancePunches
    //                .Where(p => p.AttendanceId == request.AttendanceId && p.PunchOut == null)
    //                .OrderByDescending(p => p.PunchIn)
    //                .FirstOrDefaultAsync(ct);

    //            if (punch == null)
    //                throw new Exception("No open PunchIn found for this attendance!");

    //            punch.PunchOut = now;
    //            punch.LocationId = locationId;
    //            punch.DeviceId = request.DeviceId;

    //            await _db.SaveChangesAsync(ct);

    //            // 3️⃣ حدث Attendance (تحديث الـ Total Hours)
    //            var attendance = await _db.TbEmployeeAttendances
    //                .FirstOrDefaultAsync(a => a.AttendanceId == request.AttendanceId, ct);

    //            if (attendance != null && attendance.FirstPuchin != null)
    //            {
    //                attendance.LastPuchout = now;
    //                var totalHours = (attendance.LastPuchout.Value - attendance.FirstPuchin.Value).TotalHours;
    //                attendance.TotalHours = Math.Round((decimal)totalHours, 2);

    //                _db.TbEmployeeAttendances.Update(attendance);
    //                await _db.SaveChangesAsync(ct);
    //            }

    //            return punch.PunchId;
    //        }
    //    }




}
//using MediatR;
//using Microsoft.EntityFrameworkCore;

//public class PunchInHandler : IRequestHandler<PunchInCommand, EmployeeAttendanceDto>
//{
//    private readonly DBContextHRsystem _db;
//    public PunchInHandler(DBContextHRsystem db) => _db = db;

//    public async Task<EmployeeAttendanceDto> Handle(PunchInCommand request, CancellationToken ct)
//    {
//        var today = DateTime.UtcNow.Date;

//        // 1️⃣ تحقق من وجود Activity اليوم
//        var activity = await _db.TbEmployeeActivities
//            .FirstOrDefaultAsync(a => a.EmployeeId == request.EmployeeId && a.RequestDate.Date == today, ct);

//        if (activity == null)
//        {
//            activity = new TbEmployeeActivity
//            {
//                EmployeeId = request.EmployeeId,
//                RequestDate = DateTime.UtcNow
//            };
//            _db.TbEmployeeActivities.Add(activity);
//            await _db.SaveChangesAsync(ct);
//        }

//        // 2️⃣ تحقق من وجود Attendance اليوم
//        var attendance = await _db.TbEmployeeAttendances
//            .FirstOrDefaultAsync(a => a.EmployeeId == request.EmployeeId && a.ActivityId == activity.ActivityId, ct);

//        if (attendance == null)
//        {
//            attendance = new TbEmployeeAttendance
//            {
//                EmployeeId = request.EmployeeId,
//                ActivityId = activity.ActivityId,
//                FirstPunchIn = DateTime.UtcNow,
//                StatusId = 1
//            };
//            _db.TbEmployeeAttendances.Add(attendance);
//            await _db.SaveChangesAsync(ct);
//        }

//        // 3️⃣ سجل Punch في AttendanceLog
//        var log = new TbAttendanceLog
//        {
//            AttendanceId = attendance.AttendanceId,
//            PunchTime = DateTime.UtcNow,
//            Type = "In"
//        };
//        _db.TbAttendanceLogs.Add(log);
//        await _db.SaveChangesAsync(ct);

//        return new EmployeeAttendanceDto
//        {
//            AttendanceId = attendance.AttendanceId,
//            EmployeeId = attendance.EmployeeId,
//            ActivityId = attendance.ActivityId,
//            FirstPunchIn = attendance.FirstPunchIn,
//            StatusId = attendance.StatusId
//        };
//    }
//}
