

/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET @OLD_CHARACTER_SET_RESULTS=@@CHARACTER_SET_RESULTS */;
/*!40101 SET @OLD_COLLATION_CONNECTION=@@COLLATION_CONNECTION */;
/*!40101 SET NAMES utf8 */;
/*!40103 SET @OLD_TIME_ZONE=@@TIME_ZONE */;
/*!40103 SET TIME_ZONE='+00:00' */;
/*!40014 SET @OLD_UNIQUE_CHECKS=@@UNIQUE_CHECKS, UNIQUE_CHECKS=0 */;
/*!40014 SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0 */;
/*!40101 SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='NO_AUTO_VALUE_ON_ZERO' */;
/*!40111 SET @OLD_SQL_NOTES=@@SQL_NOTES, SQL_NOTES=0 */;

ALTER USER 'root'@'localhost' PASSWORD EXPIRE NEVER
--
-- Table structure for table `active_imaging_tasks`
--

DROP TABLE IF EXISTS `active_imaging_tasks`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `active_imaging_tasks` (
  `active_task_id` int(11) NOT NULL AUTO_INCREMENT,
  `computer_id` int(11) NOT NULL,
  `task_status` varchar(45) DEFAULT NULL,
  `task_queue_position` int(11) DEFAULT '0',
  `task_elapsed` varchar(150) DEFAULT NULL,
  `task_remaining` varchar(45) DEFAULT NULL,
  `task_completed` varchar(45) DEFAULT NULL,
  `task_rate` varchar(45) DEFAULT NULL,
  `task_partition` varchar(45) DEFAULT NULL,
  `task_arguments` longtext,
  `task_type` varchar(45) DEFAULT NULL,
  `multicast_id` int(11) DEFAULT '-1',
  `user_id` int(11) DEFAULT '-1',
  PRIMARY KEY (`active_task_id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `active_imaging_tasks`
--

LOCK TABLES `active_imaging_tasks` WRITE;
/*!40000 ALTER TABLE `active_imaging_tasks` DISABLE KEYS */;
/*!40000 ALTER TABLE `active_imaging_tasks` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `active_multicast_sessions`
--

DROP TABLE IF EXISTS `active_multicast_sessions`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `active_multicast_sessions` (
  `multicast_session_id` int(11) NOT NULL AUTO_INCREMENT,
  `multicast_name` varchar(45) DEFAULT NULL,
  `multicast_pid` int(11) DEFAULT NULL,
  `multicast_port` int(11) DEFAULT NULL,
  `user_id` int(11) DEFAULT '-1',
  `ond_image_profile_id` int(11) DEFAULT '-1',
  PRIMARY KEY (`multicast_session_id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `active_multicast_sessions`
--

LOCK TABLES `active_multicast_sessions` WRITE;
/*!40000 ALTER TABLE `active_multicast_sessions` DISABLE KEYS */;
/*!40000 ALTER TABLE `active_multicast_sessions` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `admin_settings`
--

DROP TABLE IF EXISTS `admin_settings`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `admin_settings` (
  `admin_setting_id` int(11) NOT NULL AUTO_INCREMENT,
  `admin_setting_name` varchar(255) DEFAULT NULL,
  `admin_setting_value` varchar(255) DEFAULT NULL,
  `admin_setting_category` varchar(255) DEFAULT NULL,
  PRIMARY KEY (`admin_setting_id`)
) ENGINE=InnoDB AUTO_INCREMENT=51 DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `admin_settings`
--

LOCK TABLES `admin_settings` WRITE;
/*!40000 ALTER TABLE `admin_settings` DISABLE KEYS */;
INSERT INTO `admin_settings` VALUES (1,'AD Login Domain','',NULL),(2,'Client Receiver Args','',NULL),(3,'Debug Requires Login','Yes',NULL),(4,'Default Computer View','all',NULL),(5,'Force SSL','No',NULL),(6,'Global Computer Args','',NULL),(7,'Ipxe Requires Login','False',NULL),(8,'Multicast Decompression','client',NULL),(9,'On Demand','Enabled',NULL),(10,'On Demand Requires Login','Yes',NULL),(11,'Proxy Bios File','pxelinux',NULL),(12,'Proxy Dhcp','No',NULL),(13,'Proxy Efi32 File','syslinux',NULL),(14,'Proxy Efi64 File','syslinux',NULL),(15,'PXE Mode','pxelinux',NULL),(16,'Queue Size','2',NULL),(17,'Register Requires Login','Yes',NULL),(18,'Require Image Approval','True',NULL),(19,'Sender Args','',NULL),(20,'Server IP','192.168.56.1',NULL),(21,'Smtp Enabled','0',NULL),(22,'Smtp Mail From',NULL,NULL),(23,'Smtp Mail To',NULL,NULL),(24,'Smtp Password Encrypted',NULL,NULL),(25,'Smtp Port',NULL,NULL),(26,'Smtp Server',NULL,NULL),(27,'Smtp Ssl','No',NULL),(28,'Smtp Username',NULL,NULL),(29,'Tftp Path','',NULL),(30,'Udpcast End Port','10000',NULL),(31,'Udpcast Start Port','9000',NULL),(32,'Universal Token','',NULL),(33,'Web Path','http://[server-ip]/clonedeploy/service/client.asmx/',NULL),(34,'Web Server Port','80',NULL),(35,'Web Task Requires Login','Yes',NULL),(36,'Ldap Server','',NULL),(37,'Ldap Port','',NULL),(38,'Ldap Auth Attribute','',NULL),(39,'Ldap Base DN','',NULL),(40,'Ldap Auth Type','Secure',NULL),(41,'Ldap Enabled','0',NULL),(42,'Munki Base Path','',NULL),(43,'Munki Path Type','',NULL),(44,'Munki SMB Username','',NULL),(45,'Munki SMB Password Encrypted','',NULL),(46,'Munki SMB Domain','',NULL),(47,'Clobber Enabled','0',NULL),(48,'Clobber ProfileId','',NULL),(49,'Clobber Requires Login','Yes',NULL),(50,'Clobber Prompt Computer Name','0',NULL);
/*!40000 ALTER TABLE `admin_settings` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `applications`
--

DROP TABLE IF EXISTS `applications`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `applications` (
  `application_id` int(11) NOT NULL AUTO_INCREMENT,
  `application_name` varchar(255) NOT NULL,
  `application_version` varchar(255) DEFAULT NULL,
  `application_guid` varchar(255) DEFAULT NULL,
  PRIMARY KEY (`application_id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `applications`
--

LOCK TABLES `applications` WRITE;
/*!40000 ALTER TABLE `applications` DISABLE KEYS */;
/*!40000 ALTER TABLE `applications` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `boot_menu_entries`
--

DROP TABLE IF EXISTS `boot_menu_entries`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `boot_menu_entries` (
  `boot_menu_entry_id` int(11) NOT NULL AUTO_INCREMENT,
  `boot_menu_entry_name` varchar(45) DEFAULT NULL,
  `boot_menu_entry_description` longtext,
  `boot_menu_entry_type` varchar(45) DEFAULT NULL,
  `boot_menu_entry_order` int(11) DEFAULT '0',
  `boot_menu_entry_content` longtext,
  `boot_menu_entry_is_active` tinyint(4) DEFAULT '0',
  `boot_menu_entry_is_default` tinyint(4) DEFAULT '0',
  PRIMARY KEY (`boot_menu_entry_id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `boot_menu_entries`
--

LOCK TABLES `boot_menu_entries` WRITE;
/*!40000 ALTER TABLE `boot_menu_entries` DISABLE KEYS */;
/*!40000 ALTER TABLE `boot_menu_entries` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `boot_menu_templates`
--

DROP TABLE IF EXISTS `boot_menu_templates`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `boot_menu_templates` (
  `boot_menu_template_id` int(11) NOT NULL AUTO_INCREMENT,
  `boot_menu_template_name` varchar(45) DEFAULT NULL,
  `boot_menu_template_description` longtext,
  `boot_menu_template_contents` longtext,
  PRIMARY KEY (`boot_menu_template_id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `boot_menu_templates`
--

LOCK TABLES `boot_menu_templates` WRITE;
/*!40000 ALTER TABLE `boot_menu_templates` DISABLE KEYS */;
/*!40000 ALTER TABLE `boot_menu_templates` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `buildings`
--

DROP TABLE IF EXISTS `buildings`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `buildings` (
  `building_id` int(11) NOT NULL AUTO_INCREMENT,
  `building_name` varchar(45) DEFAULT NULL,
  `building_distribution_point` int(11) DEFAULT '-1',
  PRIMARY KEY (`building_id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `buildings`
--

LOCK TABLES `buildings` WRITE;
/*!40000 ALTER TABLE `buildings` DISABLE KEYS */;
/*!40000 ALTER TABLE `buildings` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `client_certificates`
--

DROP TABLE IF EXISTS `client_certificates`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `client_certificates` (
  `computer_id` int(11) DEFAULT NULL,
  `certificate_id` int(11) NOT NULL AUTO_INCREMENT,
  `subject_name` varchar(45) DEFAULT NULL,
  `key_blob` mediumblob,
  `key_password_encrypted` varchar(45) DEFAULT NULL,
  `serial_number` bigint(32) DEFAULT NULL,
  `not_after` bigint(32) DEFAULT NULL,
  `is_revoked` tinyint(1) DEFAULT NULL,
  `revoked_date` bigint(32) DEFAULT NULL,
  PRIMARY KEY (`certificate_id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `client_certificates`
--

LOCK TABLES `client_certificates` WRITE;
/*!40000 ALTER TABLE `client_certificates` DISABLE KEYS */;
/*!40000 ALTER TABLE `client_certificates` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `clonedeploy_user_group_mgmt`
--

DROP TABLE IF EXISTS `clonedeploy_user_group_mgmt`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `clonedeploy_user_group_mgmt` (
  `clonedeploy_user_group_mgmt_id` int(11) NOT NULL AUTO_INCREMENT,
  `user_id` int(11) NOT NULL,
  `group_id` int(11) NOT NULL,
  PRIMARY KEY (`clonedeploy_user_group_mgmt_id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `clonedeploy_user_group_mgmt`
--

LOCK TABLES `clonedeploy_user_group_mgmt` WRITE;
/*!40000 ALTER TABLE `clonedeploy_user_group_mgmt` DISABLE KEYS */;
/*!40000 ALTER TABLE `clonedeploy_user_group_mgmt` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `clonedeploy_user_groups`
--

DROP TABLE IF EXISTS `clonedeploy_user_groups`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `clonedeploy_user_groups` (
  `clonedeploy_user_group_id` int(11) NOT NULL AUTO_INCREMENT,
  `clonedeploy_user_group_name` varchar(45) DEFAULT NULL,
  `clonedeploy_user_group_role` varchar(45) DEFAULT NULL,
  `clonedeploy_user_group_ldap` tinyint(4) DEFAULT '0',
  `clonedeploy_user_group_ldapname` varchar(45) DEFAULT NULL,
  PRIMARY KEY (`clonedeploy_user_group_id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `clonedeploy_user_groups`
--

LOCK TABLES `clonedeploy_user_groups` WRITE;
/*!40000 ALTER TABLE `clonedeploy_user_groups` DISABLE KEYS */;
/*!40000 ALTER TABLE `clonedeploy_user_groups` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `clonedeploy_user_image_mgmt`
--

DROP TABLE IF EXISTS `clonedeploy_user_image_mgmt`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `clonedeploy_user_image_mgmt` (
  `clonedeploy_user_image_mgmt_id` int(11) NOT NULL AUTO_INCREMENT,
  `user_id` int(11) NOT NULL,
  `image_id` int(11) NOT NULL,
  PRIMARY KEY (`clonedeploy_user_image_mgmt_id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `clonedeploy_user_image_mgmt`
--

LOCK TABLES `clonedeploy_user_image_mgmt` WRITE;
/*!40000 ALTER TABLE `clonedeploy_user_image_mgmt` DISABLE KEYS */;
/*!40000 ALTER TABLE `clonedeploy_user_image_mgmt` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `clonedeploy_user_lockouts`
--

DROP TABLE IF EXISTS `clonedeploy_user_lockouts`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `clonedeploy_user_lockouts` (
  `clonedeploy_user_lockout_id` int(11) NOT NULL AUTO_INCREMENT,
  `user_id` int(11) NOT NULL,
  `bad_login_count` int(11) DEFAULT '0',
  `locked_until_time_utc` datetime DEFAULT NULL,
  PRIMARY KEY (`clonedeploy_user_lockout_id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `clonedeploy_user_lockouts`
--

LOCK TABLES `clonedeploy_user_lockouts` WRITE;
/*!40000 ALTER TABLE `clonedeploy_user_lockouts` DISABLE KEYS */;
/*!40000 ALTER TABLE `clonedeploy_user_lockouts` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `clonedeploy_user_rights`
--

DROP TABLE IF EXISTS `clonedeploy_user_rights`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `clonedeploy_user_rights` (
  `clonedeploy_user_right_id` int(11) NOT NULL AUTO_INCREMENT,
  `user_id` int(11) NOT NULL,
  `user_right` varchar(45) DEFAULT NULL,
  PRIMARY KEY (`clonedeploy_user_right_id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `clonedeploy_user_rights`
--

LOCK TABLES `clonedeploy_user_rights` WRITE;
/*!40000 ALTER TABLE `clonedeploy_user_rights` DISABLE KEYS */;
/*!40000 ALTER TABLE `clonedeploy_user_rights` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `clonedeploy_usergroup_group_mgmt`
--

DROP TABLE IF EXISTS `clonedeploy_usergroup_group_mgmt`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `clonedeploy_usergroup_group_mgmt` (
  `clonedeploy_usergroup_group_mgmt_id` int(11) NOT NULL AUTO_INCREMENT,
  `usergroup_id` int(11) NOT NULL,
  `group_id` int(11) NOT NULL,
  PRIMARY KEY (`clonedeploy_usergroup_group_mgmt_id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `clonedeploy_usergroup_group_mgmt`
--

LOCK TABLES `clonedeploy_usergroup_group_mgmt` WRITE;
/*!40000 ALTER TABLE `clonedeploy_usergroup_group_mgmt` DISABLE KEYS */;
/*!40000 ALTER TABLE `clonedeploy_usergroup_group_mgmt` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `clonedeploy_usergroup_image_mgmt`
--

DROP TABLE IF EXISTS `clonedeploy_usergroup_image_mgmt`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `clonedeploy_usergroup_image_mgmt` (
  `clonedeploy_usergroup_image_mgmt_id` int(11) NOT NULL AUTO_INCREMENT,
  `usergroup_id` int(11) NOT NULL,
  `image_id` int(11) NOT NULL,
  PRIMARY KEY (`clonedeploy_usergroup_image_mgmt_id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `clonedeploy_usergroup_image_mgmt`
--

LOCK TABLES `clonedeploy_usergroup_image_mgmt` WRITE;
/*!40000 ALTER TABLE `clonedeploy_usergroup_image_mgmt` DISABLE KEYS */;
/*!40000 ALTER TABLE `clonedeploy_usergroup_image_mgmt` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `clonedeploy_usergroup_rights`
--

DROP TABLE IF EXISTS `clonedeploy_usergroup_rights`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `clonedeploy_usergroup_rights` (
  `clonedeploy_usergroup_right_id` int(11) NOT NULL AUTO_INCREMENT,
  `usergroup_id` int(11) NOT NULL,
  `usergroup_right` varchar(45) DEFAULT NULL,
  PRIMARY KEY (`clonedeploy_usergroup_right_id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `clonedeploy_usergroup_rights`
--

LOCK TABLES `clonedeploy_usergroup_rights` WRITE;
/*!40000 ALTER TABLE `clonedeploy_usergroup_rights` DISABLE KEYS */;
/*!40000 ALTER TABLE `clonedeploy_usergroup_rights` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `clonedeploy_users`
--

DROP TABLE IF EXISTS `clonedeploy_users`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `clonedeploy_users` (
  `clonedeploy_user_id` int(11) NOT NULL AUTO_INCREMENT,
  `clonedeploy_username` varchar(45) DEFAULT NULL,
  `clonedeploy_user_pwd` varchar(255) DEFAULT NULL,
  `clonedeploy_user_salt` varchar(255) DEFAULT NULL,
  `clonedeploy_user_role` varchar(45) DEFAULT NULL,
  `clonedeploy_user_email` varchar(45) DEFAULT NULL,
  `clonedeploy_user_token` varchar(45) DEFAULT NULL,
  `notify_on_lockout` tinyint(4) DEFAULT '0',
  `notify_on_error` tinyint(4) DEFAULT '0',
  `notify_on_complete` tinyint(4) DEFAULT '0',
  `notify_on_image_approved` tinyint(4) DEFAULT '0',
  `clonedeploy_user_api_id` varchar(45) DEFAULT NULL,
  `clonedeploy_user_api_key` varchar(45) DEFAULT NULL,
  `clonedeploy_user_is_ldap` tinyint(4) DEFAULT '0',
  `clonedeploy_usergroup_id` int(11) DEFAULT '-1',
  PRIMARY KEY (`clonedeploy_user_id`)
) ENGINE=InnoDB AUTO_INCREMENT=2 DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `clonedeploy_users`
--

LOCK TABLES `clonedeploy_users` WRITE;
/*!40000 ALTER TABLE `clonedeploy_users` DISABLE KEYS */;
INSERT INTO `clonedeploy_users` VALUES (1,'clonedeploy','K2X58/PExEJMF2PmtcK5Wgz6HVRB+paPF30JIOZvgG8=','1iobxjZtR++hzgM2xLiDRgrfF0/sibaJaLCWgoTV46qBLL+ZFBp3dnxPEU7ZvsTk7PMgDbO/la/IBZ2SQBaXdw==','Administrator','','11aee8db-ccfe-4d43-b53e-1a77c611485a',1,1,1,1,NULL,NULL,0,-1);
/*!40000 ALTER TABLE `clonedeploy_users` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `clonedeploy_version`
--

DROP TABLE IF EXISTS `clonedeploy_version`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `clonedeploy_version` (
  `clonedeploy_version_id` int(11) NOT NULL AUTO_INCREMENT,
  `app_version` varchar(45) DEFAULT NULL,
  `database_version` varchar(45) DEFAULT NULL,
  `first_run_completed` tinyint(4) DEFAULT '0',
  PRIMARY KEY (`clonedeploy_version_id`)
) ENGINE=InnoDB AUTO_INCREMENT=2 DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `clonedeploy_version`
--

LOCK TABLES `clonedeploy_version` WRITE;
/*!40000 ALTER TABLE `clonedeploy_version` DISABLE KEYS */;
INSERT INTO `clonedeploy_version` VALUES (1,'1.2.0','102',0);
/*!40000 ALTER TABLE `clonedeploy_version` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `computer_alt_mac_addresses`
--

DROP TABLE IF EXISTS `computer_alt_mac_addresses`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `computer_alt_mac_addresses` (
  `computer_alt_mac_addresses_id` int(11) NOT NULL AUTO_INCREMENT,
  `computer_id` int(11) DEFAULT NULL,
  `mac_address` varchar(45) DEFAULT NULL,
  PRIMARY KEY (`computer_alt_mac_addresses_id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `computer_alt_mac_addresses`
--

LOCK TABLES `computer_alt_mac_addresses` WRITE;
/*!40000 ALTER TABLE `computer_alt_mac_addresses` DISABLE KEYS */;
/*!40000 ALTER TABLE `computer_alt_mac_addresses` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `computer_applications`
--

DROP TABLE IF EXISTS `computer_applications`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `computer_applications` (
  `computer_application_id` int(11) NOT NULL AUTO_INCREMENT,
  `computer_id` int(11) DEFAULT NULL,
  `application_id` int(11) DEFAULT NULL,
  PRIMARY KEY (`computer_application_id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `computer_applications`
--

LOCK TABLES `computer_applications` WRITE;
/*!40000 ALTER TABLE `computer_applications` DISABLE KEYS */;
/*!40000 ALTER TABLE `computer_applications` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `computer_boot_menus`
--

DROP TABLE IF EXISTS `computer_boot_menus`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `computer_boot_menus` (
  `computer_boot_menu_id` int(11) NOT NULL AUTO_INCREMENT,
  `computer_id` int(11) NOT NULL,
  `bios_menu` longtext,
  `efi32_menu` longtext,
  `efi64_menu` longtext,
  PRIMARY KEY (`computer_boot_menu_id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `computer_boot_menus`
--

LOCK TABLES `computer_boot_menus` WRITE;
/*!40000 ALTER TABLE `computer_boot_menus` DISABLE KEYS */;
/*!40000 ALTER TABLE `computer_boot_menus` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `computer_harddrives`
--

DROP TABLE IF EXISTS `computer_harddrives`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `computer_harddrives` (
  `computer_harddrive_id` int(11) NOT NULL AUTO_INCREMENT,
  `computer_id` int(11) NOT NULL,
  `model` varchar(255) DEFAULT NULL,
  `serial_number` varchar(255) DEFAULT NULL,
  `capacity` varchar(45) DEFAULT NULL,
  `smart_status` varchar(45) DEFAULT NULL,
  PRIMARY KEY (`computer_harddrive_id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `computer_harddrives`
--

LOCK TABLES `computer_harddrives` WRITE;
/*!40000 ALTER TABLE `computer_harddrives` DISABLE KEYS */;
/*!40000 ALTER TABLE `computer_harddrives` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `computer_inventory`
--

DROP TABLE IF EXISTS `computer_inventory`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `computer_inventory` (
  `computer_inventory_id` int(11) NOT NULL AUTO_INCREMENT,
  `computer_id` int(11) NOT NULL,
  `last_inventory_update` datetime DEFAULT NULL,
  `last_checkin` datetime DEFAULT NULL,
  `ip_address` varchar(45) DEFAULT NULL,
  `client_version` varchar(45) DEFAULT NULL,
  `manufacturer` varchar(45) DEFAULT NULL,
  `model` varchar(45) DEFAULT NULL,
  `uuid` varchar(45) DEFAULT NULL,
  `uuid_type` varchar(45) DEFAULT NULL,
  `serial_number` varchar(45) DEFAULT NULL,
  `processor` varchar(45) DEFAULT NULL,
  `total_ram` varchar(45) DEFAULT NULL,
  `boot_rom` varchar(45) DEFAULT NULL,
  `os_name` varchar(45) DEFAULT NULL,
  `os_version` varchar(45) DEFAULT NULL,
  `os_service_pack` varchar(45) DEFAULT NULL,
  `os_service_release` varchar(45) DEFAULT NULL,
  PRIMARY KEY (`computer_inventory_id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `computer_inventory`
--

LOCK TABLES `computer_inventory` WRITE;
/*!40000 ALTER TABLE `computer_inventory` DISABLE KEYS */;
/*!40000 ALTER TABLE `computer_inventory` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `computer_logins`
--

DROP TABLE IF EXISTS `computer_logins`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `computer_logins` (
  `computer_login_id` int(11) NOT NULL AUTO_INCREMENT,
  `computer_id` int(11) NOT NULL,
  `computer_user_id` int(11) NOT NULL,
  `login_time_utc` datetime NOT NULL,
  `logout_time_utc` datetime NOT NULL,
  PRIMARY KEY (`computer_login_id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `computer_logins`
--

LOCK TABLES `computer_logins` WRITE;
/*!40000 ALTER TABLE `computer_logins` DISABLE KEYS */;
/*!40000 ALTER TABLE `computer_logins` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `computer_logs`
--

DROP TABLE IF EXISTS `computer_logs`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `computer_logs` (
  `computer_log_id` int(11) NOT NULL AUTO_INCREMENT,
  `computer_id` int(11) NOT NULL,
  `log_type` varchar(45) DEFAULT NULL,
  `log_sub_type` varchar(45) DEFAULT NULL,
  `log_contents` longtext,
  `log_time` timestamp NOT NULL DEFAULT CURRENT_TIMESTAMP,
  `computer_mac` varchar(45) DEFAULT NULL,
  PRIMARY KEY (`computer_log_id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `computer_logs`
--

LOCK TABLES `computer_logs` WRITE;
/*!40000 ALTER TABLE `computer_logs` DISABLE KEYS */;
/*!40000 ALTER TABLE `computer_logs` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `computer_munki_templates`
--

DROP TABLE IF EXISTS `computer_munki_templates`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `computer_munki_templates` (
  `computer_munki_template_id` int(11) NOT NULL AUTO_INCREMENT,
  `computer_id` int(11) DEFAULT NULL,
  `munki_template_id` int(11) DEFAULT NULL,
  PRIMARY KEY (`computer_munki_template_id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `computer_munki_templates`
--

LOCK TABLES `computer_munki_templates` WRITE;
/*!40000 ALTER TABLE `computer_munki_templates` DISABLE KEYS */;
/*!40000 ALTER TABLE `computer_munki_templates` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `computer_printers`
--

DROP TABLE IF EXISTS `computer_printers`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `computer_printers` (
  `computer_printer_id` int(11) NOT NULL AUTO_INCREMENT,
  `computer_id` int(11) NOT NULL,
  `printer_name` varchar(255) DEFAULT NULL,
  `printer_model` varchar(255) DEFAULT NULL,
  `printer_uri` varchar(255) DEFAULT NULL,
  PRIMARY KEY (`computer_printer_id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `computer_printers`
--

LOCK TABLES `computer_printers` WRITE;
/*!40000 ALTER TABLE `computer_printers` DISABLE KEYS */;
/*!40000 ALTER TABLE `computer_printers` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `computer_proxy_reservations`
--

DROP TABLE IF EXISTS `computer_proxy_reservations`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `computer_proxy_reservations` (
  `computer_proxy_reservation_id` int(11) NOT NULL AUTO_INCREMENT,
  `computer_id` int(11) DEFAULT NULL,
  `next_server` varchar(45) DEFAULT NULL,
  `boot_file` varchar(255) DEFAULT NULL,
  PRIMARY KEY (`computer_proxy_reservation_id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `computer_proxy_reservations`
--

LOCK TABLES `computer_proxy_reservations` WRITE;
/*!40000 ALTER TABLE `computer_proxy_reservations` DISABLE KEYS */;
/*!40000 ALTER TABLE `computer_proxy_reservations` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `computer_users`
--

DROP TABLE IF EXISTS `computer_users`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `computer_users` (
  `computer_user_id` int(11) NOT NULL,
  `username` varchar(45) NOT NULL,
  PRIMARY KEY (`computer_user_id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `computer_users`
--

LOCK TABLES `computer_users` WRITE;
/*!40000 ALTER TABLE `computer_users` DISABLE KEYS */;
/*!40000 ALTER TABLE `computer_users` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `computers`
--

DROP TABLE IF EXISTS `computers`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `computers` (
  `computer_id` int(11) NOT NULL AUTO_INCREMENT,
  `computer_name` varchar(45) NOT NULL,
  `computer_primary_mac` varchar(45) NOT NULL,
  `computer_description` longtext,
  `computer_site_id` int(11) DEFAULT '-1',
  `computer_building_id` int(11) DEFAULT '-1',
  `computer_room_id` int(11) DEFAULT '-1',
  `computer_image_id` int(11) DEFAULT '-1',
  `computer_image_profile_id` int(11) DEFAULT '-1',
  `computer_has_custom_menu` tinyint(4) DEFAULT '0',
  `custom_attr_1` varchar(255) DEFAULT NULL,
  `custom_attr_2` varchar(255) DEFAULT NULL,
  `custom_attr_3` varchar(255) DEFAULT NULL,
  `custom_attr_4` varchar(255) DEFAULT NULL,
  `custom_attr_5` varchar(255) DEFAULT NULL,
  `proxy_reservation_enabled` tinyint(4) DEFAULT '0',
  PRIMARY KEY (`computer_id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `computers`
--

LOCK TABLES `computers` WRITE;
/*!40000 ALTER TABLE `computers` DISABLE KEYS */;
/*!40000 ALTER TABLE `computers` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `distribution_points`
--

DROP TABLE IF EXISTS `distribution_points`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `distribution_points` (
  `distribution_point_id` int(11) NOT NULL AUTO_INCREMENT,
  `distribution_point_display_name` varchar(45) DEFAULT NULL,
  `distribution_point_server` varchar(45) DEFAULT NULL,
  `distribution_point_protocol` varchar(45) DEFAULT NULL,
  `distribution_point_share_name` varchar(45) DEFAULT NULL,
  `distribution_point_domain` varchar(45) DEFAULT NULL,
  `distribution_point_rw_username` varchar(45) DEFAULT NULL,
  `distribution_point_rw_password_encrypted` varchar(255) DEFAULT NULL,
  `distribution_point_is_primary` tinyint(4) DEFAULT '0',
  `distribution_point_physical_path` varchar(255) DEFAULT NULL,
  `distribution_point_is_backend` tinyint(4) DEFAULT '0',
  `distribution_point_backend_server` varchar(45) DEFAULT NULL,
  `distribution_point_ro_username` varchar(45) DEFAULT NULL,
  `distribution_point_ro_password_encrypted` varchar(255) DEFAULT NULL,
  PRIMARY KEY (`distribution_point_id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `distribution_points`
--

LOCK TABLES `distribution_points` WRITE;
/*!40000 ALTER TABLE `distribution_points` DISABLE KEYS */;
/*!40000 ALTER TABLE `distribution_points` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `files_folders`
--

DROP TABLE IF EXISTS `files_folders`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `files_folders` (
  `file_folder_id` int(11) NOT NULL AUTO_INCREMENT,
  `file_folder_display_name` varchar(45) DEFAULT NULL,
  `file_folder_path` varchar(255) DEFAULT NULL,
  `file_folder_type` varchar(45) DEFAULT NULL,
  PRIMARY KEY (`file_folder_id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `files_folders`
--

LOCK TABLES `files_folders` WRITE;
/*!40000 ALTER TABLE `files_folders` DISABLE KEYS */;
/*!40000 ALTER TABLE `files_folders` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `group_boot_menus`
--

DROP TABLE IF EXISTS `group_boot_menus`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `group_boot_menus` (
  `group_boot_menu_id` int(11) NOT NULL AUTO_INCREMENT,
  `group_id` int(11) NOT NULL,
  `bios_menu` longtext,
  `efi32_menu` longtext,
  `efi64_menu` longtext,
  PRIMARY KEY (`group_boot_menu_id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `group_boot_menus`
--

LOCK TABLES `group_boot_menus` WRITE;
/*!40000 ALTER TABLE `group_boot_menus` DISABLE KEYS */;
/*!40000 ALTER TABLE `group_boot_menus` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `group_computer_properties`
--

DROP TABLE IF EXISTS `group_computer_properties`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `group_computer_properties` (
  `group_computer_property_id` int(11) NOT NULL AUTO_INCREMENT,
  `group_id` int(11) NOT NULL,
  `image_id` int(11) DEFAULT '-1',
  `image_profile_id` int(11) DEFAULT '-1',
  `description` longtext,
  `site_id` int(11) DEFAULT '-1',
  `building_id` int(11) DEFAULT '-1',
  `room_id` int(11) DEFAULT '-1',
  `custom_attr_1` varchar(255) DEFAULT NULL,
  `custom_attr_2` varchar(255) DEFAULT NULL,
  `custom_attr_3` varchar(255) DEFAULT NULL,
  `custom_attr_4` varchar(255) DEFAULT NULL,
  `custom_attr_5` varchar(255) DEFAULT NULL,
  `image_enabled` tinyint(4) DEFAULT '0',
  `image_profile_enabled` tinyint(4) DEFAULT '0',
  `description_enabled` tinyint(4) DEFAULT '0',
  `site_enabled` tinyint(4) DEFAULT '0',
  `building_enabled` tinyint(4) DEFAULT '0',
  `room_enabled` tinyint(4) DEFAULT '0',
  `custom_1_enabled` tinyint(4) DEFAULT '0',
  `custom_2_enabled` tinyint(4) DEFAULT '0',
  `custom_3_enabled` tinyint(4) DEFAULT '0',
  `custom_4_enabled` tinyint(4) DEFAULT '0',
  `custom_5_enabled` tinyint(4) DEFAULT '0',
  `proxy_enabled_enabled` tinyint(4) DEFAULT '0',
  `tftp_server_enabled` tinyint(4) DEFAULT '0',
  `bootfile_enabled` tinyint(4) DEFAULT '0',
  `proxy_enabled` tinyint(4) DEFAULT '0',
  `tftp_server` varchar(45) DEFAULT NULL,
  `bootfile` varchar(255) DEFAULT NULL,
  PRIMARY KEY (`group_computer_property_id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `group_computer_properties`
--

LOCK TABLES `group_computer_properties` WRITE;
/*!40000 ALTER TABLE `group_computer_properties` DISABLE KEYS */;
/*!40000 ALTER TABLE `group_computer_properties` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `group_membership`
--

DROP TABLE IF EXISTS `group_membership`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `group_membership` (
  `group_membership_id` int(11) NOT NULL AUTO_INCREMENT,
  `computer_id` int(11) NOT NULL,
  `group_id` int(11) NOT NULL,
  PRIMARY KEY (`group_membership_id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `group_membership`
--

LOCK TABLES `group_membership` WRITE;
/*!40000 ALTER TABLE `group_membership` DISABLE KEYS */;
/*!40000 ALTER TABLE `group_membership` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `group_munki_templates`
--

DROP TABLE IF EXISTS `group_munki_templates`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `group_munki_templates` (
  `group_munki_template_id` int(11) NOT NULL AUTO_INCREMENT,
  `group_id` int(11) DEFAULT NULL,
  `munki_template_id` int(11) DEFAULT NULL,
  PRIMARY KEY (`group_munki_template_id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `group_munki_templates`
--

LOCK TABLES `group_munki_templates` WRITE;
/*!40000 ALTER TABLE `group_munki_templates` DISABLE KEYS */;
/*!40000 ALTER TABLE `group_munki_templates` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `groups`
--

DROP TABLE IF EXISTS `groups`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `groups` (
  `group_id` int(11) NOT NULL AUTO_INCREMENT,
  `group_name` varchar(45) NOT NULL,
  `group_description` longtext,
  `group_image_id` int(11) DEFAULT '-1',
  `group_image_profile_id` int(11) DEFAULT '-1',
  `group_type` varchar(45) DEFAULT NULL,
  `group_smart_criteria` varchar(45) DEFAULT NULL,
  `group_default_properties_enabled` tinyint(4) DEFAULT '0',
  `group_default_bootmenu_enabled` tinyint(4) DEFAULT '0',
  PRIMARY KEY (`group_id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `groups`
--

LOCK TABLES `groups` WRITE;
/*!40000 ALTER TABLE `groups` DISABLE KEYS */;
/*!40000 ALTER TABLE `groups` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `history_user`
--

DROP TABLE IF EXISTS `history_user`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `history_user` (
  `history_user_id` int(11) NOT NULL,
  `user_id` int(11) NOT NULL,
  `history_type` varchar(45) DEFAULT NULL,
  `history_result` varchar(45) DEFAULT NULL,
  `time_utc` datetime DEFAULT NULL,
  `computer_id` int(11) DEFAULT '-1',
  `group_id` int(11) DEFAULT '-1',
  `image_id` int(11) DEFAULT '-1',
  PRIMARY KEY (`history_user_id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `history_user`
--

LOCK TABLES `history_user` WRITE;
/*!40000 ALTER TABLE `history_user` DISABLE KEYS */;
/*!40000 ALTER TABLE `history_user` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `image_profile_files_folders`
--

DROP TABLE IF EXISTS `image_profile_files_folders`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `image_profile_files_folders` (
  `image_profile_files_folders_id` int(11) NOT NULL AUTO_INCREMENT,
  `image_profile_id` int(11) NOT NULL,
  `file_folder_id` int(11) NOT NULL,
  `priority` int(11) DEFAULT '0',
  `destination_partition` varchar(45) DEFAULT NULL,
  `destination_folder` varchar(255) DEFAULT NULL,
  `folder_copy_type` varchar(45) DEFAULT NULL,
  PRIMARY KEY (`image_profile_files_folders_id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `image_profile_files_folders`
--

LOCK TABLES `image_profile_files_folders` WRITE;
/*!40000 ALTER TABLE `image_profile_files_folders` DISABLE KEYS */;
/*!40000 ALTER TABLE `image_profile_files_folders` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `image_profile_partition_layouts`
--

DROP TABLE IF EXISTS `image_profile_partition_layouts`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `image_profile_partition_layouts` (
  `image_profile_partition_layout_id` int(11) NOT NULL AUTO_INCREMENT,
  `image_profile_id` int(11) NOT NULL,
  `partition_layout_id` int(11) NOT NULL,
  PRIMARY KEY (`image_profile_partition_layout_id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `image_profile_partition_layouts`
--

LOCK TABLES `image_profile_partition_layouts` WRITE;
/*!40000 ALTER TABLE `image_profile_partition_layouts` DISABLE KEYS */;
/*!40000 ALTER TABLE `image_profile_partition_layouts` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `image_profile_scripts`
--

DROP TABLE IF EXISTS `image_profile_scripts`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `image_profile_scripts` (
  `image_profile_script_id` int(11) NOT NULL AUTO_INCREMENT,
  `image_profile_id` int(11) NOT NULL,
  `script_id` int(11) NOT NULL,
  `run_pre` tinyint(4) DEFAULT '0',
  `run_post` tinyint(4) DEFAULT '0',
  `priority` int(11) DEFAULT '0',
  PRIMARY KEY (`image_profile_script_id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `image_profile_scripts`
--

LOCK TABLES `image_profile_scripts` WRITE;
/*!40000 ALTER TABLE `image_profile_scripts` DISABLE KEYS */;
/*!40000 ALTER TABLE `image_profile_scripts` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `image_profile_sysprep_tags`
--

DROP TABLE IF EXISTS `image_profile_sysprep_tags`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `image_profile_sysprep_tags` (
  `image_profile_sysprep_tag_id` int(11) NOT NULL AUTO_INCREMENT,
  `image_profile_id` int(11) NOT NULL,
  `sysprep_tag_id` int(11) NOT NULL,
  `priority` int(11) DEFAULT '0',
  PRIMARY KEY (`image_profile_sysprep_tag_id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `image_profile_sysprep_tags`
--

LOCK TABLES `image_profile_sysprep_tags` WRITE;
/*!40000 ALTER TABLE `image_profile_sysprep_tags` DISABLE KEYS */;
/*!40000 ALTER TABLE `image_profile_sysprep_tags` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `image_profiles`
--

DROP TABLE IF EXISTS `image_profiles`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `image_profiles` (
  `image_profile_id` int(11) NOT NULL AUTO_INCREMENT,
  `image_id` int(11) NOT NULL,
  `profile_kernel` varchar(45) DEFAULT NULL,
  `profile_boot_image` varchar(45) DEFAULT NULL,
  `profile_name` varchar(45) DEFAULT NULL,
  `profile_description` longtext,
  `profile_kernel_arguments` longtext,
  `skip_core_download` tinyint(4) DEFAULT '0',
  `skip_set_clock` tinyint(4) DEFAULT '0',
  `task_completed_action` varchar(45) DEFAULT 'Reboot',
  `remove_gpt_structures` tinyint(4) DEFAULT '0',
  `skip_volume_shrink` tinyint(4) DEFAULT '0',
  `skip_lvm_shrink` tinyint(4) DEFAULT '0',
  `skip_volume_expand` tinyint(4) DEFAULT '0',
  `fix_bcd` tinyint(4) DEFAULT '0',
  `fix_bootloader` tinyint(4) DEFAULT '1',
  `partition_method` varchar(45) DEFAULT 'Dynamic',
  `force_dynamic_partitions` tinyint(4) DEFAULT '0',
  `custom_partition_script` longtext,
  `compression_algorithm` varchar(45) DEFAULT 'lz4',
  `compression_level` varchar(45) DEFAULT '1',
  `custom_image_schema` longtext,
  `custom_upload_schema` longtext,
  `upload_schema_only` tinyint(4) DEFAULT '0',
  `multicast_sender_arguments` varchar(45) DEFAULT NULL,
  `multicast_receiver_arguments` varchar(45) DEFAULT NULL,
  `web_cancel` tinyint(4) DEFAULT '0',
  `change_name` tinyint(4) DEFAULT '1',
  `osx_target_volume` varchar(45) DEFAULT NULL,
  `osx_install_munki` tinyint(4) DEFAULT '0',
  `munki_repo_url` varchar(255) DEFAULT NULL,
  `munki_auth_username` varchar(45) DEFAULT NULL,
  `munki_auth_password` varchar(45) DEFAULT NULL,
  `wim_enabled_multicast` tinyint(4) DEFAULT '0',
  PRIMARY KEY (`image_profile_id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `image_profiles`
--

LOCK TABLES `image_profiles` WRITE;
/*!40000 ALTER TABLE `image_profiles` DISABLE KEYS */;
/*!40000 ALTER TABLE `image_profiles` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `images`
--

DROP TABLE IF EXISTS `images`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `images` (
  `image_id` int(11) NOT NULL AUTO_INCREMENT,
  `image_name` varchar(45) NOT NULL,
  `image_os` varchar(45) DEFAULT NULL,
  `image_description` longtext,
  `image_is_protected` tinyint(1) DEFAULT '0',
  `image_is_viewable_ond` tinyint(1) DEFAULT '1',
  `image_enabled` tinyint(4) DEFAULT '1',
  `image_type` varchar(45) DEFAULT NULL,
  `image_environment` varchar(45) DEFAULT NULL,
  `image_approved` tinyint(4) DEFAULT '0',
  `image_osx_type` varchar(45) DEFAULT NULL,
  `image_osx_thin_os` varchar(100) DEFAULT NULL,
  `image_osx_thin_recovery` varchar(100) DEFAULT NULL,
  PRIMARY KEY (`image_id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `images`
--

LOCK TABLES `images` WRITE;
/*!40000 ALTER TABLE `images` DISABLE KEYS */;
/*!40000 ALTER TABLE `images` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `multicast_ports`
--

DROP TABLE IF EXISTS `multicast_ports`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `multicast_ports` (
  `multicast_port_id` int(11) NOT NULL AUTO_INCREMENT,
  `multicast_port_number` int(11) NOT NULL,
  PRIMARY KEY (`multicast_port_id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `multicast_ports`
--

LOCK TABLES `multicast_ports` WRITE;
/*!40000 ALTER TABLE `multicast_ports` DISABLE KEYS */;
/*!40000 ALTER TABLE `multicast_ports` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `munki_manifest_catalogs`
--

DROP TABLE IF EXISTS `munki_manifest_catalogs`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `munki_manifest_catalogs` (
  `munki_manifest_catalog_id` int(11) NOT NULL AUTO_INCREMENT,
  `munki_manifest_catalog_name` varchar(45) DEFAULT NULL,
  `munki_manifest_catalog_priority` int(11) DEFAULT NULL,
  `munki_manifest_template_id` int(11) NOT NULL,
  PRIMARY KEY (`munki_manifest_catalog_id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `munki_manifest_catalogs`
--

LOCK TABLES `munki_manifest_catalogs` WRITE;
/*!40000 ALTER TABLE `munki_manifest_catalogs` DISABLE KEYS */;
/*!40000 ALTER TABLE `munki_manifest_catalogs` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `munki_manifest_included_manifests`
--

DROP TABLE IF EXISTS `munki_manifest_included_manifests`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `munki_manifest_included_manifests` (
  `munki_manifest_included_manifest_id` int(11) NOT NULL AUTO_INCREMENT,
  `munki_manifest_included_manifest_name` varchar(45) DEFAULT NULL,
  `munki_manifest_template_id` varchar(45) DEFAULT NULL,
  `munki_manifest_condition` text,
  PRIMARY KEY (`munki_manifest_included_manifest_id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `munki_manifest_included_manifests`
--

LOCK TABLES `munki_manifest_included_manifests` WRITE;
/*!40000 ALTER TABLE `munki_manifest_included_manifests` DISABLE KEYS */;
/*!40000 ALTER TABLE `munki_manifest_included_manifests` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `munki_manifest_managed_installs`
--

DROP TABLE IF EXISTS `munki_manifest_managed_installs`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `munki_manifest_managed_installs` (
  `munki_manifest_managed_install_id` int(11) NOT NULL AUTO_INCREMENT,
  `munki_manifest_managed_install_name` varchar(45) DEFAULT NULL,
  `munki_manifest_managed_install_version` varchar(45) DEFAULT NULL,
  `munki_manifest_managed_install_include_version` tinyint(4) DEFAULT NULL,
  `munki_manifest_template_id` int(11) DEFAULT NULL,
  `munki_manifest_condition` text,
  PRIMARY KEY (`munki_manifest_managed_install_id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `munki_manifest_managed_installs`
--

LOCK TABLES `munki_manifest_managed_installs` WRITE;
/*!40000 ALTER TABLE `munki_manifest_managed_installs` DISABLE KEYS */;
/*!40000 ALTER TABLE `munki_manifest_managed_installs` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `munki_manifest_managed_uninstalls`
--

DROP TABLE IF EXISTS `munki_manifest_managed_uninstalls`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `munki_manifest_managed_uninstalls` (
  `munki_manifest_managed_uninstall_id` int(11) NOT NULL AUTO_INCREMENT,
  `munki_manifest_managed_uninstall_name` varchar(45) DEFAULT NULL,
  `munki_manifest_managed_uninstall_version` varchar(45) DEFAULT NULL,
  `munki_manifest_managed_uninstall_include_version` tinyint(4) DEFAULT NULL,
  `munki_manifest_template_id` int(11) DEFAULT NULL,
  `munki_manifest_condition` text,
  PRIMARY KEY (`munki_manifest_managed_uninstall_id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `munki_manifest_managed_uninstalls`
--

LOCK TABLES `munki_manifest_managed_uninstalls` WRITE;
/*!40000 ALTER TABLE `munki_manifest_managed_uninstalls` DISABLE KEYS */;
/*!40000 ALTER TABLE `munki_manifest_managed_uninstalls` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `munki_manifest_managed_updates`
--

DROP TABLE IF EXISTS `munki_manifest_managed_updates`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `munki_manifest_managed_updates` (
  `munki_manifest_managed_update_id` int(11) NOT NULL AUTO_INCREMENT,
  `munki_manifest_managed_update_name` varchar(45) DEFAULT NULL,
  `munki_manifest_template_id` int(11) DEFAULT NULL,
  `munki_manifest_condition` text,
  PRIMARY KEY (`munki_manifest_managed_update_id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `munki_manifest_managed_updates`
--

LOCK TABLES `munki_manifest_managed_updates` WRITE;
/*!40000 ALTER TABLE `munki_manifest_managed_updates` DISABLE KEYS */;
/*!40000 ALTER TABLE `munki_manifest_managed_updates` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `munki_manifest_optional_installs`
--

DROP TABLE IF EXISTS `munki_manifest_optional_installs`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `munki_manifest_optional_installs` (
  `munki_manifest_optional_install_id` int(11) NOT NULL AUTO_INCREMENT,
  `munki_manifest_optional_install_name` varchar(45) DEFAULT NULL,
  `munki_manifest_optional_install_version` varchar(45) DEFAULT NULL,
  `munki_manifest_optional_install_include_version` tinyint(4) DEFAULT NULL,
  `munki_manifest_template_id` int(11) DEFAULT NULL,
  `munki_manifest_condition` text,
  PRIMARY KEY (`munki_manifest_optional_install_id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `munki_manifest_optional_installs`
--

LOCK TABLES `munki_manifest_optional_installs` WRITE;
/*!40000 ALTER TABLE `munki_manifest_optional_installs` DISABLE KEYS */;
/*!40000 ALTER TABLE `munki_manifest_optional_installs` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `munki_manifest_templates`
--

DROP TABLE IF EXISTS `munki_manifest_templates`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `munki_manifest_templates` (
  `manifest_template_id` int(11) NOT NULL AUTO_INCREMENT,
  `manifest_template_name` varchar(45) DEFAULT NULL,
  `manifest_template_description` varchar(45) DEFAULT NULL,
  `changes_applied` tinyint(4) DEFAULT '0',
  PRIMARY KEY (`manifest_template_id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `munki_manifest_templates`
--

LOCK TABLES `munki_manifest_templates` WRITE;
/*!40000 ALTER TABLE `munki_manifest_templates` DISABLE KEYS */;
/*!40000 ALTER TABLE `munki_manifest_templates` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `partition_layouts`
--

DROP TABLE IF EXISTS `partition_layouts`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `partition_layouts` (
  `partition_layout_id` int(11) NOT NULL AUTO_INCREMENT,
  `partition_layout_name` varchar(45) DEFAULT NULL,
  `partition_layout_table` varchar(45) DEFAULT NULL,
  `imaging_environment` varchar(45) DEFAULT NULL,
  `partition_layout_priority` int(11) DEFAULT NULL,
  PRIMARY KEY (`partition_layout_id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `partition_layouts`
--

LOCK TABLES `partition_layouts` WRITE;
/*!40000 ALTER TABLE `partition_layouts` DISABLE KEYS */;
/*!40000 ALTER TABLE `partition_layouts` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `partitions`
--

DROP TABLE IF EXISTS `partitions`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `partitions` (
  `partition_id` int(11) NOT NULL AUTO_INCREMENT,
  `partition_layout_id` int(11) DEFAULT NULL,
  `partition_number` tinyint(4) DEFAULT NULL,
  `partition_type` varchar(45) DEFAULT NULL,
  `partition_fstype` varchar(45) DEFAULT NULL,
  `partition_size` int(11) DEFAULT NULL,
  `partition_size_unit` varchar(45) DEFAULT NULL,
  `partition_boot_flag` tinyint(4) DEFAULT NULL,
  PRIMARY KEY (`partition_id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `partitions`
--

LOCK TABLES `partitions` WRITE;
/*!40000 ALTER TABLE `partitions` DISABLE KEYS */;
/*!40000 ALTER TABLE `partitions` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `rooms`
--

DROP TABLE IF EXISTS `rooms`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `rooms` (
  `room_id` int(11) NOT NULL AUTO_INCREMENT,
  `room_name` varchar(45) DEFAULT NULL,
  `room_distribution_point` int(11) DEFAULT '-1',
  PRIMARY KEY (`room_id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `rooms`
--

LOCK TABLES `rooms` WRITE;
/*!40000 ALTER TABLE `rooms` DISABLE KEYS */;
/*!40000 ALTER TABLE `rooms` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `scripts`
--

DROP TABLE IF EXISTS `scripts`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `scripts` (
  `script_id` int(11) NOT NULL AUTO_INCREMENT,
  `script_name` varchar(45) DEFAULT NULL,
  `script_description` longtext,
  `script_contents` longtext,
  PRIMARY KEY (`script_id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `scripts`
--

LOCK TABLES `scripts` WRITE;
/*!40000 ALTER TABLE `scripts` DISABLE KEYS */;
/*!40000 ALTER TABLE `scripts` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `sites`
--

DROP TABLE IF EXISTS `sites`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `sites` (
  `site_id` int(11) NOT NULL AUTO_INCREMENT,
  `site_name` varchar(45) DEFAULT NULL,
  `site_distribution_point` int(11) DEFAULT '-1',
  PRIMARY KEY (`site_id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `sites`
--

LOCK TABLES `sites` WRITE;
/*!40000 ALTER TABLE `sites` DISABLE KEYS */;
/*!40000 ALTER TABLE `sites` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `sysprep_tags`
--

DROP TABLE IF EXISTS `sysprep_tags`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `sysprep_tags` (
  `sysprep_tag_id` int(11) NOT NULL AUTO_INCREMENT,
  `sysprep_tag_name` varchar(45) DEFAULT NULL,
  `sysprep_tag_open` varchar(45) DEFAULT NULL,
  `sysprep_tag_close` varchar(45) DEFAULT NULL,
  `sysprep_tag_description` longtext,
  `sysprep_tag_contents` longtext,
  PRIMARY KEY (`sysprep_tag_id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `sysprep_tags`
--

LOCK TABLES `sysprep_tags` WRITE;
/*!40000 ALTER TABLE `sysprep_tags` DISABLE KEYS */;
/*!40000 ALTER TABLE `sysprep_tags` ENABLE KEYS */;
UNLOCK TABLES;
/*!40103 SET TIME_ZONE=@OLD_TIME_ZONE */;

/*!40101 SET SQL_MODE=@OLD_SQL_MODE */;
/*!40014 SET FOREIGN_KEY_CHECKS=@OLD_FOREIGN_KEY_CHECKS */;
/*!40014 SET UNIQUE_CHECKS=@OLD_UNIQUE_CHECKS */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
/*!40111 SET SQL_NOTES=@OLD_SQL_NOTES */;

-- Dump completed on 2016-09-30 23:10:53
