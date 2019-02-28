using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CloneDeploy_Entities
{
    [Table("image_profile_sysprep_tags")]
    public class ImageProfileSysprepTagEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("image_profile_sysprep_tag_id")]
        public int Id { get; set; }

        [Column("priority")]
        public int Priority { get; set; }

        [Column("image_profile_id")]
        public int ProfileId { get; set; }

        [Column("sysprep_tag_id")]
        public int SysprepId { get; set; }
    }
}