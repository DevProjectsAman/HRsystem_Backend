using FluentValidation;
using HRsystem.Api.Database.DataTables;
using HRsystem.Api.Shared.DTO;
using HRsystem.Api.Database;
using HRsystem.Api.Database.Entities;
using HRsystem.Api.Services.CurrentUser;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Routing;
using Microsoft.EntityFrameworkCore;
using System;


namespace HRsystem.Api.Features.Organization.JobManagment

{
    public static class JobLevelEndpoints
    {
        public static void MapJobLevelEndpoints(this IEndpointRouteBuilder app)
        {
            var group = app.MapGroup("/api/Organization/JobLevel").WithTags("Organization");

            group.MapGet("/ListJobLevels/{companyId}", [Authorize] async (IMediator mediator, int companyId) =>
            {
                try
                {
                    var result = await mediator.Send(new GetAllJobLevelsQuery(companyId));
                    return result.Success ? Results.Ok(result) : Results.BadRequest(result);
                }
                catch (Exception ex)
                {
                    return Results.Json(new ResponseResultDTO { Success = false, Message = ex.InnerException?.Message ?? ex.Message }, statusCode: 500);
                }
            })
            .WithName("ListJobLevels");


            group.MapGet("/GetOneJobLevel/{id}", [Authorize] async (IMediator mediator, int id) =>
            {
                try
                {
                    var result = await mediator.Send(new GetJobLevelByIdQuery(id));
                    return result.Success ? Results.Ok(result) : Results.NotFound(result);
                }
                catch (Exception ex)
                {
                    return Results.Json(new ResponseResultDTO { Success = false, Message = ex.InnerException?.Message ?? ex.Message }, statusCode: 500);
                }
            })
            .WithName("GetOneJobLevel");


            group.MapPost("/CreateJobLevel", [Authorize] async (IMediator mediator, CreateJobLevelCommand cmd) =>
            {
                try
                {
                    var result = await mediator.Send(cmd);
                    return result.Success
                        ? Results.Created($"/api/Organization/JobLevel/{result.Data}", result)
                        : Results.BadRequest(result);
                }
                catch (Exception ex)
                {
                    return Results.Json(new ResponseResultDTO<int> { Success = false, Message = ex.InnerException?.Message ?? ex.Message }, statusCode: 500);
                }
            })
            .WithName("CreateJobLevel");


            group.MapPut("/UpdateJobLevel/{id}", [Authorize] async (IMediator mediator, int id, UpdateJobLevelCommand cmd) =>
            {
                try
                {
                    if (id != cmd.JobLevelId)
                        return Results.BadRequest(new ResponseResultDTO { Success = false, Message = "Id mismatch" });

                    var result = await mediator.Send(cmd);
                    return result.Success ? Results.Ok(result) : Results.NotFound(result);
                }
                catch (Exception ex)
                {
                    return Results.Json(new ResponseResultDTO<bool> { Success = false, Message = ex.InnerException?.Message ?? ex.Message }, statusCode: 500);
                }
            })
            .WithName("UpdateJobLevel");


            group.MapDelete("/DeleteJobLevel/{id}", [Authorize] async (IMediator mediator, int id) =>
            {
                try
                {
                    var result = await mediator.Send(new DeleteJobLevelCommand(id));
                    return result.Success ? Results.Ok(result) : Results.NotFound(result);
                }
                catch (Exception ex)
                {
                    return Results.Json(new ResponseResultDTO<bool> { Success = false, Message = ex.InnerException?.Message ?? ex.Message }, statusCode: 500);
                }
            })
            .WithName("DeleteJobLevel")
            .WithTags("Job Management");

        }
    }

    public class JobLevelDto
    {
        public int JobLevelId { get; set; }
        public string? JobLevelDesc { get; set; }
        public string? JobLevelCode { get; set; }
        public List<JobTitleDto> JobTitles { get; set; } = new();
    }

