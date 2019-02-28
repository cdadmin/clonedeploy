using System.ComponentModel.DataAnnotations.Schema;

namespace CloneDeploy_Entities
{
    [Table("computer_proxy_reservations")]
    public class ComputerProxyReservationEntity
    {
        [Column("boot_file")]
        public string BootFile { get; set; }

        [Column("computer_id")]
        public int ComputerId { get; set; }

        [Column("computer_proxy_reservation_id")]
        public int Id { get; set; }

        [Column("next_server")]
        public string NextServer { get; set; }
    }
}