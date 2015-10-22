using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.Core.Metadata.Edm;

namespace Models
{
    [Table("group_computer_properties")]
    public class GroupProperty
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("group_computer_property_id", Order = 1)]
        public int Id { get; set; }

        [Column("group_id", Order = 2)]
        public int GroupId { get; set; }

        [Column("image_id", Order = 3)]
        public int ImageId { get; set; }

        [Column("image_profile_id", Order = 4)]
        public int ImageProfileId { get; set; }

        [Column("description", Order = 5)]
        public string Description { get; set; }

        [Column("site_id", Order = 6)]
        public int SiteId { get; set; }

        [Column("building_id", Order = 7)]
        public int BuildingId { get; set; }

        [Column("room_id", Order = 8)]
        public int RoomId { get; set; }

        [Column("custom_attr_1", Order = 9)]
        public string CustomAttribute1 { get; set; }

        [Column("custom_attr_2", Order = 10)]
        public string CustomAttribute2 { get; set; }

        [Column("custom_attr_3", Order = 11)]
        public string CustomAttribute3 { get; set; }

        [Column("custom_attr_4", Order = 12)]
        public string CustomAttribute4 { get; set; }

        [Column("custom_attr_5", Order = 13)]
        public string CustomAttribute5 { get; set; }

        [Column("image_enabled", Order = 14)]
        public int ImageEnabled { get; set; }

        [Column("image_profile_enabled", Order = 15)]
        public int ImageProfileEnabled { get; set; }

        [Column("description_enabled", Order = 16)]
        public int DescriptionEnabled { get; set; }

        [Column("site_enabled", Order = 17)]
        public int SiteEnabled { get; set; }

        [Column("building_enabled", Order = 18)]
        public int BuildingEnabled { get; set; }

        [Column("room_enabled", Order = 19)]
        public int RoomEnabled { get; set; }

        [Column("custom_1_enabled", Order = 20)]
        public int CustomAttribute1Enabled { get; set; }

        [Column("custom_2_enabled", Order = 21)]
        public int CustomAttribute2Enabled { get; set; }

        [Column("custom_3_enabled", Order = 22)]
        public int CustomAttribute3Enabled { get; set; }

        [Column("custom_4_enabled", Order = 23)]
        public int CustomAttribute4Enabled { get; set; }

        [Column("custom_5_enabled", Order = 24)]
        public int CustomAttribute5Enabled { get; set; }


     

      

    }
}