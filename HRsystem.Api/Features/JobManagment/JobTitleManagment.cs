using FluentValidation;
using HRsystem.Api.Database;
using HRsystem.Api.Database.DataTables;
using HRsystem.Api.Features.JobTitles.GetFilteredJobTitles;
using HRsystem.Api.Services.CurrentUser;
using HRsystem.Api.Shared.DTO;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Routing;
using Microsoft.EntityFrameworkCore;

namespace HRsystem.Api.Features.JobManagment
{
    public static class JobTitleEndpoints
    {
        public static void MapJobTitleEndpoints(this IEndpointRouteBuilder app)
        {
            app.MapGet("/api/ListJobTitles", [Authorize] async (IMediator mediator) =>
                await mediator.Send(new GetAllJobTitlesQuery()))
                .WithName("ListJobTitles")
                .WithTags("Job Management");

            app.MapGet("/api/GetOneJobTitle/{id}", [Authorize] async (IMediator mediator, int id) =>
                await mediator.Send(new GetJobTitleByIdQuery(id)))
                .WithName("GetOneJobTitle")
                .WithTags("Job Management");

            app.MapPost("/api/CreateJobTitle", [Authorize] async (IMediator mediator, CreateJobTitleCommand cmd) =>
                await mediator.Send(cmd))
                .WithName("CreateJobTitle")
                .WithTags("Job Management");

            app.MapPut("/api/UpdateJobTitle/{id}", [Authorize] async (IMediator mediator, int id, UpdateJobTitleCommand cmd) =>
            {
                cmd.JobTitleId = id; // make sure ID is set
                return await mediator.Send(cmd);
            })
            .WithName("UpdateJobTitle")
            .WithTags("Job Management");

            app.MapDelete("/api/DeleteJobTitle/{id}", [Authorize] async (IMediator mediator, int id) =>
                await mediator.Send(new DeleteJobTitleCommand(id)))
                .WithName("DeleteJobTitle")
                .WithTags("Job Management");

            var group = app.MapGroup("/api/jobtitles").WithTags("Job Titles");

            group.MapGet("/filter", async (int companyId, int departmentId, int jobLevelId, ISender mediator) =>
            {
                var result = await mediator.Send(new GetFilteredJobTitlesQuery(companyId, departmentId, jobLevelId));

                if (result == null || !result.Any())
                    return Results.NotFound(new { Success = false, Message = "No job titles found for the given filters" });

                return Results.Ok(new { Success = true, Data = result });
            });
        }
    }

    #region Get All
    public record GetAllJobTitlesQuery : IRequest<ResponseResultDTO<List<TbJobTitle>>>;

    public class GetAllJobTitlesHandler(DBContextHRsystem db) : IRequestHandler<GetAllJobTitlesQuery, ResponseResultDTO<List<TbJobTitle>>>
    {
        public async Task<ResponseResultDTO<List<TbJobTitle>>> Handle(GetAllJobTitlesQuery request, CancellationToken ct)
        {
            var data = await db.TbJobTitles
                .AsNoTracking()
                .Include(x => x.Department)
                .Include(x => x.Company)
                .Include(x => x.JobLevel)
                .ToListAsync(ct);

            return new ResponseResultDTO<List<TbJobTitle>>
            {
                Success = true,
                Message = "Data returned successfully",
                Data = data
            };
        }
    }
    #endregion

    #region Get One
    public record GetJobTitleByIdQuery(int Id) : IRequest<ResponseResultDTO<TbJobTitle?>>;

    public class GetJobTitleByIdHandler(DBContextHRsystem db) : IRequestHandler<GetJobTitleByIdQuery, ResponseResultDTO<TbJobTitle?>>
    {
        public async Task<ResponseResultDTO<TbJobTitle?>> Handle(GetJobTitleByIdQuery request, CancellationToken ct)
        {
            var entity = await db.TbJobTitles
                .AsNoTracking()
                .Include(x => x.Department)
                .Include(x => x.Company)
                .Include(x => x.JobLevel)
                .FirstOrDefaultAsync(x => x.JobTitleId == request.Id, ct);

            return new ResponseResultDTO<TbJobTitle?>
            {
                Success = entity != null,
                Message = entity != null ? "Record found" : "Not found",
                Data = entity
            };
        }
    }
    #endregion

