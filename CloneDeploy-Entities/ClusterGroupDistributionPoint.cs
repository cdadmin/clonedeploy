using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CloneDeploy_Entities
{
    [Table("cluster_group_distribution_points")]
    public class ClusterGroupDistributionPointEntity
    {
        [Column("cluster_group_id")]
        public int ClusterGroupId { get; set; }

        [Column("distribution_point_id")]
        public int DistributionPointId { get; set; }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("cluster_group_distribution_points_id")]
        public int Id { get; set; }
    }
}