using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HRsystem.Api.Database.DataTables;

[Table("Tb_Employee")]
public partial class TbEmployee
{
    [Key]
    public int EmployeeId { get; set; }

    public string? EmployeeCodeFinance { get; set; }

    public string? EmployeeCodeHr { get; set; }

    public int JobTitleId { get; set; }

    public string FirstName { get; set; } = null!;

    public string? ArabicFirstName { get; set; }

    public string LastName { get; set; } = null!;

    public string? ArabicLastName { get; set; }

    public string? ArabicFullName { get; set; }

    public DateOnly? HireDate { get; set; }

    public DateOnly? Birthdate { get; set; }

    public string Gender { get; set; } = null!;

    public string? Nationality { get; set; }

    public string? NationalId { get; set; }

    public string? PassportNumber { get; set; }

    public string? MaritalStatus { get; set; }

    public string? PlaceOfBirth { get; set; }

    public string? BloodGroup { get; set; }

    public int? ManagerId { get; set; }

    public int CompanyId { get; set; }

    public int? CreatedBy { get; set; }

    public DateTime? CreatedAt { get; set; }

    public int? UpdatedBy { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public string? FullName { get; set; }

    public string? UserName { get; set; }

    public string? ArabicUserName { get; set; }

    public string? PrivateMobile { get; set; }

    public string? BuisnessMobile { get; set; }

    public string? Email { get; set; }

    public string? SerialMobile { get; set; }

    public DateTime? StartDate { get; set; }

    public DateTime? EndDate { get; set; }

    public sbyte? IsTopmanager { get; set; }

    public sbyte? IsFulldocument { get; set; }

    public string? Note { get; set; }

    public string? Status { get; set; }

    public string? Password { get; set; }

    public int? CategoryId { get; set; }

    public int? DepartmentId { get; set; }

    public int? ShiftId { get; set; }

    public virtual TbCompany Company { get; set; } = null!;

    public virtual ICollection<TbEmployee> InverseManager { get; set; } = new List<TbEmployee>();

    public virtual TbJobTitle JobTitle { get; set; } = null!;

    public virtual TbEmployee? Manager { get; set; }

    public virtual ICollection<TbEmployeeActivity> TbEmployeeActivities { get; set; } = new List<TbEmployeeActivity>();

    public virtual ICollection<TbEmployeeProject> TbEmployeeProjects { get; set; } = new List<TbEmployeeProject>();

    public virtual ICollection<TbEmployeeShift> TbEmployeeShifts { get; set; } = new List<TbEmployeeShift>();

    public virtual ICollection<TbEmployeeVacationBalance> TbEmployeeVacationBalances { get; set; } = new List<TbEmployeeVacationBalance>();

    public virtual ICollection<TbEmployeeWorkLocation> TbEmployeeWorkLocations { get; set; } = new List<TbEmployeeWorkLocation>();
}
