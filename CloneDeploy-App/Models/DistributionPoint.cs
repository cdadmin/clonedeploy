using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CloneDeploy_App.Models
{

    [Table("distribution_points")]
    public class DistributionPoint
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("distribution_point_id", Order = 1)]
        public int Id { get; set; }
        [Column("distribution_point_display_name", Order = 2)]
        public string DisplayName { get; set; }
        [Column("distribution_point_server", Order = 3)]
        public string Server { get; set; }
        [Column("distribution_point_protocol", Order = 4)]
        public string Protocol { get; set; }
        [Column("distribution_point_share_name", Order = 5)]
        public string ShareName { get; set; }
        [Column("distribution_point_domain", Order = 6)]
        public string Domain { get; set; }
        [Column("distribution_point_rw_username", Order = 7)]
        public string RwUsername { get; set; }
        [Column("distribution_point_rw_password_encrypted", Order = 8)]
        public string RwPassword { get; set; }
        [Column("distribution_point_is_Primary", Order = 9)]
        public int IsPrimary { get; set; }
        [Column("distribution_point_physical_path", Order = 10)]
        public string PhysicalPath { get; set; }
        [Column("distribution_point_is_backend", Order = 11)]
        public int IsBackend { get; set; }
        [Column("distribution_point_backend_server", Order = 12)]
        public string BackendServer { get; set; }
        [Column("distribution_point_ro_username", Order = 13)]
        public string RoUsername { get; set; }
        [Column("distribution_point_ro_password_encrypted", Order = 14)]
        public string RoPassword { get; set; }



    }
}