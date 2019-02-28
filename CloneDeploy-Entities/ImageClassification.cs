using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CloneDeploy_Entities
{
    [Table("image_classifications")]
    public class ImageClassificationEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("image_classification_id")]
        public int Id { get; set; }

        [Column("image_classification_name")]
        public string Name { get; set; }
    }
}