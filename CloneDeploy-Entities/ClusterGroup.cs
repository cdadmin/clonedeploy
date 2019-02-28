using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CloneDeploy_Entities
{
    [Table("cluster_groups")]
    public class ClusterGroupEntity
    {
        [Column("default_cluster_group")]
        public int Default { get; set; }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("cluster_group_id")]
        public int Id { get; set; }

        [Column("cluster_group_name")]
        public string Name { get; set; }
    }
}