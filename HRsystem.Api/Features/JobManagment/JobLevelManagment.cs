using FluentValidation;
using global::HRsystem.Api.Database.DataTables;
using global::HRsystem.Api.Shared.DTO;
using HRsystem.Api.Database;
using HRsystem.Api.Database.Entities;
using HRsystem.Api.Features.JobManagment;
using HRsystem.Api.Services.CurrentUser;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Routing;
using Microsoft.EntityFrameworkCore;
using System;


namespace HRsystem.Api.Features.JobManagment

{
     
    public static class JobLevelEndpoints
    {
        public static void MapJobLevelEndpoints(this IEndpointRouteBuilder app)
        {
            app.MapGet("/api/ListJobLevels", [Authorize] async (IMediator mediator) =>
                await mediator.Send(new GetAllJobLevelsQuery()))
                .WithName("ListJobLevels")
                .WithTags("Job Management");

            app.MapGet("/api/GetOneJobLevel/{id}", [Authorize] async (IMediator mediator, int id) =>
                await mediator.Send(new GetJobLevelByIdQuery(id)))
                .WithName("GetOneJobLevel")
                .WithTags("Job Management");

            app.MapPost("/api/CreateJobLevel", [Authorize] async (IMediator mediator, CreateJobLevelCommand cmd) =>
                await mediator.Send(cmd))
                .WithName("CreateJobLevel")
                .WithTags("Job Management");

            app.MapPut("/api/UpdateJobLevel/{id}", [Authorize] async (IMediator mediator, int id, UpdateJobLevelCommand cmd) =>
            {
                
                return await mediator.Send(cmd);
            })
            .WithName("UpdateJobLevel")
            .WithTags("Job Management");

            app.MapDelete("/api/DeleteJobLevel/{id}", [Authorize] async (IMediator mediator, int id) =>
                await mediator.Send(new DeleteJobLevelCommand(id)))
                .WithName("DeleteJobLevel")
                .WithTags("Job Management");

        }
    }

    #region Get All
    public record GetAllJobLevelsQuery : IRequest<ResponseResultDTO<List<TbJobLevel>>>;

    public class GetAllJobLevelsHandler(DBContextHRsystem db) : IRequestHandler<GetAllJobLevelsQuery, ResponseResultDTO<List<TbJobLevel>>>
    {
        public async Task<ResponseResultDTO<List<TbJobLevel>>> Handle(GetAllJobLevelsQuery request, CancellationToken ct)
        {
            var data = await db.TbJobLevels.AsNoTracking().ToListAsync(ct);

            return new ResponseResultDTO<List<TbJobLevel>>
            {
                Success = true,
                Message = "Data returned successfully",
                Data = data
            };
        }
    }
    #endregion

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
    public record CreateJobLevelCommand(string? JobLevelDesc, string? JobLevelCode) : IRequest<ResponseResultDTO<int>>;

    public class CreateJobLevelValidator : AbstractValidator<CreateJobLevelCommand>
    {
        public CreateJobLevelValidator()
        {
            RuleFor(x => x.JobLevelDesc).NotEmpty().MaximumLength(55);
            RuleFor(x => x.JobLevelCode).NotEmpty().MaximumLength(25);
        }
    }

    public class CreateJobLevelHandler(DBContextHRsystem db ,ICurrentUserService userService) : IRequestHandler<CreateJobLevelCommand, ResponseResultDTO<int>>
    {

        public async Task<ResponseResultDTO<int>> Handle(CreateJobLevelCommand request, CancellationToken ct)
        {
            try
            {

          
            var entity = new TbJobLevel
            {
                JobLevelDesc = request.JobLevelDesc,
                JobLevelCode = request.JobLevelCode,
                CreatedAt = DateTime.UtcNow ,
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
                    Message = ex.InnerException.Message

                }; ;
            }
        }
    }
    #endregion

    #region Update
    public record UpdateJobLevelCommand : IRequest<ResponseResultDTO<bool>>
    {
       
        public string? JobLevelDesc { get; set; }
        public string? JobLevelCode { get; set; }
    }

    public class UpdateJobLevelValidator : AbstractValidator<UpdateJobLevelCommand>
    {
        public UpdateJobLevelValidator()
        {
            RuleFor(x => x.JobLevelDesc).NotEmpty().MaximumLength(55);
            RuleFor(x => x.JobLevelCode).NotEmpty().MaximumLength(25);
        }
    }

    public class UpdateJobLevelHandler(DBContextHRsystem db,ICurrentUserService userService) : IRequestHandler<UpdateJobLevelCommand, ResponseResultDTO<bool>>
    {
        public async Task<ResponseResultDTO<bool>> Handle(UpdateJobLevelCommand request, CancellationToken ct)
        {
            var entity = await db.TbJobLevels.FindAsync(new object?[] {  }, ct);
            if (entity == null)
                return new ResponseResultDTO<bool> { Success = false, Message = "Not found" };

            entity.JobLevelDesc = request.JobLevelDesc;
            entity.JobLevelCode = request.JobLevelCode;
            entity.UpdatedAt = DateTime.UtcNow;
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
