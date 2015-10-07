using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Models
{

    [Table("sites")]
    public class Site
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("site_id", Order = 1)]
        public int Id { get; set; }
        [Column("site_name", Order = 2)]
        public string Name { get; set; }
        [ForeignKey("dp")]
        [Column("site_distribution_point", Order = 3)]
        public int DistributionPoint { get; set; }

        //Navigation Properties
        public virtual Models.DistributionPoint dp { get; set; }
    }
}