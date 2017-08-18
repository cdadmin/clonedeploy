using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CloneDeploy_Entities
{
    [Table("sites")]
    public class SiteEntity
    {
        [Column("site_distribution_point", Order = 3)]
        public int ClusterGroupId { get; set; }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("site_id", Order = 1)]
        public int Id { get; set; }

        [Column("site_name", Order = 2)]
        public string Name { get; set; }
    }

    [NotMapped]
    public class SiteWithClusterGroup : SiteEntity
    {
        public ClusterGroupEntity ClusterGroup { get; set; }
    }
}