using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CloneDeploy_App.Models
{
    [Table("computer_users")]
    public class ComputerUser
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("computer_user_id", Order = 1)]
        public int Id { get; set; }

        [Column("username", Order = 2)]
        public string ComputerId { get; set; }
    }
}