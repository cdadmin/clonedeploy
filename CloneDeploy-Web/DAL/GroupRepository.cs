using System.Collections.Generic;
using System.Linq;
using CloneDeploy_Web.Models;

namespace DAL
{

    public class GroupRepository : GenericRepository<Group>
    {
        private CloneDeployDbContext _context;

        public GroupRepository(CloneDeployDbContext context)
            : base(context)
        {
            _context = context;
        }
    

        public List<Computer> GetGroupMembers(int searchGroupId, string searchString)
        {

            return (from h in _context.Computers
                    join g in _context.GroupMemberships on h.Id equals g.ComputerId
                    where (g.GroupId == searchGroupId) && (h.Name.Contains(searchString) || h.Mac.Contains(searchString))
                    select h).ToList();
        }
    
    }
}