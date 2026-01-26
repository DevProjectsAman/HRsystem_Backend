using HRsystem.Api.Database;
using HRsystem.Api.Services.CurrentUser;
using MediatR;
using Microsoft.EntityFrameworkCore;


namespace HRsystem.Api.Features.EmployeeDashboard.EmployeeApp
{
    public record EmployeeInfoQueury() : IRequest<EmployeeInfoDto>;

    public class EmployeeInfoDto
    {
        public string EmployeeName { get; set; }

        public int EmployeeId { get; set; }

        public string? EmployeeDepartement {  get; set; }


    }
    public class EmployeeInfoQueuryHandler : IRequestHandler<EmployeeInfoQueury, EmployeeInfoDto>
    {
        DBContextHRsystem _db;
        ICurrentUserService _currentUserService;
        public EmployeeInfoQueuryHandler(DBContextHRsystem db ,ICurrentUserService currentUserService)
        {
             _db = db;
             _currentUserService = currentUserService;

        }

        public async Task<EmployeeInfoDto> Handle(EmployeeInfoQueury request, CancellationToken ct)
        {
            var employeeId = _currentUserService.EmployeeID;
            var language = _currentUserService.UserLanguage;

            var EmployeeInfo = await _db.TbEmployees 
                     .Include(a => a.Department)
                    .FirstOrDefaultAsync(b => b.EmployeeId == employeeId , ct);

            if (EmployeeInfo == null) throw new Exception($"Employee Not Found ID= {employeeId}");


            return new EmployeeInfoDto
            {
                EmployeeName= EmployeeInfo.ArabicFullName,
                EmployeeId= EmployeeInfo.EmployeeId,
                EmployeeDepartement = language == "ar"
                ?EmployeeInfo.Department.DepartmentName.ar
                :EmployeeInfo.Department.DepartmentName.en,

            };           
        }
    }
}
