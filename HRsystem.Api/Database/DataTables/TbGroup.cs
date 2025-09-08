using System.ComponentModel.DataAnnotations;

namespace HRsystem.Api.Database.DataTables
{
    
    public class TbGroup
    {
        [Key]
        public int group_id { get; set; }

        public string group_name { get; set; }

        // Navigation property
        public virtual ICollection<TbCompany> TbCompanies { get; set; }
    }
}
