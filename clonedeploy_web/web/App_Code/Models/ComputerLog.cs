using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Models
{
    [Table("computer_logs")]
    public class ComputerLog
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("computer_log_id", Order = 1)]
        public int Id { get; set; }

        [Column("computer_id", Order = 2)]
        public int ComputerId { get; set; }

        [Column("log_type", Order = 3)]
        public string Type { get; set; }

        [Column("log_sub_type", Order = 4)]
        public string SubType { get; set; }

        [Column("log_contents", Order = 5)]
        public string Contents { get; set; }

        [Column("log_time", Order = 6)]
        public DateTime? LogTime { get; set; }

        [Column("computer_mac", Order = 7)]
        public string Mac { get; set; }
    }
}