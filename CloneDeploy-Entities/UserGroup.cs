using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CloneDeploy_Entities
{
    [Table("clonedeploy_user_groups")]
    public class CloneDeployUserGroupEntity
    {
        [Column("clonedeploy_user_group_ldapname")]
        public string GroupLdapName { get; set; }

        [Column("group_management_enabled")]
        public int GroupManagementEnabled { get; set; }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("clonedeploy_user_group_id")]
        public int Id { get; set; }

        [Column("image_management_enabled")]
        public int ImageManagementEnabled { get; set; }

        [Column("clonedeploy_user_group_ldap")]
        public int IsLdapGroup { get; set; }

        [Column("clonedeploy_user_group_role")]
        public string Membership { get; set; }

        [Column("clonedeploy_user_group_name")]
        public string Name { get; set; }
    }
}