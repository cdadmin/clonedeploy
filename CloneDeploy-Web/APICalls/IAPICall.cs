namespace CloneDeploy_Web.APICalls
{
    public interface IAPICall
    {
        IGenericAPI<Models.Computer> ComputerApi { get; }
        GroupAPI GroupApi { get; }
        TokenApi TokenApi { get; }
        User CloneDeployUserApi { get; }
    }
}