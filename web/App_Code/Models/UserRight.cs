using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Models
{
    [Table("clonedeploy_user_rights")]
    public class UserRight
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("clonedeploy_user_right_id", Order = 1)]
        public int Id { get; set; }

        [Column("user_id", Order = 2)]
        public int UserId { get; set; }

        [Column("user_right", Order = 3)]
        public string Right { get; set; }
    }
}