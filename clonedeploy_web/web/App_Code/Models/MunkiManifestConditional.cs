using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Models
{
    [Table("munki_manifest_conditionals")]
    public class MunkiManifestConditional
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("munki_manifest_conditional_id", Order = 1)]
        public int Id { get; set; }

        [Column("munki_conditional_id", Order = 2)]
        public int ConditionalId { get; set; }

        [Column("munki_manifest_template_id", Order = 3)]
        public int ManifestTemplateId { get; set; }
  
    }
}