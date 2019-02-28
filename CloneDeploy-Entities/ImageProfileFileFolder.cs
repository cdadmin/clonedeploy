using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CloneDeploy_Entities
{
    [Table("image_profile_files_folders")]
    public class ImageProfileFileFolderEntity
    {
        [Column("destination_folder")]
        public string DestinationFolder { get; set; }

        [Column("destination_partition")]
        public string DestinationPartition { get; set; }

        [Column("file_folder_id")]
        public int FileFolderId { get; set; }

        [Column("folder_copy_type")]
        public string FolderCopyType { get; set; }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("image_profile_files_folders_id")]
        public int Id { get; set; }

        [Column("priority")]
        public int Priority { get; set; }

        [Column("image_profile_id")]
        public int ProfileId { get; set; }
    }
}