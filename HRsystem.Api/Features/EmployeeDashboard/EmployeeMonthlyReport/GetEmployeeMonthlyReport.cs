//using HRsystem.Api.Database;
//using HRsystem.Api.Database.DataTables;
//using HRsystem.Api.Features.EmployeeDashboard.EmployeeApp;
//using HRsystem.Api.Services.CurrentUser;
//using HRsystem.Api.Shared.DTO;
//using HRsystem.Api.Shared.ExceptionHandling;
//using MediatR;
//using Microsoft.EntityFrameworkCore;
//using System.Collections.Generic;
//using System.ComponentModel.DataAnnotations;
//using System.Globalization;
//using System.Text.Json;

//namespace HRsystem.Api.Features.EmployeeDashboard.EmployeeMonthlyReport
//{
//    public record GetEmployeeMonthlyReport() : IRequest<GetEmployeeMonthlyReportDto>;

//    public class GetEmployeeMonthlyReportDto
//    {
//        public int DayId { get; set; }

//        public DateTime Date { get; set; }
//        public int EmployeeId { get; set; }

//        [MaxLength(200)]
//        public string EnglishFullName { get; set; }   // الاسم بالإنجليزي

//        [MaxLength(200)]
//        public string ArabicFullName { get; set; }    // الاسم بالعربي

//        public int ContractTypeId { get; set; }

//        [MaxLength(55)]
//        public string EmployeeCodeFinance { get; set; }

//        [MaxLength(55)]
//        public string EmployeeCodeHr { get; set; }

//        public int JobTitleId { get; set; }

//        public int? JobLevelId { get; set; }
//        public int ManagerId { get; set; }

//        public int CompanyId { get; set; }

//        public int DepartmentId { get; set; }

//        public int ShiftId { get; set; }

//        public int WorkDaysId { get; set; }
//        public int? RemoteWorkDaysId { get; set; }

//        public long ActivityId { get; set; }


//        public int ActivityTypeId { get; set; }

//        public int StatusId { get; set; }

//        public long RequestBy { get; set; }

//        public long? ApprovedBy { get; set; }

//        public DateTime RequestDate { get; set; }

//        public DateTime? ApprovedDate { get; set; }

//        public long AttendanceId { get; set; }


//        public DateTime AttendanceDate { get; set; }

//        public DateTime? FirstPuchin { get; set; }

//        public statues AttStatues { get; set; } // for know if ontime or late
//        public DateTime? LastPuchout { get; set; }
//        [Precision(5, 2)]
//        public decimal? TotalHours { get; set; }

//        [Precision(5, 2)]
//        public decimal? ActualWorkingHours { get; set; }

//        public bool IsHoliday { get; set; }

//        public bool IsWorkday { get; set; }

//        public bool IsRemoteday { get; set; }

//        public LocalizedData Details { get; set; }

//    }

//    public class GetEmployeeMonthlyReportHandler : IRequestHandler<GetEmployeeMonthlyReport, GetEmployeeMonthlyReportDto>
//    {
//        private readonly DBContextHRsystem _db;

//        private readonly ICurrentUserService _currentUser;


//        public GetEmployeeMonthlyReportHandler(DBContextHRsystem db, ICurrentUserService currentUser)
//        {
//            _db = db;
//            _currentUser = currentUser;
//        }
//        public async Task<EmployeeAnnualBalanceDto> Handle(EmployeeAnnualBalance request, CancellationToken ct)
//        {

//            DateTime Today = DateTime.Now.Date;
//            //string dayName = Today.DayOfWeek.ToString();
//            // Output: "Monday"


//            var employeeActivities = await _db.TbEmployeeActivities
//                                    .GroupBy(a => a.EmployeeId)
//                                    .Select(g => new
//                                    {
//                                        EmployeeId = g.Key,
//                                        Activities = g
//                                            .Where(a => a.RequestDate.Date == Today)
//                                            .Select(a => new
//                                            {
//                                                a.ActivityId,
//                                                a.RequestDate,
//                                                a.StatusId,
//                                                a.EmployeeId,
//                                                a.CompanyId,
//                                                a.ActivityTypeId,
//                                                a.ApprovedDate,
//                                                a.ApprovedBy
//                                            })
//                                            .ToList()
//                                    })
//                                    .ToListAsync(ct);


