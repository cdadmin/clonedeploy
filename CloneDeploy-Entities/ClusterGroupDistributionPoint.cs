using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CloneDeploy_Entities
{
    [Table("cluster_group_distribution_points")]
    public class ClusterGroupDistributionPointEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("cluster_group_distribution_points_id", Order = 1)]
        public int Id { get; set; }

        [Column("cluster_group_id", Order = 2)]
        public int ClusterGroupId { get; set; }

        [Column("distribution_point_id", Order = 3)]
        public int DistributionPointId { get; set; }
    }
}