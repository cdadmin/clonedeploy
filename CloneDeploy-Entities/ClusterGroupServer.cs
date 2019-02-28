using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CloneDeploy_Entities
{
    [Table("cluster_group_servers")]
    public class ClusterGroupServerEntity
    {
        [Column("cluster_group_id")]
        public int ClusterGroupId { get; set; }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("cluster_group_servers_id")]
        public int Id { get; set; }

        [Column("multicast_role")]
        public int MulticastRole { get; set; }

        [Column("secondary_server_id")]
        public int ServerId { get; set; }

        [Column("tftp_role")]
        public int TftpRole { get; set; }
    }
}