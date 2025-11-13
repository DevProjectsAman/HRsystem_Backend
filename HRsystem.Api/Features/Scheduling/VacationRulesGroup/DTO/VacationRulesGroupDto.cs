using static HRsystem.Api.Enums.EnumsList;

namespace HRsystem.Api.Features.Scheduling.VacationRulesGroup.DTO
{
    public class VacationRulesGroupDto
    {
        public int GroupId { get; set; }
        public int CompanyId { get; set; }
        public string GroupName { get; set; } = string.Empty;

        // Employee Filters (master-level eligibility)
        public int? MinAge { get; set; }
        public int? MaxAge { get; set; }
        public int? MinServiceYears { get; set; }
        public int? MaxServiceYears { get; set; }
        public int? WorkingYearsAtCompany { get; set; }

        // List of vacation-type rules
        public List<VacationRulesGroupDetailDto> Details { get; set; } = new();
    }

    public class VacationRulesGroupDetailDto
    {
        public int DetailId { get; set; }
        public int GroupId { get; set; }
        public int VacationTypeId { get; set; }
        public string? VacationTypeName { get; set; }

        // Vacation behavior
        public int YearlyBalance { get; set; }
        public bool? Prorate { get; set; }
        public int? Priority { get; set; }

        // Vacation restrictions (specific to type)
        public EnumGenderType Gender { get; set; } = EnumGenderType.All;
        public EnumReligionType Religion { get; set; } = EnumReligionType.All;
    }
}
