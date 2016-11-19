using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CloneDeploy_App.Models
{
    [Table("clonedeploy_user_image_mgmt")]
    public class UserImageManagement
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("clonedeploy_user_image_mgmt_id", Order = 1)]
        public int Id { get; set; }

        [Column("user_id", Order = 2)]
        public int UserId { get; set; }

        [Column("image_id", Order = 3)]
        public int ImageId { get; set; }
    }
}