    //public class JobTitleDto
    //{
    //    public int JobTitleId { get; set; }
    //    public string? TitleName { get; set; }  // لو عندك LocalizedData، هترجع string أو JSON
    //    public int DepartmentId { get; set; }
    //    public int CompanyId { get; set; }
    //}

    public record GetAllJobLevelsQuery(int companyId) : IRequest<ResponseResultDTO<List<JobLevelDto>>>;

    public class GetAllJobLevelsHandler(DBContextHRsystem db)
        : IRequestHandler<GetAllJobLevelsQuery, ResponseResultDTO<List<JobLevelDto>>>
    {
        public async Task<ResponseResultDTO<List<JobLevelDto>>> Handle(GetAllJobLevelsQuery request, CancellationToken ct)
        {
            var data = await db.TbJobLevels.Where(c=>c.CompanyId==request.companyId)
                .Select(jl => new JobLevelDto
                {
                    JobLevelId = jl.JobLevelId,
                    JobLevelDesc = jl.JobLevelDesc,
                    JobLevelCode = jl.JobLevelCode,
                    JobTitles = jl.TbJobTitles.Select(t => new JobTitleDto
                    {
                        JobTitleId = t.JobTitleId,
                        TitleName = t.TitleName, // لو LocalizedData رجعها كـ string

                    }).ToList()
                })
                .ToListAsync(ct);

            return new ResponseResultDTO<List<JobLevelDto>>
            {
                Success = true,
                Message = "Data returned successfully",
                Data = data
            };
        }
    }


    #region Get One
    public record GetJobLevelByIdQuery(int Id) : IRequest<ResponseResultDTO<TbJobLevel?>>;

    public class GetJobLevelByIdHandler(DBContextHRsystem db) : IRequestHandler<GetJobLevelByIdQuery, ResponseResultDTO<TbJobLevel?>>
    {
        public async Task<ResponseResultDTO<TbJobLevel?>> Handle(GetJobLevelByIdQuery request, CancellationToken ct)
        {
            var entity = await db.TbJobLevels.AsNoTracking()
                .FirstOrDefaultAsync(x => x.JobLevelId == request.Id, ct);

            return new ResponseResultDTO<TbJobLevel?>
            {
                Success = entity != null,
                Message = entity != null ? "Record found" : "Not found",
                Data = entity
            };
        }
    }
    #endregion

    #region Create
    public record CreateJobLevelCommand(int companyId ,string? JobLevelDesc, string? JobLevelCode) : IRequest<ResponseResultDTO<int>>;

    public class CreateJobLevelValidator : AbstractValidator<CreateJobLevelCommand>
    {

        private readonly DBContextHRsystem _db;

        public CreateJobLevelValidator(DBContextHRsystem dBContextHRsystem)

        {
            _db = dBContextHRsystem;

            RuleFor(x => x.JobLevelDesc)
                .NotEmpty().MaximumLength(55)
                 .MustAsync(BeUniqueDescPerCompany)
              .WithMessage("Job level Description already exists for this company.");
            ;

             
           
            RuleFor(x => x.JobLevelCode)
              .NotEmpty()
              .MaximumLength(25)
              .MustAsync(BeUniqueCodePerCompany)
              .WithMessage("Job level code already exists for this company.");
        }

        private async Task<bool> BeUniqueCodePerCompany(CreateJobLevelCommand cmd, string jobLevelCode, CancellationToken ct)
        {
            return !await _db.TbJobLevels
                .AnyAsync(x => x.CompanyId == cmd.companyId && x.JobLevelCode == jobLevelCode, ct);
        } 
        private async Task<bool> BeUniqueDescPerCompany(CreateJobLevelCommand cmd, string jobLevelDesc, CancellationToken ct)
        {
            return !await _db.TbJobLevels
                .AnyAsync(x => x.CompanyId == cmd.companyId && x.JobLevelDesc == jobLevelDesc, ct);
        }
    }

    public class CreateJobLevelHandler(DBContextHRsystem db, ICurrentUserService userService) : IRequestHandler<CreateJobLevelCommand, ResponseResultDTO<int>>
    {

