using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CloneDeploy_Web.Models
{
    [Table("computer_logins")]
    public class ComputerLogin
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("computer_login_id", Order = 1)]
        public int Id { get; set; }

        [Column("computer_id", Order = 2)]
        public int ComputerId { get; set; }

        [Column("computer_user_id", Order = 3)]
        public string ComputerUserId { get; set; }

        [Column("login_time_utc", Order = 4)]
        public DateTime LoginTime { get; set; }

        [Column("logout_time_utc", Order = 5)]
        public DateTime LogoutTime { get; set; }
    }
}