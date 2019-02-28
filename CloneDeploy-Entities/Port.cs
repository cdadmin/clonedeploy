using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CloneDeploy_Entities
{
    [Table("multicast_ports", Schema = "public")]
    public class PortEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("multicast_port_id")]
        public int Id { get; set; }

        [Column("multicast_port_number")]
        public int Number { get; set; }
    }
}