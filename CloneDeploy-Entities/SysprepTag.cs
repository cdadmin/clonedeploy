using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CloneDeploy_Entities
{
    [Table("sysprep_tags")]
    public class SysprepTagEntity
    {
        [Column("sysprep_tag_close")]
        public string ClosingTag { get; set; }

        [Column("sysprep_tag_contents")]
        public string Contents { get; set; }

        [Column("sysprep_tag_description")]
        public string Description { get; set; }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("sysprep_tag_id")]
        public int Id { get; set; }

        [Column("sysprep_tag_name")]
        public string Name { get; set; }

        [Column("sysprep_tag_open")]
        public string OpeningTag { get; set; }
    }
}