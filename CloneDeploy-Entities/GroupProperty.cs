using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CloneDeploy_Entities
{
    [Table("group_computer_properties")]
    public class GroupPropertyEntity
    {
        [Column("alternate_server_ip_id_enabled")]
        public int AlternateServerIpEnabled { get; set; }

        [Column("alternate_server_ip_id")]
        public int AlternateServerIpId { get; set; }

        [Column("bootfile")]
        public string BootFile { get; set; }

        [Column("bootfile_enabled")]
        public int BootFileEnabled { get; set; }

        [Column("building_enabled")]
        public int BuildingEnabled { get; set; }

        [Column("building_id")]
        public int BuildingId { get; set; }

        [Column("cluster_group_id_enabled")]
        public int ClusterGroupEnabled { get; set; }

        [Column("cluster_group_id")]
        public int ClusterGroupId { get; set; }

        [Column("custom_attr_1")]
        public string CustomAttribute1 { get; set; }

        [Column("custom_1_enabled")]
        public int CustomAttribute1Enabled { get; set; }

        [Column("custom_attr_2")]
        public string CustomAttribute2 { get; set; }

        [Column("custom_2_enabled")]
        public int CustomAttribute2Enabled { get; set; }

        [Column("custom_attr_3")]
        public string CustomAttribute3 { get; set; }

        [Column("custom_3_enabled")]
        public int CustomAttribute3Enabled { get; set; }

        [Column("custom_attr_4")]
        public string CustomAttribute4 { get; set; }

        [Column("custom_4_enabled")]
        public int CustomAttribute4Enabled { get; set; }

        [Column("custom_attr_5")]
        public string CustomAttribute5 { get; set; }

        [Column("custom_5_enabled")]
        public int CustomAttribute5Enabled { get; set; }

        [Column("description")]
        public string Description { get; set; }

        [Column("description_enabled")]
        public int DescriptionEnabled { get; set; }

        [Column("group_id")]
        public int GroupId { get; set; }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("group_computer_property_id")]
        public int Id { get; set; }

        [Column("image_classifications_enabled")]
        public int ImageClassificationsEnabled { get; set; }

        [Column("image_enabled")]
        public int ImageEnabled { get; set; }

        [Column("image_id")]
        public int ImageId { get; set; }

        [Column("image_profile_enabled")]
        public int ImageProfileEnabled { get; set; }

        [Column("image_profile_id")]
        public int ImageProfileId { get; set; }

        [Column("proxy_enabled")]
        public int ProxyEnabled { get; set; }

        [Column("proxy_enabled_enabled")]
        public int ProxyEnabledEnabled { get; set; }

        [Column("room_enabled")]
        public int RoomEnabled { get; set; }

        [Column("room_id")]
        public int RoomId { get; set; }

        [Column("site_enabled")]
        public int SiteEnabled { get; set; }

        [Column("site_id")]
        public int SiteId { get; set; }

        [Column("tftp_server")]
        public string TftpServer { get; set; }

        [Column("tftp_server_enabled")]
        public int TftpServerEnabled { get; set; }
    }
}