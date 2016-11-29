using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CloneDeploy_Entities
{
    [Table("clonedeploy_users")]
    public class CloneDeployUserEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("clonedeploy_user_id", Order = 1)]
        public int Id { get; set; }

        [Column("clonedeploy_username", Order = 2)]
        public string Name { get; set; }

        [Column("clonedeploy_user_pwd", Order = 3)]
        public string Password { get; set; }

        [Column("clonedeploy_user_salt", Order = 4)]
        public string Salt { get; set; }

        [Column("clonedeploy_user_role", Order = 5)]
        public string Membership { get; set; }

        [Column("clonedeploy_user_email", Order = 6)]
        public string Email { get; set; }

        [Column("clonedeploy_user_token", Order = 7)]
        public string Token { get; set; }

        [Column("notify_on_lockout", Order = 8)]
        public int NotifyLockout { get; set; }

        [Column("notify_on_error", Order = 9)]
        public int NotifyError { get; set; }

        [Column("notify_on_complete", Order = 10)]
        public int NotifyComplete { get; set; }

        [Column("notify_on_image_approved", Order = 11)]
        public int NotifyImageApproved { get; set; }

        [Column("clonedeploy_user_api_id", Order = 12)]
        public string ApiId { get; set; }

        [Column("clonedeploy_user_api_key", Order = 13)]
        public string ApiKey { get; set; }

        [Column("clonedeploy_user_is_ldap", Order = 14)]
        public int IsLdapUser { get; set; }

        [Column("clonedeploy_usergroup_id", Order = 15)]
        public int UserGroupId { get; set; }

        [NotMapped]
        public virtual CloneDeployUserGroupEntity UserGroup { get; set; }
    }
}