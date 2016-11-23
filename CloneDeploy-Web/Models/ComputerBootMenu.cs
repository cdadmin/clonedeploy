using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CloneDeploy_Web.Models
{
    [Table("computer_boot_menus")]
    public class ComputerBootMenu
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("computer_boot_menu_id", Order = 1)]
        public int Id { get; set; }

        [Column("computer_id", Order = 2)]
        public int ComputerId { get; set; }

        [Column("bios_menu", Order = 3)]
        public string BiosMenu { get; set; }

        [Column("efi32_menu", Order = 4)]
        public string Efi32Menu { get; set; }

        [Column("efi64_menu", Order = 5)]
        public string Efi64Menu { get; set; }


      



     

      

    }
}