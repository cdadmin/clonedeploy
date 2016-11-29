using System.Collections.Generic;
using System.Linq;
using CloneDeploy_Entities;

namespace CloneDeploy_DataModel
{
    public class ActiveImagingTaskRepository : GenericRepository<ActiveImagingTaskEntity>
    {
        private CloneDeployDbContext _context;

        public ActiveImagingTaskRepository(CloneDeployDbContext context)
            : base(context)
        {
            _context = context;
        }

 
        public List<ComputerEntity> MulticastComputers(int multicastId)
        {
         
            return
                (from t in _context.ActiveImagingTasks 
                 join c in _context.Computers on t.ComputerId equals c.Id 
                 where t.MulticastId == multicastId orderby t.ComputerId select c)
                    .ToList();

        }

        public List<ActiveImagingTaskEntity> MulticastProgress(int multicastId)
        {

            return
                (from t in _context.ActiveImagingTasks
                    where t.MulticastId == multicastId && t.Status == "3"
                    orderby t.ComputerId
                    select t).Take(1).ToList();

        } 
    }
}