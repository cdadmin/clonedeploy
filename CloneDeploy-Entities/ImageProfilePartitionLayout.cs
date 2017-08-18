using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CloneDeploy_Entities
{
    /// <summary>
    ///     Summary description for ImageProfilePartitions
    /// </summary>
    [Table("image_profile_partition_layouts")]
    public class ImageProfilePartitionLayoutEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("image_profile_partition_layout_id", Order = 1)]
        public int Id { get; set; }

        [Column("partition_layout_id", Order = 3)]
        public int LayoutId { get; set; }

        [Column("image_profile_id", Order = 2)]
        public int ProfileId { get; set; }
    }
}