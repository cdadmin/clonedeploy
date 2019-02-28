using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CloneDeploy_Entities
{
    [Table("clonedeploy_user_group_mgmt")]
    public class UserGroupManagementEntity
    {
        [Column("group_id")]
        public int GroupId { get; set; }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("clonedeploy_user_group_mgmt_id")]
        public int Id { get; set; }

        [Column("user_id")]
        public int UserId { get; set; }
    }
}