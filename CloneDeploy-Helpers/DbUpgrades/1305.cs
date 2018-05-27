namespace CloneDeploy_Common.DbUpgrades
{
    public class _1305 : IDbScript
    {
        public string Get()
        {
            return
@"UPDATE `clonedeploy_version` SET `app_version`='135', `database_version`='1305' WHERE `clonedeploy_version_id`='1';";
        }
    }
}
