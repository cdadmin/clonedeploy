using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CloneDeploy_App.Models
{
    [Table("munki_manifest_catalogs")]
    public class MunkiManifestCatalog
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("munki_manifest_catalog_id", Order = 1)]
        public int Id { get; set; }

        [Column("munki_manifest_catalog_name", Order = 2)]
        public string Name { get; set; }

        [Column("munki_manifest_catalog_priority", Order = 3)]
        public int Priority { get; set; }

        [Column("munki_manifest_template_id", Order = 4)]
        public int ManifestTemplateId { get; set; }
  
    }
}