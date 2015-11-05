-- MySQL dump 10.13  Distrib 5.6.24, for Win64 (x86_64)
--
-- Host: localhost    Database: clonedeploy
-- ------------------------------------------------------
-- Server version	5.7.8-rc-log

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

--
-- Table structure for table `active_imaging_tasks`
--

DROP TABLE IF EXISTS `active_imaging_tasks`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `active_imaging_tasks` (
  `active_task_id` int(11) NOT NULL AUTO_INCREMENT,
  `computer_id` varchar(45) DEFAULT NULL,
  `task_status` varchar(45) DEFAULT NULL,
  `task_queue_position` int(11) DEFAULT NULL,
  `task_elapsed` varchar(45) DEFAULT NULL,
  `task_remaining` varchar(45) DEFAULT NULL,
  `task_completed` varchar(45) DEFAULT NULL,
  `task_rate` varchar(45) DEFAULT NULL,
  `task_partition` varchar(45) DEFAULT NULL,
  `task_arguments` varchar(255) DEFAULT NULL,
  `task_type` varchar(45) DEFAULT NULL,
  `multicast_id` int(11) DEFAULT NULL,
  PRIMARY KEY (`active_task_id`)
) ENGINE=MyISAM AUTO_INCREMENT=36 DEFAULT CHARSET=utf8;
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
  `multicast_session_id` int(11) NOT NULL,
  `multicast_name` varchar(45) DEFAULT NULL,
  `multicast_pid` int(11) DEFAULT NULL,
  `multicast_port` int(11) DEFAULT NULL,
  PRIMARY KEY (`multicast_session_id`)
) ENGINE=MyISAM DEFAULT CHARSET=utf8;
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
) ENGINE=MyISAM AUTO_INCREMENT=53 DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `admin_settings`
--

LOCK TABLES `admin_settings` WRITE;
/*!40000 ALTER TABLE `admin_settings` DISABLE KEYS */;
INSERT INTO `admin_settings` VALUES (34,'On Demand Requires Login','Yes',''),(33,'Web Task Requires Login','No',NULL),(32,'Proxy Efi64 File','grub_64_efi',NULL),(31,'Proxy Efi32 File','ipxe_32_efi',NULL),(30,'Proxy Bios File','pxelinux',NULL),(29,'Proxy Dhcp','Yes',NULL),(28,'Web Server Port','90',NULL),(27,'Image Checksum','Off',NULL),(26,'Server Key Mode','NULL',''),(25,'Global Host Args','',''),(24,'Client Receiver Args','ab',''),(23,'SMB Password','NULL',''),(22,'SMB User Name','abc247',''),(21,'SMB Path','//192.168.56.1',''),(20,'Force SSL','No',NULL),(19,'Nfs Deploy Path','c:\\/',''),(18,'Image Hold Path','c:\\inetpub\\wwwroot\\clonedeploy\\image_hold\\',NULL),(17,'Udpcast End Port','102',''),(16,'Udpcast Start Port','100',''),(15,'Receiver Args','ad',''),(14,'On Demand','Enabled',NULL),(13,'Server IP','192.168.1.101',NULL),(12,'Compression Level','1',''),(11,'Compression Algorithm','gzip',''),(10,'Server Key','e991eb3d-2de9-ae19',NULL),(9,'Default Host View','all',NULL),(8,'Image Transfer Mode','smb',''),(7,'AD Login Domain','',NULL),(6,'Web Path','http://[server-ip]:90/cruciblewds/service/client.asmx/',NULL),(5,'PXE Mode','pxelinux',NULL),(4,'Tftp Path','C:\\inetpub\\wwwroot\\clonedeploy\\tftpboot\\',NULL),(2,'Image Store Path','c:\\inetpub\\wwwroot\\clonedeploy\\image_store\\',NULL),(3,'Nfs Upload Path','c:\\/',''),(51,'Queue Size','3',''),(1,'Sender Args','asdb',''),(35,'Debug Requires Login','Yes',''),(36,'Register Requires Login','Yes',''),(37,'Smtp Server','abc',''),(38,'Smtp Port','',''),(39,'Smtp Username','',''),(40,'Smtp Password','NULL',''),(41,'Smtp Mail From','',''),(42,'Smtp Mail To','',''),(43,'Smtp Ssl','Yes',''),(44,'Notify Successful Login','0',''),(45,'Notify Failed Login','0',''),(46,'Notify Task Started','0',''),(47,'Notify Task Completed','0',''),(48,'Notify Image Approved','0',''),(49,'Notify Resize Failed','0',''),(52,'Require Image Approval','False',NULL);
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
) ENGINE=MyISAM DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `applications`
--

LOCK TABLES `applications` WRITE;
/*!40000 ALTER TABLE `applications` DISABLE KEYS */;
/*!40000 ALTER TABLE `applications` ENABLE KEYS */;
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
) ENGINE=MyISAM AUTO_INCREMENT=4 DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `boot_menu_templates`
--

LOCK TABLES `boot_menu_templates` WRITE;
/*!40000 ALTER TABLE `boot_menu_templates` DISABLE KEYS */;
INSERT INTO `boot_menu_templates` VALUES (3,'abc','ss','asda\r\nd\r\n\r\nad\r\nassdsd');
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
) ENGINE=MyISAM AUTO_INCREMENT=7 DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `buildings`
--

LOCK TABLES `buildings` WRITE;
/*!40000 ALTER TABLE `buildings` DISABLE KEYS */;
INSERT INTO `buildings` VALUES (2,'building2',2),(3,'building2',0),(4,'test',2),(5,'building3',2),(6,'abc',1);
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
) ENGINE=MyISAM DEFAULT CHARSET=utf8;
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
  `user_id` int(11) DEFAULT NULL,
  `group_id` int(11) DEFAULT NULL,
  PRIMARY KEY (`clonedeploy_user_group_mgmt_id`)
) ENGINE=MyISAM AUTO_INCREMENT=9 DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `clonedeploy_user_group_mgmt`
--

