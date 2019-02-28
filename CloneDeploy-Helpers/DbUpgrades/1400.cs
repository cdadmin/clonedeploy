namespace CloneDeploy_Common.DbUpgrades
{
    public class _1400 : IDbScript
    {
        public string Get()
        {
            return
@"delete FROM `images`
WHERE `images`.`image_environment` = 'macOS';

delete FROM `admin_settings`
where `admin_settings`.`admin_setting_name` like 'Munki%';

delete FROM `admin_settings`
where `admin_settings`.`admin_setting_name` = 'Force SSL';

delete `clonedeploy_user_group_mgmt`.*
FROM `clonedeploy_user_group_mgmt` LEFT JOIN `clonedeploy_users`
ON `clonedeploy_user_group_mgmt`.`user_id` = `clonedeploy_users`.`clonedeploy_user_id`
WHERE `clonedeploy_users`.`clonedeploy_user_id` IS NULL;
delete `clonedeploy_user_group_mgmt`.*
FROM `clonedeploy_user_group_mgmt` LEFT JOIN `groups`
ON `clonedeploy_user_group_mgmt`.`group_id` = `groups`.`group_id`
WHERE `groups`.`group_id` IS NULL;

delete `clonedeploy_user_image_mgmt`.*
FROM `clonedeploy_user_image_mgmt` LEFT JOIN `clonedeploy_users`
ON `clonedeploy_user_image_mgmt`.`user_id` = `clonedeploy_users`.`clonedeploy_user_id`
WHERE `clonedeploy_users`.`clonedeploy_user_id` IS NULL;
delete `clonedeploy_user_image_mgmt`.*
FROM `clonedeploy_user_image_mgmt` LEFT JOIN `images`
ON `clonedeploy_user_image_mgmt`.`image_id` = `images`.`image_id`
WHERE `images`.`image_id` IS NULL;

delete `clonedeploy_user_lockouts`.*
FROM `clonedeploy_user_lockouts` LEFT JOIN `clonedeploy_users`
ON `clonedeploy_user_lockouts`.`user_id` = `clonedeploy_users`.`clonedeploy_user_id`
WHERE `clonedeploy_users`.`clonedeploy_user_id` IS NULL;

delete `clonedeploy_user_rights`.*
FROM `clonedeploy_user_rights` LEFT JOIN `clonedeploy_users`
ON `clonedeploy_user_rights`.`user_id` = `clonedeploy_users`.`clonedeploy_user_id`
WHERE `clonedeploy_users`.`clonedeploy_user_id` IS NULL;

delete `clonedeploy_usergroup_group_mgmt`.*
FROM `clonedeploy_usergroup_group_mgmt` LEFT JOIN `clonedeploy_user_groups`
ON `clonedeploy_usergroup_group_mgmt`.`usergroup_id` = `clonedeploy_user_groups`.`clonedeploy_user_group_id`
WHERE `clonedeploy_user_groups`.`clonedeploy_user_group_id` IS NULL;
delete `clonedeploy_usergroup_group_mgmt`.*
FROM `clonedeploy_usergroup_group_mgmt` LEFT JOIN `groups`
ON `clonedeploy_usergroup_group_mgmt`.`group_id` = `groups`.`group_id`
WHERE `groups`.`group_id` IS NULL;

delete `clonedeploy_usergroup_image_mgmt`.*
FROM `clonedeploy_usergroup_image_mgmt` LEFT JOIN `clonedeploy_user_groups`
ON `clonedeploy_usergroup_image_mgmt`.`usergroup_id` = `clonedeploy_user_groups`.`clonedeploy_user_group_id`
WHERE `clonedeploy_user_groups`.`clonedeploy_user_group_id` IS NULL;
delete `clonedeploy_usergroup_image_mgmt`.*
FROM `clonedeploy_usergroup_image_mgmt` LEFT JOIN `images`
ON `clonedeploy_usergroup_image_mgmt`.`image_id` = `images`.`image_id`
WHERE `images`.`image_id` IS NULL;

delete `cluster_group_distribution_points`.*
FROM `cluster_group_distribution_points` LEFT JOIN `cluster_groups`
ON `cluster_group_distribution_points`.`cluster_group_id` = `cluster_groups`.`cluster_group_id`
WHERE `cluster_groups`.`cluster_group_id` IS NULL;
delete `cluster_group_distribution_points`.*
FROM `cluster_group_distribution_points` LEFT JOIN `distribution_points`
ON `cluster_group_distribution_points`.`distribution_point_id` = `distribution_points`.`distribution_point_id`
WHERE `distribution_points`.`distribution_point_id` IS NULL;

delete `cluster_group_servers`.*
FROM `cluster_group_servers` LEFT JOIN `cluster_groups`
ON `cluster_group_servers`.`cluster_group_id` = `cluster_groups`.`cluster_group_id`
WHERE `cluster_groups`.`cluster_group_id` IS NULL;
delete `cluster_group_servers`.*
FROM `cluster_group_servers` LEFT JOIN `secondary_servers`
ON `cluster_group_servers`.`secondary_server_id` = `secondary_servers`.`secondary_server_id`
WHERE `secondary_servers`.`secondary_server_id` IS NULL;

delete `computer_boot_menus`.*
FROM `computer_boot_menus` LEFT JOIN `computers`
ON `computer_boot_menus`.`computer_id` = `computers`.`computer_id`
WHERE `computers`.`computer_id` IS NULL;

delete `computer_image_classifications`.*
FROM `computer_image_classifications` LEFT JOIN `computers`
ON `computer_image_classifications`.`computer_id` = `computers`.`computer_id`
WHERE `computers`.`computer_id` IS NULL;
delete `computer_image_classifications`.*
FROM `computer_image_classifications` LEFT JOIN `image_classifications`
ON `computer_image_classifications`.`image_classification_id` = `image_classifications`.`image_classification_id`
WHERE `image_classifications`.`image_classification_id` IS NULL;

delete `computer_proxy_reservations`.*
FROM `computer_proxy_reservations` LEFT JOIN `computers`
ON `computer_proxy_reservations`.`computer_id` = `computers`.`computer_id`
WHERE `computers`.`computer_id` IS NULL;

delete `group_boot_menus`.*
FROM `group_boot_menus` LEFT JOIN `groups`
ON `group_boot_menus`.`group_id` = `groups`.`group_id`
WHERE `groups`.`group_id` IS NULL;

delete `group_computer_properties`.*
FROM `group_computer_properties` LEFT JOIN `groups`
ON `group_computer_properties`.`group_id` = `groups`.`group_id`
WHERE `groups`.`group_id` IS NULL;

delete `group_image_classifications`.*
FROM `group_image_classifications` LEFT JOIN `groups`
ON `group_image_classifications`.`group_id` = `groups`.`group_id`
WHERE `groups`.`group_id` IS NULL;
delete `group_image_classifications`.*
FROM `group_image_classifications` LEFT JOIN `image_classifications`
ON `group_image_classifications`.`image_classification_id` = `image_classifications`.`image_classification_id`
WHERE `image_classifications`.`image_classification_id` IS NULL;

delete `group_membership`.*
FROM `group_membership` LEFT JOIN `groups`
ON `group_membership`.`group_id` = `groups`.`group_id`
WHERE `groups`.`group_id` IS NULL;
delete `group_membership`.*
FROM `group_membership` LEFT JOIN `computers`
ON `group_membership`.`computer_id` = `computers`.`computer_id`
WHERE `computers`.`computer_id` IS NULL;

delete `image_profile_files_folders`.*
FROM `image_profile_files_folders` LEFT JOIN `files_folders`
ON `image_profile_files_folders`.`file_folder_id` = `files_folders`.`file_folder_id`
WHERE `files_folders`.`file_folder_id` IS NULL;
delete `image_profile_files_folders`.*
FROM `image_profile_files_folders` LEFT JOIN `image_profiles`
ON `image_profile_files_folders`.`image_profile_id` = `image_profiles`.`image_profile_id`
WHERE `image_profiles`.`image_profile_id` IS NULL;


delete `image_profile_scripts`.*
FROM `image_profile_scripts` LEFT JOIN `scripts`
ON `image_profile_scripts`.`script_id` = `scripts`.`script_id`
WHERE `scripts`.`script_id` IS NULL;
delete `image_profile_scripts`.*
FROM `image_profile_scripts` LEFT JOIN `image_profiles`
ON `image_profile_scripts`.`image_profile_id` = `image_profiles`.`image_profile_id`
WHERE `image_profiles`.`image_profile_id` IS NULL;


delete `image_profile_sysprep_tags`.*
FROM `image_profile_sysprep_tags` LEFT JOIN `sysprep_tags`
ON `image_profile_sysprep_tags`.`sysprep_tag_id` = `sysprep_tags`.`sysprep_tag_id`
WHERE `sysprep_tags`.`sysprep_tag_id` IS NULL;
delete `image_profile_sysprep_tags`.*
FROM `image_profile_sysprep_tags` LEFT JOIN `image_profiles`
ON `image_profile_sysprep_tags`.`image_profile_id` = `image_profiles`.`image_profile_id`
WHERE `image_profiles`.`image_profile_id` IS NULL;

delete `image_profiles`.*
FROM `image_profiles` LEFT JOIN `images`
ON `image_profiles`.`image_id` = `images`.`image_id`
WHERE `images`.`image_id` IS NULL;



ALTER TABLE `image_profiles` 
DROP COLUMN `erase_partitions`,
DROP COLUMN `munki_auth_password`,
DROP COLUMN `munki_auth_username`,
DROP COLUMN `munki_repo_url`,
DROP COLUMN `osx_install_munki`,
DROP COLUMN `osx_target_volume`;

ALTER TABLE `images` 
DROP COLUMN `image_osx_thin_recovery`,
DROP COLUMN `image_osx_thin_os`,
DROP COLUMN `image_osx_type`;

DROP TABLE `applications`;
DROP TABLE `client_certificates`;
DROP TABLE `computer_alt_mac_addresses`;
DROP TABLE `computer_applications`;
DROP TABLE `computer_harddrives`;
DROP TABLE `computer_inventory`;
DROP TABLE `computer_logins`;
DROP TABLE `computer_munki_templates`;
DROP TABLE `computer_printers`;
DROP TABLE `computer_users`;
DROP TABLE `group_munki_templates`;
DROP TABLE `history_user`;
DROP TABLE `image_profile_partition_layouts`;
DROP TABLE `munki_manifest_catalogs`, `munki_manifest_included_manifests`, `munki_manifest_managed_installs`, `munki_manifest_managed_uninstalls`, `munki_manifest_managed_updates`, `munki_manifest_optional_installs`, `munki_manifest_templates`, `nbi_entries`, `netboot_profiles`, `partition_layouts`;
DROP TABLE `partitions`;




ALTER TABLE `clonedeploy_user_group_mgmt` 
ADD INDEX `fk_ug_mgmt_user_idx` (`user_id` ASC),
ADD INDEX `fk_ug_mgmt_group_idx` (`group_id` ASC);
ALTER TABLE `clonedeploy_user_group_mgmt` 
ADD CONSTRAINT `fk_ug_mgmt_user`
  FOREIGN KEY (`user_id`)
  REFERENCES `clonedeploy_users` (`clonedeploy_user_id`)
  ON DELETE CASCADE
  ON UPDATE NO ACTION,
ADD CONSTRAINT `fk_ug_mgmt_group`
  FOREIGN KEY (`group_id`)
  REFERENCES `groups` (`group_id`)
  ON DELETE CASCADE
  ON UPDATE NO ACTION;

ALTER TABLE `clonedeploy_user_image_mgmt` 
ADD INDEX `fk_uimgmt_user_idx` (`user_id` ASC),
ADD INDEX `fk_uimgmt_image_idx` (`image_id` ASC);
ALTER TABLE `clonedeploy_user_image_mgmt` 
ADD CONSTRAINT `fk_uimgmt_user`
  FOREIGN KEY (`user_id`)
  REFERENCES `clonedeploy_users` (`clonedeploy_user_id`)
  ON DELETE CASCADE
  ON UPDATE NO ACTION,
ADD CONSTRAINT `fk_uimgmt_image`
  FOREIGN KEY (`image_id`)
  REFERENCES `images` (`image_id`)
  ON DELETE CASCADE
  ON UPDATE NO ACTION;

ALTER TABLE `clonedeploy_user_lockouts` 
ADD INDEX `fk_ul_user_idx` (`user_id` ASC);
ALTER TABLE `clonedeploy_user_lockouts` 
ADD CONSTRAINT `fk_ul_user`
  FOREIGN KEY (`user_id`)
  REFERENCES `clonedeploy_users` (`clonedeploy_user_id`)
  ON DELETE CASCADE
  ON UPDATE NO ACTION;

ALTER TABLE `clonedeploy_user_rights` 
ADD INDEX `fk_ur_user_idx` (`user_id` ASC);
ALTER TABLE `clonedeploy_user_rights` 
ADD CONSTRAINT `fk_ur_user`
  FOREIGN KEY (`user_id`)
  REFERENCES `clonedeploy_users` (`clonedeploy_user_id`)
  ON DELETE CASCADE
  ON UPDATE NO ACTION;

ALTER TABLE `clonedeploy_usergroup_group_mgmt` 
ADD INDEX `fk_ug_gmgmt_idx` (`usergroup_id` ASC),
ADD INDEX `fk_ug_group_idx` (`group_id` ASC);
ALTER TABLE `clonedeploy_usergroup_group_mgmt` 
ADD CONSTRAINT `fk_ug_gmgmt`
  FOREIGN KEY (`usergroup_id`)
  REFERENCES `clonedeploy_user_groups` (`clonedeploy_user_group_id`)
  ON DELETE CASCADE
  ON UPDATE NO ACTION,
ADD CONSTRAINT `fk_ug_group`
  FOREIGN KEY (`group_id`)
  REFERENCES `groups` (`group_id`)
  ON DELETE CASCADE
  ON UPDATE NO ACTION;

ALTER TABLE `clonedeploy_usergroup_image_mgmt` 
ADD INDEX `fk_ug_img_ug_idx` (`usergroup_id` ASC),
ADD INDEX `fk_ug_img_image_idx` (`image_id` ASC);
ALTER TABLE `clonedeploy_usergroup_image_mgmt` 
ADD CONSTRAINT `fk_ug_img_ug`
  FOREIGN KEY (`usergroup_id`)
  REFERENCES `clonedeploy_user_groups` (`clonedeploy_user_group_id`)
  ON DELETE CASCADE
  ON UPDATE NO ACTION,
ADD CONSTRAINT `fk_ug_img_image`
  FOREIGN KEY (`image_id`)
  REFERENCES `images` (`image_id`)
  ON DELETE CASCADE
  ON UPDATE NO ACTION;

ALTER TABLE `clonedeploy_usergroup_rights` 
ADD INDEX `fk_ug_right_user_idx` (`usergroup_id` ASC);
ALTER TABLE `clonedeploy_usergroup_rights` 
ADD CONSTRAINT `fk_ug_right_user`
  FOREIGN KEY (`usergroup_id`)
  REFERENCES `clonedeploy_user_groups` (`clonedeploy_user_group_id`)
  ON DELETE CASCADE
  ON UPDATE NO ACTION;

ALTER TABLE `cluster_group_distribution_points` 
ADD INDEX `fk_cgdp_clust_idx` (`cluster_group_id` ASC),
ADD INDEX `fk_cgdp_dp_idx` (`distribution_point_id` ASC);
ALTER TABLE `cluster_group_distribution_points` 
ADD CONSTRAINT `fk_cgdp_clust`
  FOREIGN KEY (`cluster_group_id`)
  REFERENCES `cluster_groups` (`cluster_group_id`)
  ON DELETE CASCADE
  ON UPDATE NO ACTION,
ADD CONSTRAINT `fk_cgdp_dp`
  FOREIGN KEY (`distribution_point_id`)
  REFERENCES `distribution_points` (`distribution_point_id`)
  ON DELETE CASCADE
  ON UPDATE NO ACTION;

ALTER TABLE `cluster_group_servers` 
ADD INDEX `fk_cgs_serv_idx` (`secondary_server_id` ASC),
ADD INDEX `fk_cgs_cg_idx` (`cluster_group_id` ASC);
ALTER TABLE `cluster_group_servers` 
ADD CONSTRAINT `fk_cgs_serv`
  FOREIGN KEY (`secondary_server_id`)
  REFERENCES `secondary_servers` (`secondary_server_id`)
  ON DELETE CASCADE
  ON UPDATE NO ACTION,
ADD CONSTRAINT `fk_cgs_cg`
  FOREIGN KEY (`cluster_group_id`)
  REFERENCES `cluster_groups` (`cluster_group_id`)
  ON DELETE CASCADE
  ON UPDATE NO ACTION;

ALTER TABLE `computer_boot_menus` 
ADD INDEX `fk_cbm_comp_idx` (`computer_id` ASC);
ALTER TABLE `computer_boot_menus` 
ADD CONSTRAINT `fk_cbm_comp`
  FOREIGN KEY (`computer_id`)
  REFERENCES `computers` (`computer_id`)
  ON DELETE CASCADE
  ON UPDATE NO ACTION;

ALTER TABLE `computer_image_classifications` 
ADD INDEX `fk_cic_comp_idx` (`computer_id` ASC);
ALTER TABLE `computer_image_classifications` 
ADD CONSTRAINT `fk_cic_comp`
  FOREIGN KEY (`computer_id`)
  REFERENCES `computers` (`computer_id`)
  ON DELETE CASCADE
  ON UPDATE NO ACTION;

ALTER TABLE `computer_proxy_reservations` 
ADD INDEX `fk_cpr_comp_idx` (`computer_id` ASC);
ALTER TABLE `computer_proxy_reservations` 
ADD CONSTRAINT `fk_cpr_comp`
  FOREIGN KEY (`computer_id`)
  REFERENCES `computers` (`computer_id`)
  ON DELETE CASCADE
  ON UPDATE NO ACTION;

ALTER TABLE `group_boot_menus` 
ADD INDEX `fk_gpm_group_idx` (`group_id` ASC);
ALTER TABLE `group_boot_menus` 
ADD CONSTRAINT `fk_gpm_group`
  FOREIGN KEY (`group_id`)
  REFERENCES `groups` (`group_id`)
  ON DELETE CASCADE
  ON UPDATE NO ACTION;

ALTER TABLE `group_computer_properties` 
ADD INDEX `fk_gcp_group_idx` (`group_id` ASC);
ALTER TABLE `group_computer_properties` 
ADD CONSTRAINT `fk_gcp_group`
  FOREIGN KEY (`group_id`)
  REFERENCES `groups` (`group_id`)
  ON DELETE CASCADE
  ON UPDATE NO ACTION;

ALTER TABLE `group_image_classifications` 
ADD INDEX `fk_gic_group_idx` (`group_id` ASC),
ADD INDEX `fk_gic_ic_idx` (`image_classification_id` ASC);
ALTER TABLE `group_image_classifications` 
ADD CONSTRAINT `fk_gic_group`
  FOREIGN KEY (`group_id`)
  REFERENCES `groups` (`group_id`)
  ON DELETE CASCADE
  ON UPDATE NO ACTION,
ADD CONSTRAINT `fk_gic_ic`
  FOREIGN KEY (`image_classification_id`)
  REFERENCES `image_classifications` (`image_classification_id`)
  ON DELETE CASCADE
  ON UPDATE NO ACTION;

ALTER TABLE `group_membership` 
ADD INDEX `fk_gm_group_idx` (`group_id` ASC),
ADD INDEX `fk_gm_computer_idx` (`computer_id` ASC);
ALTER TABLE `group_membership` 
ADD CONSTRAINT `fk_gm_group`
  FOREIGN KEY (`group_id`)
  REFERENCES `groups` (`group_id`)
  ON DELETE CASCADE
  ON UPDATE NO ACTION,
ADD CONSTRAINT `fk_gm_computer`
  FOREIGN KEY (`computer_id`)
  REFERENCES `computers` (`computer_id`)
  ON DELETE CASCADE
  ON UPDATE NO ACTION;

ALTER TABLE `image_profile_files_folders` 
ADD INDEX `fk_ip_ip_idx` (`image_profile_id` ASC),
ADD INDEX `fk_ip_ff_idx` (`file_folder_id` ASC);
ALTER TABLE `image_profile_files_folders` 
ADD CONSTRAINT `fk_ip_ip`
  FOREIGN KEY (`image_profile_id`)
  REFERENCES `image_profiles` (`image_profile_id`)
  ON DELETE CASCADE
  ON UPDATE NO ACTION,
ADD CONSTRAINT `fk_ip_ff`
  FOREIGN KEY (`file_folder_id`)
  REFERENCES `files_folders` (`file_folder_id`)
  ON DELETE CASCADE
  ON UPDATE NO ACTION;

ALTER TABLE `image_profile_scripts` 
ADD INDEX `fk_ip_script_ip_idx` (`image_profile_id` ASC),
ADD INDEX `fk_ip_script_script_idx` (`script_id` ASC);
ALTER TABLE `image_profile_scripts` 
ADD CONSTRAINT `fk_ip_script_ip`
  FOREIGN KEY (`image_profile_id`)
  REFERENCES `image_profiles` (`image_profile_id`)
  ON DELETE CASCADE
  ON UPDATE NO ACTION,
ADD CONSTRAINT `fk_ip_script_script`
  FOREIGN KEY (`script_id`)
  REFERENCES `scripts` (`script_id`)
  ON DELETE CASCADE
  ON UPDATE NO ACTION;

ALTER TABLE `image_profile_sysprep_tags` 
ADD INDEX `fk_ip_sp_ip_idx` (`image_profile_id` ASC),
ADD INDEX `fk_ip_sp_sysprep_idx` (`sysprep_tag_id` ASC);
ALTER TABLE `image_profile_sysprep_tags` 
ADD CONSTRAINT `fk_ip_sp_ip`
  FOREIGN KEY (`image_profile_id`)
  REFERENCES `image_profiles` (`image_profile_id`)
  ON DELETE CASCADE
  ON UPDATE NO ACTION,
ADD CONSTRAINT `fk_ip_sp_sysprep`
  FOREIGN KEY (`sysprep_tag_id`)
  REFERENCES `sysprep_tags` (`sysprep_tag_id`)
  ON DELETE CASCADE
  ON UPDATE NO ACTION;

ALTER TABLE `image_profiles` 
ADD INDEX `fk_ip_image_idx` (`image_id` ASC);
ALTER TABLE `image_profiles` 
ADD CONSTRAINT `fk_ip_image`
  FOREIGN KEY (`image_id`)
  REFERENCES `images` (`image_id`)
  ON DELETE CASCADE
  ON UPDATE NO ACTION;

DROP TABLE IF EXISTS `image_profile_templates`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `image_profile_templates` (
  `image_profile_template_id` int(11) NOT NULL AUTO_INCREMENT,
  `profile_kernel` varchar(45) DEFAULT NULL,
  `profile_boot_image` varchar(45) DEFAULT NULL,
  `profile_name` varchar(45) DEFAULT NULL,
  `profile_description` longtext DEFAULT NULL,
  `profile_kernel_arguments` longtext DEFAULT NULL,
  `skip_core_download` tinyint(4) DEFAULT 0,
  `skip_set_clock` tinyint(4) DEFAULT 0,
  `task_completed_action` varchar(45) DEFAULT 'Reboot',
  `remove_gpt_structures` tinyint(4) DEFAULT 0,
  `skip_volume_shrink` tinyint(4) DEFAULT 0,
  `skip_lvm_shrink` tinyint(4) DEFAULT 0,
  `skip_volume_expand` tinyint(4) DEFAULT 0,
  `fix_bcd` tinyint(4) DEFAULT 0,
  `fix_bootloader` tinyint(4) DEFAULT 1,
  `partition_method` varchar(45) DEFAULT 'Dynamic',
  `force_dynamic_partitions` tinyint(4) DEFAULT 0,
  `custom_partition_script` longtext DEFAULT NULL,
  `compression_algorithm` varchar(45) DEFAULT 'lz4',
  `compression_level` varchar(45) DEFAULT '1',
  `custom_image_schema` longtext DEFAULT NULL,
  `custom_upload_schema` longtext DEFAULT NULL,
  `upload_schema_only` tinyint(4) DEFAULT 0,
  `multicast_sender_arguments` varchar(45) DEFAULT NULL,
  `multicast_receiver_arguments` varchar(45) DEFAULT NULL,
  `web_cancel` tinyint(4) DEFAULT 0,
  `change_name` tinyint(4) DEFAULT 1,
  `wim_enabled_multicast` tinyint(4) DEFAULT 0,
  `skip_nvram` tinyint(4) DEFAULT 0,
  `randomize_guids` tinyint(4) DEFAULT 0,
  `force_standard_efi` tinyint(4) DEFAULT 0,
  `force_standard_legacy` tinyint(4) DEFAULT 0,
  `simple_upload_schema` tinyint(4) DEFAULT 0,
  `template_type` int(11) DEFAULT 0,
  `skip_hibernation_check` tinyint(4) DEFAULT 0,
  `skip_bitlocker_check` tinyint(4) DEFAULT 0,
  PRIMARY KEY (`image_profile_template_id`)
) ENGINE=InnoDB AUTO_INCREMENT=4 DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

LOCK TABLES `image_profile_templates` WRITE;
/*!40000 ALTER TABLE `image_profile_templates` DISABLE KEYS */;
INSERT INTO `image_profile_templates` VALUES (1,'4.20.10x64','initrd.xz','Default LIE Block','Generated via the LinuxBlock image profile template.',NULL,0,0,'Reboot',0,1,0,0,0,1,'Dynamic',0,NULL,'gzip','1',NULL,NULL,0,'','',0,1,0,0,0,0,0,0,0,0,0),(2,'4.20.10x64','initrd.xz','Default LIE File','Generated via the LinuxFile image profile template.',NULL,0,0,'Reboot',0,0,0,0,0,1,'Dynamic',0,NULL,'lz4','1',NULL,NULL,0,'','',0,1,0,0,0,0,0,0,1,0,0),(3,'3.18.1','debug.xz','Default WIE','Generated via the WinPE image profile template.',NULL,0,0,'Reboot',0,0,0,0,0,1,'Dynamic',0,NULL,'lz4','1',NULL,NULL,0,'','',0,1,0,0,0,0,0,0,2,0,0);
/*!40000 ALTER TABLE `image_profile_templates` ENABLE KEYS */;
UNLOCK TABLES;
  
ALTER TABLE `image_profiles` 
ADD COLUMN `model_match` VARCHAR(200) NULL COMMENT '' AFTER `simple_upload_schema`,
ADD COLUMN `model_match_type` VARCHAR(45) NULL DEFAULT 'Disabled' COMMENT '' AFTER `model_match`,
ADD COLUMN `skip_hibernation_check` TINYINT(4) NULL DEFAULT 0 AFTER `model_match_type`,
ADD COLUMN `skip_bitlocker_check` TINYINT(4) NULL DEFAULT 0 AFTER `skip_hibernation_check`;

ALTER TABLE `groups` 
ADD COLUMN `smart_type` VARCHAR(45) NULL DEFAULT NULL COMMENT '' AFTER `cluster_group_id`;
UPDATE `groups` SET `smart_type`='like' WHERE `smart_type` IS NULL;

INSERT INTO `admin_settings` (`admin_setting_name`, `admin_setting_value`) VALUES ('Ipxe SSL', '0');
INSERT INTO `admin_settings` (`admin_setting_name`, `admin_setting_value`) VALUES ('On Demand Prompt Computer Name', 'Yes');

UPDATE `clonedeploy_version` SET `app_version`='140', `database_version`='1400' WHERE `clonedeploy_version_id`='1';
";
        }
    }
}
