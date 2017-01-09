using System;
using System.Diagnostics.Eventing.Reader;
using CloneDeploy_Entities;
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

        public ActiveImagingTaskAPI ActiveImagingTaskApi
        {
            get { return new ActiveImagingTaskAPI("ActiveImagingTask"); }
        }

        public ActiveMulticastSessionAPI ActiveMulticastSessionApi
        {
            get { return new ActiveMulticastSessionAPI("ActiveMulticastSession"); }
        }

        public ComputerAPI ComputerApi
        {
            get { return new ComputerAPI("Computer"); }
        }

        public ComputerLogAPI ComputerLogApi
        {
            get { return new ComputerLogAPI("ComputerLog"); }
        }

        public ComputerMunkiAPI ComputerMunkiApi
        {
            get { return new ComputerMunkiAPI("ComputerMunki"); }
        }

        public GroupMunkiAPI GroupMunkiApi
        {
            get { return new GroupMunkiAPI("GroupMunki"); }
        }

      

        public FilesystemAPI FilesystemApi
        {
            get { return new FilesystemAPI("FileSystem"); }
        }

        public GroupAPI GroupApi
        {
            get { return new GroupAPI("Group"); }
        }

        public GroupMembershipAPI GroupMembershipApi
        {
            get { return new GroupMembershipAPI("GroupMembership"); }
        }

        public ImageAPI ImageApi
        {
            get { return new ImageAPI("Image"); }
        }

        public ImageProfileAPI ImageProfileApi
        {
            get { return new ImageProfileAPI("ImageProfile"); }
        }

        public ImageSchemaAPI ImageSchemaApi
        {
            get { return new ImageSchemaAPI("ImageSchema"); }
        }

        public MunkiManifestTemplateAPI MunkiManifestTemplateApi
        {
            get { return new MunkiManifestTemplateAPI("MunkiManifestTemplate"); }
        }

        public SettingAPI SettingApi
        {
            get { return _cApiDto != null ? new SettingAPI("Setting",_cApiDto) : new SettingAPI("Setting"); }
        }

        public TokenApi TokenApi
        {
            get { return _cApiDto != null ? new TokenApi(_cApiDto.BaseUrl,"Token") : new TokenApi("Token"); }
        }

        public UserAPI CloneDeployUserApi
        {
            get { return new UserAPI("User"); }
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

        public CdVersionAPI CdVersionApi
        {
            get { return new CdVersionAPI("CdVersion"); }
        }

        public AuthorizationAPI AuthorizationApi
        {
            get { return new AuthorizationAPI("Authorization"); }
        }

        public ClusterGroupAPI ClusterGroupApi
        {
            get { return new ClusterGroupAPI("ClusterGroup"); }
        }

        public ClusterGroupServerAPI ClusterGroupServerApi
        {
            get {  return new ClusterGroupServerAPI("ClusterGroupServers");}
        }
        


        public IGenericAPI<BootEntryEntity> BootEntryApi
        {
            get { return new GenericAPI<BootEntryEntity>("BootEntry"); }
        }

        public IGenericAPI<BootTemplateEntity> BootTemplateApi
        {
            get { return new GenericAPI<BootTemplateEntity>("BootTemplate"); }
        }

        public IGenericAPI<BuildingEntity> BuildingApi
        {
            get { return new GenericAPI<BuildingEntity>("Building"); }
        }

        public IGenericAPI<ComputerBootMenuEntity> ComputerBootMenuApi
        {
            get { return new GenericAPI<ComputerBootMenuEntity>("ComputerBootMenu"); }
        }

        public IGenericAPI<ComputerProxyReservationEntity> ComputerProxyReservationApi
        {
            get { return new GenericAPI<ComputerProxyReservationEntity>("ComputerProxyReservation"); }
        }

        public IGenericAPI<FileFolderEntity> FileFolderApi
        {
            get { return new GenericAPI<FileFolderEntity>("FileFolder"); }
        }

        public IGenericAPI<GroupBootMenuEntity> GroupBootMenuApi
        {
            get { return new GenericAPI<GroupBootMenuEntity>("GroupBootMenu"); }
        }

        public IGenericAPI<GroupPropertyEntity> GroupPropertyApi
        {
            get { return new GenericAPI<GroupPropertyEntity>("GroupProperty"); }
        }

        public IGenericAPI<ImageProfileFileFolderEntity> ImageProfileFileFolderApi
        {
            get { return new GenericAPI<ImageProfileFileFolderEntity>("ImageProfileFileFolder"); }
        }

        public IGenericAPI<ImageProfileScriptEntity> ImageProfileScriptApi
        {
            get { return new GenericAPI<ImageProfileScriptEntity>("ImageProfileScript"); }
        }

        public IGenericAPI<ImageProfileSysprepTagEntity> ImageProfileSysprepTagApi
        {
            get { return new GenericAPI<ImageProfileSysprepTagEntity>("ImageProfileSysprepTag"); }
        }

        public IGenericAPI<MunkiManifestCatalogEntity> MunkiManifestCatalogApi
        {
            get { return new GenericAPI<MunkiManifestCatalogEntity>("MunkiManifestCatalog"); }
        }

        public IGenericAPI<MunkiManifestIncludedManifestEntity> MunkiManifestIncludedManifestApi
        {
            get { return new GenericAPI<MunkiManifestIncludedManifestEntity>("MunkiManifestIncludedManifest"); }
        }

        public IGenericAPI<MunkiManifestManagedInstallEntity> MunkiManifestManagedInstallApi
        {
            get { return new GenericAPI<MunkiManifestManagedInstallEntity>("MunkiManifestManagedInstall"); }
        }

        public IGenericAPI<MunkiManifestManagedUnInstallEntity> MunkiManifestManagedUnInstallEntityApi
        {
            get { return new GenericAPI<MunkiManifestManagedUnInstallEntity>("MunkiManifestManagedUninstall"); }
        }

        public IGenericAPI<MunkiManifestManagedUpdateEntity> MunkiManifestManagedUpdateEntityApi
        {
            get { return new GenericAPI<MunkiManifestManagedUpdateEntity>("MunkiManifestManagedUpdate"); }
        }

        public IGenericAPI<MunkiManifestOptionInstallEntity> MunkiManifestOptionInstallEntity
        {
            get { return new GenericAPI<MunkiManifestOptionInstallEntity>("MunkiManifestOptionalInstall"); }
        }

        public IGenericAPI<PortEntity> PortApi
        {
            get { return new GenericAPI<PortEntity>("Port"); }
        }

        public IGenericAPI<RoomEntity> RoomApi
        {
            get { return new GenericAPI<RoomEntity>("Room"); }
        }

        public IGenericAPI<ScriptEntity> ScriptApi
        {
            get { return new GenericAPI<ScriptEntity>("Script"); }
        }

        public IGenericAPI<SiteEntity> SiteApi
        {
            get { return new GenericAPI<SiteEntity>("Site"); }
        }

        public IGenericAPI<SysprepTagEntity> SysprepTagApi
        {
            get { return new GenericAPI<SysprepTagEntity>("SysprepTag"); }
        }

        public IGenericAPI<SecondaryServerEntity> SecondaryServerApi
        {
            get { return new GenericAPI<SecondaryServerEntity>("SecondaryServer"); }
        }

      
    }
}