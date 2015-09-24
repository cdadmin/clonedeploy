using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.Infrastructure;
using System.IO;
using System.Linq;
using System.Web;
using Global;

namespace Models
{
    [Table("admin_settings")]
    public class Setting
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("admin_setting_id", Order = 1)]
        public string Id { get; set; }

        [Column("admin_setting_name", Order = 2)]
        public string Name { get; set; }

        [Column("admin_setting_value", Order = 3)]
        public string Value { get; set; }

        [Column("admin_setting_category", Order = 4)]
        public string Category { get; set; }

        
    }
}