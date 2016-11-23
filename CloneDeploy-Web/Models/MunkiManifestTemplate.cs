using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CloneDeploy_Web.Models
{
    [Table("munki_manifest_templates")]
    public class MunkiManifestTemplate
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("manifest_template_id", Order = 1)]
        public int Id { get; set; }

        [Column("manifest_template_name", Order = 2)]
        public string Name { get; set; }

        [Column("manifest_template_description", Order = 3)]
        public string Description { get; set; }

        [Column("changes_applied", Order = 4)]
        public int ChangesApplied { get; set; }
  
    }
}