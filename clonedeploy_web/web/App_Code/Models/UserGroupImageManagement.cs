using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Models
{
    [Table("clonedeploy_usergroup_image_mgmt")]
    public class UserGroupImageManagement
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("clonedeploy_usergroup_image_mgmt_id", Order = 1)]
        public int Id { get; set; }

        [Column("usergroup_id", Order = 2)]
        public int UserGroupId { get; set; }

        [Column("image_id", Order = 3)]
        public int ImageId { get; set; }
    }
}