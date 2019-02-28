using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CloneDeploy_Entities
{
    [Table("computer_boot_menus")]
    public class ComputerBootMenuEntity
    {
        [Column("bios_menu")]
        public string BiosMenu { get; set; }

        [Column("computer_id")]
        public int ComputerId { get; set; }

        [Column("efi32_menu")]
        public string Efi32Menu { get; set; }

        [Column("efi64_menu")]
        public string Efi64Menu { get; set; }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("computer_boot_menu_id")]
        public int Id { get; set; }
    }
}