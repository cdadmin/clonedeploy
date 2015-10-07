using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Linq;
using Helpers;

namespace DAL
{
    public class ImageProfileScriptRepository : GenericRepository<Models.ImageProfileScript>
    {
        private readonly CloneDeployDbContext _context;

        public ImageProfileScriptRepository(CloneDeployDbContext context)
            : base(context)
        {
            _context = context;
        }

        public List<Models.ImageProfileScript> Find(int profileId)
        {
            return (from p in _context.ImageProfileScripts join s in _context.Scripts on p.ScriptId equals s.Id where p.ProfileId == profileId orderby s.Priority ascending, s.Name ascending select p).ToList();
        }
    }
}