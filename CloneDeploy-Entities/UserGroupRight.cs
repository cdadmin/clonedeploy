using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CloneDeploy_Entities
{
    [Table("clonedeploy_usergroup_rights")]
    public class UserGroupRightEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("clonedeploy_usergroup_right_id")]
        public int Id { get; set; }

        [Column("usergroup_right")]
        public string Right { get; set; }

        [Column("usergroup_id")]
        public int UserGroupId { get; set; }
    }
}