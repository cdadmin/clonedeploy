using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Models
{
    [Table("computer_printers")]
    public class ComputerPrinter
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("computer_printer_id", Order = 1)]
        public int Id { get; set; }

        [Column("computer_id", Order = 2)]
        public int ComputerId { get; set; }

        [Column("printer_name", Order = 3)]
        public string Name { get; set; }

        [Column("printer_model", Order = 4)]
        public string Model { get; set; }

        [Column("printer_uri", Order = 5)]
        public string Uri { get; set; }
    }
}