using FluentValidation;
using HRsystem.Api.Database;
using HRsystem.Api.Shared.DTO;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Data;

namespace HRsystem.Api.Features.Groups.GetGroupById
{

    public record GetGroupByIdCommand(int GroupId) : IRequest<GetGroupResponse>;
    public class GetGroupResponse
    {
        public int group_id { get; set; }
        public string group_name { get; set; }
    }

    public class GetGroupHandler : IRequestHandler<GetGroupByIdCommand, GetGroupResponse>
    {
        private readonly DBContextHRsystem _db;

        public GetGroupHandler(DBContextHRsystem db) => _db = db;

        public async Task<ResponseResultDTO<GetGroupResponse>> Handle(GetGroupByIdCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var response = new ResponseResultDTO<GetGroupResponse>();

                


                var group = await _db.TbGroups
                .Where(g => g.GroupId == request.GroupId)
                .Select(g => new GetGroupResponse
                {
                    group_id = g.GroupId,
                    group_name = g.GroupName
                })
                .FirstOrDefaultAsync(cancellationToken);

                if (group == null)
                {
                    response.Success = false;
                    response.Message = "Group not found";

                  
                }
                else
                {
                    response.Success = true;
                    response.Message = "Data returned successfully";
                    response.Data = group;
                    
                }

                return response;


            }
            catch (Exception ex)
            {

               return new ResponseResultDTO<GetGroupResponse>
               {
                   Success = false,
                   Message = $"Error occurred in GetGroupById: {ex.Message}"
               };

            }
        }
    }

    public class GetGroupCommandValidator : AbstractValidator<GetGroupByIdCommand>
    {
        public GetGroupCommandValidator()
        {
            RuleFor(x => x.GroupId)
                .GreaterThan(0).WithMessage("Group id must be greater than 0");
        }
    }
}


