using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CloneDeploy_Web.Models
{
    [Table("munki_manifest_managed_updates")]
    public class MunkiManifestManagedUpdate
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("munki_manifest_managed_update_id", Order = 1)]
        public int Id { get; set; }

        [Column("munki_manifest_managed_update_name", Order = 2)]
        public string Name { get; set; }

        [Column("munki_manifest_template_id", Order = 3)]
        public int ManifestTemplateId { get; set; }

        [Column("munki_manifest_condition", Order = 4)]
        public string Condition { get; set; }
    }
}