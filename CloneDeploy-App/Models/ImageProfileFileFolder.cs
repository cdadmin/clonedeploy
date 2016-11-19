using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CloneDeploy_App.Models
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
        [Column("destination_partition", Order = 5)]
        public string DestinationPartition { get; set; }
        [Column("destination_folder", Order = 6)]
        public string DestinationFolder { get; set; }
        [Column("folder_copy_type", Order = 7)]
        public string FolderCopyType { get; set; }
    }
}