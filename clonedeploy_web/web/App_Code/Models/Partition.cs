using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Web;
using Global;


namespace Models
{
    [Table("partitions", Schema = "public")]
    public class Partition
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("partition_id", Order = 1)]
        public int Id { get; set; }
        [Column("partition_layout_id", Order = 2)]
        public int LayoutId { get; set; }
        [Column("partition_number", Order = 3)]
        public int Number { get; set; }
        [Column("partition_type", Order = 4)]
        public string Type { get; set; }
        [Column("partition_fstype", Order = 5)]
        public string FsType { get; set; }
        [Column("partition_size", Order = 6)]
        public int Size { get; set; }
        [Column("partition_size_unit", Order = 7)]
        public string Unit { get; set; }
        [Column("partition_boot_flag", Order = 8)]
        public int Boot { get; set; }

        public bool Create()
        {
            using (var db = new DB())
            {
                try
                {                
                    db.Partition.Add(this);
                    db.SaveChanges();
                }
                catch (DbUpdateException ex)
                {
                    Logger.Log(ex.InnerException.InnerException.Message);
                    Utility.Message = "Could Not Create Partition.";
                    return false;
                }
            }

            Utility.Message = "Successfully Created Partition.";
            return true;
        }

        public void Delete()
        {
            using (var db = new DB())
            {
                try
                {
                    db.Partition.Attach(this);
                    db.Partition.Remove(this);
                    db.SaveChanges();
                }

                catch (DbUpdateException ex)
                {
                    Logger.Log(ex.InnerException.InnerException.Message);
                    Utility.Message = "Could Not Delete Partition.";
                    return;
                }

                Utility.Message = "Successfully Deleted Partition.";
            }
        }

        public string GetTotalCount()
        {
            using (var db = new DB())
            {
                return db.Partition.Count().ToString();
            }
        }

        public void Read()
        {
            using (var db = new DB())
            {
                var partition = db.Partition.FirstOrDefault(s => s.Id == Id);
                if (partition == null) return;
                LayoutId = partition.LayoutId;
                Number = partition.Number;
                Type = partition.Type;
                FsType = partition.FsType;
                Size = partition.Size;
                Unit = partition.Unit;
                Boot = partition.Boot;
            }
        }

        public List<Partition> Search(int layoutId)
        {
            List<Partition> list = new List<Partition>();
            using (var db = new DB())
            {
                list.AddRange(from p in db.Partition where p.LayoutId == layoutId orderby p.Number select p);
            }

            return list;
        }

        public bool Update()
        {
            using (var db = new DB())
            {
                try
                {
                    var partition = db.Partition.Find(Id);
                    if (partition != null)
                    {
                        partition.LayoutId = LayoutId;
                        partition.Number = Number;
                        partition.Type = Type;
                        partition.FsType = FsType;
                        partition.Size = Size;
                        partition.Unit = Unit;
                        partition.Boot = Boot;
                        db.SaveChanges();
                    }
                }
                catch (DbUpdateException ex)
                {
                    Logger.Log(ex.InnerException.InnerException.Message);
                    Utility.Message = "Could Not Update Partition.";
                    return false;
                }
            }
            return true;
        }
    }
}