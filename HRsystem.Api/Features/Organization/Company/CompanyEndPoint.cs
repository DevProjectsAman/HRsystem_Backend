using HRsystem.Api.Features.Company.GetAllCompany;
using HRsystem.Api.Features.Company.GetCompanyById;
//using HRsystem.Api.Features.Company.DeleteCompany;
using MediatR;
using FluentValidation;
using System.Linq;
using HRsystem.Api.Features.Organization.Company.CreateCompany;
using HRsystem.Api.Features.Organization.Company.UpdateCompany;

namespace HRsystem.Api.Features.Organization.Company
{
    public static class CompanyEndpoints
    {
        public static void MapCompanyEndpoints(this IEndpointRouteBuilder app)
        {
            var group = app.MapGroup("/api/organization/companies").WithTags("Companies");

            // Get All
            group.MapGet("/ListOfCompany", async (ISender mediator) =>
            {
                var result = await mediator.Send(new GetAllCompanyCommand());
                return Results.Ok(new { Success = true, Data = result });
            });

            // Get One
            group.MapGet("/GetOneCompany/{id}", async (int id, ISender mediator) =>
            {
                var result = await mediator.Send(new GetCompanyByIdCommand(id));
                return result == null
                    ? Results.NotFound(new { Success = false, Message = $"Company {id} not found" })
                    : Results.Ok(new { Success = true, Data = result });
            });

            // Create
            group.MapPost("/CreateCompany", async (CreateCompanyCommand cmd, ISender mediator, IValidator<CreateCompanyCommand> validator) =>
            {
                var validationResult = await validator.ValidateAsync(cmd);
                if (!validationResult.IsValid)
                    return Results.BadRequest(new
                    {
                        Success = false,
                        Errors = validationResult.Errors.Select(e => e.ErrorMessage)
                    });

                var result = await mediator.Send(cmd);
                return Results.Created($"/api/companies/{result.CompanyId}", new { Success = true, Data = result });
            });

            // Update
            group.MapPut("/UpdateCompany/{id}", async (int id, UpdateCompanyCommand cmd, ISender mediator, IValidator<UpdateCompanyCommand> validator) =>
            {
                if (id != cmd.CompanyId)
                    return Results.BadRequest(new { Success = false, Message = "Id mismatch" });

                var validationResult = await validator.ValidateAsync(cmd);
                if (!validationResult.IsValid)
                    return Results.BadRequest(new
                    {
                        Success = false,
                        Errors = validationResult.Errors.Select(e => e.ErrorMessage)
                    });

                var result = await mediator.Send(cmd);
                return result == null
                    ? Results.NotFound(new { Success = false, Message = $"Company {id} not found" })
                    : Results.Ok(new { Success = true, Data = result });
            });

            // Delete
            //group.MapDelete("/{id}", async (int id, ISender mediator) =>
            //{
            //    var result = await mediator.Send(new DeleteCompanyCommand(id));
            //    return !result
            //        ? Results.NotFound(new { Success = false, Message = $"Company {id} not found" })
            //        : Results.Ok(new { Success = true, Message = $"Company {id} deleted successfully" });
            //});
        }
    }
}
