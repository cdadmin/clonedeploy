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
        ComputerProxyReservationAPI ComputerProxyReservationApi { get; }
        DistributionPointAPI DistributionPointApi { get; }
        FileFolderAPI FileFolderApi { get; }
        FilesystemAPI FilesystemApi { get; }
        GroupAPI GroupApi { get; }
        GroupBootMenuAPI GroupBootMenuApi { get; }
        GroupMembershipAPI GroupMembershipApi { get; }  
        GroupPropertyAPI GroupPropertyApi { get; }
        ImageAPI ImageApi { get; }
        ImageClassificationAPI ImageClassificationApi { get; }
        ImageProfileAPI ImageProfileApi { get; }
        ImageProfileFileFolderAPI ImageProfileFileFolderApi { get; }
        ImageProfileScriptAPI ImageProfileScriptApi { get; }
        ImageProfileSysprepTagAPI ImageProfileSysprepTagApi { get; }
        ImageSchemaAPI ImageSchemaApi { get; }
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
        ImageProfileTemplateAPI ImageProfileTemplateApi { get; }
    }
}