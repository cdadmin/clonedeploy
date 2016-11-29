using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using CsvHelper.Configuration;

namespace CloneDeploy_Entities
{
    [Table("images")]
    public class ImageEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("image_id", Order = 1)]
        public int Id { get; set; }

        [Column("image_name", Order = 2)]
        public string Name { get; set; }

        [Column("image_os", Order = 3)]
        public string Os { get; set; }

        [Column("image_description", Order = 4)]
        public string Description { get; set; }

        [Column("image_is_protected", Order = 5)]
        public int Protected { get; set; }

        [Column("image_is_viewable_ond", Order = 6)]
        public int IsVisible { get; set; }

        [Column("image_enabled", Order = 7)]
        public int Enabled { get; set; }

        [Column("image_type", Order = 8)]
        public string Type { get; set; }

        [Column("image_environment", Order = 9)]
        public string Environment { get; set; }

        [Column("image_approved", Order = 10)]
        public int Approved { get; set; }

        [Column("image_osx_type", Order = 11)]
        public string OsxType { get; set; }

        [Column("image_osx_thin_os", Order = 12)]
        public string OsxThinOs { get; set; }

        [Column("image_osx_thin_recovery", Order = 13)]
        public string OsxThinRecovery { get; set; }        
    }

    public sealed class ImageCsvMap : CsvClassMap<ImageEntity>
    {
        public ImageCsvMap()
        {
            Map(m => m.Name).Name("Name");
            Map(m => m.Description).Name("Description");
            Map(m => m.Type).Name("Type");
            Map(m => m.Environment).Name("Environment");
        }
    }
}