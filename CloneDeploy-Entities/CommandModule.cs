using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CloneDeploy_Entities
{
    [Table("command_modules")]
    public class CommandModule
    {

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("command_module_id", Order = 1)]
        public int Id { get; set; }

        [Column("filecopy_module_guid", Order = 1)]
        public int Guid { get; set; }

        [Column("command_module_name", Order = 2)]
        public string Name { get; set; }

        [Column("command_module_description", Order = 3)]
        public string Description { get; set; }

        [Column("command_module_command", Order = 3)]
        public string Command { get; set; }

        [Column("command_module_returnresult", Order = 3)]
        public int ReturnResult { get; set; }

        [Column("command_module_timeout", Order = 3)]
        public int Timeout { get; set; }


    }
}