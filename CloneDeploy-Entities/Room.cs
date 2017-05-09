using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CloneDeploy_Entities
{

    [Table("rooms")]
    public class RoomEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("room_id", Order = 1)]
        public int Id { get; set; }
        [Column("room_name", Order = 2)]
        public string Name { get; set; }

        [Column("room_distribution_point", Order = 3)]
        public int ClusterGroupId { get; set; }

      
    }

    [NotMapped]
    public class RoomWithClusterGroup : RoomEntity
    {
        public ClusterGroupEntity ClusterGroup { get; set; }
    }
}