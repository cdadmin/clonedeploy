using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CloneDeploy_Entities
{
    [Table("computer_image_classifications")]
    public class ComputerImageClassificationEntity
    {
        [Column("computer_id")]
        public int ComputerId { get; set; }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("computer_image_classification_id")]
        public int Id { get; set; }

        [Column("image_classification_id")]
        public int ImageClassificationId { get; set; }
    }
}