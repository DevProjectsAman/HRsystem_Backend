using HRsystem.Api.Database.DataTables;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace HRsystem.Api.Features.EmployeeVacation
{
    public class EpmloyeeVacationDto
    {

        public long VacationId { get; set; }

        public long ActivityId { get; set; }

        public DateOnly StartDate { get; set; }

        public DateOnly EndDate { get; set; }

        public int VacationTypeId { get; set; }

        [MaxLength(255)]
        public string? Notes { get; set; }

        public int? DaysCount { get; set; }
    }

    public class EmployeeVacationBalanceDto
    {

        public int BalanceId { get; set; }

        public int EmployeeId { get; set; }

        public int VacationTypeId { get; set; }

        public int Year { get; set; }

        [Precision(5, 2)]
        public decimal TotalDays { get; set; }
        [Precision(5, 2)]
        public decimal? UsedDays { get; set; }
        [Precision(5, 2)]
        public decimal? RemainingDays { get; set; }

    }

    public class VacationTypeDto
    {
        public int VacationTypeId { get; set; }

        //[MaxLength(55)]
        public string VacationName { get; set; } = null!;

        [MaxLength(80)]
        public string? Description { get; set; }

        public bool? IsPaid { get; set; }

        public bool? RequiresHrApproval { get; set; }
    }
}
