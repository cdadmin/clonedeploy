using System.Data.Entity;
using CloneDeploy_Web.Models;

namespace DAL
{
    public class CloneDeployDbContext : DbContext
    {
        public CloneDeployDbContext() : base("clonedeploy")
        {
          

        }
        public DbSet<Computer> Computers { get; set; }
        public DbSet<ImageProfile> ImageProfiles { get; set; }
        public DbSet<ActiveImagingTask> ActiveImagingTasks { get; set; }
        public DbSet<ActiveMulticastSession> ActiveMulticastSessions { get; set; }
        public DbSet<BootTemplate> BootTemplates { get; set; }
        public DbSet<Group> Groups { get; set; }
        public DbSet<Image> Images { get; set; }
        public DbSet<CloneDeployUser> Users { get; set; }
        public DbSet<Port> Ports { get; set; }
        public DbSet<Setting> Settings { get; set; }
        public DbSet<SysprepTag> SysprepTags { get; set; }
        public DbSet<Script> Scripts { get; set; }
        public DbSet<PartitionLayout> PartitionLayouts { get; set; }
        public DbSet<Partition> Partitions { get; set; }
        public DbSet<ImageProfilePartitionLayout> ImageProfilePartitions { get; set; }
        public DbSet<ImageProfileScript> ImageProfileScripts { get; set; }
        public DbSet<ImageProfileSysprepTag> ImageProfileSysprepTags { get; set; }
        public DbSet<ImageProfileFileFolder> ImageProfileFilesFolders { get; set; }
        public DbSet<GroupMembership> GroupMemberships { get; set; }
        public DbSet<Site> Sites { get; set; }
        public DbSet<Building> Buildings { get; set; }
        public DbSet<Room> Rooms { get; set; }
        public DbSet<DistributionPoint> DistributionPoints { get; set; }
        public DbSet<ComputerBootMenu> ComputerBootMenus { get; set; }
        public DbSet<GroupBootMenu> GroupBootMenus { get; set; }
        public DbSet<ComputerLog> ComputerLogs { get; set; }
        public DbSet<UserRight> UserRight { get; set; }
        public DbSet<UserGroupManagement> UserGroupManagements { get; set; }
        public DbSet<UserImageManagement> UserImageManagements { get; set; }
        public DbSet<UserGroupRight> UserGroupRight { get; set; }
        public DbSet<UserGroupGroupManagement> UserGroupGroupManagements { get; set; }
        public DbSet<UserGroupImageManagement> UserGroupImageManagements { get; set; }
        public DbSet<UserLockout> UserLockouts { get; set; }
        public DbSet<GroupProperty> GroupProperties { get; set; }
        public DbSet<FileFolder> FilesFolders { get; set; }
        public DbSet<CdVersion> CdVersions { get; set; }
        public DbSet<MunkiManifestTemplate> MunkiManifestTemplates { get; set; }
        public DbSet<MunkiManifestCatalog> MunkiManifestCatalogs { get; set; }
        public DbSet<MunkiManifestManagedInstall> MunkiManifestManagedInstalls { get; set; }
        public DbSet<MunkiManifestManagedUnInstall> MunkiManifestManagedUnInstalls { get; set; }
        public DbSet<MunkiManifestManagedUpdate> MunkiManifestManagedUpdates { get; set; }
        public DbSet<MunkiManifestOptionInstall> MunkiManifestOptionalInstalls { get; set; }
        public DbSet<MunkiManifestIncludedManifest> MunkiManifestIncludedManifests { get; set; }
        public DbSet<ComputerMunki> ComputerMunkis { get; set; }
        public DbSet<GroupMunki> GroupMunkis { get; set; }
        public DbSet<ComputerProxyReservation> ProxyReservations { get; set; }
        public DbSet<BootEntry> BootEntries { get; set; }
        public DbSet<CloneDeployUserGroup> UserGroups { get; set; }

    }

}