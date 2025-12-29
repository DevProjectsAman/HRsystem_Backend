using FluentValidation;
using HRsystem.Api.Features.Organization.Company.CreateCompany;
using HRsystem.Api.Features.Organization.Company.GetAllCompany;
using HRsystem.Api.Features.Organization.Company.GetCompanyById;
using HRsystem.Api.Features.Organization.Company.UpdateCompany;
using HRsystem.Api.Shared.DTO;
using MediatR;
using Microsoft.AspNetCore.Authorization;

namespace HRsystem.Api.Features.Organization.Company
{
    public static class CompanyEndpoints
    {
        public static void MapCompanyEndpoints(this IEndpointRouteBuilder app)
        {
            var group = app.MapGroup("/api/Organization/companies").WithTags("Companies");

            // ✅ Get All
            group.MapGet("/ListOfCompany", [Authorize] async (ISender mediator) =>
            {
                var result = await mediator.Send(new GetAllCompanyCommand());
                return Results.Ok(new ResponseResultDTO<object>
                {
                    Data = result,
                    Message = "Companies retrieved successfully",
                    Success = true
                });
            });

            // ✅ Get One
            group.MapGet("/GetOneCompany/{id}", [Authorize] async (int id, ISender mediator) =>
            {
                var result = await mediator.Send(new GetCompanyByIdCommand(id));

                if (result == null)
                {
                    return Results.NotFound(new ResponseResultDTO
                    {
                        Success = false,
                        Message = $"Company {id} not found"
                    });
                }

                return Results.Ok(new ResponseResultDTO<object>
                {
                    Success = true,
                    Data = result,
                    Message = "Company retrieved successfully"
                });
            });

            // ✅ Create
            group.MapPost("/CreateCompany", [Authorize] async (CreateCompanyCommand cmd, ISender mediator, IValidator<CreateCompanyCommand> validator) =>
            {
                var validationResult = await validator.ValidateAsync(cmd);
                if (!validationResult.IsValid)
                {
                    var errors = validationResult.Errors
                        .Select(e => new ResponseErrorDTO
                        {
                            Property = e.PropertyName,
                            Error = e.ErrorMessage
                        })
                        .ToList();

                    return Results.BadRequest(new ResponseResultDTO
                    {
                        Success = false,
                        Message = "Validation failed",
                        Errors = errors
                    });
                }

                var result = await mediator.Send(cmd);
                return Results.Created($"/api/Organization/companies/{result.CompanyId}", new ResponseResultDTO<object>
                {
                    Success = true,
                    Data = result,
                    Message = "Company created successfully"
                });
            });

            // ✅ Update
            group.MapPut("/UpdateCompany/{id}", [Authorize] async (int id, UpdateCompanyCommand cmd, ISender mediator, IValidator<UpdateCompanyCommand> validator) =>
            {
                if (id != cmd.CompanyId)
                {
                    return Results.BadRequest(new ResponseResultDTO
                    {
                        Success = false,
                        Message = "Id mismatch"
                    });
                }

                var validationResult = await validator.ValidateAsync(cmd);
                if (!validationResult.IsValid)
                {
                    var errors = validationResult.Errors
                        .Select(e => new ResponseErrorDTO
                        {
                            Property = e.PropertyName,
                            Error = e.ErrorMessage
                        })
                        .ToList();

                    return Results.BadRequest(new ResponseResultDTO
                    {
                        Success = false,
                        Message = "Validation failed",
                        Errors = errors
                    });
                }

                var result = await mediator.Send(cmd);

                if (result == null)
                {
                    return Results.NotFound(new ResponseResultDTO
                    {
                        Success = false,
                        Message = $"Company {id} not found"
                    });
                }

                return Results.Ok(new ResponseResultDTO<object>
                {
                    Success = true,
                    Data = result,
                    Message = "Company updated successfully"
                });
            });

            // ✅ Optional: Delete (if you re-enable later)
            // group.MapDelete("/{id}", [Authorize] async (int id, ISender mediator) =>
            // {
            //     var result = await mediator.Send(new DeleteCompanyCommand(id));
            //     if (!result)
            //     {
            //         return Results.NotFound(new ResponseResultDTO
            //         {
            //             Success = false,
            //             Message = $"Company {id} not found"
            //         });
            //     }

            //     return Results.Ok(new ResponseResultDTO
            //     {
            //         Success = true,
            //         Message = $"Company {id} deleted successfully"
            //     });
            // });
        }
    }
}
