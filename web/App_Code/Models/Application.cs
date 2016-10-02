using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Models
{
    [Table("applications")]
    public class Application
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("application_id", Order = 1)]
        public int Id { get; set; }

        [Column("application_name", Order = 2)]
        public string Name { get; set; }

        [Column("application_version", Order = 3)]
        public string Version { get; set; }

        [Column("application_guid", Order = 4)]
        public string Guid { get; set; }
    }
}