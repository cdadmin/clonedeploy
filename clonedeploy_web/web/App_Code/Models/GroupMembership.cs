using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Web;
using Global;
using Models;

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

    public bool Create()
    {
        using (var db = new DB())
        {
            try
            {
                if (db.GroupMembership.Any(g => g.ComputerId == ComputerId && g.GroupId == GroupId))
                {               
                    return false;
                }
                db.GroupMembership.Add(this);
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

    public bool Delete()
    {
        using (var db = new DB())
        {
            try
            {
                db.GroupMembership.RemoveRange(db.GroupMembership.Where(g => g.ComputerId == ComputerId && g.GroupId == GroupId));
                db.SaveChanges();
                return true;
            }

            catch (DbUpdateException ex)
            {
                Logger.Log(ex.InnerException.InnerException.Message);
                return false;
            }
        }
    }

    public string GetTotalCount(int groupId)
    {
        using (var db = new DB())
        {
            return db.GroupMembership.Count(g => g.GroupId == groupId).ToString();
        }
    }
    public List<Computer> Search(int searchGroupId, string searchString)
    {
        List<Computer> list = new List<Computer>();
        using (var db = new DB())
        {
         
            list.AddRange(from h in db.Hosts
                join g in db.GroupMembership on h.Id equals g.ComputerId
                          where (g.GroupId == searchGroupId) && (h.Name.Contains(searchString) || h.Mac.Contains(searchString))
                select h);
        }

        return list;
    }
}