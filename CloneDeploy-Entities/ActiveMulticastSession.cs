using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CloneDeploy_Entities
{
    [Table("active_multicast_sessions")]
    public class ActiveMulticastSessionEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("multicast_session_id", Order = 1)]
        public int Id { get; set; }

        [Column("ond_image_profile_id", Order = 6)]
        public int ImageProfileId { get; set; }

        [Column("multicast_name", Order = 2)]
        public string Name { get; set; }

        [Column("multicast_pid", Order = 3)]
        public int Pid { get; set; }

        [Column("multicast_port", Order = 4)]
        public int Port { get; set; }

        [Column("server_id", Order = 7)]
        public int ServerId { get; set; }

        [Column("user_id", Order = 5)]
        public int UserId { get; set; }
    }
}