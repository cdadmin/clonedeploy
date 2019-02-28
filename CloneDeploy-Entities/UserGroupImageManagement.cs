using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CloneDeploy_Entities
{
    [Table("clonedeploy_usergroup_image_mgmt")]
    public class UserGroupImageManagementEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("clonedeploy_usergroup_image_mgmt_id")]
        public int Id { get; set; }

        [Column("image_id")]
        public int ImageId { get; set; }

        [Column("usergroup_id")]
        public int UserGroupId { get; set; }
    }
}