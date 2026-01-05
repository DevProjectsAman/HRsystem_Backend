using global::HRsystem.Api.Database;
using Microsoft.EntityFrameworkCore;
using static global::HRsystem.Api.Enums.EnumsList;



namespace HRsystem.Api.Services.VacationCalculation
    {
        public interface IVacationDaysCalculator
        {
            Task<VacationCalculationResult> CalculateActualVacationDaysAsync(
                int employeeId,
                DateOnly startDate,
                DateOnly endDate,
                CancellationToken cancellationToken = default);
        }

        public class VacationCalculationResult
        {
            public bool IsValid { get; set; }
            public decimal ActualDaysToDeduct { get; set; }
            public int TotalCalendarDays { get; set; }
            public int WorkingDaysInRange { get; set; }
            public int HolidaysInRange { get; set; }
            public int WeekendDaysInRange { get; set; }
            public List<DateOnly> HolidayDates { get; set; } = new();
            public List<DateOnly> WeekendDates { get; set; } = new();
            public List<DateOnly> WorkingDates { get; set; } = new();
            public List<string> HolidayNames { get; set; } = new();
            public string? ErrorMessage { get; set; }

            public string GetSummary() =>
                $"Total: {TotalCalendarDays} days | Working: {WorkingDaysInRange} days | " +
                $"Weekends: {WeekendDaysInRange} days | Holidays: {HolidaysInRange} days | " +
                $"To Deduct: {ActualDaysToDeduct} days";
        }

        public class VacationDaysCalculator : IVacationDaysCalculator
        {
            private readonly DBContextHRsystem _db;

            public VacationDaysCalculator(DBContextHRsystem db)
            {
                _db = db;
            }

            public async Task<VacationCalculationResult> CalculateActualVacationDaysAsync(
                int employeeId,
                DateOnly startDate,
                DateOnly endDate,
                CancellationToken cancellationToken = default)
            {
                var result = new VacationCalculationResult();

                try
                {
                    // Validate date range
                    if (endDate < startDate)
                    {
                        result.IsValid = false;
                        result.ErrorMessage = "End date cannot be before start date";
                        return result;
                    }

                    // Get employee with work days and navigation properties
                    var employee = await _db.TbEmployees
                        .Include(e => e.TbWorkDays)
                        .Include(e => e.Company)
                        .FirstOrDefaultAsync(e => e.EmployeeId == employeeId, cancellationToken);

                    if (employee == null)
                    {
                        result.IsValid = false;
                        result.ErrorMessage = "Employee not found";
                        return result;
                    }

                    // Get employee's working days (e.g., ["Sunday", "Monday", "Tuesday", "Wednesday", "Thursday"])
                    var workingDayNames = employee.TbWorkDays?.WorkDaysNames ?? new List<string>();

                    if (!workingDayNames.Any())
                    {
                        result.IsValid = false;
                        result.ErrorMessage = "Employee has no work days configured";
                        return result;
                    }

                    // Get holidays in the date range
                    var startDateTime = startDate.ToDateTime(TimeOnly.MinValue);
                    var endDateTime = endDate.ToDateTime(TimeOnly.MaxValue);

                    var holidays = await _db.TbHolidays
                        .Include(h => h.HolidayType)
                        .Where(h => h.IsActive &&
                                   h.StartDate <= endDateTime &&
                                   h.EndDate >= startDateTime &&
                                   (h.CompanyId == null || h.CompanyId == employee.CompanyId))
                        .ToListAsync(cancellationToken);

                    // Filter holidays based on employee's religion
                    var applicableHolidays = holidays.Where(h =>
                    {
                        // If holiday is for Christians only, check employee religion
                        if (h.IsForChristiansOnly)
                        {
                            return employee.Religion == EnumReligionType.Christian;
                        }
                        // If holiday is NOT for Christians only, it applies to everyone
                        return true;
                    }).ToList();

                    // Calculate actual vacation days
                    result.TotalCalendarDays = endDate.DayNumber - startDate.DayNumber + 1;

                    // Iterate through each day in the range
                    var currentDate = startDate;
                    while (currentDate <= endDate)
                    {
                        var dayOfWeek = currentDate.DayOfWeek.ToString();

                        // Check if this day is a holiday
                        bool isHoliday = applicableHolidays.Any(h =>
                        {
                            var holidayStart = DateOnly.FromDateTime(h.StartDate);
                            var holidayEnd = DateOnly.FromDateTime(h.EndDate);
                            return currentDate >= holidayStart && currentDate <= holidayEnd;
                        });

                        // Check if this day is a working day for the employee
                        bool isWorkingDay = workingDayNames.Contains(dayOfWeek, StringComparer.OrdinalIgnoreCase);

                        if (isHoliday)
                        {
                            // Holiday - don't count (even if it falls on a working day)
                            result.HolidaysInRange++;
                            result.HolidayDates.Add(currentDate);

                            // Get holiday name
                            var holiday = applicableHolidays.FirstOrDefault(h =>
                            {
                                var holidayStart = DateOnly.FromDateTime(h.StartDate);
                                var holidayEnd = DateOnly.FromDateTime(h.EndDate);
                                return currentDate >= holidayStart && currentDate <= holidayEnd;
                            });

                            if (holiday != null && !result.HolidayNames.Contains(holiday.HolidayName.ar))
                            {
                                result.HolidayNames.Add(holiday.HolidayName.ar);
                            }
                        }
                        else if (isWorkingDay)
                        {
                            // Working day and not a holiday - count it for deduction
                            result.WorkingDaysInRange++;
                            result.WorkingDates.Add(currentDate);
                        }
                        else
                        {
                            // Weekend (non-working day) - don't count
                            result.WeekendDaysInRange++;
                            result.WeekendDates.Add(currentDate);
                        }

                        currentDate = currentDate.AddDays(1);
                    }

                    // Actual days to deduct = only working days (excluding holidays and weekends)
                    result.ActualDaysToDeduct = result.WorkingDaysInRange;
                    result.IsValid = true;

                    return result;
                }
                catch (Exception ex)
                {
                    result.IsValid = false;
                    result.ErrorMessage = $"Error calculating vacation days: {ex.Message}";
                    return result;
                }
            }
        }
    }
