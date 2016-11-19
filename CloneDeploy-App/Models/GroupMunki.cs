using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CloneDeploy_App.Models
{
    [Table("group_munki_templates")]
    public class GroupMunki
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("group_munki_template_id", Order = 1)]
        public int Id { get; set; }

        [Column("group_id", Order = 2)]
        public int GroupId { get; set; }

        [Column("munki_template_id", Order = 3)]
        public int MunkiTemplateId { get; set; }

    }
}