LOCK TABLES `clonedeploy_user_group_mgmt` WRITE;
/*!40000 ALTER TABLE `clonedeploy_user_group_mgmt` DISABLE KEYS */;
INSERT INTO `clonedeploy_user_group_mgmt` VALUES (7,4,1),(8,2,1);
/*!40000 ALTER TABLE `clonedeploy_user_group_mgmt` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `clonedeploy_user_image_mgmt`
--

DROP TABLE IF EXISTS `clonedeploy_user_image_mgmt`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `clonedeploy_user_image_mgmt` (
  `clonedeploy_user_image_mgmt_id` int(11) NOT NULL AUTO_INCREMENT,
  `user_id` int(11) DEFAULT NULL,
  `image_id` int(11) DEFAULT NULL,
  PRIMARY KEY (`clonedeploy_user_image_mgmt_id`)
) ENGINE=MyISAM AUTO_INCREMENT=6 DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `clonedeploy_user_image_mgmt`
--

LOCK TABLES `clonedeploy_user_image_mgmt` WRITE;
/*!40000 ALTER TABLE `clonedeploy_user_image_mgmt` DISABLE KEYS */;
INSERT INTO `clonedeploy_user_image_mgmt` VALUES (5,2,3),(4,4,6);
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
  `user_id` int(11) DEFAULT NULL,
  `bad_login_count` int(11) DEFAULT NULL,
  `locked_until_time_utc` datetime DEFAULT NULL,
  PRIMARY KEY (`clonedeploy_user_lockout_id`)
) ENGINE=MyISAM AUTO_INCREMENT=5 DEFAULT CHARSET=utf8;
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
  `user_id` int(11) DEFAULT NULL,
  `user_right` varchar(45) DEFAULT NULL,
  PRIMARY KEY (`clonedeploy_user_right_id`)
) ENGINE=MyISAM AUTO_INCREMENT=61 DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `clonedeploy_user_rights`
--

LOCK TABLES `clonedeploy_user_rights` WRITE;
/*!40000 ALTER TABLE `clonedeploy_user_rights` DISABLE KEYS */;
INSERT INTO `clonedeploy_user_rights` VALUES (60,2,'AdminUpdate'),(59,2,'GlobalUpdate'),(53,2,'ComputerRead'),(54,2,'ImageRead'),(55,2,'GroupCreate'),(56,2,'GroupRead'),(57,2,'GroupDelete'),(58,2,'ProfileUpdate');
/*!40000 ALTER TABLE `clonedeploy_user_rights` ENABLE KEYS */;
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
  `clonedeploy_user_pwd` varchar(45) DEFAULT NULL,
  `clonedeploy_user_salt` varchar(45) DEFAULT NULL,
  `clonedeploy_user_role` varchar(45) DEFAULT NULL,
  PRIMARY KEY (`clonedeploy_user_id`)
) ENGINE=MyISAM AUTO_INCREMENT=5 DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `clonedeploy_users`
--

LOCK TABLES `clonedeploy_users` WRITE;
/*!40000 ALTER TABLE `clonedeploy_users` DISABLE KEYS */;
INSERT INTO `clonedeploy_users` VALUES (1,'clonedeploy','C7C74B503262CAE9982185D3F15F77AE63354700','A+ruRjxCfVfCSy/KZwIIaw==','Administrator'),(2,'test','3A0F18783D0EB520C99F496BD4A429AE4898DA52','Yb5vjY/FzcmCpLcZG8qpwQ==','User'),(3,'abc','1589246206C3576F2D34405DECEAB20526B358D4','JqdjSvBYom3fJn5V1hUT9A==','User'),(4,'gghh','688BB8E2C75D39BFCEA5953A8B09216E62BFCCE5','5nhrEe6Y5FTi6S20j/ksmw==','User');
/*!40000 ALTER TABLE `clonedeploy_users` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `computer_alt_mac_addresses`
--

DROP TABLE IF EXISTS `computer_alt_mac_addresses`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `computer_alt_mac_addresses` (
  `computer_alt_mac_addresses_id` int(11) NOT NULL AUTO_INCREMENT,
  `computer_id` int(11) NOT NULL,
  `mac_address` varchar(45) DEFAULT NULL,
  `computers_computer_id` int(11) NOT NULL,
  PRIMARY KEY (`computer_alt_mac_addresses_id`,`computers_computer_id`)
) ENGINE=MyISAM DEFAULT CHARSET=utf8;
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
  `computer_id` int(11) NOT NULL,
  `application_id` int(11) NOT NULL,
  PRIMARY KEY (`computer_application_id`)
) ENGINE=MyISAM DEFAULT CHARSET=utf8;
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
) ENGINE=MyISAM AUTO_INCREMENT=19 DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `computer_boot_menus`
--

LOCK TABLES `computer_boot_menus` WRITE;
/*!40000 ALTER TABLE `computer_boot_menus` DISABLE KEYS */;
INSERT INTO `computer_boot_menus` VALUES (1,24,'asda\nd\n\nad\nassdsd',NULL,NULL),(2,21,'asda\nd\n\nad\nassdsd',NULL,NULL),(3,29,'asda\nd\n\nad\nassdsd',NULL,NULL),(4,13,'asda\nd\n\nad\nassdsd',NULL,NULL),(5,26,'asda\nd\n\nad\nassdsd',NULL,NULL),(6,16,'asda\nd\n\nad\nassdsd',NULL,NULL),(7,17,'asda\nd\n\nad\nassdsd',NULL,NULL),(8,18,'asda\nd\n\nad\nassdsd',NULL,NULL),(9,19,'asda\nd\n\nad\nassdsd',NULL,NULL),(10,20,'asda\nd\n\nad\nassdsd',NULL,NULL),(11,22,'asda\nd\n\nad\nassdsd',NULL,NULL),(12,25,'asda\nd\n\nad\nassdsd',NULL,NULL),(13,28,'asda\nd\n\nad\nassdsd',NULL,NULL),(14,30,'asda\nd\n\nad\nassdsd',NULL,NULL),(15,31,'asda\nd\n\nad\nassdsd',NULL,NULL),(16,32,'asda\nd\n\nad\nassdsd',NULL,NULL),(17,33,'asda\nd\n\nad\nassdsd',NULL,NULL),(18,34,'asda\nd\n\nad\nassdsd',NULL,NULL);
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
) ENGINE=MyISAM DEFAULT CHARSET=utf8;
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
) ENGINE=MyISAM DEFAULT CHARSET=utf8;
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
) ENGINE=MyISAM DEFAULT CHARSET=utf8;
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
  `computer_id` int(11) DEFAULT NULL,
  `log_type` varchar(45) DEFAULT NULL,
  `log_sub_type` varchar(45) DEFAULT NULL,
  `log_contents` longtext,
  `log_time` timestamp NOT NULL DEFAULT CURRENT_TIMESTAMP,
  PRIMARY KEY (`computer_log_id`)
) ENGINE=MyISAM AUTO_INCREMENT=7 DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `computer_logs`
--

