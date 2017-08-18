using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CloneDeploy_Entities
{
    [Table("clonedeploy_usergroup_rights")]
    public class UserGroupRightEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("clonedeploy_usergroup_right_id", Order = 1)]
        public int Id { get; set; }

        [Column("usergroup_right", Order = 3)]
        public string Right { get; set; }

        [Column("usergroup_id", Order = 2)]
        public int UserGroupId { get; set; }
    }
}