    #region Create
    public record CreateJobTitleCommand(
        int DepartmentId,
        string TitleName,
        int? JobLevelId,
        int CompanyId
    ) : IRequest<ResponseResultDTO<int>>;

    public class CreateJobTitleValidator : AbstractValidator<CreateJobTitleCommand>
    {
        public CreateJobTitleValidator()
        {
            RuleFor(x => x.DepartmentId).GreaterThan(0);
            RuleFor(x => x.CompanyId).GreaterThan(0);
            RuleFor(x => x.TitleName).NotEmpty().MaximumLength(55);
        }
    }

    public class CreateJobTitleHandler(DBContextHRsystem db, ICurrentUserService userService)
        : IRequestHandler<CreateJobTitleCommand, ResponseResultDTO<int>>
    {
        public async Task<ResponseResultDTO<int>> Handle(CreateJobTitleCommand request, CancellationToken ct)
        {
            var entity = new TbJobTitle
            {
                DepartmentId = request.DepartmentId,
                TitleName = request.TitleName,
                JobLevelId = request.JobLevelId,
                CompanyId = request.CompanyId,
                CreatedAt = DateTime.UtcNow,
                CreatedBy = userService.UserId
            };

            db.TbJobTitles.Add(entity);
            await db.SaveChangesAsync(ct);

            return new ResponseResultDTO<int>
            {
                Success = true,
                Message = "JobTitle created successfully",
                Data = entity.JobTitleId
            };
        }
    }
    #endregion

    #region Update
    public record UpdateJobTitleCommand : IRequest<ResponseResultDTO<bool>>
    {
        public int JobTitleId { get; set; }
        public int DepartmentId { get; set; }
        public string TitleName { get; set; } = string.Empty;
        public int? JobLevelId { get; set; }
        public int CompanyId { get; set; }
    }

    public class UpdateJobTitleValidator : AbstractValidator<UpdateJobTitleCommand>
    {
        public UpdateJobTitleValidator()
        {
            RuleFor(x => x.JobTitleId).GreaterThan(0);
            RuleFor(x => x.DepartmentId).GreaterThan(0);
            RuleFor(x => x.CompanyId).GreaterThan(0);
            RuleFor(x => x.TitleName).NotEmpty().MaximumLength(55);
        }
    }

    public class UpdateJobTitleHandler(DBContextHRsystem db, ICurrentUserService userService)
        : IRequestHandler<UpdateJobTitleCommand, ResponseResultDTO<bool>>
    {
        public async Task<ResponseResultDTO<bool>> Handle(UpdateJobTitleCommand request, CancellationToken ct)
        {
            var entity = await db.TbJobTitles.FindAsync(new object?[] { request.JobTitleId }, ct);
            if (entity == null)
                return new ResponseResultDTO<bool> { Success = false, Message = "Not found" };

            entity.DepartmentId = request.DepartmentId;
            entity.TitleName = request.TitleName;
            entity.JobLevelId = request.JobLevelId;
            entity.CompanyId = request.CompanyId;
            entity.UpdatedAt = DateTime.UtcNow;
            entity.UpdatedBy = userService.UserId;

            await db.SaveChangesAsync(ct);

            return new ResponseResultDTO<bool> { Success = true, Message = "Updated successfully", Data = true };
        }
    }
    #endregion

    #region Delete
    public record DeleteJobTitleCommand(int Id) : IRequest<ResponseResultDTO<bool>>;

    public class DeleteJobTitleHandler(DBContextHRsystem db)
        : IRequestHandler<DeleteJobTitleCommand, ResponseResultDTO<bool>>
    {
        public async Task<ResponseResultDTO<bool>> Handle(DeleteJobTitleCommand request, CancellationToken ct)
        {
            var entity = await db.TbJobTitles.FindAsync(new object?[] { request.Id }, ct);
            if (entity == null)
                return new ResponseResultDTO<bool> { Success = false, Message = "Not found" };

            db.TbJobTitles.Remove(entity);
            await db.SaveChangesAsync(ct);

            return new ResponseResultDTO<bool> { Success = true, Message = "Deleted successfully", Data = true };
        }
    }
    #endregion
}
