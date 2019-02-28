using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using CsvHelper.Configuration;

namespace CloneDeploy_Entities
{
    [Table("computers")]
    public class ComputerEntity
    {
        [Column("alternate_server_ip_id")]
        public int AlternateServerIpId { get; set; }

        [Column("computer_building_id")]
        public int BuildingId { get; set; }

        [Column("client_identifier")]
        public string ClientIdentifier { get; set; }

        [Column("cluster_group_id")]
        public int ClusterGroupId { get; set; }

        [Column("custom_attr_1")]
        public string CustomAttribute1 { get; set; }

        [Column("custom_attr_2")]
        public string CustomAttribute2 { get; set; }

        [Column("custom_attr_3")]
        public string CustomAttribute3 { get; set; }

        [Column("custom_attr_4")]
        public string CustomAttribute4 { get; set; }

        [Column("custom_attr_5")]
        public string CustomAttribute5 { get; set; }

        [Column("computer_has_custom_menu")]
        public int CustomBootEnabled { get; set; }

        [Column("computer_description")]
        public string Description { get; set; }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("computer_id")]
        public int Id { get; set; }

        [Column("computer_image_id")]
        public int ImageId { get; set; }

        [Column("computer_image_profile_id")]
        public int ImageProfileId { get; set; }

        [Column("computer_primary_mac")]
        public string Mac { get; set; }

        [Column("computer_name")]
        public string Name { get; set; }

        [Column("proxy_reservation_enabled")]
        public int ProxyReservation { get; set; }

        [Column("computer_room_id")]
        public int RoomId { get; set; }

        [Column("computer_site_id")]
        public int SiteId { get; set; }
    }

    [NotMapped]
    public class ComputerWithImage : ComputerEntity
    {
        public ImageEntity Image { get; set; }
        public ImageProfileEntity ImageProfile { get; set; }
    }

    public sealed class ComputerCsvMap : ClassMap<ComputerEntity>
    {
        public ComputerCsvMap()
        {
            Map(m => m.Name).Name("Name");
            Map(m => m.Mac).Name("Mac");
            Map(m => m.Description).Name("Description");
            Map(m => m.CustomAttribute1).Name("CustomAttribute1");
            Map(m => m.CustomAttribute2).Name("CustomAttribute2");
            Map(m => m.CustomAttribute3).Name("CustomAttribute3");
            Map(m => m.CustomAttribute4).Name("CustomAttribute4");
            Map(m => m.CustomAttribute5).Name("CustomAttribute5");
            Map(m => m.ClientIdentifier).Name("ClientIdentifier");
        }
    }
}