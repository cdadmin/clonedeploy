using CloneDeploy_Entities.DTOs;

namespace CloneDeploy_ApiCalls
{
    public class APICall : IAPICall
    {
        private readonly CustomApiCallDTO _cApiDto;

        public APICall(CustomApiCallDTO cApi)
        {
            _cApiDto = cApi;
        }

        public APICall()
        {
        }

        public ComputerImageClassificationAPI ComputerImageClassificationApi
        {
            get { return new ComputerImageClassificationAPI("ComputerImageClassification"); }
        }

        public GroupImageClassificationAPI GroupImageClassificationApi
        {
            get { return new GroupImageClassificationAPI("GroupImageClassification"); }
        }

        public ActiveImagingTaskAPI ActiveImagingTaskApi
        {
            get { return new ActiveImagingTaskAPI("ActiveImagingTask"); }
        }

        public ActiveMulticastSessionAPI ActiveMulticastSessionApi
        {
            get { return new ActiveMulticastSessionAPI("ActiveMulticastSession"); }
        }

        public AlternateServerIpAPI AlternateServerIpApi
        {
            get { return new AlternateServerIpAPI("AlternateServerIp"); }
        }

        public AuthorizationAPI AuthorizationApi
        {
            get { return new AuthorizationAPI("Authorization"); }
        }

        public BootEntryAPI BootEntryApi
        {
            get { return new BootEntryAPI("BootEntry"); }
        }

        public BootTemplateAPI BootTemplateApi
        {
            get { return new BootTemplateAPI("BootTemplate"); }
        }

        public BuildingAPI BuildingApi
        {
            get { return new BuildingAPI("Building"); }
        }

        public CdVersionAPI CdVersionApi
        {
            get { return new CdVersionAPI("CdVersion"); }
        }

        public UserAPI CloneDeployUserApi
        {
            get { return new UserAPI("User"); }
        }

        public ClusterGroupAPI ClusterGroupApi
        {
            get { return new ClusterGroupAPI("ClusterGroup"); }
        }

        public ClusterGroupDistributionPointAPI ClusterGroupDistributionPointApi
        {
            get { return new ClusterGroupDistributionPointAPI("ClusterGroupDistributionPoints"); }
        }

        public ClusterGroupServerAPI ClusterGroupServerApi
        {
            get { return new ClusterGroupServerAPI("ClusterGroupServers"); }
        }

        public ComputerAPI ComputerApi
        {
            get { return new ComputerAPI("Computer"); }
        }

        public ComputerBootMenuAPI ComputerBootMenuApi
        {
            get { return new ComputerBootMenuAPI("ComputerBootMenu"); }
        }

        public ComputerLogAPI ComputerLogApi
        {
            get { return new ComputerLogAPI("ComputerLog"); }
        }

        public ComputerMunkiAPI ComputerMunkiApi
        {
            get { return new ComputerMunkiAPI("ComputerMunki"); }
        }

        public ComputerProxyReservationAPI ComputerProxyReservationApi
        {
            get { return new ComputerProxyReservationAPI("ComputerProxyReservation"); }
        }

        public DistributionPointAPI DistributionPointApi
        {
            get { return new DistributionPointAPI("DistributionPoint"); }
        }

        public FileFolderAPI FileFolderApi
        {
            get { return new FileFolderAPI("FileFolder"); }
        }

        public FilesystemAPI FilesystemApi
        {
            get { return new FilesystemAPI("FileSystem"); }
        }

        public GroupAPI GroupApi
        {
            get { return new GroupAPI("Group"); }
        }

        public GroupBootMenuAPI GroupBootMenuApi
        {
            get { return new GroupBootMenuAPI("GroupBootMenu"); }
        }

        public GroupMembershipAPI GroupMembershipApi
        {
            get { return new GroupMembershipAPI("GroupMembership"); }
        }

        public GroupMunkiAPI GroupMunkiApi
        {
            get { return new GroupMunkiAPI("GroupMunki"); }
        }

        public GroupPropertyAPI GroupPropertyApi
        {
            get { return new GroupPropertyAPI("GroupProperty"); }
        }

        public ImageAPI ImageApi
        {
            get { return new ImageAPI("Image"); }
        }

        public ImageClassificationAPI ImageClassificationApi
        {
            get { return new ImageClassificationAPI("ImageClassification"); }
        }

        public ImageProfileAPI ImageProfileApi
        {
            get { return new ImageProfileAPI("ImageProfile"); }
        }

        public ImageProfileFileFolderAPI ImageProfileFileFolderApi
        {
            get { return new ImageProfileFileFolderAPI("ImageProfileFileFolder"); }
        }

