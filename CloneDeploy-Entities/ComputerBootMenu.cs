using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CloneDeploy_Entities
{
    [Table("computer_boot_menus")]
    public class ComputerBootMenuEntity
    {
        [Column("bios_menu", Order = 3)]
        public string BiosMenu { get; set; }

        [Column("computer_id", Order = 2)]
        public int ComputerId { get; set; }

        [Column("efi32_menu", Order = 4)]
        public string Efi32Menu { get; set; }

        [Column("efi64_menu", Order = 5)]
        public string Efi64Menu { get; set; }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("computer_boot_menu_id", Order = 1)]
        public int Id { get; set; }
    }
}