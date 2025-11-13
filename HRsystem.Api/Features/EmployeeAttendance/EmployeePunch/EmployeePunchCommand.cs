using HRsystem.Api.Database;
using HRsystem.Api.Database.DataTables;
using HRsystem.Api.Features.EmployeeDashboard.EmployeeApp;
using HRsystem.Api.Services.CurrentUser;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading;



namespace HRsystem.Api.Features.EmployeeActivityDt.EmployeePunch
{

    public record PunchInCommand(
     double Latitude,
     double Longitude
 ) : IRequest<EmployeeAttendanceDto>;

    public record PunchOutCommand(
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

        private double GetDistanceInMeters(double lat1, double lon1, double lat2, double lon2)
        {
            const double R = 6371000;
            var latRad1 = lat1 * Math.PI / 180;
            var latRad2 = lat2 * Math.PI / 180;
            var deltaLat = (lat2 - lat1) * Math.PI / 180;
            var deltaLon = (lon2 - lon1) * Math.PI / 180;

            var a = Math.Sin(deltaLat / 2) * Math.Sin(deltaLat / 2) +
                    Math.Cos(latRad1) * Math.Cos(latRad2) *
                    Math.Sin(deltaLon / 2) * Math.Sin(deltaLon / 2);

            var c = 2 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1 - a));
            return R * c;
        }

        public async Task<EmployeeAttendanceDto> Handle(PunchInCommand request, CancellationToken ct)
        {
            var employeeId = _currentUserService.EmployeeID;
            var employee = await _db.TbEmployees.FirstOrDefaultAsync(e => e.EmployeeId == employeeId, ct);
            if (employee == null) throw new Exception("Employee not found");

            var locations = await _db.TbEmployeeWorkLocations
                .Where(l => l.EmployeeId == employee.EmployeeId)
                .ToListAsync(ct);

            int? WorkLOC = null;
            foreach (var loc in locations)
            {
                var WID = await _db.TbWorkLocations.FirstOrDefaultAsync(e => e.WorkLocationId == loc.WorkLocationId, ct);
                var distance = GetDistanceInMeters(request.Latitude, request.Longitude, (double)WID.Latitude, (double)WID.Longitude);
                if (distance <= WID.AllowedRadiusM)
                {
                    WorkLOC = WID.WorkLocationId;
                    break;
                }
            }
            if (WorkLOC == null) throw new Exception("Not In Allowed Radius");

            var today = DateTime.Now.Date;
            var activity = await _db.TbEmployeeActivities
                .FirstOrDefaultAsync(a => a.EmployeeId == employee.EmployeeId && a.RequestDate.Date == today && a.ActivityTypeId == 1, ct);

            if (activity == null)
            {
                activity = new TbEmployeeActivity
                {
                    ActivityTypeId = 1,
                    EmployeeId = employee.EmployeeId,
                    StatusId = 16,
                    RequestBy = employee.EmployeeId,
                    RequestDate = DateTime.Now,
                    CompanyId = employee.CompanyId
                };
                _db.TbEmployeeActivities.Add(activity);
                await _db.SaveChangesAsync(ct);
            }

            var attendance = await _db.TbEmployeeAttendances
                .FirstOrDefaultAsync(a => a.ActivityId == activity.ActivityId && a.AttendanceDate == today, ct);

            if (attendance == null)
            {
                attendance = new TbEmployeeAttendance
                {
                    ActivityId = activity.ActivityId,
                    AttendanceDate = today,
                    FirstPuchin = DateTime.Now
                };
                var Shift = await _db.TbShifts
                .FirstOrDefaultAsync(b => b.ShiftId == employee.ShiftId, ct);

                var ShiftINfo = new EmployeeGetShiftDto
                {
                    EndTime = Shift.EndTime,
                    StartTime = Shift.StartTime,
                    GracePeriodMinutes = Shift.GracePeriodMinutes,
                    IsFlexible = Shift.IsFlexible,
                    MaxStartTime = Shift.MaxStartTime,
                };

                switch (Shift.IsFlexible)
                {
                    case true:
                        if ((ShiftINfo.MaxStartTime ?? new TimeOnly(0, 0)).AddMinutes(ShiftINfo.GracePeriodMinutes) < TimeOnly.FromDateTime(DateTime.Now)
                            )
                            attendance.AttStatues = statues.Late;
                        else
                            attendance.AttStatues = statues.OnTime;
                        break;
                    case false:
                        if ((ShiftINfo.StartTime.AddMinutes(ShiftINfo.GracePeriodMinutes)) <= TimeOnly.FromDateTime(DateTime.Now))
                            attendance.AttStatues = statues.Late;
                        else
                            attendance.AttStatues = statues.OnTime;
                        break;
                }

                _db.TbEmployeeAttendances.Add(attendance);
                await _db.SaveChangesAsync(ct);
            }

            var punch = new TbEmployeeAttendancePunch
            {
                AttendanceId = attendance.AttendanceId,
                PunchTime = DateTime.Now,
                PunchType = "PunchIn",
                LocationId = WorkLOC.Value
            };
            _db.TbEmployeeAttendancePunches.Add(punch);
            await _db.SaveChangesAsync(ct);

            return new EmployeeAttendanceDto
            {
                AttendanceId = attendance.AttendanceId,
                EmployeeId = employee.EmployeeId,
                ActivityId = activity.ActivityId,
                FirstPunchIn = attendance.FirstPuchin
            };
        }
    }


    // ================= PunchOut ==================

    public class PunchOutHandler : IRequestHandler<PunchOutCommand, EmployeeAttendanceDto>
    {
        private readonly DBContextHRsystem _db;
        private readonly ICurrentUserService _currentUserService;

        public PunchOutHandler(DBContextHRsystem db, ICurrentUserService currentUserService)
        {
            _db = db;
            _currentUserService = currentUserService;
        }

        private double GetDistanceInMeters(double lat1, double lon1, double lat2, double lon2)
        {
            const double R = 6371000;
            var latRad1 = lat1 * Math.PI / 180;
            var latRad2 = lat2 * Math.PI / 180;
            var deltaLat = (lat2 - lat1) * Math.PI / 180;
            var deltaLon = (lon2 - lon1) * Math.PI / 180;

            var a = Math.Sin(deltaLat / 2) * Math.Sin(deltaLat / 2) +
                    Math.Cos(latRad1) * Math.Cos(latRad2) *
                    Math.Sin(deltaLon / 2) * Math.Sin(deltaLon / 2);

            var c = 2 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1 - a));
            return R * c;
        }

        public async Task<EmployeeAttendanceDto> Handle(PunchOutCommand request, CancellationToken ct)
        {
            var employeeId = _currentUserService.EmployeeID;
            var employee = await _db.TbEmployees.FirstOrDefaultAsync(e => e.EmployeeId == employeeId, ct);
            if (employee == null) throw new Exception("Employee not found");

            var locations = await _db.TbEmployeeWorkLocations
                .Where(l => l.EmployeeId == employee.EmployeeId)
                .ToListAsync(ct);

            int? WorkLOC = null;
            foreach (var loc in locations)
            {
                var WID = await _db.TbWorkLocations.FirstOrDefaultAsync(e => e.WorkLocationId == loc.WorkLocationId, ct);
                var distance = GetDistanceInMeters(request.Latitude, request.Longitude, (double)WID.Latitude, (double)WID.Longitude);
                if (distance <= WID.AllowedRadiusM)
                {
                    WorkLOC = WID.WorkLocationId;
                    break;
                }
            }
            if (WorkLOC == null) throw new Exception("Not In Allowed Radius");

            var today = DateTime.Now.Date;
            var now = DateTime.Now;

            var activity = await _db.TbEmployeeActivities
                .FirstOrDefaultAsync(a => a.EmployeeId == employee.EmployeeId && a.RequestDate.Date == today && a.ActivityTypeId == 1, ct);

            if (activity == null)
            {
                activity = new TbEmployeeActivity
                {
                    ActivityTypeId = 1 ,
                    EmployeeId = employee.EmployeeId,
                    StatusId = 7,
                    RequestBy = employee.EmployeeId,
                    RequestDate = now,
                    CompanyId = employee.CompanyId
                };
                _db.TbEmployeeActivities.Add(activity);
                await _db.SaveChangesAsync(ct);
            }

            var attendance = await _db.TbEmployeeAttendances
                .FirstOrDefaultAsync(a => a.ActivityId == activity.ActivityId && a.AttendanceDate == today , ct);

            if (attendance == null)
            {
                attendance = new TbEmployeeAttendance
                {
                    ActivityId = activity.ActivityId,
                    AttendanceDate = today,
                    FirstPuchin = now.AddMinutes(-1),
                    LastPuchout = now,
                    TotalHours = (decimal)1 / 60m
                };
                _db.TbEmployeeAttendances.Add(attendance);
                await _db.SaveChangesAsync(ct);
            }
            else
            {
                attendance.LastPuchout = now;

                if (attendance.FirstPuchin.HasValue)
                {
                    attendance.TotalHours = (decimal)(attendance.LastPuchout.Value - attendance.FirstPuchin.Value).TotalMinutes / 60m;
                }
                else
                {
                    attendance.TotalHours = 0;
                }
            }

            var punch = new TbEmployeeAttendancePunch
            {
                AttendanceId = attendance.AttendanceId,
                PunchTime = now,
                PunchType = "PunchOut",
                LocationId = WorkLOC.Value
            };
            _db.TbEmployeeAttendancePunches.Add(punch);

            await _db.SaveChangesAsync(ct);

            // === Actual Working Hours ===
            var punches = await _db.TbEmployeeAttendancePunches
                .Where(p => p.AttendanceId == attendance.AttendanceId)
                .OrderBy(p => p.PunchTime)
                .ToListAsync(ct);

            double totalMinutes = 0;
            DateTime? lastIn = null;

            foreach (var p in punches)
            {
                if (p.PunchType == "PunchIn")
                {
                    lastIn = p.PunchTime;
                }
                else if (p.PunchType == "PunchOut" && lastIn.HasValue)
                {
                    totalMinutes += (p.PunchTime.Value - lastIn.Value).TotalMinutes;
                    lastIn = null;
                }
            }

            attendance.ActualWorkingHours = (decimal)(totalMinutes / 60.0);
            await _db.SaveChangesAsync(ct);

            return new EmployeeAttendanceDto
            {
                AttendanceId = attendance.AttendanceId,
                EmployeeId = employee.EmployeeId,
                ActivityId = activity.ActivityId,
                FirstPunchIn = attendance.FirstPuchin,
                LastPunchOut = attendance.LastPuchout,
                TotalHours = attendance.TotalHours,
                ActualWorkingHours = attendance.ActualWorkingHours
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
