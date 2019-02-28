using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CloneDeploy_Entities
{
    [Table("group_boot_menus")]
    public class GroupBootMenuEntity
    {
        [Column("bios_menu")]
        public string BiosMenu { get; set; }

        [Column("efi32_menu")]
        public string Efi32Menu { get; set; }

        [Column("efi64_menu")]
        public string Efi64Menu { get; set; }

        [Column("group_id")]
        public int GroupId { get; set; }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("group_boot_menu_id")]
        public int Id { get; set; }
    }
}