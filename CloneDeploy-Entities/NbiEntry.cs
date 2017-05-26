using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CloneDeploy_Entities
{
    [Table("nbi_entries")]
    public class NbiEntryEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("nbi_entry_id", Order = 1)]
        public int Id { get; set; }

        [Column("netboot_profile_id", Order = 2)]
        public int ProfileId { get; set; }

        [Column("nbi_id", Order = 3)]
        public int NbiId { get; set; }

        [Column("nbi_name", Order = 4)]
        public string NbiName { get; set; }

    }


}