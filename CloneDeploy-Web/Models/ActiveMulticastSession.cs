using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CloneDeploy_Web.Models
{
    [Table("active_multicast_sessions")]
    public class ActiveMulticastSession
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("multicast_session_id", Order = 1)]
        public int Id { get; set; }

        [Column("multicast_name", Order = 2)]
        public string Name { get; set; }

        [Column("multicast_pid", Order = 3)]
        public int Pid { get; set; }

        [Column("multicast_port", Order = 4)]
        public int Port { get; set; }

        [Column("user_id", Order = 5)]
        public int UserId { get; set; }

        [Column("ond_image_profile_id", Order = 6)]
        public int ImageProfileId { get; set; }
        
    }
}