LOCK TABLES `computer_logs` WRITE;
/*!40000 ALTER TABLE `computer_logs` DISABLE KEYS */;
INSERT INTO `computer_logs` VALUES (1,18,'imaging',NULL,'/* Base\n================================================== */\n*{margin:0;padding:0;}\nhtml, body {margin:0;padding:0;height:100%}\nhtml,body,div,span,h1,h2,h3,h4,h5,h6,p{margin:0;padding:0;border:0;font-size:100%;font:inherit;}\nbody{background:#fff;font-size:14px;font-family:opensans, \"Calibri Light\", \"Helvetica Neue\", Helvetica, Arial;color:#444;-webkit-font-smoothing:antialiased;-webkit-text-size-adjust:100%;}\na, a:visited { color: #333; text-decoration: none; outline: 0; }\na:hover, a:focus { color: #000; }\np a, p a:visited { line-height: inherit; }\na img{border: 0;}\nul{ list-style-type: none;}\n\n\n/* Typography\n================================================== */\n@font-face {\n	font-family: \'icomoon\';\n	src:url(\'../fonts/icomoon.eot?-txmuxo\');\n	src:url(\'../fonts/icomoon.eot?#iefix-txmuxo\') format(\'embedded-opentype\'),\n		url(\'../fonts/icomoon.ttf?-txmuxo\') format(\'truetype\'),\n		url(\'../fonts/icomoon.woff?-txmuxo\') format(\'woff\'),\n		url(\'../fonts/icomoon.svg?-txmuxo#icomoon\') format(\'svg\');\n	font-weight: normal;\n	font-style: normal;\n}\n@font-face {\n	font-family: \"Flaticon\";\n	src: url(\"../fonts/flaticon.eot\");\n	src: url(\"../fonts/flaticon.eot#iefix\") format(\"embedded-opentype\"),\n	/*url(\"../fonts/flaticon.woff\") format(\"woff\"),*/\n	url(\"../fonts/flaticon.ttf\") format(\"truetype\"),\n	url(\"../fonts/flaticon.svg\") format(\"svg\");\n	font-weight: normal;\n	font-style: normal;\n}\n@font-face\n{\n    font-family:opensans;\n    src: url(\"../fonts/OpenSans-Light.eot\");\n	src: url(\"../fonts/OpenSans-Light.eot#iefix\") format(\"embedded-opentype\"),\n    url(\"../fonts/OpenSans-Light.ttf\") format(\"truetype\");\n    font-weight:normal;\n    font-style:normal;\n}\nh1,h2,h3,h4,h5,h6{font-family:opensans, Calibri, Arial, sans-serif;color:#47a3da;font-weight:normal;}\nh1 a,h2 a,h3 a,h4 a,h5 a,h6 a{font-weight:inherit;}\nh1{display:block;font-weight:normal;font-size:2em;text-transform:uppercase;letter-spacing:.3em;padding:0 0 .6em;margin-top:0px;font-size:40px;}\nh2{font-size:35px;line-height:40px;margin-bottom:10px;}\nh3{font-size:28px;line-height:34px;margin-bottom:8px;text-align:right;}\nh4{font-size:21px;line-height:30px;margin-bottom:4px;}\nh5{font-size:17px;line-height:24px;}\nh6{font-size:14px;line-height:21px;}\nhr{border:solid #ddd;border-width:1px 0 0;clear:both;margin:10px 0 0px;height:0;}\n\n/* Desktop Menu Left\n================================================== */\n.nav li a{display:block;height:4em;width:200px;line-height:10px;text-align: left;color:#999;position:relative;-webkit-transition:background .1s ease-in-out;-moz-transition:background .1s ease-in-out;transition:background .1s ease-in-out;}\n.nav li a:hover{color:#47a3da;border: none;}\n.nav li:first-child a{color:#47a3da;border: none;background: #ddd;}\n.nav li.nav-current a{background:#fff;color:#47a3da;border-right:none;}\n.nav li a:before{font-family:Flaticon;speak:none;font-style:normal;font-weight:normal;text-indent:0;position:absolute;top:-5px;left:0;width:100%;height:100%;font-size:1em;-webkit-font-smoothing:antialiased;}\n.nav-text{padding-top:25px;display:inline-block; margin-left:40px}\n\n/* Sub Menu Left\n================================================== */\n.sub-nav-top { margin-top: 0px;}\n.sub-nav-top li a{display:block;height:4.5em;width:200px;line-height:20px;text-align: left;background: #fff;color:#666666;position:relative;-webkit-transition:background .1s ease-in-out;-moz-transition:background .1s ease-in-out;transition:background .1s ease-in-out;}\n.sub-nav-top li a:hover{color:#47a3da;background: #fff;border: none;}\n.sub-nav-top li.nav-current a{background:#eee;color:#47a3da;border-right:none;}\n.sub-nav-top li a:before{font-family:Flaticon;speak:none;font-style:normal;font-weight:normal;text-indent:0;position:absolute;left:0;width:100%;height: 100%;-webkit-font-smoothing:antialiased;}\n.sub-nav-text{padding-top:25px;display:inline-block;margin-left: 20px; font-size:18px}\n\n.sub-nav-bottom{ background: #fff; }\n.sub-nav-bottom li a{display:block;height:4em;width:200px;line-height:10px;text-align: left;background: #fff;color:#47a3da;position:relative;-webkit-transition:background .1s ease-in-out;-moz-transition:background .1s ease-in-out;transition:background .1s ease-in-out;}\n.sub-nav-bottom li a:hover{color:#fff;background: #666;border: none;}\n.sub-nav-bottom li.nav-current a{background:#fff;color:#47a3da;border-right:none;}\n.sub-nav-bottom li a:before{font-family:Flaticon;speak:none;font-style:normal;font-weight:normal;text-indent:0;position:absolute;left:0;width:100%;height:100%;-webkit-font-smoothing:antialiased;}\n.sub-nav-text2{padding-top:25px;display:inline-block;margin-left: 40px; }\n.indent{ margin-left: 5px; }\n\n/* Mobile Menu Top\n================================================== */\n.nav-bar-mobile {display:none;position: fixed;overflow-y: hidden;top: 0;left: 0;height: 30px;width:100%;list-style-type: none;margin: 0;padding: 0;background: #47a3da;z-index:1001;}\n.mobile-menu-left{float:left;width:50%;}\n.mobile-menu-right{float:right;width:50%;}\n.nav-menu-mobile{background:#47a3da;position:fixed;width:100%;height:300px;left:0;z-index:1000;overflow:hidden;top:-300px;}\n.nav-menu-mobile a{float:left;width:100%;padding:20px 0;text-align:center;font-size:2em;}\n.nav-menu-mobile a:hover{background:#258ecd;}\n.nav-menu-mobile a:active{background:#afdefa;color:#47a3da;}\n.nav-menu-mobile.nav-menu-mobile-open{top:30px;}\n.nav-menu-mobile-push{overflow-x:hidden;position:relative;left:0;}\n.nav-menu-mobile,.nav-menu-mobile-push{-webkit-transition:all .3s ease;-moz-transition:all .3s ease;transition:all .3s ease;}\n\n/* Sub Menus\n================================================== */\n.nav-btn-round{float:right;margin-top:20px;}\n.nav-btn-square{float:left;background: #666;margin-left: 0px;width: 100%;margin-bottom:-10px;padding-bottom:0px;padding-right:0;}\n.nav-btn-round a{display:block;float:left;position:relative;width:2.5em;height:2.5em;background:#fff;border-radius:50%;color:transparent;margin:0 .5em;border:4px solid #47a3da;}\n.nav-btn-square a{display:block;color: white;float:left;position:relative;width:150px;height:35px;background:#666666;margin:0 5px 0 0;font-size: 18px;font-weight: 500;text-align: center;padding-top:5px;}\n.nav-btn-round a:after{content:attr(data-info);color:#47a3da;position:absolute;width:600%;top:120%;text-align:right;right:0;opacity:0;pointer-events:none;}\n\n.nav-btn-round a:hover:after,.nav-btn-square a:hover:after{opacity:1;}\n.nav-btn-round a:hover,.nav-btn-square a:hover{background:#47a3da;}\n.nav-btn-round a:hover:before,.nav-btn-square a:hover:before{color:#fff;}\n.nav-btn-square h3{text-align:left;margin-left:8px;color:#47a3da;padding-bottom:5px;}\n.nav-btn-round a.nav-current, .nav-btn-square a.nav-current{background:#47a3da;}\n.nav-btn-round a.nav-current:before, .nav-btn-square a.nav-current:before{color:#fff;}\n.nav-btn-square-large a{ width: 7em;height: 7em;}\n.icon:before{font-family:Flaticon;position:absolute;top:0;width:100%;height:100%;speak:none;font-size:2em;font-style:normal;font-weight:normal;line-height:1.3;text-align:center;color:#47a3da;-webkit-font-smoothing:antialiased;}\n.icon2:before{font-family:icomoon;position:absolute;top:0;width:100%;height:100%;speak:none;font-size:2em;font-style:normal;font-weight:normal;line-height:1.3;text-align:center;color:#47a3da;-webkit-font-smoothing:antialiased;}\n.sub-nav-title{ color: #47a3da;font-weight: 500;font-size: 16px;margin-bottom: 10px;margin-left: 5px;text-decoration: underline;}\n\n/* Admin Chooser\n================================================== */\n.nav-btn-square-large a{ width: 4em;height: 4em;text-align: center;margin-top: 5px;margin-left: 20px;}\n.nav-btn-square-large{ padding-bottom: 40px;margin-left: 30px;margin-top: 5px;padding-right: 5px;}\n.chooser-text {padding-top:60px;display:inline-block;color: #444;font-size: 14px;}\n.margin-top-min20{ margin-top: -20px;}\n.no-margin{ margin: 0;}\n\n\n/* Confirm Slide In Top\n================================================== */\n.confirm-box-inner h4{color:#fff;text-align:center;margin-bottom: 30px;}\n.confirm-box-inner{margin:30px auto;width:600px;}\n.confirm-box-btns{width:350px;margin:0 auto;text-align:center;}\n.confirm-box-outer{background:#666666;position:fixed;color:#fff;top:-200px;width:100%;height:200px;left:0;z-index:1999;overflow:hidden;}\n.confirm-box-outer.confirm-box-outer-open{top:0;}\n.confirm-box-outer-push{overflow-x:hidden;position:relative;left:0;}\n.confirm-box-outer,.confirm-box-outer-push{-webkit-transition:all .3s ease;-moz-transition:all .3s ease;transition:all .3s ease;}\n\n/* Gridviews\n================================================== */\ntable{margin-bottom:30px;margin-top:0px;}\nTable.Gridview{border:none;white-space:nowrap; width:100%;}\n.GridviewTable{border:none;}\n.Gridview th{text-align:left;color:White;border:none;background-color:#47a3da;padding: 3px 0 3px 10px;}\n.Gridview th a:link,a:visited{color:White;text-decoration:none;}\n.Gridview td{border:none;padding: 10px 1em 10px 10px;}\n.Gridview tr{color:#666;background-color:white;text-align:left;border-bottom:1px solid #f5f5f5;}\n.Gridview .alt{background:#f8f8f8;}\n.Gridview td a:link, .GridView td a:visited {color:white;text-decoration:none;background:#666666;-webkit-border-radius:5px;-moz-border-radius:3px;-ms-border-radius:3px;-o-border-radius:3px;border-radius:3px;padding:8px 12px;}\n.Gridview a:visited {background:#666666; }\n\nTable.log{white-space:normal; word-wrap:break-word;}\n.log tr {border:none; }\n.log td {padding:2px; }\n\nTable.gv-confirm { width:600px; }\nTable.gv_members{margin-left:15px;border-left:2px solid #e8e8e8;border-bottom:2px solid #e8e8e8; margin-top:-5px; width:780px;}\nTable.gv_parts{margin-left:15px;border-left:2px solid #e8e8e8;border-bottom:2px solid #e8e8e8; margin-top:-5px; width:1000px; padding:0;}\n.gv_parts th {background-color:#eee; color:#47a3da;}\n\nTable.gv_vg{margin-left:30px;border-left:2px solid #e8e8e8;border-bottom:2px solid #e8e8e8; margin-top:-5px; width:1000px; padding:0;}\n.gv_vg th {background-color:#666; color:#fff;}\n\n.hdlist td{padding:0;}\n\n.width_300{width:300px;}\n.width_200{width:200px;}\n.width_150{width:150px;}\n.width_105{width:105px;}\n.width_50{width:50px;}\n.width_30{width:20px;}\n\n.height_800{ height: 800px;}\n.height_600{ height: 600px;}\n.height_400{ height: 400px;}\n\n/* #Forms\n================================================== */\n.settings input[type=\"text\"]{width:390px;}\ninput[type=\"text\"],input[type=\"password\"],textarea,select{border:1px solid #ccc;padding:6px 4px;outline:none;-moz-border-radius:2px;-webkit-border-radius:2px;border-radius:2px;font-family:opensans, \"Calibri Light\", \"Helvetica Neue\", Helvetica, Arial;font-size:20px;color:#777;margin:0;width:97%;max-width:97%;display:block;margin-bottom:10px;background:#fff;}\n#settings input[type=\"text\"],input[type=\"password\"],textarea,select{font-size:14px; }\nselect{padding:0;}\ninput[type=\"text\"]:focus,input[type=\"password\"]:focus,textarea:focus{border:1px solid #aaa;color:#444;-moz-box-shadow:0 0 3px rgba(0,0,0,.2);-webkit-box-shadow:0 0 3px rgba(0,0,0,.2);box-shadow:0 0 3px rgba(0,0,0,.2);}\ntextarea{min-height:60px;}\nselect{width:100%;}\ninput[type=\"checkbox\"]{display:inline;}\n.searchbox {height:35px; }\n.textbox,.textboxsettings{height:25px;}\n.textbox_specs{height:16px;}\n.ddlist{height:35px;width:100%;font-size:20px;}\n#settings .ddlist{height:35px;width:100%;font-size:14px;}\n.descbox{height:100px;}\n.descboxboot{height:800px;}\n.sysprepcontent{ height: 300px;}\n.chkboxwidth{width:20px;}\n.small-boot-box{height:200px; }\n.txt-left{ text-align: left;}\n.small-text{ font-size: 12px !important}\n\n/* Buttons\n================================================== */\na.confirm,a.submits,input[type=\"submit\"]{background:#fff;border:1px solid #aaa;border-top:1px solid #ccc;border-left:1px solid #ccc;-moz-border-radius:3px;-webkit-border-radius:3px;border-radius:3px;color:#444;display:inline-block;text-decoration:none;text-shadow:0 1px rgba(255,255,255,.75);cursor:pointer;margin-bottom:3px;line-height:normal;padding:10px 10px; font-size:16px;}\na.confirm:hover,a.submits:hover,input[type=\"submit\"]:hover,a.boot-active{color:#fff;background:#47a3da;-webkit-transition:background .2s ease-in-out;-moz-transition:background .2s ease-in-out;transition:background .2s ease-in-out;border:1px solid #47a3da;text-shadow:none;}\n\na.confirm:active,a.submits:active,input[type=\"submit\"]:active{border:1px solid #47a3da;background:#ccc;}\na.confirm{float:right; margin-top:20px;}\na.confirm_yes{width:100px;display:inline-block;background-color:#fff;padding:15px 30px;color:#47a3da;-webkit-border-radius:5px;-moz-border-radius:5px;-ms-border-radius:5px;-o-border-radius:5px;border-radius:5px;text-decoration:none; }\na.confirm_no{width:100px;display:inline-block;background-color:#fff;padding:15px 30px;color:#47a3da;-webkit-border-radius:5px;-moz-border-radius:5px;-ms-border-radius:5px;-o-border-radius:5px;border-radius:5px;text-decoration:none; }\na.confirm_yes:link,a.confirm_yes:visited,a.confirm_no:link,a.confirm_no:visited{color:#47a3da;outline:none;font:normal 1em/1em Calibri , Arial;font-size:24px;}\na.submits{margin-top:20px;float: right;}\na.left {float:left; margin-top:0;}\na.right { float: right;}\na.static-width{ width:100px; text-align:center; margin-right:5px; margin-top:0; }\na.static-width-nomarg{ width:100px; text-align:center; margin-right:5px; }\n\n/* Toast Notifications\n================================================== */\n.toast-container{width:100%;z-index:9999;}\n.toast-container{position:absolute;}\n.toast-item{height:100%;padding: 0 6px 20px 6px;display:block;position:relative;margin:0;}\n.toast-item p{text-align:center;font-size:18px; color:#fff;}\n.toast-item-close{width:100%;height:100%; position:absolute;}\n.toast-type-success{color:#fff;background:#666666;}\n.toast-position-top{position:fixed;top:0;}\n\n/* Misc\n================================================== */\n.header-title { text-align: left;margin-left:15px;}\n.header-title-text{ margin-top: 20px;font-size: 30px;vertical-align: bottom;}\n.denied_text { font-size:20px; color:Red; }\n.total {color:#666; font-size:16px; text-align:right; line-height:1px;margin-bottom: 10px; }\n.container:after{content:\"\\0020\";display:block;height:0;clear:both;visibility:hidden;}\n.clearfix:before,.clearfix:after,.row:before,.row:after{content:\'\\0020\';display:block;overflow:hidden;visibility:hidden;width:0;height:0;}\n.row:after,.clearfix:after{clear:both;}\n.row,.clearfix{zoom:1;}\n.clear{clear:both;display:block;overflow:hidden;visibility:hidden;width:0;height:0;}\n.settingsh2{margin-bottom:20px;}\n.editor {\n    margin: 0;\n    position: relative;\n    top: 0;\n    bottom: 0;\n    left: 0;\n    width: 100%;\n    right: 0;\n}\n\n\n\n','2015-10-12 17:31:09'),(3,18,'image','upload','sdfasdf','2015-10-12 22:18:14'),(4,18,'image','upload','my latest log','2015-10-12 22:20:35'),(5,18,'image','upload','my latest log','2015-10-12 18:22:22'),(6,18,'image','upload','my latest log','2015-10-12 18:22:46');
/*!40000 ALTER TABLE `computer_logs` ENABLE KEYS */;
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
) ENGINE=MyISAM DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `computer_printers`
--

