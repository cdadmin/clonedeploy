using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using CsvHelper.Configuration;

namespace CloneDeploy_Entities
{
    [Table("computers")]
    public class ComputerEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("computer_id", Order = 1)]
        public int Id { get; set; }

        [Column("computer_name", Order = 2)]
        public string Name { get; set; }

        [Column("computer_primary_mac", Order = 3)]
        public string Mac { get; set; }

        [Column("computer_description", Order = 4)]
        public string Description { get; set; }

        [Column("computer_site_id", Order = 6)]
        public int SiteId { get; set; }

        [Column("computer_building_id", Order = 7)]
        public int BuildingId { get; set; }

        [Column("computer_room_id", Order = 8)]
        public int RoomId { get; set; }

        [Column("computer_image_id", Order = 9)]
        public int ImageId { get; set; }

        [Column("computer_image_profile_id", Order = 10)]
        public int ImageProfileId { get; set; }

        [Column("computer_has_custom_menu", Order = 11)]
        public int CustomBootEnabled { get; set; }

        [Column("custom_attr_1", Order = 12)]
        public string CustomAttribute1 { get; set; }

        [Column("custom_attr_2", Order = 13)]
        public string CustomAttribute2 { get; set; }

        [Column("custom_attr_3", Order = 14)]
        public string CustomAttribute3 { get; set; }

        [Column("custom_attr_4", Order = 15)]
        public string CustomAttribute4 { get; set; }

        [Column("custom_attr_5", Order = 16)]
        public string CustomAttribute5 { get; set; }

        [Column("proxy_reservation_enabled", Order = 17)]
        public int ProxyReservation { get; set; }

        [Column("cluster_group_id", Order = 18)]
        public int ClusterGroupId { get; set; }   
    }

    [NotMapped]
    public class ComputerWithImage : ComputerEntity
    {
        public ImageEntity Image { get; set; }
    }

    public sealed class ComputerCsvMap : CsvClassMap<ComputerEntity>
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
        }
    }

  
}