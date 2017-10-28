using CloneDeploy_DataModel;

namespace CloneDeploy_Services
{
    public class RawSqlServices
    {
        private readonly RawSqlRepository _rawSqlRepository;

        public RawSqlServices()
        {
            _rawSqlRepository = new RawSqlRepository();
        }

        public int ExecuteQuery(string query)
        {
            return _rawSqlRepository.Execute(query);
        }
    }
}