using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CloneDeploy_Entities
{
    [Table("clonedeploy_user_rights")]
    public class UserRightEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("clonedeploy_user_right_id")]
        public int Id { get; set; }

        [Column("user_right")]
        public string Right { get; set; }

        [Column("user_id")]
        public int UserId { get; set; }
    }
}