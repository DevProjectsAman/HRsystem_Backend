using FluentValidation;
using HRsystem.Api.Database;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Data;

namespace HRsystem.Api.Features.Groups.GetALL
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

        public GetGroupHandler (DBContextHRsystem db) => _db = db;

        public async Task<GetGroupResponse> Handle(GetGroupByIdCommand request, CancellationToken cancellationToken)
        {
            var group = await _db.TbGroups
                .Where(g => g.GroupId == request.GroupId)
                .Select(g => new GetGroupResponse
                {
                    group_id = g.GroupId,
                    group_name = g.GroupName
                })
                .FirstOrDefaultAsync(cancellationToken);

            if (group == null)
                throw new KeyNotFoundException("Group not found");

            return group;

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


