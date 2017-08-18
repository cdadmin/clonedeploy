using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CloneDeploy_Entities
{
    [Table("group_boot_menus")]
    public class GroupBootMenuEntity
    {
        [Column("bios_menu", Order = 3)]
        public string BiosMenu { get; set; }

        [Column("efi32_menu", Order = 4)]
        public string Efi32Menu { get; set; }

        [Column("efi64_menu", Order = 5)]
        public string Efi64Menu { get; set; }

        [Column("group_id", Order = 2)]
        public int GroupId { get; set; }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("group_boot_menu_id", Order = 1)]
        public int Id { get; set; }
    }
}