//            foreach (var employeeGroup in employeeActivities)
//            {/*
//                var employeeinfo =  await _db.TbEmployees.FirstOrDefaultAsync(e => e.EmployeeId == employeeGroup.EmployeeId);

//                var jobTittle = await _db.TbJobTitles.FirstOrDefaultAsync(e => e.JobTitleId == employeeinfo.JobTitleId && e.CompanyId == employeeinfo.CompanyId && e.DepartmentId == employeeinfo.DepartmentId) ;

//                var joblevel = await _db.TbJobLevels.FirstOrDefaultAsync(e => e.JobLevelId == jobTittle.JobLevelId);

//                var remotedays = await _db.TbRemoteWorkDays.FirstOrDefaultAsync(e => e.RemoteWorkDaysId == employeeinfo.RemoteWorkDaysId);

//                var workdays = await _db.TbWorkDays.FirstOrDefaultAsync(e => e.WorkDaysId == employeeinfo.WorkDaysId);
//                */
//                var employeeDetails = await _db.TbEmployees
//                                            .Where(e => e.EmployeeId == employeeGroup.EmployeeId)
//                                            .Include(e => e.JobTitle)
//                                                .ThenInclude(j => j.JobLevel)
//                                            .Include(e => e.RemoteWorkDayS)
//                                            .Include(e => e.WorkDays)
//                                            .FirstOrDefaultAsync(ct);



//                var workDays = JsonSerializer.Deserialize<List<string>>(employeeDetails.WorkDays.WorkDaysNames);
//                var remoteworkDays = JsonSerializer.Deserialize<List<string>>(employeeDetails.RemoteWorkDayS.RemoteWorkDaysNames);

//                // 2️⃣ Get today's name
//                string todayName = DateTime.Now.ToString("dddd", new CultureInfo("en-US"));

//                // 3️⃣ Compare (case-insensitive)
//                dayReport.IsRemoteWorkday = remoteworkDays.Any(day =>
//                    day.Equals(todayName, StringComparison.OrdinalIgnoreCase));

//                // 3️⃣ Compare (case-insensitive)
//                dayReport.IsWorkday = workDays.Any(day =>
//                    day.Equals(todayName, StringComparison.OrdinalIgnoreCase));

//                var hoildays = await _db.TbHolidays;

//                foreach (var a in hoildays)
//                {
//                    var hoilworkDays = JsonSerializer.Deserialize<List<string>>(a.HolidayName);

//                    dayReport.IsHoilday = hoilworkDays.Any(day =>
//                   day.Equals(todayName, StringComparison.OrdinalIgnoreCase));

//                }

//                foreach (var activity in employeeGroup.Activities)
//                {
//                    var dayReport = await _db.TbEmployeeMonthlyReports
//                            .FirstOrDefaultAsync(e => e.EmployeeId == employeeGroup.EmployeeId && e.Date.Date == Today, ct);

//                    switch (activity.ActivityTypeId)
//                    {
//                        case 1: // atten
//                            var attendance = await _db.TbEmployeeAttendances.FirstOrDefaultAsync(e => e.ActivityId == activity.ActivityId);
//                            /*
//                             * update attendance's data of this day 
//                             */

