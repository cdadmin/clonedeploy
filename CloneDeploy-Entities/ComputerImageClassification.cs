using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CloneDeploy_Entities
{
    [Table("computer_image_classifications")]
    public class ComputerImageClassificationEntity
    {
        [Column("computer_id", Order = 2)]
        public int ComputerId { get; set; }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("computer_image_classification_id", Order = 1)]
        public int Id { get; set; }

        [Column("image_classification_id", Order = 3)]
        public int ImageClassificationId { get; set; }
    }
}