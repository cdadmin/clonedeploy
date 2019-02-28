using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CloneDeploy_Entities
{
    [Table("rooms")]
    public class RoomEntity
    {
        [Column("room_distribution_point")]
        public int ClusterGroupId { get; set; }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("room_id")]
        public int Id { get; set; }

        [Column("room_name")]
        public string Name { get; set; }
    }

    [NotMapped]
    public class RoomWithClusterGroup : RoomEntity
    {
        public ClusterGroupEntity ClusterGroup { get; set; }
    }
}