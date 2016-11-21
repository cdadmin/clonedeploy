namespace CloneDeploy_Web.APICalls
{
    public class APICall : IAPICall
    {
        public IGenericAPI<Models.Computer> ComputerApi
        {
            get { return new GenericAPI<Models.Computer>("Computer"); }
        }

        public GroupAPI GroupApi
        {
            get { return new GroupAPI("Group"); }
        }

        public TokenApi TokenApi
        {
            get { return new TokenApi("Token"); }
        }

        public User CloneDeployUserApi
        {
            get { return new User("User"); }
        }
    }
}