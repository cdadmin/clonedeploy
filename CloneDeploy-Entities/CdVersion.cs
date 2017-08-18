using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CloneDeploy_Entities
{
    [Table("clonedeploy_version")]
    public class CdVersionEntity
    {
        [Column("app_version", Order = 2)]
        public string AppVersion { get; set; }

        [Column("database_version", Order = 3)]
        public string DatabaseVersion { get; set; }

        [Column("first_run_completed", Order = 4)]
        public int FirstRunCompleted { get; set; }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("clonedeploy_version_id", Order = 1)]
        public int Id { get; set; }
    }
}