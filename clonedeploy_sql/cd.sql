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
) ENGINE=MyISAM AUTO_INCREMENT=25 DEFAULT CHARSET=utf8;
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
) ENGINE=MyISAM AUTO_INCREMENT=52 DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `admin_settings`
--

LOCK TABLES `admin_settings` WRITE;
/*!40000 ALTER TABLE `admin_settings` DISABLE KEYS */;
INSERT INTO `admin_settings` VALUES (34,'On Demand Requires Login','Yes',''),(33,'Web Task Requires Login','No',''),(32,'Proxy Efi64 File','ipxe_64_efi_snp',NULL),(31,'Proxy Efi32 File','ipxe_32_efi_snp',NULL),(30,'Proxy Bios File','ipxe',NULL),(29,'Proxy Dhcp','Yes',NULL),(28,'Web Server Port','90',NULL),(27,'Image Checksum','On',''),(26,'Server Key Mode','NULL',''),(25,'Global Host Args','',''),(24,'Client Receiver Args','ab',''),(23,'SMB Password','NULL',''),(22,'SMB User Name','abc247',''),(21,'SMB Path','//192.168.56.1',''),(20,'Force SSL','No',''),(19,'Nfs Deploy Path','c:\\/',''),(18,'Image Hold Path','c:\\inetpub\\wwwroot\\clonedeploy\\image_hold\\',NULL),(17,'Udpcast End Port','102',''),(16,'Udpcast Start Port','100',''),(15,'Receiver Args','ad',''),(14,'On Demand','Enabled',''),(13,'Server IP','192.168.1.101',NULL),(12,'Compression Level','1',''),(11,'Compression Algorithm','gzip',''),(10,'Server Key','e991eb3d-2de9-ae19',''),(9,'Default Host View','all',NULL),(8,'Image Transfer Mode','smb',''),(7,'AD Login Domain','',''),(6,'Web Path','http://[server-ip]:90/cruciblewds/service/client.asmx/',NULL),(5,'PXE Mode','pxelinux',NULL),(4,'Tftp Path','C:\\inetpub\\wwwroot\\clonedeploy\\tftpboot\\',NULL),(2,'Image Store Path','c:\\inetpub\\wwwroot\\clonedeploy\\image_store\\',NULL),(3,'Nfs Upload Path','c:\\/',''),(51,'Queue Size','3',''),(1,'Sender Args','asdb',''),(35,'Debug Requires Login','Yes',''),(36,'Register Requires Login','Yes',''),(37,'Smtp Server','abc',''),(38,'Smtp Port','',''),(39,'Smtp Username','',''),(40,'Smtp Password','NULL',''),(41,'Smtp Mail From','',''),(42,'Smtp Mail To','',''),(43,'Smtp Ssl','Yes',''),(44,'Notify Successful Login','0',''),(45,'Notify Failed Login','1',''),(46,'Notify Task Started','0',''),(47,'Notify Task Completed','1',''),(48,'Notify Image Approved','0',''),(49,'Notify Resize Failed','0','');
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
-- Table structure for table `categories`
--

DROP TABLE IF EXISTS `categories`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `categories` (
  `category_id` int(11) NOT NULL AUTO_INCREMENT,
  `category_name` varchar(45) DEFAULT NULL,
  `category_priority` int(11) DEFAULT NULL,
  PRIMARY KEY (`category_id`)
) ENGINE=MyISAM DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `categories`
--

LOCK TABLES `categories` WRITE;
/*!40000 ALTER TABLE `categories` DISABLE KEYS */;
/*!40000 ALTER TABLE `categories` ENABLE KEYS */;
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
) ENGINE=MyISAM AUTO_INCREMENT=3 DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `clonedeploy_users`
--

LOCK TABLES `clonedeploy_users` WRITE;
/*!40000 ALTER TABLE `clonedeploy_users` DISABLE KEYS */;
INSERT INTO `clonedeploy_users` VALUES (1,'clonedeploy','C7C74B503262CAE9982185D3F15F77AE63354700','A+ruRjxCfVfCSy/KZwIIaw==','Administrator'),(2,'test','5F71626C61E6C1492AAE7371F630F4F00D89B05B','frKMAFhwbP08/eYzCP9/lg==','Administrator');
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
  `computer_applications_id` int(11) NOT NULL AUTO_INCREMENT,
  `computer_id` int(11) NOT NULL,
  `application_id` int(11) NOT NULL,
  `computers_computer_id` int(11) NOT NULL,
  `applications_application_id` int(11) NOT NULL,
  PRIMARY KEY (`computer_applications_id`,`computers_computer_id`,`applications_application_id`)
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
) ENGINE=MyISAM DEFAULT CHARSET=utf8;
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
  `computer_harddrives_id` int(11) NOT NULL AUTO_INCREMENT,
  `computer_id` int(11) NOT NULL,
  `model` varchar(255) DEFAULT NULL,
  `serial_number` varchar(255) DEFAULT NULL,
  `capacity` varchar(45) DEFAULT NULL,
  `smart_status` varchar(45) DEFAULT NULL,
  `computers_computer_id` int(11) NOT NULL,
  PRIMARY KEY (`computer_harddrives_id`,`computers_computer_id`)
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
  `last_inventory_update` bigint(32) DEFAULT NULL,
  `last_checkin` bigint(32) DEFAULT NULL,
  `last_enrollment` bigint(32) DEFAULT NULL,
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
  `computer_logins_id` int(11) NOT NULL AUTO_INCREMENT,
  `computer_id` int(11) NOT NULL,
  `computer_user_id` int(11) NOT NULL,
  `login_time` bigint(32) NOT NULL,
  `logout_time` bigint(32) NOT NULL,
  `computers_computer_id` int(11) NOT NULL,
  `computer_users_computer_users_id` int(11) NOT NULL,
  PRIMARY KEY (`computer_logins_id`,`computers_computer_id`,`computer_users_computer_users_id`)
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
-- Table structure for table `computer_printers`
--

