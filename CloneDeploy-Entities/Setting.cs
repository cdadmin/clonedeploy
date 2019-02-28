using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CloneDeploy_Entities
{
    [Table("admin_settings")]
    public class SettingEntity
    {
        [Column("admin_setting_category")]
        public string Category { get; set; }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("admin_setting_id")]
        public string Id { get; set; }

        [Column("admin_setting_name")]
        public string Name { get; set; }

        [Column("admin_setting_value")]
        public string Value { get; set; }
    }
}