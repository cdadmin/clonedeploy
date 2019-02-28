using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using CsvHelper.Configuration;

namespace CloneDeploy_Entities
{
    [Table("groups", Schema = "public")]
    public class GroupEntity
    {
        public GroupEntity()
        {
            ImageId = -1;
            ImageProfileId = -1;
            SetDefaultProperties = 0;
            SetDefaultBootMenu = 0;
        }

        [Column("cluster_group_id")]
        public int ClusterGroupId { get; set; }

        [Column("group_description")]
        public string Description { get; set; }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("group_id")]
        public int Id { get; set; }

        [Column("group_image_id")]
        public int ImageId { get; set; }

        [Column("group_image_profile_id")]
        public int ImageProfileId { get; set; }

        [Column("group_name")]
        public string Name { get; set; }

        [Column("group_default_bootmenu_enabled")]
        public int SetDefaultBootMenu { get; set; }

        [Column("group_default_properties_enabled")]
        public int SetDefaultProperties { get; set; }

        [Column("group_smart_criteria")]
        public string SmartCriteria { get; set; }

        [Column("group_type")]
        public string Type { get; set; }

        [Column("smart_type")]
        public string SmartType { get; set; }
    }

    [NotMapped]
    public class GroupWithImage : GroupEntity
    {
        public ImageEntity Image { get; set; }
    }

    public sealed class GroupCsvMap : ClassMap<GroupEntity>
    {
        public GroupCsvMap()
        {
            Map(m => m.Name).Name("Name");
            Map(m => m.Description).Name("Description");
            Map(m => m.Type).Name("Type");
            Map(m => m.SmartCriteria).Name("SmartCriteria");
        }
    }
}