DROP TABLE IF EXISTS `computer_printers`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `computer_printers` (
  `computer_printers_id` int(11) NOT NULL AUTO_INCREMENT,
  `computer_id` int(11) NOT NULL,
  `printer_name` varchar(255) DEFAULT NULL,
  `printer_model` varchar(255) DEFAULT NULL,
  `printer_uri` varchar(255) DEFAULT NULL,
  `computer_printerscol` varchar(45) DEFAULT NULL,
  `computers_computer_id` int(11) NOT NULL,
  PRIMARY KEY (`computer_printers_id`,`computers_computer_id`)
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
  `computer_users_id` int(11) NOT NULL AUTO_INCREMENT,
  `computer_users_username` varchar(45) NOT NULL,
  PRIMARY KEY (`computer_users_id`)
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
  PRIMARY KEY (`computer_id`)
) ENGINE=MyISAM AUTO_INCREMENT=31 DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `computers`
--

LOCK TABLES `computers` WRITE;
/*!40000 ALTER TABLE `computers` DISABLE KEYS */;
INSERT INTO `computers` VALUES (24,'mynewhost','84020298484','',-1,0,0,3,6,0),(12,'test987','11111111111111111','',-1,0,0,3,7,0),(13,'abcd','1','',-1,0,0,0,-1,0),(16,'zzzzz','1112223333','',-1,0,0,0,0,0),(17,'cccc','4455666','',-1,0,0,0,0,0),(18,'bbbbb','444343','',-1,0,0,0,0,0),(19,'ddeeff','56789','',-1,0,0,0,0,0),(20,'bbssese','123','',-1,0,0,0,0,0),(21,'asdfer','99403949','',-1,0,0,0,0,0),(22,'8589383984','949039034','',-1,0,0,0,0,0),(25,'abcd','','',-1,0,0,0,0,0),(26,'zzddd','4444646','abc',-1,0,0,3,7,0),(27,'asdfafetee','4455667788','',-1,0,0,3,7,0),(28,'newcomptuer1','89402JSJDF','',-1,0,0,-1,-1,0),(29,'ss','12','',0,0,0,3,6,0),(30,'sitetest','94943','',13,2,4,-1,-1,0);
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
INSERT INTO `distribution_points` VALUES (1,'abc123','192.168.1.1','SMB','cd_dp','workgroup','user','pass',0,'',0,''),(2,'asdfsdf','[server-ip]','SMB+HTTP','asdfsdaf','asdf','adsf','aaa',1,'c:\\',1,'');
/*!40000 ALTER TABLE `distribution_points` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `drivers`
--

DROP TABLE IF EXISTS `drivers`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `drivers` (
  `driver_id` int(11) NOT NULL AUTO_INCREMENT,
  `driver_path` varchar(255) DEFAULT NULL,
  `driver_display_name` varchar(45) DEFAULT NULL,
  PRIMARY KEY (`driver_id`)
) ENGINE=MyISAM DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `drivers`
--

LOCK TABLES `drivers` WRITE;
/*!40000 ALTER TABLE `drivers` DISABLE KEYS */;
/*!40000 ALTER TABLE `drivers` ENABLE KEYS */;
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
) ENGINE=MyISAM AUTO_INCREMENT=14 DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `group_membership`
--

LOCK TABLES `group_membership` WRITE;
/*!40000 ALTER TABLE `group_membership` DISABLE KEYS */;
INSERT INTO `group_membership` VALUES (9,12,1),(10,24,1),(12,20,1),(13,21,1);
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
  `group_image_id` int(11) DEFAULT NULL,
  `group_image_profile_id` int(11) DEFAULT NULL,
  `group_type` varchar(45) DEFAULT NULL,
  `group_sender_arguments` longtext,
  `group_receiver_arguments` longtext,
  PRIMARY KEY (`group_id`)
) ENGINE=MyISAM AUTO_INCREMENT=2 DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `groups`
--

