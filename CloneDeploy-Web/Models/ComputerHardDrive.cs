using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Models
{
    [Table("computer_harddrives")]
    public class ComputerHardDrive
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("computer_harddrive_id", Order = 1)]
        public int Id { get; set; }

        [Column("computer_id", Order = 2)]
        public int ComputerId { get; set; }

        [Column("model", Order = 3)]
        public string Model { get; set; }

        [Column("serial_number", Order = 4)]
        public string Serial { get; set; }

        [Column("capacity", Order = 5)]
        public string Capacity { get; set; }

        [Column("smart_status", Order = 6)]
        public string SmartStatus { get; set; }
    }
}