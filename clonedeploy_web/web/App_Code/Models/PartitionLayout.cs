using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.Infrastructure;
using System.Linq;
using Global;

namespace Models
{
    /// <summary>
    /// Summary description for PartitionLayout
    /// </summary>
    [Table("partition_layouts", Schema = "public")]
    public class PartitionLayout
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("partition_layout_id", Order = 1)]
        public int Id { get; set; }
        [Column("partition_layout_name", Order = 2)]
        public string Name { get; set; }
        [Column("partition_layout_table", Order = 3)]
        public string Table { get; set; }
        [Column("imaging_environment", Order = 4)]
        public string ImageEnvironment { get; set; }
        [Column("partition_layout_priority", Order = 5)]
        public int Priority { get; set; }

        

    }
}