using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Models
{
    [Table("image_profile_files_folders")]
    public class ImageProfileFileFolder
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("image_profile_files_folders_id", Order = 1)]
        public int Id { get; set; }
        [Column("image_profile_id", Order = 2)]
        public int ProfileId { get; set; }
        [Column("file_folder_id", Order = 3)]
        public int FileFolderId { get; set; }
        [Column("priority", Order = 4)]
        public int Priority { get; set; }
    }
}