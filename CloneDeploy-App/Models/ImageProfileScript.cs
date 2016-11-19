using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CloneDeploy_App.Models
{
    /// <summary>
    /// Summary description for ImageProfilePartitions
    /// </summary>
    [Table("image_profile_scripts")]
    public class ImageProfileScript
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("image_profile_script_id", Order = 1)]
        public int Id { get; set; }
        [Column("image_profile_id", Order = 2)]
        public int ProfileId { get; set; }
        [Column("script_id", Order = 3)]
        public int ScriptId { get; set; }
        [Column("run_pre", Order = 4)]
        public int RunPre { get; set; }
        [Column("run_post", Order = 5)]
        public int RunPost { get; set; }
        [Column("priority", Order = 6)]
        public int Priority { get; set; }

    }
}