//                            if(dayReport ==  null)
//                            {
//                                // create
//                                var DayMonthReport = new GetEmployeeMonthlyReportDto
//                                {
//                                        Date = Today,
//                                        EmployeeId = employeeDetails.EmployeeId,
//                                        EnglishFullName= employeeDetails.EnglishFullName,
//                                        ArabicFullName = employeeDetails.ArabicFullName,
//                                        ContractTypeId = employeeDetails.ContractTypeId,
//                                        EmployeeCodeFinance = employeeDetails.EmployeeCodeFinance,
//                                        EmployeeCodeHr = employeeDetails.EmployeeCodeHr,
//                                        JobTitleId = employeeDetails.JobTitleId,
//                                        JobLevelId = employeeDetails.JobTitle.JobLevelId,
//                                        ManagerId = employeeDetails.ManagerId,
//                                        CompanyId = employeeDetails.CompanyId,
//                                        DepartmentId = employeeDetails.DepartmentId,
//                                        ShiftId = employeeDetails.ShiftId,
//                                        WorkDaysId = employeeDetails.WorkDaysId,
//                                        RemoteWorkDaysId = employeeDetails.RemoteWorkDaysId,
//                                        ActivityId = activity.ActivityId,
//                                        ActivityTypeId = activity.ActivityTypeId,
//                                        StatusId = activity.StatusId,
//                                        ApprovedBy = activity.ApprovedBy,
//                                        RequestDate = activity.RequestDate,
//                                        ApprovedDate = activity.ApprovedDate,
//                                        AttendanceId = attendance.AttendanceId,
//                                        AttendanceDate = attendance.AttendanceDate,
//                                        FirstPuchin = attendance.FirstPuchin,
//                                        AttStatues = attendance.AttStatues,
//                                        LastPuchout = attendance.LastPuchout,
//                                        TotalHours = attendance.TotalHours,
//                                        ActualWorkingHours = attendance.ActualWorkingHours,
//                                };

//                                _db.TbEmployeeMonthlyReports.Add(DayMonthReport);
//                                await _db.SaveChangesAsync(ct);
//                            }
//                            else
//                            {
//                                // update
//                                var DayMonthReport = new GetEmployeeMonthlyReportDto
//                                {
//                                    DayId = dayReport.DayId,
//                                    ActivityId = activity.ActivityId,
//                                    ActivityTypeId = activity.ActivityTypeId,
//                                    StatusId = activity.StatusId,
//                                    ApprovedBy = activity.ApprovedBy,
//                                    RequestDate = activity.RequestDate,
//                                    ApprovedDate = activity.ApprovedDate,
//                                    AttendanceId = attendance.AttendanceId,
//                                    AttendanceDate = attendance.AttendanceDate,
//                                    FirstPuchin = attendance.FirstPuchin,
//                                    AttStatues = attendance.AttStatues,
//                                    LastPuchout = attendance.LastPuchout,
//                                    TotalHours = attendance.TotalHours,
//                                    ActualWorkingHours = attendance.ActualWorkingHours,
//                                };
//                                _db.TbEmployeeMonthlyReports.Add(DayMonthReport);
//                                await _db.SaveChangesAsync(ct);

//                            }
//                                break;
//                        case 5: // vacation
//                            var vacation = await _db.TbEmployeeVacations.FirstOrDefaultAsync(e => e.ActivityId == activity.ActivityId);
//                            /*update with data */
//                            int count = 0;
//                                foreach(var e in vacation.DaysCount)
//                                {

//                                var DayMonthReport = new list<GetEmployeeMonthlyReportDto>
//                                {
//                                    Date = Today + count,
//                                    EmployeeId = employeeDetails.EmployeeId,
//                                    EnglishFullName = employeeDetails.EnglishFullName,
//                                    ArabicFullName = employeeDetails.ArabicFullName,
//                                    ContractTypeId = employeeDetails.ContractTypeId,
//                                    EmployeeCodeFinance = employeeDetails.EmployeeCodeFinance,
//                                    EmployeeCodeHr = employeeDetails.EmployeeCodeHr,
//                                    JobTitleId = employeeDetails.JobTitleId,
//                                    JobLevelId = employeeDetails.JobTitle.JobLevelId,
//                                    ManagerId = employeeDetails.ManagerId,
//                                    CompanyId = employeeDetails.CompanyId,
//                                    DepartmentId = employeeDetails.DepartmentId,
//                                    ShiftId = employeeDetails.ShiftId,
//                                    WorkDaysId = employeeDetails.WorkDaysId,
//                                    RemoteWorkDaysId = employeeDetails.RemoteWorkDaysId,
//                                    StatusId= activity.StatusId,

