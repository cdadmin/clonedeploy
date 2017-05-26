using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CloneDeploy_Entities
{
    [Table("netboot_profiles")]
    public class NetBootProfileEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("netboot_profile_id", Order = 1)]
        public int Id { get; set; }

        [Column("netboot_profile_name", Order = 2)]
        public string Name { get; set; }

        [Column("netboot_profile_ip", Order = 3)]
        public string Ip { get; set; }

    }


}