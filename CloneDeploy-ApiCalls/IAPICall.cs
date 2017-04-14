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
        ClusterGroupAPI ClusterGroupApi { get; }
        ClusterGroupServerAPI ClusterGroupServerApi { get; }
        ClusterGroupDistributionPointAPI ClusterGroupDistributionPointApi { get; }
        ServiceAccountAPI ServiceAccountApi { get; }
        SecondaryServerAPI SecondaryServerApi { get; }

        BootEntryAPI BootEntryApi { get; }
        BootTemplateAPI BootTemplateApi { get; }
        BuildingAPI BuildingApi { get; }
        ComputerBootMenuAPI ComputerBootMenuApi { get; }
        ComputerProxyReservationAPI ComputerProxyReservationApi { get; }
        FileFolderAPI FileFolderApi { get; }
        GroupBootMenuAPI GroupBootMenuApi { get; }
        GroupPropertyAPI GroupPropertyApi { get; }
        ImageProfileFileFolderAPI ImageProfileFileFolderApi { get; }
        ImageProfileScriptAPI ImageProfileScriptApi { get; }
        ImageProfileSysprepTagAPI ImageProfileSysprepTagApi { get; }
        MunkiManifestCatalogAPI MunkiManifestCatalogApi { get; }
        MunkiManifestIncludedManifestAPI MunkiManifestIncludedManifestApi { get; }
        MunkiManifestManagedInstallAPI MunkiManifestManagedInstallApi { get; }
        MunkiManifestManagedUnInstallAPI MunkiManifestManagedUnInstallEntityApi { get; }
        MunkiManifestManagedUpdateAPI MunkiManifestManagedUpdateEntityApi { get; }
        MunkiManifestOptionInstallAPI MunkiManifestOptionInstallEntity { get; }
        PortAPI PortApi { get; }
        RoomAPI RoomApi { get; }
        ScriptAPI ScriptApi { get; }
        SiteAPI SiteApi { get; }
        SysprepTagAPI SysprepTagApi { get; }
        
        DistributionPointAPI DistributionPointApi { get; }
        

        
 
     
      
       

        
    }
}