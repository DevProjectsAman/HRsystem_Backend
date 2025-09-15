using HRsystem.Api.Database;
using HRsystem.Api.Features.Employee.DTO;
using HRsystem.Api.Shared.DTO;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;


namespace HRsystem.Api.Features.Employee
{

    public record GetEmployeeByIdQuery(int EmployeeId) : IRequest<ResponseResultDTO<EmployeeReadDto?>>;

    public class GetEmployeeByIdHandler : IRequestHandler<GetEmployeeByIdQuery, ResponseResultDTO<EmployeeReadDto?>>
    {
        private readonly DBContextHRsystem _db;

        public GetEmployeeByIdHandler(DBContextHRsystem db) => _db = db;

        public async Task<ResponseResultDTO<EmployeeReadDto?>> Handle(GetEmployeeByIdQuery request, CancellationToken cancellationToken)
        {
            try
            {

           
            var e = await _db.TbEmployees
                .Include(x => x.JobTitle)
                .Include(x => x.Company)
                .Include(x => x.Manager)
                .FirstOrDefaultAsync(x => x.EmployeeId == request.EmployeeId, cancellationToken);

            if (e == null) 
                    return new ResponseResultDTO<EmployeeReadDto?> { Success = false, Message = "Not Found" } ;

            EmployeeReadDto emp =  new EmployeeReadDto(
                e.EmployeeId,
                e.EmployeeCodeHr!,
                e.FirstName,
                e.LastName,
                e.Email,
                e.PrivateMobile,
                e.BuisnessMobile,
                e.Gender,
                e.JobTitle.TitleName,
                e.Company.CompanyName,
                e.DepartmentId?.ToString(),
                e.Manager != null ? e.Manager.FirstName + " " + e.Manager.LastName : null,
                e.HireDate,
                e.Birthdate,
                e.Status
            );

                return new ResponseResultDTO<EmployeeReadDto?> { Success = true, Data = emp };

            }
            catch (Exception ex)
            {

                return new ResponseResultDTO<EmployeeReadDto?> { Success = false, Message = ex.Message };
            }

        }
    }

}
