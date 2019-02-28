using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using CsvHelper.Configuration;

namespace CloneDeploy_Entities
{
    [Table("images")]
    public class ImageEntity
    {
        [Column("image_approved")]
        public int Approved { get; set; }

        [Column("image_classification_id")]
        public int ClassificationId { get; set; }

        [Column("image_description")]
        public string Description { get; set; }

        [Column("image_enabled")]
        public int Enabled { get; set; }

        [Column("image_environment")]
        public string Environment { get; set; }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("image_id")]
        public int Id { get; set; }

        [Column("image_is_viewable_ond")]
        public int IsVisible { get; set; }

        [Column("last_upload_guid")]
        public string LastUploadGuid { get; set; }

        [Column("image_name")]
        public string Name { get; set; }

        [Column("image_os")]
        public string Os { get; set; }

        [Column("image_is_protected")]
        public int Protected { get; set; }

        [Column("image_type")]
        public string Type { get; set; }
    }

    [NotMapped]
    public class ImageWithDate : ImageEntity
    {
        public DateTime? LastUsed { get; set; }
    }

    public sealed class ImageCsvMap : ClassMap<ImageEntity>
    {
        public ImageCsvMap()
        {
            Map(m => m.Name).Name("Name");
            Map(m => m.Description).Name("Description");
            Map(m => m.Type).Name("Type");
            Map(m => m.Environment).Name("Environment");
            Map(m => m.LastUploadGuid).Name("LastUploadGuid");
        }
    }
}