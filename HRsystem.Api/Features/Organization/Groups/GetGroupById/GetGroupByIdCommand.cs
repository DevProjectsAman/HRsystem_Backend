using FluentValidation;
using HRsystem.Api.Database;
using HRsystem.Api.Shared.DTO;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Data;

namespace HRsystem.Api.Features.Organization.Groups.GetGroupById
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

        public GetGroupHandler(DBContextHRsystem db)
        {
            _db = db;
        }

        public async Task<GetGroupResponse> Handle(GetGroupByIdCommand request, CancellationToken cancellationToken)
        {
            var group = await _db.TbGroups.FindAsync(new object[] { request.GroupId }, cancellationToken);

            if (group == null)
                return null!;

            return new GetGroupResponse
            {
                 group_id = group.GroupId,
                group_name = group.GroupName
            };
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


