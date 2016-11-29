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
        public int DistributionPointId { get; set; }

        [NotMapped]
        public virtual DistributionPointEntity DistributionPoint { get; set; }
    }
}