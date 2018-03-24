namespace CloneDeploy_Common.DbUpgrades
{
    public class _1304 : IDbScript
    {
        public string Get()
        {
            return
@"UPDATE `clonedeploy_version` SET `app_version`='134', `database_version`='1304' WHERE `clonedeploy_version_id`='1';";
        }
    }
}