LOCK TABLES `computer_printers` WRITE;
/*!40000 ALTER TABLE `computer_printers` DISABLE KEYS */;
/*!40000 ALTER TABLE `computer_printers` ENABLE KEYS */;
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
) ENGINE=MyISAM DEFAULT CHARSET=utf8;
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
  PRIMARY KEY (`computer_id`)
) ENGINE=MyISAM AUTO_INCREMENT=35 DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `computers`
--

LOCK TABLES `computers` WRITE;
/*!40000 ALTER TABLE `computers` DISABLE KEYS */;
INSERT INTO `computers` VALUES (13,'abcd','1','added via smart',-1,-1,-1,8,8,0,'','','','',''),(16,'zzzzz','1112223333','added via smart',-1,0,0,6,6,1,NULL,NULL,NULL,NULL,NULL),(17,'cccc','4455666','added via smart',-1,0,0,6,6,1,NULL,NULL,NULL,NULL,NULL),(18,'bbbbb','444343','added via smart',-1,0,0,6,6,1,NULL,NULL,NULL,NULL,NULL),(19,'ddeeff','56789','added via smart',-1,0,0,6,6,1,NULL,NULL,NULL,NULL,NULL),(20,'bbssese','123','added via smart',-1,0,0,6,6,1,NULL,NULL,NULL,NULL,NULL),(21,'asdfer','99403949','added via smart',-1,-1,-1,6,6,1,NULL,NULL,NULL,NULL,NULL),(22,'8589383984','949039034','added via smart',-1,0,0,6,6,1,NULL,NULL,NULL,NULL,NULL),(25,'abcd','','',-1,0,0,0,0,0,NULL,NULL,NULL,NULL,NULL),(26,'zzddd','4444646','added via smart',-1,-1,4,6,6,1,'abc','','','',''),(28,'newcomptuer1','89402JSJDF','added via smart',-1,0,0,6,6,1,NULL,NULL,NULL,NULL,NULL),(29,'ss','12','added via smart',0,0,4,6,6,1,NULL,NULL,NULL,NULL,NULL),(30,'sitetest','94943','added via smart',13,2,4,6,6,1,NULL,NULL,NULL,NULL,NULL),(31,'myhostskks','94903U094U','added via smart',-1,-1,-1,6,6,1,NULL,NULL,NULL,NULL,NULL),(32,'newpc','8580298343','added via smart',-1,-1,-1,6,6,1,'5','4','0','2','1'),(33,'new17','5002048','added via smart',-1,-1,-1,6,6,1,'','','','',''),(34,'affeetta','645565','added via smart',-1,-1,-1,6,6,1,'','','','','');
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
  `distribution_point_username` varchar(45) DEFAULT NULL,
  `distribution_point_password` varchar(45) DEFAULT NULL,
  `distribution_point_is_primary` tinyint(4) DEFAULT NULL,
  `distribution_point_physical_path` varchar(255) DEFAULT NULL,
  `distribution_point_is_backend` tinyint(4) DEFAULT NULL,
  `distribution_point_backend_server` varchar(45) DEFAULT NULL,
  PRIMARY KEY (`distribution_point_id`)
) ENGINE=MyISAM AUTO_INCREMENT=3 DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `distribution_points`
--

