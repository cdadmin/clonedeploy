using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.Core.Metadata.Edm;

namespace Models
{
    [Table("computers")]
    public class Computer
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
        public int ImageProfile { get; set; }

        [Column("computer_has_custom_menu", Order = 11)]
        public int CustomBootEnabled { get; set; }

      

        [NotMapped]
        public string TaskId { get; set; }

        [NotMapped]
        public virtual Models.Image Image { get; set; }



     

      

    }
}