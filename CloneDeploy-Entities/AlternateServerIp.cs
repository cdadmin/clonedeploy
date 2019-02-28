using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CloneDeploy_Entities
{
    [Table("alternate_server_ips")]
    public class AlternateServerIpEntity
    {
        [Column("alternate_server_ip_api")]
        public string ApiUrl { get; set; }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("alternate_server_ip_id")]
        public int Id { get; set; }

        [Column("alternate_server_ip")]
        public string Ip { get; set; }
    }
}