LOCK TABLES `distribution_points` WRITE;
/*!40000 ALTER TABLE `distribution_points` DISABLE KEYS */;
INSERT INTO `distribution_points` VALUES (2,'asdfsdf','[server-ip]','SMB','asdfsdaf','dom','adsf','aaa',1,'C:\\inetpub\\wwwroot\\clonedeploy\\distribution\\images\\',0,'');
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
  `copy_folder_contents_only` tinyint(4) DEFAULT '0',
  PRIMARY KEY (`file_folder_id`)
) ENGINE=InnoDB AUTO_INCREMENT=3 DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `files_folders`
--

LOCK TABLES `files_folders` WRITE;
/*!40000 ALTER TABLE `files_folders` DISABLE KEYS */;
INSERT INTO `files_folders` VALUES (2,'file1','abc/test.gz','File',0);
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
  `group_id` int(11) DEFAULT NULL,
  `bios_menu` longtext,
  `efi32_menu` longtext,
  `efi64_menu` longtext,
  PRIMARY KEY (`group_boot_menu_id`)
) ENGINE=MyISAM AUTO_INCREMENT=3 DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `group_boot_menus`
--

LOCK TABLES `group_boot_menus` WRITE;
/*!40000 ALTER TABLE `group_boot_menus` DISABLE KEYS */;
INSERT INTO `group_boot_menus` VALUES (1,1,'new group menu',NULL,NULL),(2,3,'asda\nd\n\nad\nassdsd',NULL,NULL);
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
  `group_id` int(11) DEFAULT NULL,
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
  PRIMARY KEY (`group_computer_property_id`)
) ENGINE=MyISAM AUTO_INCREMENT=3 DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `group_computer_properties`
--

