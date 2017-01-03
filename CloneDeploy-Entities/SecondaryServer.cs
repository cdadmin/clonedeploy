using System.ComponentModel.DataAnnotations.Schema;

namespace CloneDeploy_Entities
{
    [Table("secondary_servers")]
    public class SecondaryServerEntity
    {
        [Column("secondary_server_id", Order = 1)]
        public int Id { get; set; }

        [Column("secondary_server_name", Order = 2)]
        public string Name { get; set; }

        [Column("api_url", Order = 3)]
        public string ApiURL { get; set; }

        [Column("service_account_name", Order = 4)]
        public string ServiceAccountName { get; set; }

        [Column("service_account_password_enc", Order = 5)]
        public string ServiceAccountPassword { get; set; } 
    }
}