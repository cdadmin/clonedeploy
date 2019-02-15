﻿using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using CsvHelper.Configuration;

namespace CloneDeploy_Entities
{
    [Table("images")]
    public class ImageEntity
    {
        [Column("image_approved", Order = 10)]
        public int Approved { get; set; }

        [Column("image_classification_id", Order = 14)]
        public int ClassificationId { get; set; }

        [Column("image_description", Order = 4)]
        public string Description { get; set; }

        [Column("image_enabled", Order = 7)]
        public int Enabled { get; set; }

        [Column("image_environment", Order = 9)]
        public string Environment { get; set; }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("image_id", Order = 1)]
        public int Id { get; set; }

        [Column("image_is_viewable_ond", Order = 6)]
        public int IsVisible { get; set; }

        [Column("last_upload_guid", Order = 15)]
        public string LastUploadGuid { get; set; }

        [Column("image_name", Order = 2)]
        public string Name { get; set; }

        [Column("image_os", Order = 3)]
        public string Os { get; set; }

        [Column("image_is_protected", Order = 5)]
        public int Protected { get; set; }

        [Column("image_type", Order = 8)]
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