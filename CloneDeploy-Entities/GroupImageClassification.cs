using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CloneDeploy_Entities
{
    [Table("group_image_classifications")]
    public class GroupImageClassificationEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("group_image_classification_id", Order = 1)]
        public int Id { get; set; }

        [Column("group_id", Order = 2)]
        public int GroupId { get; set; }
        
        [Column("image_classification_id", Order = 3)]
        public int ImageClassificationId { get; set; }
    }
}