using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CloneDeploy_Entities
{
    [Table("computer_inventory")]
    public class ComputerInventoryEntity
    {
        [Column("boot_rom", Order = 14)]
        public string BootRom { get; set; }

        [Column("client_version", Order = 6)]
        public string ClientVersion { get; set; }

        [Column("computer_id", Order = 2)]
        public int ComputerId { get; set; }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("computer_inventory_id", Order = 1)]
        public int Id { get; set; }

        [Column("ip_address", Order = 5)]
        public string Ip { get; set; }

        [Column("last_checkin", Order = 4)]
        public DateTime LastCheckinTime { get; set; }

        [Column("last_inventory_update", Order = 3)]
        public DateTime LastUpdateTime { get; set; }

        [Column("manufacturer", Order = 7)]
        public string Manufacturer { get; set; }

        [Column("model", Order = 8)]
        public string Model { get; set; }

        [Column("os_name", Order = 15)]
        public string OsName { get; set; }

        [Column("os_service_pack", Order = 17)]
        public string OsServicepack { get; set; }

        [Column("os_service_release", Order = 18)]
        public string OsServiceRelease { get; set; }

        [Column("os_version", Order = 16)]
        public string OsVersion { get; set; }

        [Column("processor", Order = 12)]
        public string Processor { get; set; }

        [Column("total_ram", Order = 13)]
        public string Ram { get; set; }

        [Column("serial_number", Order = 11)]
        public string Serial { get; set; }

        [Column("uuid", Order = 9)]
        public string Uuid { get; set; }

        [Column("uuid_type", Order = 10)]
        public string UuidType { get; set; }
    }
}