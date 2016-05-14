using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Models
{
    [Table("munki_conditionals")]
    public class MunkiConditional
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("munki_conditional_id", Order = 1)]
        public int Id { get; set; }

        [Column("munki_conditional_name", Order = 2)]
        public string Name { get; set; }

        [Column("munki_conditional_description", Order = 3)]
        public string Description { get; set; }

        [Column("munki_conditional_content", Order = 4)]
        public string Content { get; set; }
  
    }
}