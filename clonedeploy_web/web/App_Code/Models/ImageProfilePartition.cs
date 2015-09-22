using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.Infrastructure;
using System.Linq;
using Global;

namespace Models
{
    /// <summary>
    /// Summary description for ImageProfilePartitions
    /// </summary>
    [Table("image_profile_partition_layouts")]
    public class ImageProfilePartition
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("image_profile_partition_layout_id", Order = 1)]
        public int Id { get; set; }
        [Column("image_profile_id", Order = 2)]
        public int ProfileId { get; set; }
        [Column("partition_layout_id", Order = 3)]
        public int LayoutId { get; set; }

        public bool Create()
        {
            using (var db = new DB())
            {
                try
                {         
                    db.ImageProfilePartition.Add(this);
                    db.SaveChanges();
                }
                catch (DbUpdateException ex)
                {
                    Logger.Log(ex.InnerException.InnerException.Message);
                    return false;
                }
            }
            return true;
        }

        public void DeleteAllForProfile(int profileId)
        {
            using (var db = new DB())
            {
                db.ImageProfilePartition.RemoveRange(db.ImageProfilePartition.Where(x => x.ProfileId == profileId));
                db.SaveChanges();
            }
        }
   
        public List<ImageProfilePartition> Search(int searchId)
        {
            List<ImageProfilePartition> list = new List<ImageProfilePartition>();
            using (var db = new DB())
            {
                list.AddRange(from p in db.ImageProfilePartition where p.ProfileId == searchId orderby p.Id select p);
            }

            return list;
        }

   
    }
}