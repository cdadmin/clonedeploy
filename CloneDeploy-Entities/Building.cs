using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CloneDeploy_Entities
{
    [Table("buildings")]
    public class BuildingEntity
    {
        [Column("building_distribution_point")]
        public int ClusterGroupId { get; set; }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("building_id")]
        public int Id { get; set; }

        [Column("building_name")]
        public string Name { get; set; }
    }

    [NotMapped]
    public class BuildingWithClusterGroup : BuildingEntity
    {
        public ClusterGroupEntity ClusterGroup { get; set; }
    }
}