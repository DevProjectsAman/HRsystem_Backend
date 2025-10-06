
using HRsystem.Api.Features.Employee.DTO;
using HRsystem.Api.Database;
using HRsystem.Api.Shared.DTO;
using MediatR;
using Microsoft.EntityFrameworkCore;
using HRsystem.Api.Services.CurrentUser;

namespace HRsystem.Api.Features.Employee.UpdateEmployee
{
    // Command
    public record UpdateEmployeeCommand(int Id, EmployeeUpdateDto Dto)
        : IRequest<ResponseResultDTO<EmployeeReadDto?>>;

    // Handler
    public class UpdateEmployeeHandler
        : IRequestHandler<UpdateEmployeeCommand, ResponseResultDTO<EmployeeReadDto?>>
    {
        private readonly DBContextHRsystem _db;
        private readonly ICurrentUserService _currentUserService;
        public UpdateEmployeeHandler(DBContextHRsystem db , ICurrentUserService currentUserService)
        {
            _db = db;
            _currentUserService = currentUserService;
        }

        public async Task<ResponseResultDTO<EmployeeReadDto?>> Handle(UpdateEmployeeCommand request, CancellationToken cancellationToken)
        {
            var HREmployeeID = _currentUserService.EmployeeID;
            var language = _currentUserService.UserLanguage;
            var entity = await _db.TbEmployees
                .FirstOrDefaultAsync(e => e.EmployeeId == request.Id, cancellationToken);

            if (entity == null)
                return new ResponseResultDTO<EmployeeReadDto?>
                {
                    Success = false,
                    Message = "Employee not found"
                };

            // ✅ Update fields from DTO
            var dto = request.Dto;
            entity.EmployeeCodeFinance = dto.EmployeeCodeFinance;
            entity.EmployeeCodeHr = dto.EmployeeCodeHr;
            //entity.EnglishFullName = dto.EnglishFullName;
            //entity.ArabicFullName = dto.ArabicFullName;
            
            entity.Birthdate = dto.Birthdate;
            entity.HireDate = dto.HireDate;
            entity.Gender = dto.Gender;
            entity.NationalId = dto.NationalId;
            entity.PassportNumber = dto.PassportNumber;
            entity.PlaceOfBirth = dto.PlaceOfBirth;
            entity.BloodGroup = dto.BloodGroup;
            entity.JobTitleId = dto.JobTitleId;
            entity.CompanyId = dto.CompanyId;
            entity.DepartmentId = dto.DepartmentId;
            entity.ManagerId = dto.ManagerId;
            entity.ShiftId = dto.ShiftId;
            entity.MaritalStatusId = dto.MaritalStatusId;
            entity.NationalityId = dto.NationalityId;
            entity.Email = dto.Email;
            entity.PrivateMobile = dto.PrivateMobile;
            entity.BuisnessMobile = dto.BuisnessMobile;
            entity.SerialMobile = dto.SerialMobile;
            entity.StartDate = dto.StartDate;
            entity.EndDate = dto.EndDate;
            entity.IsTopmanager = dto.IsTopManager.HasValue ? (sbyte?)(dto.IsTopManager.Value ? 1 : 0) : null;
            entity.IsFulldocument = dto.IsFullDocument.HasValue ? (sbyte?)(dto.IsFullDocument.Value ? 1 : 0) : null;
            entity.Note = dto.Note;
            entity.Status = dto.Status;
            entity.UpdatedAt = DateTime.Now;
            entity.UpdatedBy = (int)HREmployeeID;

            _db.TbEmployees.Update(entity);
            await _db.SaveChangesAsync(cancellationToken);

            // ✅ Convert updated entity to ReadDto (you already have mapping logic)
            var updatedDto = new EmployeeReadDto
            {
                EmployeeId = entity.EmployeeId,
                EmployeeCodeFinance = entity.EmployeeCodeFinance,
                EmployeeCodeHr = entity.EmployeeCodeHr,
                JobTitleId = entity.JobTitleId,
                JobTitleName = entity.JobTitle.TitleName,
                //EnglishFullName = entity.EnglishFullName,
                //ArabicFullName = entity.ArabicFullName,
                HireDate = entity.HireDate,
                Birthdate = entity.Birthdate,
                Gender = entity.Gender,
                NationalId = entity.NationalId,
                PassportNumber = entity.PassportNumber,
                PlaceOfBirth = entity.PlaceOfBirth,
                BloodGroup = entity.BloodGroup,
                ManagerId = entity.ManagerId,
                ManagerName = entity.Manager != null ? $"{entity.Manager.EnglishFullName}" : null,
                CompanyId = entity.CompanyId,
                CompanyName = entity.Company?.CompanyName,
                CreatedBy = entity.CreatedBy,
                CreatedAt = entity.CreatedAt,
                UpdatedBy = entity.UpdatedBy,
                UpdatedAt = entity.UpdatedAt,
                PrivateMobile = entity.PrivateMobile,
                BuisnessMobile = entity.BuisnessMobile,
                Email = entity.Email,
                SerialMobile = entity.SerialMobile,
                StartDate = entity.StartDate,
                EndDate = entity.EndDate,
                IsTopManager = dto.IsTopManager,
                IsFullDocument = dto.IsFullDocument,
                Note = dto.Note,
                Status = dto.Status,
                NationalityId = entity.NationalityId,
                NationalityName = entity.Nationality?.NameEn,
                DepartmentId = entity.DepartmentId,
                DepartmentName = entity.Department.DepartmentName,
                ShiftId = entity.ShiftId,
                ShiftName = entity.Shifts.ShiftName,
                MaritalStatusId = entity.MaritalStatusId,
                MaritalStatusName = entity.MaritalStatus?.NameAr
            };

            return new ResponseResultDTO<EmployeeReadDto?>
            {
                Success = true,
                Data = updatedDto,
                Message = "Employee updated successfully"
            };
        }
    }
}
