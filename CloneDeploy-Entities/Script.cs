using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CloneDeploy_Entities
{
    [Table("scripts", Schema = "public")]
    public class ScriptEntity
    {
        [Column("script_contents", Order = 4)]
        public string Contents { get; set; }

        [Column("script_description", Order = 3)]
        public string Description { get; set; }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("script_id", Order = 1)]
        public int Id { get; set; }

        [Column("script_name", Order = 2)]
        public string Name { get; set; }
    }
}