LOCK TABLES `group_computer_properties` WRITE;
/*!40000 ALTER TABLE `group_computer_properties` DISABLE KEYS */;
INSERT INTO `group_computer_properties` VALUES (1,1,6,-1,'',-1,-1,-1,'','','','','',1,1,0,1,1,1,0,0,0,0,0),(2,3,6,-1,'added via smart',-1,-1,-1,'','','','','',1,1,1,0,0,0,0,0,0,0,0);
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
  `computer_id` int(11) DEFAULT NULL,
  `group_id` int(11) DEFAULT NULL,
  PRIMARY KEY (`group_membership_id`)
) ENGINE=MyISAM AUTO_INCREMENT=37 DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `group_membership`
--

LOCK TABLES `group_membership` WRITE;
/*!40000 ALTER TABLE `group_membership` DISABLE KEYS */;
INSERT INTO `group_membership` VALUES (22,29,3),(9,12,1),(10,24,1),(23,13,3),(19,21,1),(21,21,3),(20,26,3),(24,16,3),(25,17,3),(26,18,3),(27,19,3),(28,20,3),(29,22,3),(30,25,3),(31,28,3),(32,30,3),(33,31,3),(34,32,3),(35,33,3),(36,34,3);
/*!40000 ALTER TABLE `group_membership` ENABLE KEYS */;
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
  `group_sender_arguments` longtext,
  `group_receiver_arguments` longtext,
  `group_smart_criteria` varchar(45) DEFAULT NULL,
  `group_default_properties_enabled` tinyint(4) DEFAULT '0',
  `group_default_bootmenu_enabled` tinyint(4) DEFAULT '0',
  PRIMARY KEY (`group_id`)
) ENGINE=MyISAM AUTO_INCREMENT=4 DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `groups`
--

