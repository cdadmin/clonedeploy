using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Linq;
using Helpers;

namespace DAL
{

    public class GroupRepository : GenericRepository<Models.Group>
    {
        private CloneDeployDbContext _context;

        public GroupRepository(CloneDeployDbContext context)
            : base(context)
        {
            _context = context;
        }
    

      

     

        public void Import()
        {
          throw new Exception("Not Implemented");
        }

   
        public List<Models.Computer> SearchSmartHosts(string searchString)
        {
         
            return new List<Models.Computer>();
        }

        public List<Models.Computer> GetGroupMembers(int searchGroupId, string searchString)
        {

            return (from h in _context.Computers
                    join g in _context.GroupMemberships on h.Id equals g.ComputerId
                    where (g.GroupId == searchGroupId) && (h.Name.Contains(searchString) || h.Mac.Contains(searchString))
                    select h).ToList();
        }
    
    }
}