using CloneDeploy_Entities;

namespace CloneDeploy_ApiCalls
{
    public interface IAPICall
    {
        IGenericAPI<ComputerEntity> ComputerApi { get; }
        IGenericAPI<MunkiManifestTemplateEntity> MunkiManifestTemplateApi { get; }
        ComputerProxyReservationAPI ComputerProxyReservationApi { get; }
        ComputerMunkiAPI ComputerMunkiApi { get; }
        FilesystemAPI FilesystemApi { get; }
        GroupAPI GroupApi { get; }
        TokenApi TokenApi { get; }
        UserAPI CloneDeployUserApi { get; }
    }
}