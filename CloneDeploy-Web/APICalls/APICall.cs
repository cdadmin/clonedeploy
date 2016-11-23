namespace CloneDeploy_Web.APICalls
{
    public class APICall : IAPICall
    {
        public IGenericAPI<Models.Computer> ComputerApi
        {
            get { return new GenericAPI<Models.Computer>("Computer"); }
        }

        public IGenericAPI<Models.MunkiManifestTemplate> MunkiManifestTemplateApi
        {
            get { return new GenericAPI<Models.MunkiManifestTemplate>("MunkiManifestTemplate"); }
        }

        public ComputerProxyReservationAPI ComputerProxyReservationApi
        {
            get { return new ComputerProxyReservationAPI("ComputerProxyReservation"); }
        }

        public ComputerMunkiAPI ComputerMunkiApi
        {
            get { return new ComputerMunkiAPI("ComputerMunki"); }
        }

        public FilesystemAPI FilesystemApi
        {
            get { return new FilesystemAPI("FileSystem");}
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