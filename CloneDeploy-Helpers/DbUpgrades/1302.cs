namespace CloneDeploy_Common.DbUpgrades
{
    public class _1302 : IDbScript
    {
        public string Get()
        {
            return
@"ALTER TABLE `image_profiles` 
ADD COLUMN `randomize_guids` TINYINT(4) NULL DEFAULT 0 COMMENT '' AFTER `skip_nvram`,
ADD COLUMN `force_standard_efi` TINYINT(4) NULL DEFAULT 0 COMMENT '' AFTER `randomize_guids`,
ADD COLUMN `force_standard_legacy` TINYINT(4) NULL DEFAULT 0 COMMENT '' AFTER `force_standard_efi`;
UPDATE `clonedeploy_version` SET `app_version`='132', `database_version`='1302' WHERE `clonedeploy_version_id`='1';";
        }
    }
}
