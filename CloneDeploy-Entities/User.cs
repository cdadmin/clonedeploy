using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CloneDeploy_Entities
{
    [Table("clonedeploy_users")]
    public class CloneDeployUserEntity
    {
        [Column("clonedeploy_user_api_id")]
        public string ApiId { get; set; }

        [Column("clonedeploy_user_api_key")]
        public string ApiKey { get; set; }

        [Column("clonedeploy_user_email")]
        public string Email { get; set; }

        [Column("group_management_enabled")]
        public int GroupManagementEnabled { get; set; }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("clonedeploy_user_id")]
        public int Id { get; set; }

        [Column("image_management_enabled")]
        public int ImageManagementEnabled { get; set; }

        [Column("clonedeploy_user_is_ldap")]
        public int IsLdapUser { get; set; }

        [Column("clonedeploy_user_role")]
        public string Membership { get; set; }

        [Column("clonedeploy_username")]
        public string Name { get; set; }

        [Column("notify_on_complete")]
        public int NotifyComplete { get; set; }

        [Column("notify_on_error")]
        public int NotifyError { get; set; }

        [Column("notify_on_image_approved")]
        public int NotifyImageApproved { get; set; }

        [Column("notify_on_lockout")]
        public int NotifyLockout { get; set; }

        [Column("notify_on_server_status_change")]
        public int NotifyServerStatusChange { get; set; }

        [Column("clonedeploy_user_pwd")]
        public string Password { get; set; }

        [Column("clonedeploy_user_salt")]
        public string Salt { get; set; }

        [Column("clonedeploy_user_token")]
        public string Token { get; set; }

        [Column("clonedeploy_usergroup_id")]
        public int UserGroupId { get; set; }
    }

    [NotMapped]
    public class UserWithUserGroup : CloneDeployUserEntity
    {
        public CloneDeployUserGroupEntity UserGroup { get; set; }
    }
}