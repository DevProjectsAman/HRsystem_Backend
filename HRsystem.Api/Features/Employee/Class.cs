//using HRsystem.Api.Database;
//using MediatR;
//using Microsoft.EntityFrameworkCore;

//namespace HRsystem.Api.Features.Employee.GetEmployeeLookups
//{
//    public record GetEmployeeLookupsQuery() : IRequest<EmployeeLookupsResponse>;

//    public class GetEmployeeLookupsHandler : IRequestHandler<GetEmployeeLookupsQuery, EmployeeLookupsResponse>
//    {
//        private readonly DBContextHRsystem _db;

//        public GetEmployeeLookupsHandler(DBContextHRsystem db) => _db = db;

//        public async Task<EmployeeLookupsResponse> Handle(GetEmployeeLookupsQuery request, CancellationToken cancellationToken)
//        {
//            var companies = await _db.TbCompanies
//                .Select(c => new { c.CompanyId, c.CompanyName })
//                .ToListAsync(cancellationToken);

//            var jobTitles = await _db.TbJobTitles
//                .Select(j => new { j.JobTitleId, j.JobTitleName })
//                .ToListAsync(cancellationToken);

//            var maritalStatuses = await _db.TbMaritalStatuses
//                .Select(m => new { m.MaritalStatusId, m.NameEn, m.NameAr })
//                .ToListAsync(cancellationToken);

//            var nationalities = await _db.TbNationalities
//                .Select(n => new { n.NationalityId, n.NameEn, n.NameAr })
//                .ToListAsync(cancellationToken);

//            var departments = await _db.TbDepartments
//                .Select(d => new { d.DepartmentId, d.DepartmentName })
//                .ToListAsync(cancellationToken);

//            var shifts = await _db.TbShifts
//                .Select(s => new { s.ShiftId, s.ShiftName })
//                .ToListAsync(cancellationToken);

//            var managers = await _db.TbEmployees
//                .Select(e => new { e.EmployeeId, FullName = e.FirstName + " " + e.LastName })
//                .ToListAsync(cancellationToken);

//            return new EmployeeLookupsResponse(
//                companies, jobTitles, maritalStatuses, nationalities, departments, shifts, managers
//            );
//        }
//    }

//    public record EmployeeLookupsResponse(
//        object Companies,
//        object JobTitles,
//        object MaritalStatuses,
//        object Nationalities,
//        object Departments,
//        object Shifts,
//        object Managers
//    );
//}