//                                };
//                                count++;
//                                _db.TbEmployeeMonthlyReports.Add(DayMonthReport);
//                                await _db.SaveChangesAsync(ct);
//                            }
//                                break;
//                        case 4: //mission
//                            var mission = await _db.TbEmployeeMissions.FirstOrDefaultAsync(e => e.ActivityId == activity.ActivityId);

//                            if (dayReport == null)
//                            {
//                                // create
//                                var DayMonthReport = new GetEmployeeMonthlyReportDto
//                                {
//                                    Date = Today,
//                                    EmployeeId = employeeDetails.EmployeeId,
//                                    EnglishFullName = employeeDetails.EnglishFullName,
//                                    ArabicFullName = employeeDetails.ArabicFullName,
//                                    ContractTypeId = employeeDetails.ContractTypeId,
//                                    EmployeeCodeFinance = employeeDetails.EmployeeCodeFinance,
//                                    EmployeeCodeHr = employeeDetails.EmployeeCodeHr,
//                                    JobTitleId = employeeDetails.JobTitleId,
//                                    JobLevelId = employeeDetails.JobTitle.JobLevelId,
//                                    ManagerId = employeeDetails.ManagerId,
//                                    CompanyId = employeeDetails.CompanyId,
//                                    DepartmentId = employeeDetails.DepartmentId,
//                                    ShiftId = employeeDetails.ShiftId,
//                                    WorkDaysId = employeeDetails.WorkDaysId,
//                                    RemoteWorkDaysId = employeeDetails.RemoteWorkDaysId,
//                                    StatusId = activity.StatusId,

//                                };

//                                _db.TbEmployeeMonthlyReports.Add(DayMonthReport);
//                                await _db.SaveChangesAsync(ct);
//                            }
//                            else
//                            {
//                                // update
//                                var DayMonthReport = new GetEmployeeMonthlyReportDto
//                                {
//                                    DayId = dayReport.DayId,
//                                    StatusId = activity.StatusId,
//                                };
//                                _db.TbEmployeeMonthlyReports.Add(DayMonthReport);
//                                await _db.SaveChangesAsync(ct);

//                            }

//                            break;
//                        case 6: //excuse

//                            var excuse = await _db.TbEmployeeExcuses.FirstOrDefaultAsync(e => e.ActivityId == activity.ActivityId);

//                            if (dayReport == null)
//                            {
//                                // create
//                                var DayMonthReport = new GetEmployeeMonthlyReportDto
//                                {
//                                    Date = Today,
//                                    EmployeeId = employeeDetails.EmployeeId,
//                                    EnglishFullName = employeeDetails.EnglishFullName,
//                                    ArabicFullName = employeeDetails.ArabicFullName,
//                                    ContractTypeId = employeeDetails.ContractTypeId,
//                                    EmployeeCodeFinance = employeeDetails.EmployeeCodeFinance,
//                                    EmployeeCodeHr = employeeDetails.EmployeeCodeHr,
//                                    JobTitleId = employeeDetails.JobTitleId,
//                                    JobLevelId = employeeDetails.JobTitle.JobLevelId,
//                                    ManagerId = employeeDetails.ManagerId,
//                                    CompanyId = employeeDetails.CompanyId,
//                                    DepartmentId = employeeDetails.DepartmentId,
//                                    ShiftId = employeeDetails.ShiftId,
//                                    WorkDaysId = employeeDetails.WorkDaysId,
//                                    RemoteWorkDaysId = employeeDetails.RemoteWorkDaysId,
//                                    StatusId = activity.StatusId,

//                                };

//                                _db.TbEmployeeMonthlyReports.Add(DayMonthReport);
//                                await _db.SaveChangesAsync(ct);
//                            }
//                            else
//                            {
//                                // update
//                                var DayMonthReport = new GetEmployeeMonthlyReportDto
//                                {
//                                    DayId = dayReport.DayId,
//                                    StatusId = activity.StatusId,
//                                };
//                                _db.TbEmployeeMonthlyReports.Add(DayMonthReport);
//                                await _db.SaveChangesAsync(ct);

//                            }

//                            break;

//                        default:
//                            // we won't enter here
//                            break;


//                    }


//                }
//            }

//        }

