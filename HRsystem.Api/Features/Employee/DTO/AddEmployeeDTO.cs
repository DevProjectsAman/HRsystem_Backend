namespace HRsystem.Api.Features.Employee.DTO
{
    public class AddEmployeeDto
    {
        // Core Employee Info
        public string EmployeeCodeFinance { get; set; }
        public string EmployeeCodeHR { get; set; }
        public int JobTitleId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string ArabicFirstName { get; set; }
        public string ArabicLastName { get; set; }
        public DateTime HireDate { get; set; }
        public DateTime BirthDate { get; set; }
        public string Gender { get; set; }
        public string NationalId { get; set; }
        public string PassportNumber { get; set; }
        public string MaritalStatus { get; set; }
        public int? ManagerId { get; set; }   // FK to tb_employee

        public int CompanyId { get; set; }
        public int DepartmentId { get; set; }

        // Contact Info
        public string PrivateMobile { get; set; }
        public string BusinessMobile { get; set; }
        public string Email { get; set; }

        // Shifts (one employee can have multiple shift assignments over time)
        public List<EmployeeShiftDto> Shifts { get; set; } = new();

        // Work Locations (employee can be assigned to multiple)
        public List<EmployeeWorkLocationDto> WorkLocations { get; set; } = new();

        // Initial Vacation Balances
        public List<EmployeeVacationBalanceDto> VacationBalances { get; set; } = new();
    }

    public class EmployeeShiftDto
    {
        public int ShiftId { get; set; }    // FK to tb_shift
        public DateTime EffectiveDate { get; set; }
        public DateTime? EndDate { get; set; }
        public string Notes { get; set; }
    }

    public class EmployeeWorkLocationDto
    {
        public int WorkLocationId { get; set; } // FK to tb_work_location
        public int CityId { get; set; }         // optional extra if you need multi-level
    }

    public class EmployeeVacationBalanceDto
    {
        public int VacationTypeId { get; set; }
        public int Year { get; set; }
        public decimal TotalDays { get; set; }
        public decimal UsedDays { get; set; }
        public decimal RemainingDays { get; set; }
    }

}
