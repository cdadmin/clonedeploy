using System.ComponentModel.DataAnnotations.Schema;

namespace CloneDeploy_Entities
{
    [Table("boot_menu_templates")]
    public class BootTemplateEntity
    {
        [Column("boot_menu_template_contents")]
        public string Contents { get; set; }

        [Column("boot_menu_template_description")]
        public string Description { get; set; }

        [Column("boot_menu_template_id")]
        public int Id { get; set; }

        [Column("boot_menu_template_name")]
        public string Name { get; set; }
    }
}