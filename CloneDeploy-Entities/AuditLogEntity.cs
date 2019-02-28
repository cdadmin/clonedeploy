using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CloneDeploy_Entities
{
    [Table("audit_logs")]
    public class AuditLogEntity
    {
        public AuditLogEntity()
        {
            DateTime = DateTime.Now;
        }

        [Column("audit_type")]
        public AuditEntry.Type AuditType { get; set; }

        [Column("date_time")]
        public DateTime DateTime { get; set; }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("audit_log_id")]
        public int Id { get; set; }

        [Column("ip_address")]
        public string Ip { get; set; }

        [Column("object_id")]
        public int ObjectId { get; set; }

        [Column("object_json")]
        public string ObjectJson { get; set; }

        [Column("object_name")]
        public string ObjectName { get; set; }

        [Column("object_type")]
        public string ObjectType { get; set; }

        [Column("user_id")]
        public int UserId { get; set; }

        [Column("user_name")]
        public string UserName { get; set; }
    }
}