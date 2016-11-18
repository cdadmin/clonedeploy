using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Models
{
    [Table("clonedeploy_user_group_mgmt")]
    public class UserGroupManagement
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("clonedeploy_user_group_mgmt_id", Order = 1)]
        public int Id { get; set; }

        [Column("user_id", Order = 2)]
        public int UserId { get; set; }

        [Column("group_id", Order = 3)]
        public int GroupId { get; set; }
    }
}