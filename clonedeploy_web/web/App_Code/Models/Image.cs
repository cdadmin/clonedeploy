using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Models
{
    [Table("images")]
    public class Image
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("image_id", Order = 1)]
        public int Id { get; set; }

        [Column("image_name", Order = 2)]
        public string Name { get; set; }

        [Column("image_os", Order = 3)]
        public string Os { get; set; }

        [Column("image_description", Order = 4)]
        public string Description { get; set; }

        [Column("image_is_protected", Order = 5)]
        public int Protected { get; set; }

        [Column("image_is_viewable_ond", Order = 6)]
        public int IsVisible { get; set; }

        [Column("image_checksum", Order = 7)]
        public string Checksum { get; set; }

        [Column("image_type", Order = 8)]
        public string Type { get; set; }

        [Column("image_approved", Order = 9)]
        public int Approved { get; set; }

        [NotMapped]
        public string ClientSize { get; set; }

        [NotMapped]
        public string ClientSizeCustom { get; set; }


      
        
    }
}