        public async Task<ResponseResultDTO<int>> Handle(CreateJobLevelCommand request, CancellationToken ct)
        {
            try
            {


                var entity = new TbJobLevel
                {
                    CompanyId = request.companyId,
                    JobLevelDesc = request.JobLevelDesc,
                    JobLevelCode = request.JobLevelCode,
                    CreatedAt = DateTime.Now,
                    CreatedBy = userService.UserId // Uncomment if you want to track who created the record

                };

                db.TbJobLevels.Add(entity);
                await db.SaveChangesAsync(ct);

                return new ResponseResultDTO<int>
                {
                    Success = true,
                    Message = "JobLevel created successfully",
                    Data = entity.JobLevelId
                };

            }
            catch (Exception ex)
            {

                return new ResponseResultDTO<int>
                {
                    Success = false,
                    Message = ex.InnerException?.Message ?? ex.Message

                }; ;
            }
        }
    }
    #endregion

    #region Update
    public record UpdateJobLevelCommand : IRequest<ResponseResultDTO<bool>>
    {
        public int JobLevelId { get; set; }
        public string JobLevelDesc { get; set; }
        public string JobLevelCode { get; set; }
    }

    public class UpdateJobLevelValidator : AbstractValidator<UpdateJobLevelCommand>
    {
        public UpdateJobLevelValidator()
        {
            RuleFor(x => x.JobLevelDesc).NotEmpty().MaximumLength(55);
            RuleFor(x => x.JobLevelCode).NotEmpty().MaximumLength(25);
            RuleFor(x => x.JobLevelId).GreaterThan(0);

        }
    }

    public class UpdateJobLevelHandler(DBContextHRsystem db, ICurrentUserService userService) : IRequestHandler<UpdateJobLevelCommand, ResponseResultDTO<bool>>
    {
        public async Task<ResponseResultDTO<bool>> Handle(UpdateJobLevelCommand request, CancellationToken ct)
        {
            var entity = await db.TbJobLevels.FindAsync(new object[] { request.JobLevelId }, ct);
            if (entity == null)
                return new ResponseResultDTO<bool> { Success = false, Message = "Not found" };

            // Optional: Check if the new code already exists for the same company (and not the same record)
            var exists = await db.TbJobLevels
                .AnyAsync(x => x.CompanyId == entity.CompanyId
                            && (x.JobLevelCode == request.JobLevelCode || x.JobLevelDesc == request.JobLevelDesc)
                            && x.JobLevelId != request.JobLevelId, ct);

            if (exists)
                return new ResponseResultDTO<bool>
                {
                    Success = false,
                    Message = "Job level Code Or Description already exists for this company"
                };

            entity.JobLevelDesc = request.JobLevelDesc;
            entity.JobLevelCode = request.JobLevelCode;
            entity.UpdatedAt = DateTime.Now;
            entity.UpdatedBy = userService.UserId; // Uncomment if you want to track who updated the record

            await db.SaveChangesAsync(ct);

            return new ResponseResultDTO<bool> { Success = true, Message = "Updated successfully", Data = true };
        }
    }
    #endregion

    #region Delete
    public record DeleteJobLevelCommand(int Id) : IRequest<ResponseResultDTO<bool>>;

    public class DeleteJobLevelHandler(DBContextHRsystem db) : IRequestHandler<DeleteJobLevelCommand, ResponseResultDTO<bool>>
    {
        public async Task<ResponseResultDTO<bool>> Handle(DeleteJobLevelCommand request, CancellationToken ct)
        {
            var entity = await db.TbJobLevels.FindAsync(new object?[] { request.Id }, ct);
            if (entity == null)
                return new ResponseResultDTO<bool> { Success = false, Message = "Not found" };

            db.TbJobLevels.Remove(entity);
            await db.SaveChangesAsync(ct);

            return new ResponseResultDTO<bool> { Success = true, Message = "Deleted successfully", Data = true };
        }
    }
    #endregion

}