        public ImageProfileScriptAPI ImageProfileScriptApi
        {
            get { return new ImageProfileScriptAPI("ImageProfileScript"); }
        }

        public ImageProfileSysprepTagAPI ImageProfileSysprepTagApi
        {
            get { return new ImageProfileSysprepTagAPI("ImageProfileSysprepTag"); }
        }

        public ImageSchemaAPI ImageSchemaApi
        {
            get { return new ImageSchemaAPI("ImageSchema"); }
        }

        public MunkiManifestCatalogAPI MunkiManifestCatalogApi
        {
            get { return new MunkiManifestCatalogAPI("MunkiManifestCatalog"); }
        }

        public MunkiManifestIncludedManifestAPI MunkiManifestIncludedManifestApi
        {
            get { return new MunkiManifestIncludedManifestAPI("MunkiManifestIncludedManifest"); }
        }

        public MunkiManifestManagedInstallAPI MunkiManifestManagedInstallApi
        {
            get { return new MunkiManifestManagedInstallAPI("MunkiManifestManagedInstall"); }
        }

        public MunkiManifestManagedUnInstallAPI MunkiManifestManagedUnInstallEntityApi
        {
            get { return new MunkiManifestManagedUnInstallAPI("MunkiManifestManagedUninstall"); }
        }

        public MunkiManifestManagedUpdateAPI MunkiManifestManagedUpdateEntityApi
        {
            get { return new MunkiManifestManagedUpdateAPI("MunkiManifestManagedUpdate"); }
        }

        public MunkiManifestOptionInstallAPI MunkiManifestOptionInstallEntity
        {
            get { return new MunkiManifestOptionInstallAPI("MunkiManifestOptionalInstall"); }
        }

        public MunkiManifestTemplateAPI MunkiManifestTemplateApi
        {
            get { return new MunkiManifestTemplateAPI("MunkiManifestTemplate"); }
        }

        public NbiEntryAPI NbiEntryApi
        {
            get { return new NbiEntryAPI("NbiEntry"); }
        }

        public NetBootProfileAPI NetBootProfileApi
        {
            get { return new NetBootProfileAPI("NetBootProfile"); }
        }

        public OnlineKernelAPI OnlineKernelApi
        {
            get { return new OnlineKernelAPI("OnlineKernel"); }
        }

        public PortAPI PortApi
        {
            get { return new PortAPI("Port"); }
        }

        public RoomAPI RoomApi
        {
            get { return new RoomAPI("Room"); }
        }

        public ScriptAPI ScriptApi
        {
            get { return new ScriptAPI("Script"); }
        }

        public SecondaryServerAPI SecondaryServerApi
        {
            get { return new SecondaryServerAPI("SecondaryServer"); }
        }

        public ServiceAccountAPI ServiceAccountApi
        {
            get
            {
                return _cApiDto != null
                    ? new ServiceAccountAPI("ServiceAccount", _cApiDto)
                    : new ServiceAccountAPI("ServiceAccount");
            }
        }

        public SettingAPI SettingApi
        {
            get { return _cApiDto != null ? new SettingAPI("Setting", _cApiDto) : new SettingAPI("Setting"); }
        }

        public SiteAPI SiteApi
        {
            get { return new SiteAPI("Site"); }
        }

        public SysprepTagAPI SysprepTagApi
        {
            get { return new SysprepTagAPI("SysprepTag"); }
        }

        public TokenApi TokenApi
        {
            get { return _cApiDto != null ? new TokenApi(_cApiDto.BaseUrl, "Token") : new TokenApi("Token"); }
        }

        public UserGroupAPI UserGroupApi
        {
            get { return new UserGroupAPI("UserGroup"); }
        }

        public UserGroupGroupManagementAPI UserGroupGroupManagementApi
        {
            get { return new UserGroupGroupManagementAPI("UserGroupGroupManagement"); }
        }

        public UserGroupImageManagementAPI UserGroupImageManagementApi
        {
            get { return new UserGroupImageManagementAPI("UserGroupImageManagement"); }
        }

        public UserGroupManagementAPI UserGroupManagementApi
        {
            get { return new UserGroupManagementAPI("UserGroupManagement"); }
        }

        public UserGroupRightAPI UserGroupRightApi
        {
            get { return new UserGroupRightAPI("UserGroupRight"); }
        }

        public UserImageManagementAPI UserImageManagementApi
        {
            get { return new UserImageManagementAPI("UserImageManagement"); }
        }

        public UserRightAPI UserRightApi
        {
            get { return new UserRightAPI("UserRight"); }
        }

        public WorkflowAPI WorkflowApi
        {
            get { return new WorkflowAPI("Workflow"); }
        }
    }
}