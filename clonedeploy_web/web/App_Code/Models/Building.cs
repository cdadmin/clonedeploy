using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Models
{
    [Table("buildings")]
    public class Building
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("building_id", Order = 1)]
        public int Id { get; set; }
        [Column("building_name", Order = 2)]
        public string Name { get; set; }  
    }
}