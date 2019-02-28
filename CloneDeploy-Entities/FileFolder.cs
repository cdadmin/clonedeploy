using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CloneDeploy_Entities
{
    [Table("files_folders")]
    public class FileFolderEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("file_folder_id")]
        public int Id { get; set; }

        [Column("file_folder_display_name")]
        public string Name { get; set; }

        [Column("file_folder_path")]
        public string Path { get; set; }

        [Column("file_folder_type")]
        public string Type { get; set; }
    }
}