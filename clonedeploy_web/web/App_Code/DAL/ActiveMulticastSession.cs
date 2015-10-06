using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Linq;
using Helpers;

namespace DAL
{
    public class ActiveMulticastSessionRepository : GenericRepository<Models.ActiveMulticastSession>
    {
        private CloneDeployDbContext _context;

         public ActiveMulticastSessionRepository(CloneDeployDbContext context)
            : base(context)
        {
            _context = context;
        }
    }
}