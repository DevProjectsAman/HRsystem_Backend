
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using static HRsystem.Api.Enums.EnumsList;
namespace HRsystem.Api.Database.DataTables
{
   

    [Table("Tb_Vacation_Rules_Group_Detail")]
    public class TbVacationRulesGroupDetail
    {
        [Key]
        public int DetailId { get; set; }

        [ForeignKey(nameof(VacationRulesGroup))]
        public int GroupId { get; set; }

        public int VacationTypeId { get; set; }

        // 🌴 Vacation behavior
        public int YearlyBalance { get; set; }
        public bool? Prorate { get; set; }
        public int? Priority { get; set; }

        // 👤 Vacation restrictions (specific to type)
        public EnumGenderType Gender { get; set; } = EnumGenderType.All;
        public EnumReligionType Religion { get; set; } = EnumReligionType.All;

        public virtual TbVacationRulesGroup VacationRulesGroup { get; set; } = null!;
        public virtual TbVacationType VacationType { get; set; } = null!;
    }

}
