using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CloneDeploy_Entities
{
    [Table("clonedeploy_user_groups")]
    public class CloneDeployUserGroupEntity
    {
        [Column("clonedeploy_user_group_ldapname", Order = 5)]
        public string GroupLdapName { get; set; }

        [Column("group_management_enabled", Order = 7)]
        public int GroupManagementEnabled { get; set; }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("clonedeploy_user_group_id", Order = 1)]
        public int Id { get; set; }

        [Column("image_management_enabled", Order = 6)]
        public int ImageManagementEnabled { get; set; }

        [Column("clonedeploy_user_group_ldap", Order = 4)]
        public int IsLdapGroup { get; set; }

        [Column("clonedeploy_user_group_role", Order = 3)]
        public string Membership { get; set; }

        [Column("clonedeploy_user_group_name", Order = 2)]
        public string Name { get; set; }
    }
}