using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using static HRsystem.Api.Enums.EnumsList;

namespace HRsystem.Api.Database.DataTables;

[Table("Tb_Employee")]
public partial class TbEmployee
{
    [Key]
    public int EmployeeId { get; set; }

    [MaxLength(55)]
    public string EmployeeCodeFinance { get; set; }

    [MaxLength(55)]
    public string EmployeeCodeHr { get; set; }

    public int JobTitleId { get; set; }

    //[MaxLength(55)]
    //public string? FirstName { get; set; } = null!;

    //[MaxLength(55)]
    //public string? ArabicFirstName { get; set; }

    //[MaxLength(55)]
    //public string? LastName { get; set; } = null!;

    //[MaxLength(100)]
    //public string? ArabicLastName { get; set; }

    public DateOnly HireDate { get; set; }

    public DateOnly Birthdate { get; set; }

    [MaxLength(25)]
    public EnumGenderType Gender { get; set; } = EnumGenderType.Male;

    public int NationalityId { get; set; }   // FK

    [MaxLength(25)]
    public string NationalId { get; set; }

    [MaxLength(25)]
    public string? PassportNumber { get; set; }

    public int MaritalStatusId { get; set; }   // FK

    [MaxLength(55)]
    public string PlaceOfBirth { get; set; }

    [MaxLength(10)]
    public string? BloodGroup { get; set; }

    public int ManagerId { get; set; }

    public int CompanyId { get; set; }

    public int CreatedBy { get; set; }

    public DateTime CreatedAt { get; set; }

    public int UpdatedBy { get; set; }

    public DateTime UpdatedAt { get; set; }

    [MaxLength(25)]
    public string PrivateMobile { get; set; }

    [MaxLength(25)]
    public string? BuisnessMobile { get; set; }

    [MaxLength(25)]
    public string Email { get; set; }

    [MaxLength(25)]
    public string SerialMobile { get; set; }

    public DateTime StartDate { get; set; }

    public DateTime? EndDate { get; set; }

    public sbyte? IsTopmanager { get; set; }

    public sbyte? IsFulldocument { get; set; }

    public string? Note { get; set; }

    [MaxLength(25)]
    public string Status { get; set; }

    public int DepartmentId { get; set; }

    public int ShiftId { get; set; }

    public int WorkDaysId { get; set; }
    public int? RemoteWorkDaysId { get; set; }

    public virtual TbCompany Company { get; set; } = null!;

    public virtual ICollection<TbEmployee> InverseManager { get; set; } = new List<TbEmployee>();

    public virtual TbJobTitle JobTitle { get; set; } = null!;

    public virtual TbEmployee Manager { get; set; }

    public virtual TbDepartment Department { get; set; }

   

    public virtual ICollection<TbEmployeeActivity> TbEmployeeActivities { get; set; } = new List<TbEmployeeActivity>();

    public virtual ICollection<TbEmployeeProject> TbEmployeeProjects { get; set; } = new List<TbEmployeeProject>();

    public virtual ICollection<TbEmployeeShift> TbEmployeeShifts { get; set; } = new List<TbEmployeeShift>();

    public virtual ICollection<TbEmployeeVacationBalance> TbEmployeeVacationBalances { get; set; } = new List<TbEmployeeVacationBalance>();

    public virtual ICollection<TbEmployeeWorkLocation> TbEmployeeWorkLocations { get; set; } = new List<TbEmployeeWorkLocation>();

    [ForeignKey(nameof(NationalityId))]
    public virtual TbNationality Nationality { get; set; }

    [ForeignKey(nameof(ShiftId))]
    public virtual TbShift Shifts { get; set; }

    [ForeignKey(nameof(MaritalStatusId))]
    public virtual TbMaritalStatus MaritalStatus { get; set; }

    [ForeignKey(nameof(WorkDaysId))]
    public virtual TbWorkDays TbWorkDays { get; set; }

    [ForeignKey(nameof(RemoteWorkDaysId))]
    public virtual TbRemoteWorkDay? TbRemoteWorkDays { get; set; }

    [MaxLength(200)]
    public string EnglishFullName { get; set; }   // الاسم بالإنجليزي

    [MaxLength(200)]
    public string ArabicFullName { get; set; }    // الاسم بالعربي

    [MaxLength(250)]
    public string? Address { get; set; }          // العنوان

    [MaxLength(500)]
    public string? EmployeePhotoPath { get; set; } // صورة الموظف (path أو url)

    public int ContractTypeId { get; set; }


}