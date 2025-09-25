 
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using HRsystem.Api.Shared.DTO;

    namespace HRsystem.Api.Database.DataTables
    {
        [Table("tb_holiday_types")]
        public class TbHolidayType
        {
            [Key]
            public int HolidayTypeId { get; set; }

            /// <summary>
            /// Localized holiday type name (en, ar).
            /// Stored as JSON in DB.
            /// </summary>
            [Required]
            public LocalizedData HolidayTypeName { get; set; } = new LocalizedData();

            // Navigation property
            public ICollection<TbHolidays> Holidays { get; set; } = new List<TbHolidays>();
        }
    }


