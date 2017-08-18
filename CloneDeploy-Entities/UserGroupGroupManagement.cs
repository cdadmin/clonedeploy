using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CloneDeploy_Entities
{
    [Table("clonedeploy_usergroup_group_mgmt")]
    public class UserGroupGroupManagementEntity
    {
        [Column("group_id", Order = 3)]
        public int GroupId { get; set; }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("clonedeploy_usergroup_group_mgmt_id", Order = 1)]
        public int Id { get; set; }

        [Column("usergroup_id", Order = 2)]
        public int UserGroupId { get; set; }
    }
}