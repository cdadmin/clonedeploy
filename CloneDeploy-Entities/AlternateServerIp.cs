using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CloneDeploy_Entities
{
    [Table("alternate_server_ips")]
    public class AlternateServerIpEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("alternate_server_ip_id", Order = 1)]
        public int Id { get; set; }

        [Column("alternate_server_ip", Order = 2)]
        public string Ip { get; set; }

        [Column("alternate_server_ip_api", Order = 3)]
        public string ApiUrl { get; set; }
    }
}