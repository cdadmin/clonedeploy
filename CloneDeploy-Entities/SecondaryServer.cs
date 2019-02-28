using System.ComponentModel.DataAnnotations.Schema;

namespace CloneDeploy_Entities
{
    [Table("secondary_servers")]
    public class SecondaryServerEntity
    {
        [Column("api_url")]
        public string ApiURL { get; set; }

        [Column("secondary_server_id")]
        public int Id { get; set; }

        [Column("last_token")]
        public string LastToken { get; set; }

        [Column("multicast_role")]
        public int MulticastRole { get; set; }

        [Column("secondary_server_name")]
        public string Name { get; set; }

        [Column("service_account_name")]
        public string ServiceAccountName { get; set; }

        [Column("service_account_password_enc")]
        public string ServiceAccountPassword { get; set; }

        [Column("tftp_role")]
        public int TftpRole { get; set; }

        [Column("is_active")]
        public int IsActive { get; set; }
    }
}