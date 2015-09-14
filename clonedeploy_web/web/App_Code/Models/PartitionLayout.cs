using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.Infrastructure;
using System.Linq;
using Global;

namespace Models
{
    /// <summary>
    /// Summary description for PartitionLayout
    /// </summary>
    [Table("partition_layouts", Schema = "public")]
    public class PartitionLayout
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("partition_layout_id", Order = 1)]
        public int Id { get; set; }
        [Column("partition_layout_name", Order = 2)]
        public string Name { get; set; }
        [Column("partition_layout_table", Order = 3)]
        public string Table { get; set; }
        [Column("imaging_environment", Order = 4)]
        public string ImageEnvironment { get; set; }
        [Column("partition_layout_priority", Order = 5)]
        public int Priority { get; set; }

        public bool Create()
        {
            using (var db = new DB())
            {
                try
                {
                    if (db.PartitionLayout.Any(s => s.Name == Name))
                    {
                        Utility.Message = "A Partition Layout With This Name Already Exists";
                        return false;
                    }
                    db.PartitionLayout.Add(this);
                    db.SaveChanges();
                }
                catch (DbUpdateException ex)
                {
                    Logger.Log(ex.InnerException.InnerException.Message);
                    Utility.Message = "Could Not Create Partition Layout.";
                    return false;
                }
            }

            Utility.Message = "Successfully Created " + Name;
            return true;
        }

        public void Delete()
        {
            using (var db = new DB())
            {
                try
                {
                    db.PartitionLayout.Attach(this);
                    db.PartitionLayout.Remove(this);
                    db.SaveChanges();
                }

                catch (DbUpdateException ex)
                {
                    Logger.Log(ex.InnerException.InnerException.Message);
                    Utility.Message = "Could Not Delete Partition Layout.";
                    return;
                }

                Utility.Message = "Successfully Deleted " + Name;
            }
        }

        public string GetTotalCount()
        {
            using (var db = new DB())
            {
                return db.PartitionLayout.Count().ToString();
            }
        }

        public void Read()
        {
            using (var db = new DB())
            {
                var layout = db.PartitionLayout.FirstOrDefault(s => s.Id == Id);
                if (layout == null) return;
                Name = layout.Name;
                Table = layout.Table;
                ImageEnvironment = layout.ImageEnvironment;
                Priority = layout.Priority;
            }
        }

        public List<PartitionLayout> Search(string searchString)
        {
            List<PartitionLayout> list = new List<PartitionLayout>();
            using (var db = new DB())
            {
                list.AddRange(from p in db.PartitionLayout where p.Name.Contains(searchString) orderby p.Name select p);
            }

            return list;
        }

        public bool Update()
        {
            using (var db = new DB())
            {
                try
                {
                    var layout = db.PartitionLayout.Find(Id);
                    if (layout != null)
                    {
                        layout.Name = Name;
                        layout.Table = Table;
                        layout.ImageEnvironment = ImageEnvironment;
                        layout.Priority = Priority;
                        db.SaveChanges();
                    }
                }
                catch (DbUpdateException ex)
                {
                    Logger.Log(ex.InnerException.InnerException.Message);
                    Utility.Message = "Could Not Update Partition Layout.";
                    return false;
                }
            }
            return true;
        }

    }
}