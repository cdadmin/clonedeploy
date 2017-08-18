using System.Data.Entity;
using CloneDeploy_Entities;

namespace CloneDeploy_DataModel
{
    public class CloneDeployDbContext : DbContext
    {
        public CloneDeployDbContext() : base("clonedeploy")
        {
        }

        public DbSet<ActiveImagingTaskEntity> ActiveImagingTasks { get; set; }
        public DbSet<ActiveMulticastSessionEntity> ActiveMulticastSessions { get; set; }
        public DbSet<AlternateServerIpEntity> AlternateServerIps { get; set; }
        public DbSet<AuditLogEntity> AuditLogs { get; set; }
        public DbSet<BootEntryEntity> BootEntries { get; set; }
        public DbSet<BootTemplateEntity> BootTemplates { get; set; }
        public DbSet<BuildingEntity> Buildings { get; set; }
        public DbSet<CdVersionEntity> CdVersions { get; set; }
        public DbSet<ClusterGroupDistributionPointEntity> ClusterGroupDistributionPoints { get; set; }
        public DbSet<ClusterGroupEntity> ClusterGroups { get; set; }
        public DbSet<ClusterGroupServerEntity> ClusterGroupServers { get; set; }

        public DbSet<ComputerBootMenuEntity> ComputerBootMenus { get; set; }
        public DbSet<ComputerImageClassificationEntity> ComputerImageClassifications { get; set; }
        public DbSet<ComputerLogEntity> ComputerLogs { get; set; }
        public DbSet<ComputerMunkiEntity> ComputerMunkis { get; set; }
        public DbSet<ComputerEntity> Computers { get; set; }
        public DbSet<DistributionPointEntity> DistributionPoints { get; set; }
        public DbSet<FileFolderEntity> FilesFolders { get; set; }
        public DbSet<GroupBootMenuEntity> GroupBootMenus { get; set; }
        public DbSet<GroupImageClassificationEntity> GroupImageClassifications { get; set; }
        public DbSet<GroupMembershipEntity> GroupMemberships { get; set; }
        public DbSet<GroupMunkiEntity> GroupMunkis { get; set; }
        public DbSet<GroupPropertyEntity> GroupProperties { get; set; }
        public DbSet<GroupEntity> Groups { get; set; }
        public DbSet<ImageClassificationEntity> ImageClassifications { get; set; }
        public DbSet<ImageProfileFileFolderEntity> ImageProfileFilesFolders { get; set; }
        public DbSet<ImageProfilePartitionLayoutEntity> ImageProfilePartitions { get; set; }
        public DbSet<ImageProfileEntity> ImageProfiles { get; set; }
        public DbSet<ImageProfileScriptEntity> ImageProfileScripts { get; set; }
        public DbSet<ImageProfileSysprepTagEntity> ImageProfileSysprepTags { get; set; }
        public DbSet<ImageEntity> Images { get; set; }
        public DbSet<MunkiManifestCatalogEntity> MunkiManifestCatalogs { get; set; }
        public DbSet<MunkiManifestIncludedManifestEntity> MunkiManifestIncludedManifests { get; set; }
        public DbSet<MunkiManifestManagedInstallEntity> MunkiManifestManagedInstalls { get; set; }
        public DbSet<MunkiManifestManagedUnInstallEntity> MunkiManifestManagedUnInstalls { get; set; }
        public DbSet<MunkiManifestManagedUpdateEntity> MunkiManifestManagedUpdates { get; set; }
        public DbSet<MunkiManifestOptionInstallEntity> MunkiManifestOptionalInstalls { get; set; }
        public DbSet<MunkiManifestTemplateEntity> MunkiManifestTemplates { get; set; }
        public DbSet<NbiEntryEntity> NetBootProfileEntries { get; set; }
        public DbSet<NetBootProfileEntity> NetBootProfiles { get; set; }
        public DbSet<PartitionLayoutEntity> PartitionLayouts { get; set; }
        public DbSet<PartitionEntity> Partitions { get; set; }
        public DbSet<PortEntity> Ports { get; set; }
        public DbSet<ComputerProxyReservationEntity> ProxyReservations { get; set; }
        public DbSet<RoomEntity> Rooms { get; set; }
        public DbSet<ScriptEntity> Scripts { get; set; }
        public DbSet<SecondaryServerEntity> SecondaryServers { get; set; }
        public DbSet<SettingEntity> Settings { get; set; }
        public DbSet<SiteEntity> Sites { get; set; }
        public DbSet<SysprepTagEntity> SysprepTags { get; set; }
        public DbSet<UserGroupGroupManagementEntity> UserGroupGroupManagements { get; set; }
        public DbSet<UserGroupImageManagementEntity> UserGroupImageManagements { get; set; }
        public DbSet<UserGroupManagementEntity> UserGroupManagements { get; set; }
        public DbSet<UserGroupRightEntity> UserGroupRight { get; set; }
        public DbSet<CloneDeployUserGroupEntity> UserGroups { get; set; }
        public DbSet<UserImageManagementEntity> UserImageManagements { get; set; }
        public DbSet<UserLockoutEntity> UserLockouts { get; set; }
        public DbSet<UserRightEntity> UserRight { get; set; }
        public DbSet<CloneDeployUserEntity> Users { get; set; }
    }
}