using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Models
{
    [Table("groups", Schema = "public")]
    public class Group
    {
        public Group()
        {
            Members = new List<Computer>();
            Image = -1;
            ImageProfile = -1;
            SetDefaultProperties = 0;
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("group_id", Order = 1)]
        public int Id { get; set; }

        [Column("group_name", Order = 2)]
        public string Name { get; set; }

        [Column("group_description", Order = 3)]
        public string Description { get; set; }

        [Column("group_image_id", Order = 4)]
        public int Image { get; set; }

        [Column("group_image_profile_id", Order = 5)]
        public int ImageProfile { get; set; }

        [Column("group_type", Order = 6)]
        public string Type { get; set; }

        [Column("group_sender_arguments", Order = 7)]
        public string SenderArguments { get; set; }

        [Column("group_receiver_arguments", Order = 8)]
        public string ReceiverArguments { get; set; }

        [Column("group_smart_criteria", Order = 9)]
        public string SmartCriteria { get; set; }

        [Column("group_default_properties_enabled", Order = 10)]
        public int SetDefaultProperties { get; set; }

        [NotMapped] 
        public List<Computer> Members { get; set; }      
    }
}