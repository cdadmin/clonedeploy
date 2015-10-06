using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Linq;
using Helpers;

namespace DAL
{

    public class GroupMembership
    {
        private readonly CloneDeployDbContext _context = new CloneDeployDbContext();

        public bool Exists(Models.GroupMembership groupMembership)
        {
            return _context.GroupMemberships.Any(g => g.ComputerId == groupMembership.ComputerId && g.GroupId == groupMembership.GroupId);
        }

        public bool Create(Models.GroupMembership groupMembership)
        {
            try
            {
                _context.GroupMemberships.Add(groupMembership);
                _context.SaveChanges();
                return true;
            }
            catch (DbUpdateException ex)
            {
                Logger.Log(ex.InnerException.InnerException.Message);
                return false;
            }

        }

        public bool Delete(Models.GroupMembership groupMembership)
        {
                try
                {
                    _context.GroupMemberships.RemoveRange(_context.GroupMemberships.Where(g => g.ComputerId == groupMembership.ComputerId && g.GroupId == groupMembership.GroupId));
                    _context.SaveChanges();
                    return true;
                }

                catch (DbUpdateException ex)
                {
                    Logger.Log(ex.InnerException.InnerException.Message);
                    return false;
                }
            
        }

        public string GetTotalCount(int groupId)
        {
            return _context.GroupMemberships.Count(g => g.GroupId == groupId).ToString();            
        }

     
    }
}