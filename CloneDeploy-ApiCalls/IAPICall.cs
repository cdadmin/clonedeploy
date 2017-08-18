namespace CloneDeploy_ApiCalls
{
    public interface IAPICall
    {
        ActiveImagingTaskAPI ActiveImagingTaskApi { get; }
        ActiveMulticastSessionAPI ActiveMulticastSessionApi { get; }
        AlternateServerIpAPI AlternateServerIpApi { get; }
        AuthorizationAPI AuthorizationApi { get; }
        BootEntryAPI BootEntryApi { get; }
        BootTemplateAPI BootTemplateApi { get; }
        BuildingAPI BuildingApi { get; }
        CdVersionAPI CdVersionApi { get; }
        UserAPI CloneDeployUserApi { get; }
        ClusterGroupAPI ClusterGroupApi { get; }
        ClusterGroupDistributionPointAPI ClusterGroupDistributionPointApi { get; }
        ClusterGroupServerAPI ClusterGroupServerApi { get; }
        ComputerAPI ComputerApi { get; }
        ComputerBootMenuAPI ComputerBootMenuApi { get; }
        ComputerLogAPI ComputerLogApi { get; }
        ComputerMunkiAPI ComputerMunkiApi { get; }
        ComputerProxyReservationAPI ComputerProxyReservationApi { get; }
        DistributionPointAPI DistributionPointApi { get; }
        FileFolderAPI FileFolderApi { get; }
        FilesystemAPI FilesystemApi { get; }
        GroupAPI GroupApi { get; }
        GroupBootMenuAPI GroupBootMenuApi { get; }
        GroupMembershipAPI GroupMembershipApi { get; }
        GroupMunkiAPI GroupMunkiApi { get; }
        GroupPropertyAPI GroupPropertyApi { get; }
        ImageAPI ImageApi { get; }
        ImageClassificationAPI ImageClassificationApi { get; }
        ImageProfileAPI ImageProfileApi { get; }
        ImageProfileFileFolderAPI ImageProfileFileFolderApi { get; }
        ImageProfileScriptAPI ImageProfileScriptApi { get; }
        ImageProfileSysprepTagAPI ImageProfileSysprepTagApi { get; }
        ImageSchemaAPI ImageSchemaApi { get; }
        MunkiManifestCatalogAPI MunkiManifestCatalogApi { get; }
        MunkiManifestIncludedManifestAPI MunkiManifestIncludedManifestApi { get; }
        MunkiManifestManagedInstallAPI MunkiManifestManagedInstallApi { get; }
        MunkiManifestManagedUnInstallAPI MunkiManifestManagedUnInstallEntityApi { get; }
        MunkiManifestManagedUpdateAPI MunkiManifestManagedUpdateEntityApi { get; }
        MunkiManifestOptionInstallAPI MunkiManifestOptionInstallEntity { get; }
        MunkiManifestTemplateAPI MunkiManifestTemplateApi { get; }
        NbiEntryAPI NbiEntryApi { get; }
        NetBootProfileAPI NetBootProfileApi { get; }
        OnlineKernelAPI OnlineKernelApi { get; }
        PortAPI PortApi { get; }
        RoomAPI RoomApi { get; }
        ScriptAPI ScriptApi { get; }
        SecondaryServerAPI SecondaryServerApi { get; }
        ServiceAccountAPI ServiceAccountApi { get; }
        SettingAPI SettingApi { get; }
        SiteAPI SiteApi { get; }
        SysprepTagAPI SysprepTagApi { get; }
        TokenApi TokenApi { get; }
        UserGroupAPI UserGroupApi { get; }
        UserGroupGroupManagementAPI UserGroupGroupManagementApi { get; }
        UserGroupImageManagementAPI UserGroupImageManagementApi { get; }
        UserGroupManagementAPI UserGroupManagementApi { get; }
        UserGroupRightAPI UserGroupRightApi { get; }
        UserImageManagementAPI UserImageManagementApi { get; }
        UserRightAPI UserRightApi { get; }
        WorkflowAPI WorkflowApi { get; }
    }
}