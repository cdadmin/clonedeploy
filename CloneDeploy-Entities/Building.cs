using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CloneDeploy_Entities
{
    [Table("buildings")]
    public class BuildingEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("building_id", Order = 1)]
        public int Id { get; set; }
        [Column("building_name", Order = 2)]
        public string Name { get; set; }
        
        [Column("building_distribution_point", Order = 3)]
        public int ClusterGroupId { get; set; }
    }

    [NotMapped]
    public class BuildingWithClusterGroup : BuildingEntity
    {
        public ClusterGroupEntity ClusterGroup { get; set; }
    }
}