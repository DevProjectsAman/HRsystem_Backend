using FluentValidation;
using HRsystem.Api.Database;
using HRsystem.Api.Database.DataTables;
using HRsystem.Api.Services.CurrentUser;
using HRsystem.Api.Shared.DTO;
using HRsystem.Api.Shared.Tools;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace HRsystem.Api.Features.Organization.JobManagment
{
    public static class JobTitleEndpoints
    {
        public static void MapJobTitleEndpoints(this IEndpointRouteBuilder app)
        {
            var group = app.MapGroup("/api/Organization/JobTitle").WithTags("Job Titles");

            // Get All
            group.MapGet("/ListJobTitles", async (int companyId, int departmentId, IMediator mediator) =>
            {
                var result = await mediator.Send(new GetAllJobTitlesQuery(companyId, departmentId));
                return Results.Ok(new ResponseResultDTO<List<JobTitleDto>>
                {
                    Success = true,
                    Data = result
                });
            });

            // Get One
            group.MapGet("/GetOneJobTitle/{id}", async (int id, IMediator mediator) =>
            {
                var result = await mediator.Send(new GetJobTitleByIdQuery(id));
                return result.Success
                    ? Results.Ok(result)
                    : Results.NotFound(result);
            });

            // Create
            group.MapPost("/CreateJobTitle", async (CreateJobTitleCommand cmd, IMediator mediator) =>
            {
                var result = await mediator.Send(cmd);
                return result.Success
                    ? Results.Created($"/api/Organization/JobTitle/{result.Data}", result)
                    : Results.BadRequest(result);
            });

            // Update
            group.MapPut("/UpdateJobTitle/{id}", async (int id, UpdateJobTitleCommand cmd, IMediator mediator) =>
            {
                if (id != cmd.JobTitleId)
                    return Results.BadRequest(new ResponseResultDTO { Success = false, Message = "Id mismatch" });

                var result = await mediator.Send(cmd);
                return result.Success
                    ? Results.Ok(result)
                    : Results.NotFound(result);
            });

            // Delete
            group.MapDelete("/DeleteJobTitle/{id}", async (int id, IMediator mediator) =>
            {
                var result = await mediator.Send(new DeleteJobTitleCommand(id));
                return result.Success
                    ? Results.Ok(result)
                    : Results.NotFound(result);
            });

            // Filter
            group.MapGet("/filter", async (int companyId, int departmentId, int jobLevelId, IMediator mediator) =>
            {
                var result = await mediator.Send(new GetFilteredJobTitlesQuery(companyId, departmentId, jobLevelId));
                if (result == null || !result.Any())
                    return Results.NotFound(new ResponseResultDTO { Success = false, Message = "No job titles found for the given filters" });

                return Results.Ok(new ResponseResultDTO<List<JobTitleDto>>
                {
                    Success = true,
                    Data = result
                });
            });
        }
    }

    #region Get All
    public record GetAllJobTitlesQuery(int CompanyId,int DepartmentId) : IRequest<System.Collections.Generic.List<JobTitleDto>>;

    public class JobTitleDto
    {
        public int JobTitleId { get; set; }
        public LocalizedData TitleName { get; set; }
        public int CompanyId { get; set; }
        public string CompanyName { get; set; }
        public int DepartmentId { get; set; }
        public string DepartmentName { get; set; }
        public int? JobLevelId { get; set; }
        public string JobLevelDesc { get; set; }
    }

    public class Handler : IRequestHandler<GetAllJobTitlesQuery, System.Collections.Generic.List<JobTitleDto>>
    {
        private readonly DBContextHRsystem _db;
        private readonly ICurrentUserService _currentUser;

        public Handler(DBContextHRsystem db, ICurrentUserService currentUser)
        {
            _db = db;
            _currentUser = currentUser;
        }

        public async Task<System.Collections.Generic.List<JobTitleDto>> Handle(GetAllJobTitlesQuery request, CancellationToken ct)
        {
            var lang = _currentUser.UserLanguage ?? "en";

            var jobTitles = await _db.TbJobTitles.Where(j => j.CompanyId == request.CompanyId && j.DepartmentId == request.DepartmentId)
                .Include(j => j.Company)
                .Include(j => j.Department)
                .Include(j => j.JobLevel)
                .AsNoTracking()
                .ToListAsync(ct);

            return jobTitles.Select(j => new JobTitleDto
            {
                JobTitleId = j.JobTitleId,
                TitleName = j.TitleName,
                CompanyId = j.CompanyId,
                CompanyName = j.Company.CompanyName,
                DepartmentId = j.DepartmentId,
                DepartmentName = j.Department.DepartmentName.GetTranslation(lang),
                JobLevelId = j.JobLevelId,
                JobLevelDesc = j.JobLevel.JobLevelDesc
            }).ToList();
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
        LocalizedData TitleName,
        int? JobLevelId,
        int CompanyId
    ) : IRequest<ResponseResultDTO<int>>;

    public class CreateJobTitleValidator : AbstractValidator<CreateJobTitleCommand>
    {
        public CreateJobTitleValidator()
        {
            RuleFor(x => x.DepartmentId).GreaterThan(0);
            RuleFor(x => x.CompanyId).GreaterThan(0);
            RuleFor(x => x.TitleName.en).NotEmpty().MaximumLength(55);
            RuleFor(x => x.TitleName.ar).NotEmpty().MaximumLength(55);
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
                CreatedAt = DateTime.Now,
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
        public LocalizedData TitleName { get; set; } = new();
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
            RuleFor(x => x.TitleName.en).NotEmpty().MaximumLength(55);
            RuleFor(x => x.TitleName.ar).NotEmpty().MaximumLength(55);
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
            entity.UpdatedAt = DateTime.Now;
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




    public record GetFilteredJobTitlesQuery(int CompanyId, int DepartmentId, int JobLevelId) : IRequest<List<JobTitleDto>>;

    public class GetFilteredJobTitlesHandler : IRequestHandler<GetFilteredJobTitlesQuery, List<JobTitleDto>>
    {
        private readonly DBContextHRsystem _db;
        private readonly ICurrentUserService _currentUser;
        public GetFilteredJobTitlesHandler(DBContextHRsystem db, ICurrentUserService currentUser)
        {
            _db = db;
            _currentUser = currentUser;

        }

        public async Task<List<JobTitleDto>> Handle(GetFilteredJobTitlesQuery request, CancellationToken cancellationToken)
        {
            var jobTitles = await _db.TbJobTitles
                .Where(j => j.CompanyId == request.CompanyId &&
                            j.DepartmentId == request.DepartmentId &&
                            j.JobLevelId == request.JobLevelId)
                .Select(j => new { j.JobTitleId, j.TitleName }) // fetch only needed fields
                .ToListAsync(cancellationToken);

            return jobTitles
                .Select(j => new JobTitleDto
                {
                    JobTitleId = j.JobTitleId,
                    TitleName = j.TitleName
                    // Other properties can be set to default/null if not available
                })
                .ToList();
        }

    }
}
