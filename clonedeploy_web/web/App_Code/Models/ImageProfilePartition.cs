using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.Infrastructure;
using System.Linq;
using Global;

namespace Models
{
    /// <summary>
    /// Summary description for ImageProfilePartitions
    /// </summary>
    [Table("image_profile_partition_layouts")]
    public class ImageProfilePartition
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("image_profile_partition_layout_id", Order = 1)]
        public int Id { get; set; }
        [Column("image_profile_id", Order = 2)]
        public int ProfileId { get; set; }
        [Column("partition_layout_id", Order = 3)]
        public int LayoutId { get; set; }

    }
}