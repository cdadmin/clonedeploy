using System.ComponentModel.DataAnnotations.Schema;

namespace CloneDeploy_Entities
{
    [Table("boot_menu_entries")]
    public class BootEntryEntity
    {
        [Column("boot_menu_entry_is_active")]
        public int Active { get; set; }

        [Column("boot_menu_entry_content")]
        public string Content { get; set; }

        [Column("boot_menu_entry_is_default")]
        public int Default { get; set; }

        [Column("boot_menu_entry_description")]
        public string Description { get; set; }

        [Column("boot_menu_entry_id")]
        public int Id { get; set; }

        [Column("boot_menu_entry_name")]
        public string Name { get; set; }

        [Column("boot_menu_entry_order")]
        public string Order { get; set; }

        [Column("boot_menu_entry_type")]
        public string Type { get; set; }
    }
}