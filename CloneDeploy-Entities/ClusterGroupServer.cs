using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CloneDeploy_Entities
{
    [Table("cluster_group_servers")]
    public class ClusterGroupServerEntity
    {
        [Column("cluster_group_id", Order = 2)]
        public int ClusterGroupId { get; set; }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("cluster_group_servers_id", Order = 1)]
        public int Id { get; set; }

        [Column("multicast_role", Order = 5)]
        public int MulticastRole { get; set; }

        [Column("secondary_server_id", Order = 3)]
        public int ServerId { get; set; }

        [Column("tftp_role", Order = 4)]
        public int TftpRole { get; set; }
    }
}