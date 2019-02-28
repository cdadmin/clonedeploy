using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CloneDeploy_Entities
{
    /// <summary>
    ///     Summary description for ImageProfilePartitions
    /// </summary>
    [Table("image_profile_scripts")]
    public class ImageProfileScriptEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("image_profile_script_id")]
        public int Id { get; set; }

        [Column("priority")]
        public int Priority { get; set; }

        [Column("image_profile_id")]
        public int ProfileId { get; set; }

        [Column("run_post")]
        public int RunPost { get; set; }

        [Column("run_pre")]
        public int RunPre { get; set; }

        [Column("script_id")]
        public int ScriptId { get; set; }
    }
}