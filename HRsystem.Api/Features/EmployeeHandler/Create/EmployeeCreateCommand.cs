

using FluentValidation;
using MediatR;
using static global::HRsystem.Api.Enums.EnumsList;

namespace HRsystem.Api.Features.EmployeeHandler.Create
{


    #region Command

    public sealed record CreateEmployeeCommandNew(
        EmployeeBasicDataCommand EmployeeBasicData,
        EmployeeExtraDataCommand EmployeeExtraData,
        EmployeeOrganizationCommand EmployeeOrganization,
        EmployeeOrganizationHiringCommand EmployeeOrganizationHiring,
        EmployeeWorkLocationsCommand EmployeeWorkLocations,
        EmployeeShiftWorkDaysCommand EmployeeShiftWorkDays,
        EmployeeVacationsBalanceListCommand EmployeeVacationsBalance
    ) : IRequest<int>;


    #region Basic Data

    public sealed record EmployeeBasicDataCommand(
        string EnglishFullName,
        string ArabicFullName,
        string NationalId,
        DateOnly Birthdate,
        string? PlaceOfBirth,
        EnumGenderType Gender,
      //  string? EmployeePhotoPath,
           string UniqueEmployeeCode
    );

    #endregion

    #region Extra Data

    public sealed record EmployeeExtraDataCommand(
        string? PassportNumber,
        int MaritalStatusId,
        int NationalityId,
        string? Email,
        string PrivateMobile,
        string? BuisnessMobile,
        string? Address,
        EnumReligionType Religion,
        string? Note
    );

    #endregion

    #region Organization

    public sealed record EmployeeOrganizationCommand(
        int CompanyId,
        int DepartmentId,
        int JobLevelId,
        int JobTitleId,
        int ManagerId,
        int? ProjectId
    );

    #endregion

    #region Hiring

    public sealed record EmployeeOrganizationHiringCommand(
        int ContractTypeId,
        string SerialMobile,
        string? EmployeeCodeFinance,
        string? EmployeeCodeHr,
        DateOnly HireDate,
        DateTime StartDate,
        DateTime? EndDate,
        string Status
    );

    #endregion

    #region Work Locations

    public sealed record EmployeeWorkLocationsCommand(
        IReadOnlyList<EmployeeWorkLocationCommand> EmployeeWorkLocations
    );

    public sealed record EmployeeWorkLocationCommand(
        int CityId,
        int WorkLocationId,
        int CompanyId
    );

    #endregion

    #region Shift

    public sealed record EmployeeShiftWorkDaysCommand(
        int ShiftId,
        int WorkDaysId
    );

    #endregion

    #region Vacation Balances

    public sealed record EmployeeVacationsBalanceListCommand(
        IReadOnlyList<EmployeeVacationBalanceCommand> EmployeeVacationBalances
    );

    public sealed record EmployeeVacationBalanceCommand(
        int VacationTypeId,
        int Year,
        decimal TotalDays,
        bool? Prorate,
        int? Priority,
        EnumGenderType Gender,
        EnumReligionType Religion
    );

    #endregion



    #endregion






}
