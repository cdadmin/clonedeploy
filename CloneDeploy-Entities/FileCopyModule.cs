using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CloneDeploy_Entities
{
    [Table("filecopy_modules")]
    public class FileCopyModule
    {

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("filecopy_module_id", Order = 1)]
        public int Id { get; set; }

        [Column("filecopy_module_guid", Order = 1)]
        public int Guid { get; set; }

        [Column("filecopy_module_name", Order = 2)]
        public string Name { get; set; }

        [Column("filecopy_module_description", Order = 3)]
        public string Description { get; set; }

        [Column("filecopy_module_destination", Order = 3)]
        public string Destination { get; set; }

        [Column("filecopy_module_decompress", Order = 3)]
        public int DecompressAfterCopy { get; set; }

        [Column("filecopy_module_filelist", Order = 3)]
        public string FileList { get; set; }
        
        [Column("filecopy_module_timeout", Order = 3)]
        public int Timeout { get; set; }


    }
}