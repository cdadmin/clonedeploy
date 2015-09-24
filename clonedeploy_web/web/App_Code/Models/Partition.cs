using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Web;
using Global;


namespace Models
{
    [Table("partitions", Schema = "public")]
    public class Partition
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("partition_id", Order = 1)]
        public int Id { get; set; }
        [Column("partition_layout_id", Order = 2)]
        public int LayoutId { get; set; }
        [Column("partition_number", Order = 3)]
        public int Number { get; set; }
        [Column("partition_type", Order = 4)]
        public string Type { get; set; }
        [Column("partition_fstype", Order = 5)]
        public string FsType { get; set; }
        [Column("partition_size", Order = 6)]
        public int Size { get; set; }
        [Column("partition_size_unit", Order = 7)]
        public string Unit { get; set; }
        [Column("partition_boot_flag", Order = 8)]
        public int Boot { get; set; }

        
    }
}