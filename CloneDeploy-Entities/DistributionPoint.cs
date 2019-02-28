using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CloneDeploy_Entities
{
    [Table("distribution_points")]
    public class DistributionPointEntity
    {
        [Column("distribution_point_backend_server")]
        public string BackendServer { get; set; }

        [Column("distribution_point_display_name")]
        public string DisplayName { get; set; }

        [Column("distribution_point_domain")]
        public string Domain { get; set; }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("distribution_point_id")]
        public int Id { get; set; }

        [Column("distribution_point_is_backend")]
        public int IsBackend { get; set; }

        [Column("distribution_point_is_Primary")]
        public int IsPrimary { get; set; }

        [Column("distribution_point_storage_location")]
        public string Location { get; set; }

        [Column("distribution_point_physical_path")]
        public string PhysicalPath { get; set; }

        [Column("distribution_point_protocol")]
        public string Protocol { get; set; }

        [Column("distribution_point_queue_size")]
        public int QueueSize { get; set; }

        [Column("distribution_point_ro_password_encrypted")]
        public string RoPassword { get; set; }

        [Column("distribution_point_ro_username")]
        public string RoUsername { get; set; }

        [Column("distribution_point_rw_password_encrypted")]
        public string RwPassword { get; set; }

        [Column("distribution_point_rw_username")]
        public string RwUsername { get; set; }

        [Column("distribution_point_server")]
        public string Server { get; set; }

        [Column("distribution_point_share_name")]
        public string ShareName { get; set; }
    }
}