LOCK TABLES `groups` WRITE;
/*!40000 ALTER TABLE `groups` DISABLE KEYS */;
INSERT INTO `groups` VALUES (1,'Group1_Test26','',3,-1,'standard','a','b',NULL,1,1),(2,'Group1','',-1,-1,'standard','',NULL,NULL,0,0),(3,'smart1','',0,0,'smart',NULL,NULL,'',0,0);
/*!40000 ALTER TABLE `groups` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `image_profile_files_folders`
--

DROP TABLE IF EXISTS `image_profile_files_folders`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `image_profile_files_folders` (
  `image_profile_files_folders_id` int(11) NOT NULL AUTO_INCREMENT,
  `image_profile_id` int(11) DEFAULT NULL,
  `file_folder_id` int(11) DEFAULT NULL,
  `priority` int(11) DEFAULT '0',
  PRIMARY KEY (`image_profile_files_folders_id`)
) ENGINE=MyISAM AUTO_INCREMENT=2 DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `image_profile_files_folders`
--

LOCK TABLES `image_profile_files_folders` WRITE;
/*!40000 ALTER TABLE `image_profile_files_folders` DISABLE KEYS */;
INSERT INTO `image_profile_files_folders` VALUES (1,8,2,1);
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
  `image_profile_id` int(11) DEFAULT NULL,
  `partition_layout_id` int(11) DEFAULT NULL,
  PRIMARY KEY (`image_profile_partition_layout_id`)
) ENGINE=MyISAM AUTO_INCREMENT=15 DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `image_profile_partition_layouts`
--

LOCK TABLES `image_profile_partition_layouts` WRITE;
/*!40000 ALTER TABLE `image_profile_partition_layouts` DISABLE KEYS */;
INSERT INTO `image_profile_partition_layouts` VALUES (14,7,2),(13,1,1);
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
  `image_profile_id` int(11) DEFAULT NULL,
  `script_id` int(11) DEFAULT NULL,
  `run_pre` tinyint(4) DEFAULT NULL,
  `run_post` tinyint(4) DEFAULT NULL,
  `priority` int(11) DEFAULT '0',
  PRIMARY KEY (`image_profile_script_id`)
) ENGINE=MyISAM AUTO_INCREMENT=28 DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `image_profile_scripts`
--

LOCK TABLES `image_profile_scripts` WRITE;
/*!40000 ALTER TABLE `image_profile_scripts` DISABLE KEYS */;
INSERT INTO `image_profile_scripts` VALUES (10,1,2,0,1,NULL),(9,1,4,1,0,NULL),(8,1,3,1,0,NULL),(11,1,5,0,1,NULL),(19,5,5,1,0,NULL),(18,5,2,0,1,NULL),(17,5,4,0,1,NULL),(16,5,3,1,0,NULL),(23,7,4,1,1,NULL),(22,7,3,1,1,NULL),(24,7,5,1,0,NULL),(26,8,3,0,1,9),(27,8,2,1,0,1);
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
  `image_profile_id` int(11) DEFAULT NULL,
  `sysprep_tag_id` int(11) DEFAULT NULL,
  `priority` int(11) DEFAULT '0',
  PRIMARY KEY (`image_profile_sysprep_tag_id`)
) ENGINE=MyISAM AUTO_INCREMENT=3 DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `image_profile_sysprep_tags`
--

