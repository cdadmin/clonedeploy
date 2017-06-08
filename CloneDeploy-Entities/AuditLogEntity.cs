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
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("audit_log_id", Order = 1)]
        public int Id { get; set; }

        [Column("user_id", Order = 2)]
        public int UserId { get; set; }
        
        [Column("audit_type", Order = 3)]
        public AuditEntry.Type AuditType { get; set; }

        [Column("object_type", Order = 4)]
        public string ObjectType { get; set; }

        [Column("object_id", Order = 5)]
        public int ObjectId { get; set; }

        [Column("object_name", Order = 6)]
        public string ObjectName { get; set; }

        [Column("ip_address", Order = 7)]
        public string Ip { get; set; }

        [Column("date_time", Order = 8)]
        public DateTime DateTime { get; set; }

        [Column("user_name", Order = 9)]
        public string UserName { get; set; }

        [Column("object_json", Order = 10)]
        public string ObjectJson { get; set; }
      
    }
}