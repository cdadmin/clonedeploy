using System.ComponentModel.DataAnnotations.Schema;

namespace Models
{
    [Table("boot_menu_entries")]
    public class BootEntry
    {
        [Column("boot_menu_entry_id", Order = 1)]
        public int Id { get; set; }

        [Column("boot_menu_entry_name", Order = 2)]
        public string Name { get; set; }

        [Column("boot_menu_entry_description", Order = 3)]
        public string Description { get; set; }

        [Column("boot_menu_entry_type", Order = 4)]
        public string Type { get; set; }

        [Column("boot_menu_entry_order", Order = 5)]
        public string Order { get; set; }

        [Column("boot_menu_entry_content", Order = 6)]
        public string Content { get; set; }

        [Column("boot_menu_entry_is_active", Order = 7)]
        public int Active { get; set; }

        [Column("boot_menu_entry_is_default", Order = 8)]
        public int Default { get; set; }

        
    }
}