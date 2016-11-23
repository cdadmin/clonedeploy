namespace CloneDeploy_Web.APICalls
{
    public interface IAPICall
    {
        IGenericAPI<Models.Computer> ComputerApi { get; }
        IGenericAPI<Models.MunkiManifestTemplate> MunkiManifestTemplateApi { get; }
        ComputerProxyReservationAPI ComputerProxyReservationApi { get; }
        ComputerMunkiAPI ComputerMunkiApi { get; }
        FilesystemAPI FilesystemApi { get; }
        GroupAPI GroupApi { get; }
        TokenApi TokenApi { get; }
        User CloneDeployUserApi { get; }
    }
}