using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CloneDeploy_Web.Models
{
    [Table("computer_applications")]
    public class ComputerApplication
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("computer_application_id", Order = 1)]
        public int Id { get; set; }

        [Column("computer_id", Order = 2)]
        public int ComputerId { get; set; }

        [Column("application_id", Order = 3)]
        public int ApplicationId { get; set; }
    }
}