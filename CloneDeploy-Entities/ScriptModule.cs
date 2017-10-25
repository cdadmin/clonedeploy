using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CloneDeploy_Entities
{
    [Table("script_modules")]
    public class ScriptModule
    {

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("script_module_id", Order = 1)]
        public int Id { get; set; }

        [Column("script_module_guid", Order = 1)]
        public int Guid { get; set; }

        [Column("script_module_name", Order = 2)]
        public string Name { get; set; }

        [Column("script_module_description", Order = 3)]
        public string Description { get; set; }

        [Column("script_module_type", Order = 3)]
        public string ScriptType { get; set; }

        [Column("script_module_arguments", Order = 3)]
        public string AdditionalArguments { get; set; }

        [Column("sript_module_contents", Order = 3)]
        public string ScriptContents { get; set; }
        
        [Column("software_module_timeout", Order = 3)]
        public int Timeout { get; set; }


    }
}