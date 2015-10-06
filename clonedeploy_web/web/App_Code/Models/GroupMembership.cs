using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Models
{
    /// <summary>
    /// Summary description for GroupMembership
    /// </summary>
    [Table("group_membership")]
    public class GroupMembership
    {
        [Column("group_membership_id", Order = 1)]
        public int Id { get; set; }

        [Column("computer_id", Order = 2)]
        public int ComputerId { get; set; }

        [Column("group_id", Order = 3)]
        public int GroupId { get; set; }

    }
}