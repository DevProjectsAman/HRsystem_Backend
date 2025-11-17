
using AutoMapper;
using FluentValidation;
using global::HRsystem.Api.Database;
using global::HRsystem.Api.Database.DataTables;
using HRsystem.Api.Features.Scheduling.VacationRulesGroup.DTO;
using MediatR;
using static global::HRsystem.Api.Enums.EnumsList;

namespace HRsystem.Api.Features.Scheduling.VacationRulesGroup.Create
{
    public record CreateVacationRulesGroupCommand(
            int CompanyId,
            string GroupName,
            int? MinAge,
            int? MaxAge,
            int? MinServiceYears,
            int? MaxServiceYears,
            int? WorkingYearsAtCompany,
            List<CreateVacationRulesGroupDetailDto> Details
        ) : IRequest<VacationRulesGroupDto>;

    public record CreateVacationRulesGroupDetailDto(
        int VacationTypeId,
        EnumGenderType Gender,
        EnumReligionType Religion,
        int YearlyBalance,
        bool? Prorate,
        int? Priority
    );

    public class CreateVacationRulesGroupHandler : IRequestHandler<CreateVacationRulesGroupCommand, VacationRulesGroupDto>
    {
        private readonly DBContextHRsystem _db;
        private readonly IMapper _mapper;
        public CreateVacationRulesGroupHandler(DBContextHRsystem db,IMapper mapper) { _db = db; _mapper = mapper; }


        public async Task<VacationRulesGroupDto> Handle(CreateVacationRulesGroupCommand request, CancellationToken ct)
        {
            var group = new TbVacationRulesGroup
            {
                CompanyId = request.CompanyId,
                GroupName = request.GroupName,
                MinAge = request.MinAge,
                MaxAge = request.MaxAge,
                MinServiceYears = request.MinServiceYears,
                MaxServiceYears = request.MaxServiceYears,
                WorkingYearsAtCompany = request.WorkingYearsAtCompany,
                VacationRuleDetails = request.Details.Select(d => new TbVacationRulesGroupDetail
                {
                    VacationTypeId = d.VacationTypeId,
                    Gender = d.Gender,
                    Religion = d.Religion,
                    YearlyBalance = d.YearlyBalance,
                    Prorate = d.Prorate ??  false ,
                    Priority = d.Priority
                }).ToList()
            };

            _db.TbVacationRulesGroups.Add(group);
            await _db.SaveChangesAsync(ct);

            var retGroup = _mapper.Map<VacationRulesGroupDto>(group);
            return retGroup;

        //    return group;
        }
    }

    public class CreateVacationRulesGroupValidator : AbstractValidator<CreateVacationRulesGroupCommand>
    {
        public CreateVacationRulesGroupValidator()
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


