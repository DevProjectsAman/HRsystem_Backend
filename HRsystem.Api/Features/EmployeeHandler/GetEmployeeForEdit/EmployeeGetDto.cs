using HRsystem.Api.Shared.DTO;
using static HRsystem.Api.Enums.EnumsList;

namespace HRsystem.Api.Features.EmployeeHandler.GetEmployeeForEdit
{
    public class EmployeeEditDto
    {

        public EmployeeBasicDataEditDto EmployeeBasicDataEdit { get; set; } = new();

        public EmployeeExtraDataEditDto EmployeeExtraDataEdit { get; set; } = new();

        public EmployeeOrganizationEditDto EmployeeOrganizationEdit { get; set; } = new();

        public EmployeeOrganizationHiringEditDto EmployeeOrganizationHiringEdit { get; set; } = new();


        public EmployeeWorkLocationsEditDto EmployeeWorkLocationsEdit { get; set; } = new();


        public EmployeeShiftWorkDaysEditDto EmployeeShiftWorkDaysEdit { get; set; } = new();


        public EmployeeVacationsBalanceListEditDto EmployeeVacationsBalanceEdit { get; set; } = new();



    }


    // ✅ Basic personal data
    public class EmployeeBasicDataEditDto
    {
        public string UniqueEmployeeCode { get; set; } = string.Empty;  // ✅ Add this


        public string EnglishFullName { get; set; }


        public string ArabicFullName { get; set; }

        public string NationalId { get; set; } = string.Empty;


        public DateOnly Birthdate { get; set; } = DateOnly.FromDateTime(DateTime.Now);

        public string? PlaceOfBirth { get; set; }

        public EnumGenderType Gender { get; set; }


        public string? EmployeePhotoFileName { get; set; }
    }

    public class EmployeeVacationsBalanceListEditDto
    {

        public List<EmployeeVacationBalanceEditDto> EmployeeVacationBalancesEdit { get; set; } = new();
    }

    public class EmployeeVacationBalanceEditDto
    {

        public int VacationTypeId { get; set; }


        public int Year { get; set; } = DateTime.Now.Year;


        public decimal TotalDays { get; set; } = 0;

        ///  those just for reminde the HR  user but will be disabled when edit 
        public string? VacationTypeName { get; set; }

        public bool? Prorate { get; set; } = false;
        public int? Priority { get; set; } = 1;

        // Vacation restrictions (specific to type)

        public EnumGenderType Gender { get; set; } = EnumGenderType.All;

        public EnumReligionType Religion { get; set; } = EnumReligionType.All;


    }

 

    public class EmployeeExtraDataEditDto
    {




        public string? PassportNumber { get; set; }


        public int MaritalStatusId { get; set; }


        public int NationalityId { get; set; }


        public string? Email { get; set; }

        public string PrivateMobile { get; set; }

        public string? BuisnessMobile { get; set; }

        public string? Address { get; set; }


        public EnumReligionType Religion { get; set; }


        public string? Note { get; set; }
    }


    public class EmployeeOrganizationEditDto
    {
        public int CompanyId { get; set; }

        public int DepartmentId { get; set; }

        public int JobLevelId { get; set; }

        public int JobTitleId { get; set; }

        // ManagerId is optional, so no validation needed unless business rule says otherwise
        public int? ManagerId { get; set; }

        public int? ProjectId { get; set; } // nullable → optional


    }

    public class EmployeeOrganizationHiringEditDto
    {


        public int ContractTypeId { get; set; }


        //[Required, EmployeeStep(4), MaxLength(25)]
        public string? SerialMobile { get; set; } = string.Empty;



        public string? EmployeeCodeFinance { get; set; }
        public string? EmployeeCodeHr { get; set; }


        public DateOnly HireDate { get; set; } = DateOnly.FromDateTime(DateTime.Now);


        public DateTime StartDate { get; set; } = DateTime.Now;

        public DateTime? EndDate { get; set; }


        public string Status { get; set; } = "Active";
    }
    // ✅ Work conditions
    public class EmployeeWorkLocationsEditDto
    {


        public List<WorkLocationEditDto> EmployeeWorkLocationsEdit { get; set; } = new();


    }

    public class EmployeeShiftWorkDaysEditDto
    {

        public int ShiftId { get; set; }


        public int WorkDaysId { get; set; }
    }





    public class WorkLocationEditDto
    {
        public int WorkLocationId { get; set; }

        public int CompanyId { get; set; }
        public string WorkLocationCode { get; set; }


        public LocalizedData LocationName { get; set; } = new LocalizedData();


        public double? Latitude { get; set; }

        public double? Longitude { get; set; }

        public int? AllowedRadiusM { get; set; }

        public int? CityId { get; set; }
        public string CityName { get; set; }
        public int? GovId { get; set; }
        public string GovName { get; set; }
    }


}
