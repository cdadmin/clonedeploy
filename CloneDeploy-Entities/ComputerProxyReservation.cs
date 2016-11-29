using System.ComponentModel.DataAnnotations.Schema;

namespace CloneDeploy_Entities
{
    [Table("computer_proxy_reservations")]
    public class ComputerProxyReservationEntity
    {
        [Column("computer_proxy_reservation_id", Order = 1)]
        public int Id { get; set; }

        [Column("computer_id", Order = 2)]
        public int ComputerId { get; set; }

        [Column("next_server", Order = 3)]
        public string NextServer { get; set; }

        [Column("boot_file", Order = 4)]
        public string BootFile { get; set; }
    }
}