using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Models
{

    [Table("scripts", Schema = "public")]
    public class Script
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("script_id", Order = 1)]
        public int Id { get; set; }
        [Column("script_name", Order = 2)]
        public string Name { get; set; }
        [Column("script_description", Order = 3)]
        public string Description { get; set; }
        [Column("script_priority", Order = 4)]
        public int Priority { get; set; }
        [Column("script_category_id", Order = 5)]
        public int Category { get; set; }
        [Column("script_contents", Order = 6)]
        public string Contents { get; set; }

        
    }
}