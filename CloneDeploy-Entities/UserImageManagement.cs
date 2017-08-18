using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CloneDeploy_Entities
{
    [Table("clonedeploy_user_image_mgmt")]
    public class UserImageManagementEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("clonedeploy_user_image_mgmt_id", Order = 1)]
        public int Id { get; set; }

        [Column("image_id", Order = 3)]
        public int ImageId { get; set; }

        [Column("user_id", Order = 2)]
        public int UserId { get; set; }
    }
}