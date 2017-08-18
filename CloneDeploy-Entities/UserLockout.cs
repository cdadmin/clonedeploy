using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CloneDeploy_Entities
{
    [Table("clonedeploy_user_lockouts")]
    public class UserLockoutEntity
    {
        [Column("bad_login_count", Order = 3)]
        public int BadLoginCount { get; set; }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("clonedeploy_user_lockout_id", Order = 1)]
        public int Id { get; set; }

        [Column("locked_until_time_utc", Order = 4)]
        public DateTime? LockedUntil { get; set; }

        [Column("user_id", Order = 2)]
        public int UserId { get; set; }
    }
}