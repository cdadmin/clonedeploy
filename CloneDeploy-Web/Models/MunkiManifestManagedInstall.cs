using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Models
{
    [Table("munki_manifest_managed_installs")]
    public class MunkiManifestManagedInstall
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("munki_manifest_managed_install_id", Order = 1)]
        public int Id { get; set; }

        [Column("munki_manifest_managed_install_name", Order = 2)]
        public string Name { get; set; }

        [Column("munki_manifest_managed_install_version", Order = 3)]
        public string Version { get; set; }

        [Column("munki_manifest_managed_install_include_version", Order = 4)]
        public int IncludeVersion { get; set; }

        [Column("munki_manifest_template_id", Order = 5)]
        public int ManifestTemplateId { get; set; }

        [Column("munki_manifest_condition", Order = 6)]
        public string Condition { get; set; }
    }
}