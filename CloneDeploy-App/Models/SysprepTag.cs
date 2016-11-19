using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CloneDeploy_App.Models
{

    [Table("sysprep_tags")]
    public class SysprepTag
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("sysprep_tag_id", Order = 1)]
        public int Id { get; set; }
        [Column("sysprep_tag_name", Order = 2)]
        public string Name { get; set; }
        [Column("sysprep_tag_open", Order = 3)]
        public string OpeningTag { get; set; }
        [Column("sysprep_tag_close", Order = 4)]
        public string ClosingTag { get; set; }
        [Column("sysprep_tag_description", Order = 5)]
        public string Description { get; set; }
        [Column("sysprep_tag_contents", Order = 6)]
        public string Contents { get; set; }

        
    }
}