LOCK TABLES `image_profile_sysprep_tags` WRITE;
/*!40000 ALTER TABLE `image_profile_sysprep_tags` DISABLE KEYS */;
INSERT INTO `image_profile_sysprep_tags` VALUES (2,8,2,0);
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
  `image_id` int(11) DEFAULT NULL,
  `profile_kernel` varchar(45) DEFAULT NULL,
  `profile_boot_image` varchar(45) DEFAULT NULL,
  `profile_name` varchar(45) DEFAULT NULL,
  `profile_description` longtext,
  `profile_kernel_arguments` longtext,
  `skip_core_download` tinyint(4) DEFAULT NULL,
  `skip_set_clock` tinyint(4) DEFAULT NULL,
  `task_completed_action` varchar(45) DEFAULT NULL,
  `remove_gpt_structures` tinyint(4) DEFAULT NULL,
  `skip_volume_shrink` tinyint(4) DEFAULT NULL,
  `skip_lvm_shrink` tinyint(4) DEFAULT NULL,
  `skip_volume_expand` tinyint(4) DEFAULT NULL,
  `fix_bcd` tinyint(4) DEFAULT NULL,
  `fix_bootloader` tinyint(4) DEFAULT NULL,
  `partition_method` varchar(45) DEFAULT NULL,
  `force_dynamic_partitions` tinyint(4) DEFAULT NULL,
  `custom_partition_script` longtext,
  `compression_algorithm` varchar(45) DEFAULT NULL,
  `compression_level` varchar(45) DEFAULT NULL,
  `custom_image_schema` longtext,
  `custom_upload_schema` longtext,
  `upload_schema_only` tinyint(4) DEFAULT '0',
  PRIMARY KEY (`image_profile_id`)
) ENGINE=MyISAM AUTO_INCREMENT=9 DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `image_profiles`
--

LOCK TABLES `image_profiles` WRITE;
/*!40000 ALTER TABLE `image_profiles` DISABLE KEYS */;
INSERT INTO `image_profiles` VALUES (5,3,'3.12.13-WDS','initrd.gz','profile23','','',0,0,'Reboot',0,1,0,0,0,1,NULL,0,NULL,NULL,NULL,NULL,NULL,NULL),(6,3,NULL,NULL,'myprofilea','',NULL,0,0,NULL,0,0,0,0,0,0,NULL,0,NULL,NULL,NULL,NULL,NULL,NULL),(7,3,NULL,NULL,'adsf','',NULL,0,0,NULL,0,0,0,0,0,0,'Custom Layout',0,'',NULL,NULL,NULL,NULL,NULL),(8,8,NULL,NULL,'jj','',NULL,0,0,NULL,0,0,0,1,1,1,'Dynamic',1,'','gzip','1',NULL,NULL,0);
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
  `image_name` varchar(45) DEFAULT NULL,
  `image_os` varchar(45) DEFAULT NULL,
  `image_description` longtext,
  `image_is_protected` tinyint(1) DEFAULT NULL,
  `image_is_viewable_ond` tinyint(1) DEFAULT NULL,
  `image_checksum` text,
  `image_type` varchar(45) DEFAULT NULL,
  `image_environment` varchar(45) DEFAULT NULL,
  `image_approved` tinyint(4) DEFAULT '0',
  PRIMARY KEY (`image_id`)
) ENGINE=MyISAM AUTO_INCREMENT=9 DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `images`
--

LOCK TABLES `images` WRITE;
/*!40000 ALTER TABLE `images` DISABLE KEYS */;
INSERT INTO `images` VALUES (8,'image1','Windows','',0,1,NULL,NULL,NULL,0);
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
) ENGINE=MyISAM AUTO_INCREMENT=8 DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `multicast_ports`
--

LOCK TABLES `multicast_ports` WRITE;
/*!40000 ALTER TABLE `multicast_ports` DISABLE KEYS */;
INSERT INTO `multicast_ports` VALUES (1,8998),(2,63998),(3,98),(4,98),(5,100),(6,102),(7,104);
/*!40000 ALTER TABLE `multicast_ports` ENABLE KEYS */;
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
) ENGINE=MyISAM AUTO_INCREMENT=3 DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `partition_layouts`
--

LOCK TABLES `partition_layouts` WRITE;
/*!40000 ALTER TABLE `partition_layouts` DISABLE KEYS */;
INSERT INTO `partition_layouts` VALUES (2,'abc','MBR','Windows',1);
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
) ENGINE=MyISAM AUTO_INCREMENT=25 DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `partitions`
--

LOCK TABLES `partitions` WRITE;
/*!40000 ALTER TABLE `partitions` DISABLE KEYS */;
INSERT INTO `partitions` VALUES (20,0,1,'Primary','ntfs',100,'Percent',1),(21,2,7,'Logical','ntfs',100,'MB',0),(23,1,1,'Primary','ntfs',8,'MB',1),(24,2,1,'Primary','ntfs',5,'GB',0);
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
  `room_distribution_point` int(11) DEFAULT NULL,
  PRIMARY KEY (`room_id`)
) ENGINE=MyISAM AUTO_INCREMENT=6 DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `rooms`
--

LOCK TABLES `rooms` WRITE;
/*!40000 ALTER TABLE `rooms` DISABLE KEYS */;
INSERT INTO `rooms` VALUES (5,'lab-22178',2);
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
) ENGINE=MyISAM AUTO_INCREMENT=6 DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `scripts`
--

LOCK TABLES `scripts` WRITE;
/*!40000 ALTER TABLE `scripts` DISABLE KEYS */;
INSERT INTO `scripts` VALUES (3,'Hello','','aadsfasdf'),(2,'newscriptz','hello','#!/bin/bash\nhello'),(4,'Hello2','aadsf','adsfadsf');
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
  `site_distribution_point` int(11) DEFAULT NULL,
  PRIMARY KEY (`site_id`)
) ENGINE=MyISAM AUTO_INCREMENT=15 DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `sites`
--

LOCK TABLES `sites` WRITE;
/*!40000 ALTER TABLE `sites` DISABLE KEYS */;
INSERT INTO `sites` VALUES (9,'teset2',2),(13,'test1',1),(14,'test4',2);
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
) ENGINE=MyISAM AUTO_INCREMENT=3 DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `sysprep_tags`
--

LOCK TABLES `sysprep_tags` WRITE;
/*!40000 ALTER TABLE `sysprep_tags` DISABLE KEYS */;
INSERT INTO `sysprep_tags` VALUES (1,'newtags','<hostname>','</hostname>','adfefsdfa\r\nsd\r\nfasd\r\nf\r\ndf','[host-name]'),(2,'asdf','adsf','adf','adsf','adsf');
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

-- Dump completed on 2015-11-05 16:30:16
