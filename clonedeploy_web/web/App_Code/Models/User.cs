using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Models
{
    [Table("clonedeploy_users")]
    public class WdsUser
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

        [NotMapped]
        public string GroupManagement { get; set; }

        [NotMapped]
        public string OndAccess { get; set; }

        [NotMapped]
        public string DebugAccess { get; set; }

        [NotMapped]
        public string DiagAccess { get; set; }
        
        
    }
}