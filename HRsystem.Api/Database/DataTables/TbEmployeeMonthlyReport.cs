using HRsystem.Api.Features.EmployeeDashboard.EmployeeApp;
using HRsystem.Api.Shared.DTO;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace HRsystem.Api.Database.DataTables;

[Table("Tb_Employee_Monthly_Report")]


public partial class TbEmployeeMonthlyReport
{
    [Key]
    public int DayId { get; set; }

    public DateTime Date { get; set; }
    public int EmployeeId { get; set; }

    [MaxLength(200)]
    public string EnglishFullName { get; set; }   // الاسم بالإنجليزي

    [MaxLength(200)]
    public string ArabicFullName { get; set; }    // الاسم بالعربي

    public int ContractTypeId { get; set; }

    [MaxLength(55)]
    public string EmployeeCodeFinance { get; set; }

    [MaxLength(55)]
    public string EmployeeCodeHr { get; set; }

    public int JobTitleId { get; set; }

    public int JobLevelId { get; set; }
    public int ManagerId { get; set; }

    public int CompanyId { get; set; }

    public int DepartmentId { get; set; }

    public int ShiftId { get; set; }

    public int WorkDaysId { get; set; }
    public int? RemoteWorkDaysId { get; set; }

    public long ActivityId { get; set; }


    public int ActivityTypeId { get; set; }

    /*for all*/
    public int StatusId { get; set; }

    public long RequestBy { get; set; }

    public long? ApprovedBy { get; set; }

    public DateTime RequestDate { get; set; }

    public DateTime? ApprovedDate { get; set; }

    public long AttendanceId { get; set; }


    public DateTime AttendanceDate { get; set; }

    public DateTime? FirstPuchin { get; set; }

    public statues? AttStatues { get; set; } // for know if ontime or late
    public DateTime? LastPuchout { get; set; }
    [Precision(5, 2)]
    public decimal? TotalHours { get; set; }

    [Precision(5, 2)]
    public decimal? ActualWorkingHours { get; set; }

    public bool IsHoliday { get; set; } 

    public bool IsWorkday { get; set; }

    public bool IsRemoteday { get; set; }

    public string TodayStatues { get; set; } = "Attendance";
    
    [Column(TypeName = "json")]
    public string Details { get; set; } = "{}";

}

    