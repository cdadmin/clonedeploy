using log4net;

namespace CloneDeploy_DataModel
{
    public class RawSqlRepository 
    {
        private readonly CloneDeployDbContext _context;
        private readonly ILog log = LogManager.GetLogger(typeof(RawSqlRepository));

        public RawSqlRepository()
        {
            _context = new CloneDeployDbContext();
        }

        public int Execute(string sql)
        {
          
                return _context.Database.ExecuteSqlCommand(sql);
           
        }
    }
}