//    }
//}
using HRsystem.Api.Database;
using HRsystem.Api.Database.DataTables;
using HRsystem.Api.Migrations;
using HRsystem.Api.Services.CurrentUser;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics.Eventing.Reader;
using System.Globalization;
using System.Text.Json;
using static HRsystem.Api.Enums.EnumsList;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace HRsystem.Api.Features.EmployeeDashboard.EmployeeMonthlyReport
{
    public record GetEmployeeMonthlyReport() : IRequest<string>; // return success message

    public class GetEmployeeMonthlyReportHandler : IRequestHandler<GetEmployeeMonthlyReport, string>
    {
        private readonly DBContextHRsystem _db;
        private readonly ICurrentUserService _currentUser;

        public GetEmployeeMonthlyReportHandler(DBContextHRsystem db, ICurrentUserService currentUser)
        {
            _db = db;
            _currentUser = currentUser;
        }

        public async Task<string> Handle(GetEmployeeMonthlyReport request, CancellationToken ct)
        {
            var Today = DateTime.Now.Date;
            string todayName = DateTime.Now.ToString("dddd", new CultureInfo("en-US"));
            bool Flagcheck= false;
            var employees = await _db.TbEmployees
                .Include(e => e.JobTitle).ThenInclude(j => j.JobLevel)
                .Include(e => e.RemoteWorkDayS)
                .Include(e => e.WorkDays)
                .ToListAsync(ct);



            foreach (var employee in employees)
            {
                var workDays = employee.WorkDays?.WorkDaysNames ?? new List<string>();
                var remoteDays = employee.RemoteWorkDayS?.RemoteWorkDaysNames ?? new List<string>();

                var isHoliday = await _db.TbHolidays
                        .AnyAsync(h =>
                            h.IsActive &&
                            Today >= h.StartDate.Date &&
                            Today <= h.EndDate.Date &&
                            (!h.IsForChristiansOnly || employee.Religion == EnumReligionType.Christian),
                            ct);

                bool isWorkday = workDays.Any(d => d.Equals(todayName, StringComparison.OrdinalIgnoreCase));
                bool isRemoteDay = remoteDays.Any(d => d.Equals(todayName, StringComparison.OrdinalIgnoreCase));

                var activities = await _db.TbEmployeeActivities
                    .Where(a => a.EmployeeId == employee.EmployeeId && a.RequestDate.Date == Today)
                    .ToListAsync(ct);

                var existingDayReport = await _db.TbEmployeeMonthlyReports
                    .FirstOrDefaultAsync(r => r.EmployeeId == employee.EmployeeId && r.Date == Today, ct);

                // Create base report if not exists
                if (existingDayReport == null)
                {
                    existingDayReport = new TbEmployeeMonthlyReport
                    {
                        EmployeeId = employee.EmployeeId,
                        Date = Today,
                        CompanyId = employee.CompanyId,
                        DepartmentId = employee.DepartmentId,
                        ShiftId = employee.ShiftId,
                        WorkDaysId = employee.WorkDaysId,
                        RemoteWorkDaysId = employee.RemoteWorkDaysId,
                        IsWorkday = isWorkday,
                        IsRemoteday = isRemoteDay,
                        IsHoliday = isHoliday
                    };
                    _db.TbEmployeeMonthlyReports.Add(existingDayReport);
                    await _db.SaveChangesAsync(ct);
                }

                    if (isHoliday)// kda tmam
                    {
                        existingDayReport.StatusId =  1; //  anything represent statues of day
                        existingDayReport.TodayStatues = " Hoilday";
                       // existingDayReport.Details = new { Type = "Hoilday", Hoilday.StartDate, Hoilday.EndDate };
                    }
                    else if (isRemoteDay)// also tmam must be check here
                    {
                        existingDayReport.StatusId = 2; //  anything represent statues of day
                        existingDayReport.TodayStatues = " RemoteDay";
                        //existingDayReport.Details = new { Type = "RemoteDay", RemoteDay.StartDate, RemoteDay.EndDate };

                    }
                    else if (isWorkday)// kda absent
                    {
                       existingDayReport.StatusId = 3; // anything represent statues of day
                       existingDayReport.TodayStatues = " Workday";
                       //existingDayReport.Details = new { Type = "Workday", Workday.StartDate, Workday.EndDate };
                    }

                
                    // update in report
                    foreach (var activity in activities)
                    {
                        switch (activity.ActivityTypeId)
                        {
                            case 1: // Attendance
                                var attendance = await _db.TbEmployeeAttendances
                                    .FirstOrDefaultAsync(a => a.ActivityId == activity.ActivityId, ct);
                                if (attendance != null)
                                {
                                    existingDayReport.FirstPuchin = attendance.FirstPuchin;
                                    existingDayReport.LastPuchout = attendance.LastPuchout;
                                    existingDayReport.TotalHours = attendance.TotalHours;
                                    existingDayReport.ActualWorkingHours = attendance.ActualWorkingHours;
                                    existingDayReport.AttStatues = attendance.AttStatues;
                                    Flagcheck = true;
                                    existingDayReport.Details = new
                                    {
                                        Type = "Attendance",
                                        attendance.AttendanceDate,
                                        attendance.FirstPuchin,
                                        attendance.LastPuchout,
                                        attendance.TotalHours,
                                        attendance.ActualWorkingHours,
                                        attendance.AttStatues
                                    };


                            }
                            break;

                            case 5: // Vacation
                                var vacation = await _db.TbEmployeeVacations
                                    .FirstOrDefaultAsync(v => v.ActivityId == activity.ActivityId, ct);
                                if (vacation != null)
                                {
                                    for (int i = 0; i < vacation.DaysCount; i++)
                                    {
                                        var day = vacation.StartDate.AddDays(i);
                                    var newReport = new TbEmployeeMonthlyReport
                                    {
                                        EmployeeId = employee.EmployeeId,
                                        Date = Today,
                                        CompanyId = employee.CompanyId,
                                        DepartmentId = employee.DepartmentId,
                                        ShiftId = employee.ShiftId,
                                        WorkDaysId = employee.WorkDaysId,
                                        RemoteWorkDaysId = employee.RemoteWorkDaysId,
                                        IsWorkday = isWorkday,
                                        IsRemoteday = isRemoteDay,
                                        IsHoliday = isHoliday,
                                        TodayStatues = "Vacation",
                                        Details = new
                                        {
                                            Type = "Vacation",
                                            vacation.VacationTypeId,
                                            vacation.StartDate,
                                            vacation.EndDate,
                                            vacation.DaysCount,
                                        }
                                    };
                                        _db.TbEmployeeMonthlyReports.Add(newReport);
                                    }
                                }
                                break;

                            case 4: // Mission
                                var mission = await _db.TbEmployeeMissions
                                    .FirstOrDefaultAsync(m => m.ActivityId == activity.ActivityId, ct);
                                if (mission != null)
                                {
                                    existingDayReport.TodayStatues += " Mission";
                                // existingDayReport.Details = new { Type = "Mission", mission.StartDate, mission.EndDate };
                                    existingDayReport.Details = new
                                    {
                                        Type = "Mission",
                                        mission.StartDatetime,
                                        mission.EndDatetime,
                                        mission.MissionLocation,
                                        mission.MissionReason, 
                                    };
                                Flagcheck = true;
                                }
                                break;

                            case 6: // Excuse
                                var excuse = await _db.TbEmployeeExcuses
                                    .FirstOrDefaultAsync(e => e.ActivityId == activity.ActivityId, ct);
                                if (excuse != null)
                                {
                                    existingDayReport.TodayStatues += " Excuse";
                                //existingDayReport.Details = new { Type = "Excuse", excuse.StartDate, excuse.EndDate };
                                    existingDayReport.Details = new
                                    {
                                        Type = "Excuse",
                                        excuse.StartTime,
                                        excuse.EndTime,
                                        excuse.ExcuseReason,
                                        excuse.ExcuseDate,
                  
                                    };
                                }
                                break;

                            default:
                                break;
                        }
                    }    
                    await _db.SaveChangesAsync(ct);
            }
           return "Monthly report updated successfully";
        }
    }
}
