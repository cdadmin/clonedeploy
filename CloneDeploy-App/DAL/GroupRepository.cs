using System.Collections.Generic;
using System.Linq;

namespace CloneDeploy_App.DAL
{

    public class GroupRepository : GenericRepository<Models.Group>
    {
        private CloneDeployDbContext _context;

        public GroupRepository(CloneDeployDbContext context)
            : base(context)
        {
            _context = context;
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