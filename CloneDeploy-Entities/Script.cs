using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CloneDeploy_Entities
{
    [Table("scripts", Schema = "public")]
    public class ScriptEntity
    {
        [Column("script_contents")]
        public string Contents { get; set; }

        [Column("script_description")]
        public string Description { get; set; }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("script_id")]
        public int Id { get; set; }

        [Column("script_name")]
        public string Name { get; set; }
    }
}