LOCK TABLES `groups` WRITE;
/*!40000 ALTER TABLE `groups` DISABLE KEYS */;
INSERT INTO `groups` VALUES (1,'Group1_Test26','',3,1,'standard','','');
/*!40000 ALTER TABLE `groups` ENABLE KEYS */;
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
  PRIMARY KEY (`image_profile_script_id`)
) ENGINE=MyISAM AUTO_INCREMENT=25 DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `image_profile_scripts`
--

LOCK TABLES `image_profile_scripts` WRITE;
/*!40000 ALTER TABLE `image_profile_scripts` DISABLE KEYS */;
INSERT INTO `image_profile_scripts` VALUES (10,1,2,0,1),(9,1,4,1,0),(8,1,3,1,0),(11,1,5,0,1),(19,5,5,1,0),(18,5,2,0,1),(17,5,4,0,1),(16,5,3,1,0),(23,7,4,1,1),(22,7,3,1,1),(24,7,5,1,0);
/*!40000 ALTER TABLE `image_profile_scripts` ENABLE KEYS */;
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
  PRIMARY KEY (`image_id`)
) ENGINE=MyISAM AUTO_INCREMENT=7 DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `images`
--

LOCK TABLES `images` WRITE;
/*!40000 ALTER TABLE `images` DISABLE KEYS */;
INSERT INTO `images` VALUES (3,'abc','Linux','abc',1,1,NULL,NULL,NULL),(6,'test','Windows','',0,1,NULL,NULL,NULL);
/*!40000 ALTER TABLE `images` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `linux_profiles`
--

DROP TABLE IF EXISTS `linux_profiles`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `linux_profiles` (
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
  `resize_debug` tinyint(4) DEFAULT NULL,
  `skip_volume_expand` tinyint(4) DEFAULT NULL,
  `fix_bcd` tinyint(4) DEFAULT NULL,
  `fix_bootloader` tinyint(4) DEFAULT NULL,
  `partition_method` varchar(45) DEFAULT NULL,
  `force_dynamic_partitions` tinyint(4) DEFAULT NULL,
  `always_expand_partitions` tinyint(4) DEFAULT NULL,
  `custom_partition_script` longtext,
  PRIMARY KEY (`image_profile_id`)
) ENGINE=MyISAM AUTO_INCREMENT=8 DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `linux_profiles`
--

LOCK TABLES `linux_profiles` WRITE;
/*!40000 ALTER TABLE `linux_profiles` DISABLE KEYS */;
INSERT INTO `linux_profiles` VALUES (5,3,'3.12.13-WDS','initrd.gz','profile23','','',0,0,'Reboot',0,1,0,0,0,0,1,NULL,0,0,NULL),(6,3,NULL,NULL,'myprofilea','',NULL,0,0,NULL,0,0,0,0,0,0,0,NULL,0,0,NULL),(7,3,NULL,NULL,'adsf','',NULL,0,0,NULL,0,0,0,0,0,0,0,'Custom Layout',0,0,'');
/*!40000 ALTER TABLE `linux_profiles` ENABLE KEYS */;
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
) ENGINE=MyISAM AUTO_INCREMENT=6 DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `multicast_ports`
--

LOCK TABLES `multicast_ports` WRITE;
/*!40000 ALTER TABLE `multicast_ports` DISABLE KEYS */;
INSERT INTO `multicast_ports` VALUES (1,8998),(2,63998),(3,98),(4,98),(5,100);
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
INSERT INTO `partition_layouts` VALUES (1,'partlayout','MBR','Windows',1),(2,'abc','MBR','Windows',1);
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
) ENGINE=MyISAM AUTO_INCREMENT=5 DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `rooms`
--

LOCK TABLES `rooms` WRITE;
/*!40000 ALTER TABLE `rooms` DISABLE KEYS */;
INSERT INTO `rooms` VALUES (4,'room1',2);
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
  `script_priority` int(11) DEFAULT NULL,
  `script_category_id` int(11) DEFAULT NULL,
  `script_contents` longtext,
  PRIMARY KEY (`script_id`)
) ENGINE=MyISAM AUTO_INCREMENT=6 DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `scripts`
--

LOCK TABLES `scripts` WRITE;
/*!40000 ALTER TABLE `scripts` DISABLE KEYS */;
INSERT INTO `scripts` VALUES (3,'Hello','',2,0,'aadsfasdf'),(2,'newscriptz','hello',55,0,'#!/bin/bash\nhello'),(4,'Hello2','aadsf',1,0,'adsfadsf'),(5,'Zello','a',2,0,'afasfdasdfa\nasdf\nas\ndf\nasdf');
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

-- Dump completed on 2015-10-09 16:27:07
