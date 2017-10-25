using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CloneDeploy_Entities
{
    [Table("software_modules")]
    public class SoftwareModule
    {

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("software_module_id", Order = 1)]
        public int Id { get; set; }

        [Column("software_module_guid", Order = 1)]
        public int Guid { get; set; }

        [Column("software_module_name", Order = 2)]
        public string Name { get; set; }

        [Column("software_module_description", Order = 3)]
        public string Description { get; set; }

        [Column("software_module_type", Order = 3)]
        public string InstallType { get; set; }

        [Column("software_module_arguments", Order = 3)]
        public string AdditionalArguments { get; set; }

        [Column("software_module_files", Order = 3)]
        public string FileList { get; set; }
        
        [Column("software_module_timeout", Order = 3)]
        public int Timeout { get; set; }


    }
}