using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Web;
using System.Web.UI.WebControls.Expressions;
using Global;
using Models;

/// <summary>
/// Summary description for ImageProfilePartitions
/// </summary>
[Table("image_profile_scripts")]
public class ImageProfileScript
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    [Column("image_profile_script_id", Order = 1)]
    public int Id { get; set; }
    [Column("image_profile_id", Order = 2)]
    public int ProfileId { get; set; }
    [Column("script_id", Order = 3)]
    public int ScriptId { get; set; }
    [Column("run_pre", Order = 4)]
    public int RunPre { get; set; }
    [Column("run_post", Order = 5)]
    public int RunPost { get; set; }

    public bool Create()
    {
        using (var db = new DB())
        {
            try
            {         
                db.ImageProfileScript.Add(this);
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
            db.ImageProfileScript.RemoveRange(db.ImageProfileScript.Where(x => x.ProfileId == profileId));
            db.SaveChanges();
        }
    }
   
    public List<ImageProfileScript> Search(int searchId)
    {
        List<ImageProfileScript> list = new List<ImageProfileScript>();
        using (var db = new DB())
        {
            list.AddRange(from p in db.ImageProfileScript join s in db.Scripts on p.ScriptId equals s.Id where p.ProfileId == searchId orderby s.Priority ascending,s.Name ascending select p);
        }

        return list;
    }

   
}