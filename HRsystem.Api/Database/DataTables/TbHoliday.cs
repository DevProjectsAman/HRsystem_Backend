using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using HRsystem.Api.Shared.DTO;

namespace HRsystem.Api.Database.DataTables
{
    [Table("tb_holidays")]
    public class TbHolidays
    {
        [Key]
        public int HolidayId { get; set; }

        [Required]
        [ForeignKey(nameof(HolidayType))]
        public int HolidayTypeId { get; set; }

        public TbHolidayType HolidayType { get; set; } = null!;

        /// <summary>
        /// Localized holiday name (en, ar).
        /// Stored as JSON in DB.
        /// </summary>
        [Required]
        public LocalizedData HolidayName { get; set; } = new LocalizedData();

        [Required]
        public DateTime StartDate { get; set; }

        [Required]
        public DateTime EndDate { get; set; }

        /// <summary>
        /// 0 → All, 1 → For Christians only
        /// </summary>
        public bool IsForChristiansOnly { get; set; } = false;

        /// <summary>
        /// Optional company-specific holiday
        /// </summary>
        public int? CompanyId { get; set; }
    }
}
