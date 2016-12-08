using CloneDeploy_Entities;

namespace CloneDeploy_ApiCalls
{
    public interface IAPICall
    {
        ActiveImagingTaskAPI ActiveImagingTaskApi { get; }
        ActiveMulticastSessionAPI ActiveMulticastSessionApi { get; }
        ComputerAPI ComputerApi { get; }
        ComputerLogAPI ComputerLogApi { get; }
        ComputerMunkiAPI ComputerMunkiApi { get; }
        GroupMunkiAPI GroupMunkiApi { get; }
        DistributionPointAPI DistributionPointApi { get; }
        FilesystemAPI FilesystemApi { get; }
        GroupAPI GroupApi { get; }
        GroupMembershipAPI GroupMembershipApi { get; }
        ImageAPI ImageApi { get; }
        ImageProfileAPI ImageProfileApi { get; }
        ImageSchemaAPI ImageSchemaApi { get; }
        MunkiManifestTemplateAPI MunkiManifestTemplateApi { get; }
        SettingAPI SettingApi { get; }
        TokenApi TokenApi { get; }
        UserAPI CloneDeployUserApi { get; }
        UserGroupAPI UserGroupApi { get; }
        UserGroupGroupManagementAPI UserGroupGroupManagementApi { get; }
        UserGroupImageManagementAPI UserGroupImageManagementApi { get; }
        UserGroupManagementAPI UserGroupManagementApi { get; }
        UserGroupRightAPI UserGroupRightApi { get; }
        UserImageManagementAPI UserImageManagementApi { get; }
        UserRightAPI UserRightApi { get; }
        WorkflowAPI WorkflowApi { get; }
        CdVersionAPI CdVersionApi { get; }
        AuthorizationAPI AuthorizationApi { get; }

        IGenericAPI<BootEntryEntity> BootEntryApi { get; }
        IGenericAPI<BootTemplateEntity> BootTemplateApi { get; }
        IGenericAPI<BuildingEntity> BuildingApi { get; }
        IGenericAPI<ComputerBootMenuEntity> ComputerBootMenuApi { get; }
        IGenericAPI<ComputerProxyReservationEntity> ComputerProxyReservationApi { get; }
        IGenericAPI<FileFolderEntity> FileFolderApi { get; }
        IGenericAPI<GroupBootMenuEntity> GroupBootMenuApi { get; }
        IGenericAPI<GroupPropertyEntity> GroupPropertyApi { get; }
        IGenericAPI<ImageProfileFileFolderEntity> ImageProfileFileFolderApi { get; }
        IGenericAPI<ImageProfileScriptEntity> ImageProfileScriptApi { get; }
        IGenericAPI<ImageProfileSysprepTagEntity> ImageProfileSysprepTagApi { get; }
        IGenericAPI<MunkiManifestCatalogEntity> MunkiManifestCatalogApi { get; }
        IGenericAPI<MunkiManifestIncludedManifestEntity> MunkiManifestIncludedManifestApi { get; }
        IGenericAPI<MunkiManifestManagedInstallEntity> MunkiManifestManagedInstallApi { get; }
        IGenericAPI<MunkiManifestManagedUnInstallEntity> MunkiManifestManagedUnInstallEntityApi { get; }
        IGenericAPI<MunkiManifestManagedUpdateEntity> MunkiManifestManagedUpdateEntityApi { get; }
        IGenericAPI<MunkiManifestOptionInstallEntity> MunkiManifestOptionInstallEntity { get; }
        IGenericAPI<PortEntity> PortApi { get; }
        IGenericAPI<RoomEntity> RoomApi { get; }
        IGenericAPI<ScriptEntity> ScriptApi { get; }
        IGenericAPI<SiteEntity> SiteApi { get; }
        IGenericAPI<SysprepTagEntity> SysprepTagApi { get; }


        
 
     
      
       

        
    }
}