namespace CloneDeploy_Common.DbUpgrades
{
    public class _1303 : IDbScript
    {
        public string Get()
        {
            return
@"ALTER TABLE `image_profiles` 
ADD COLUMN `simple_upload_schema` TINYINT(4) NULL DEFAULT 0 COMMENT '' AFTER `force_standard_legacy`,
ADD COLUMN `erase_partitions` TINYINT(4) NULL DEFAULT 0 COMMENT '' AFTER `simple_upload_schema`;
ALTER TABLE `active_imaging_tasks` 
ADD COLUMN `last_update_time` DATETIME NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP COMMENT '' AFTER `distribution_point_id`;
INSERT INTO `admin_settings` (`admin_setting_name`, `admin_setting_value`) VALUES ('Task Timeout', '15');
UPDATE `clonedeploy_version` SET `app_version`='133', `database_version`='1303' WHERE `clonedeploy_version_id`='1';";
        }
    }
}
