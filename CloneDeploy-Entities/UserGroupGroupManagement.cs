using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CloneDeploy_Entities
{
    [Table("clonedeploy_usergroup_group_mgmt")]
    public class UserGroupGroupManagementEntity
    {
        [Column("group_id")]
        public int GroupId { get; set; }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("clonedeploy_usergroup_group_mgmt_id")]
        public int Id { get; set; }

        [Column("usergroup_id")]
        public int UserGroupId { get; set; }
    }
}