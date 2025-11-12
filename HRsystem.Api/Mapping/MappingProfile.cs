using AutoMapper;
using HRsystem.Api.Database.DataTables;
using HRsystem.Api.Features.Scheduling.Shift.GetAllShifts;
using HRsystem.Api.Features.Scheduling.VacationRule;
using HRsystem.Api.Shared.DTO;

namespace HRsystem.Api.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            // TbShift → ShiftDto
            CreateMap<TbShift, ShiftDto>().ReverseMap();

            CreateMap<TbVacationRule, VacationRuleDto>().ReverseMap();

            // Example: if you also have TbShiftRule → ShiftRuleDto
            // CreateMap<TbShiftRule, ShiftRuleDto>().ReverseMap();
        }
    }
}
