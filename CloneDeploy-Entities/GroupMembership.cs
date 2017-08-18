using System.ComponentModel.DataAnnotations.Schema;

namespace CloneDeploy_Entities
{
    /// <summary>
    ///     Summary description for GroupMembership
    /// </summary>
    [Table("group_membership")]
    public class GroupMembershipEntity
    {
        [Column("computer_id", Order = 2)]
        public int ComputerId { get; set; }

        [Column("group_id", Order = 3)]
        public int GroupId { get; set; }

        [Column("group_membership_id", Order = 1)]
        public int Id { get; set; }
    }
}