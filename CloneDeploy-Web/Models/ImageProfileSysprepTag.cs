using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CloneDeploy_Web.Models
{
    [Table("image_profile_sysprep_tags")]
    public class ImageProfileSysprepTag
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("image_profile_sysprep_tag_id", Order = 1)]
        public int Id { get; set; }
        [Column("image_profile_id", Order = 2)]
        public int ProfileId { get; set; }
        [Column("sysprep_tag_id", Order = 3)]
        public int SysprepId { get; set; }
        [Column("priority", Order = 4)]
        public int Priority { get; set; }
    }
}