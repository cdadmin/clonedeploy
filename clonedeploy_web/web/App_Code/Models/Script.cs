using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Web;
using Global;
using Models;

/// <summary>
/// Summary description for Script
/// </summary>
[Table("scripts", Schema = "public")]
public class Script
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    [Column("script_id", Order = 1)]
    public int Id { get; set; }
    [Column("script_name", Order = 2)]
    public string Name { get; set; }
    [Column("script_description", Order = 3)]
    public string Description { get; set; }
    [Column("script_priority", Order = 4)]
    public int Priority { get; set; }
    [Column("script_category_id", Order = 5)]
    public int Category { get; set; }
    [Column("script_contents", Order = 6)]
    public string Contents { get; set; }

    public bool Create()
    {
        using (var db = new DB())
        {
            try
            {
                if (db.Scripts.Any(s => s.Name == Name))
                {
                    Utility.Message = "A Script With This Name Already Exists";
                    return false;
                }
                db.Scripts.Add(this);
                db.SaveChanges();
            }
            catch (DbUpdateException ex)
            {
                Logger.Log(ex.InnerException.InnerException.Message);
                Utility.Message = "Could Not Create Script.";
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
                db.Scripts.Attach(this);
                db.Scripts.Remove(this);
                db.SaveChanges();
            }

            catch (DbUpdateException ex)
            {
                Logger.Log(ex.InnerException.InnerException.Message);
                Utility.Message = "Could Not Delete Script.";
                return;
            }

            Utility.Message = "Successfully Deleted " + Name;        
        }
    }

    public string GetTotalCount()
    {
        using (var db = new DB())
        {
            return db.Scripts.Count().ToString();
        }
    }

    public void Read()
    {
        using (var db = new DB())
        {
            var script = db.Scripts.FirstOrDefault(s => s.Id == Id);
            if (script == null) return;
            Name = script.Name;
            Description = script.Description;
            Priority = script.Priority;
            Contents = script.Contents;
        }
    }

    public List<Script> Search(string searchString)
    {
        List<Script> list = new List<Script>();
        using (var db = new DB())
        {
            list.AddRange(from s in db.Scripts where s.Name.Contains(searchString) orderby s.Name select s);
        }

        return list;
    }

    public bool Update()
    {
        using (var db = new DB())
        {
            try
            {
                var script = db.Scripts.Find(Id);
                if (script != null)
                {
                    script.Name = Name;
                    script.Description = Description;
                    script.Priority = Priority;
                    script.Contents = Contents;
                    db.SaveChanges();
                }
            }
            catch (DbUpdateException ex)
            {
                Logger.Log(ex.InnerException.InnerException.Message);
                Utility.Message = "Could Not Update Script.";
                return false;
            }
        }
        return true;
    }
}