using AutoMapper;
using FluentValidation;
using HRsystem.Api.Database;
using HRsystem.Api.Database.DataTables;
using HRsystem.Api.Features.Scheduling.VacationRulesGroup.Create;
using HRsystem.Api.Features.Scheduling.VacationRulesGroup.DTO;
using MediatR;
using Microsoft.EntityFrameworkCore;
using static HRsystem.Api.Enums.EnumsList;

namespace HRsystem.Api.Features.Scheduling.VacationRulesGroup.Update
{
    public record UpdateVacationRulesGroupCommand(
        int GroupId,
        int CompanyId,
        string GroupName,
        int? MinAge,
        int? MaxAge,
        int? MinServiceYears,
        int? MaxServiceYears,
        int? WorkingYearsAtCompany,
        List<CreateVacationRulesGroupDetailDto> Details
    ) : IRequest<VacationRulesGroupDto?>;

    public class UpdateVacationRulesGroupHandler : IRequestHandler<UpdateVacationRulesGroupCommand, VacationRulesGroupDto?>
    {
        private readonly DBContextHRsystem _db;
        private readonly IMapper _mapper;
        public UpdateVacationRulesGroupHandler(DBContextHRsystem db, IMapper mapper)
        { _db = db; _mapper = mapper; }

        public async Task<VacationRulesGroupDto?> Handle(UpdateVacationRulesGroupCommand request, CancellationToken ct)
        {
            var entity = await _db.TbVacationRulesGroups
                .Include(g => g.VacationRuleDetails)
                .FirstOrDefaultAsync(g => g.GroupId == request.GroupId, ct);

            if (entity is null) return null;

            entity.GroupName = request.GroupName;
            entity.MinAge = request.MinAge;
            entity.MaxAge = request.MaxAge;
            entity.MinServiceYears = request.MinServiceYears;
            entity.MaxServiceYears = request.MaxServiceYears;
            entity.WorkingYearsAtCompany = request.WorkingYearsAtCompany;

            // Replace details
            _db.TbVacationRulesGroupDetails.RemoveRange(entity.VacationRuleDetails);
            entity.VacationRuleDetails = request.Details.Select(d => new TbVacationRulesGroupDetail
            {
                VacationTypeId = d.VacationTypeId,
                Gender = d.Gender,
                Religion = d.Religion,
                YearlyBalance = d.YearlyBalance,
                Prorate = d.Prorate ?? false,
                Priority = d.Priority
            }).ToList();

            await _db.SaveChangesAsync(ct);


            return _mapper.Map<VacationRulesGroupDto>(  entity);
        }
    }

    
    //public class UpdateVacationRulesGroupValidator : AbstractValidator<UpdateVacationRulesGroupCommand>
    //{
    //    public UpdateVacationRulesGroupValidator()
    //    {
    //        RuleFor(x => x.GroupId).GreaterThan(0);
    //        RuleFor(x => x.GroupName).NotEmpty().MaximumLength(150);
    //        RuleForEach(x => x.Details).SetValidator(new Create.VacationRulesGroupDetailValidator());
    //    }

    //    private class VacationRulesGroupDetailValidator : AbstractValidator<CreateVacationRulesGroupDetailDto>
    //    {
    //        public VacationRulesGroupDetailValidator()
    //        {
    //            RuleFor(x => x.VacationTypeId).GreaterThan(0);
    //            RuleFor(x => x.YearlyBalance).GreaterThanOrEqualTo(0);
    //        }
    //    }
    //}
    public class UpdateVacationRulesGroupValidator : AbstractValidator<UpdateVacationRulesGroupCommand>
    {
        public UpdateVacationRulesGroupValidator()
        {
            RuleFor(x => x.CompanyId).GreaterThan(0);
            RuleFor(x => x.GroupName).NotEmpty().MaximumLength(150);

            RuleForEach(x => x.Details).SetValidator(new VacationRulesGroupDetailValidator());
        }

        private class VacationRulesGroupDetailValidator : AbstractValidator<CreateVacationRulesGroupDetailDto>
        {
            public VacationRulesGroupDetailValidator()
            {
                RuleFor(x => x.VacationTypeId).GreaterThan(0);
                RuleFor(x => x.YearlyBalance).GreaterThanOrEqualTo(0);
            }
        }
    }
}




