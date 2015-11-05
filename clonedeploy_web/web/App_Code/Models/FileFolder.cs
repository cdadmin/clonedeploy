using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Models
{
    [Table("files_folders")]
    public class FileFolder
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("file_folder_id", Order = 1)]
        public int Id { get; set; }
        [Column("file_folder_display_name", Order = 2)]
        public string Name { get; set; }
        
        [Column("file_folder_path", Order = 3)]
        public string Path { get; set; }

        [Column("file_folder_type", Order = 4)]
        public string Type { get; set; }

        [Column("copy_folder_contents_only", Order = 5)]
        public int ContentsOnly { get; set; }

       
    }
}