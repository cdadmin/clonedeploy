using CloneDeploy_Entities;

namespace CloneDeploy_ApiCalls
{
    public class APICall : IAPICall
    {
        public IGenericAPI<ComputerEntity> ComputerApi
        {
            get { return new GenericAPI<ComputerEntity>("Computer"); }
        }

        public IGenericAPI<MunkiManifestTemplateEntity> MunkiManifestTemplateApi
        {
            get { return new GenericAPI<MunkiManifestTemplateEntity>("MunkiManifestTemplate"); }
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

        public UserAPI CloneDeployUserApi
        {
            get { return new UserAPI("User"); }
        }
    }
}