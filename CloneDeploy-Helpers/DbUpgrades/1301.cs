namespace CloneDeploy_Common.DbUpgrades
{
    public class _1301 : IDbScript
    {
        public string Get()
        {
            return
@"ALTER TABLE `image_profiles` 
ADD COLUMN `skip_nvram` TINYINT(4) NULL DEFAULT 0 COMMENT '' AFTER `wim_enabled_multicast`;
INSERT INTO `admin_settings` (`admin_setting_name`, `admin_setting_value`) VALUES ('Registration Enabled', 'Yes');
UPDATE `clonedeploy_version` SET `app_version`='131', `database_version`='1301' WHERE `clonedeploy_version_id`='1';";
        }
    }
}
