using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CloneDeploy_Entities
{
    [Table("computer_logs")]
    public class ComputerLogEntity
    {
        [Column("computer_id")]
        public int ComputerId { get; set; }

        [Column("log_contents")]
        public string Contents { get; set; }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("computer_log_id")]
        public int Id { get; set; }

        [Column("log_time")]
        public DateTime? LogTime { get; set; }

        [Column("computer_mac")]
        public string Mac { get; set; }

        [Column("log_sub_type")]
        public string SubType { get; set; }

        [Column("log_type")]
        public string Type { get; set; }
    }
}