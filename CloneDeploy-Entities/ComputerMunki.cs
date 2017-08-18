using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CloneDeploy_Entities
{
    [Table("computer_munki_templates")]
    public class ComputerMunkiEntity
    {
        [Column("computer_id", Order = 2)]
        public int ComputerId { get; set; }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("computer_munki_template_id", Order = 1)]
        public int Id { get; set; }

        [Column("munki_template_id", Order = 3)]
        public int MunkiTemplateId { get; set; }
    }
}