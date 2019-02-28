using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CloneDeploy_Entities
{
    [Table("clonedeploy_user_lockouts")]
    public class UserLockoutEntity
    {
        [Column("bad_login_count")]
        public int BadLoginCount { get; set; }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("clonedeploy_user_lockout_id")]
        public int Id { get; set; }

        [Column("locked_until_time_utc")]
        public DateTime? LockedUntil { get; set; }

        [Column("user_id")]
        public int UserId { get; set; }
    }
}