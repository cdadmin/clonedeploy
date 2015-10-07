using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Models
{

    [Table("rooms")]
    public class Room
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("room_id", Order = 1)]
        public int Id { get; set; }
        [Column("room_name", Order = 2)]
        public string Name { get; set; }
        [ForeignKey("dp")]
        [Column("room_distribution_point", Order = 3)]
        public int DistributionPoint { get; set; }

        //Navigation Properties
        public virtual Models.DistributionPoint dp { get; set; }
    }
}