using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CloneDeploy_Entities
{
    [Table("clonedeploy_version")]
    public class CdVersionEntity
    {
        [Column("app_version")]
        public string AppVersion { get; set; }

        [Column("database_version")]
        public string DatabaseVersion { get; set; }

        [Column("first_run_completed")]
        public int FirstRunCompleted { get; set; }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("clonedeploy_version_id")]
        public int Id { get; set; }
    }
}