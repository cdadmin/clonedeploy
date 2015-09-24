using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.Infrastructure;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text.RegularExpressions;
using System.Threading;
using BLL;
using DAL;
using Global;
using Pxe;

namespace Models
{
    [Table("active_imaging_tasks", Schema = "public")]
    public class ActiveImagingTask
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("active_task_id", Order = 1)]
        public int Id { get; set; }

        [Column("computer_id", Order = 2)]
        public int ComputerId { get; set; }

        [Column("task_status", Order = 3)]
        public string Status { get; set; }

        [Column("task_queue_position", Order = 4)]
        public int QueuePosition { get; set; }

        [Column("task_elapsed", Order = 5)]
        public string Elapsed { get; set; }

        [Column("task_remaining", Order = 6)]
        public string Remaining { get; set; }

        [Column("task_completed", Order = 7)]
        public string Completed { get; set; }

        [Column("task_rate", Order = 8)]
        public string Rate { get; set; }

        [Column("task_partition", Order = 9)]
        public string Partition { get; set; }

        [Column("task_arguments", Order = 10)]
        public string Arguments { get; set; }

        [Column("task_type", Order = 11)]
        public string Type { get; set; }

        [Column("multicast_id", Order = 12)]
        public int MulticastId { get; set; } 
    }
}