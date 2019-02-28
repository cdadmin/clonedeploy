using System.ComponentModel.DataAnnotations.Schema;

namespace CloneDeploy_Entities
{
    /// <summary>
    ///     Summary description for GroupMembership
    /// </summary>
    [Table("group_membership")]
    public class GroupMembershipEntity
    {
        [Column("computer_id")]
        public int ComputerId { get; set; }

        [Column("group_id")]
        public int GroupId { get; set; }

        [Column("group_membership_id")]
        public int Id { get; set; }
    }
}