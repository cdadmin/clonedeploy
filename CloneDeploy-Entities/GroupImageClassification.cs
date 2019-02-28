using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CloneDeploy_Entities
{
    [Table("group_image_classifications")]
    public class GroupImageClassificationEntity
    {
        [Column("group_id")]
        public int GroupId { get; set; }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("group_image_classification_id")]
        public int Id { get; set; }

        [Column("image_classification_id")]
        public int ImageClassificationId { get; set; }
    }
}