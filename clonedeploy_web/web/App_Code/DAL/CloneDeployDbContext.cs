using System.Data.Entity;
using System.Web.Http.ModelBinding;
using Models;

namespace DAL
{
    public class CloneDeployDbContext : DbContext
    {
        public CloneDeployDbContext() : base("clonedeploy")
        {
          

        }
        public DbSet<Models.Computer> Computers { get; set; }
        public DbSet<Models.ImageProfile> ImageProfiles { get; set; }
        public DbSet<Models.ActiveImagingTask> ActiveImagingTasks { get; set; }
        public DbSet<Models.ActiveMulticastSession> ActiveMulticastSessions { get; set; }
        public DbSet<Models.BootTemplate> BootTemplates { get; set; }
        public DbSet<Models.Group> Groups { get; set; }
        public DbSet<Models.Image> Images { get; set; }
        public DbSet<CloneDeployUser> Users { get; set; }
        public DbSet<Models.Port> Ports { get; set; }
        public DbSet<Models.Setting> Settings { get; set; }
        public DbSet<Models.SysprepTag> SysprepTags { get; set; }
        public DbSet<Models.Script> Scripts { get; set; }
        public DbSet<Models.PartitionLayout> PartitionLayouts { get; set; }
        public DbSet<Models.Partition> Partitions { get; set; }
        public DbSet<Models.ImageProfilePartitionLayout> ImageProfilePartitions { get; set; }
        public DbSet<Models.ImageProfileScript> ImageProfileScripts { get; set; }
        public DbSet<Models.ImageProfileSysprepTag> ImageProfileSysprepTags { get; set; }
        public DbSet<Models.ImageProfileFileFolder> ImageProfileFilesFolders { get; set; }
        public DbSet<Models.GroupMembership> GroupMemberships { get; set; }
        public DbSet<Models.Site> Sites { get; set; }
        public DbSet<Models.Building> Buildings { get; set; }
        public DbSet<Models.Room> Rooms { get; set; }
        public DbSet<Models.DistributionPoint> DistributionPoints { get; set; }
        public DbSet<Models.ComputerBootMenu> ComputerBootMenus { get; set; }
        public DbSet<Models.GroupBootMenu> GroupBootMenus { get; set; }
        public DbSet<Models.ComputerLog> ComputerLogs { get; set; }
        public DbSet<Models.UserRight> UserRight { get; set; }
        public DbSet<Models.UserGroupManagement> UserGroupManagements { get; set; }
        public DbSet<Models.UserImageManagement> UserImageManagements { get; set; }
        public DbSet<Models.UserLockout> UserLockouts { get; set; }
        public DbSet<Models.GroupProperty> GroupProperties { get; set; }
        public DbSet<Models.FileFolder> FilesFolders { get; set; }
        public DbSet<Models.CdVersion> CdVersions { get; set; }
        public DbSet<Models.MunkiManifestTemplate> MunkiManifestTemplates { get; set; }
        public DbSet<Models.MunkiManifestCatalog> MunkiManifestCatalogs { get; set; }
        public DbSet<Models.MunkiManifestManagedInstall> MunkiManifestManagedInstalls { get; set; }
        public DbSet<Models.MunkiManifestManagedUnInstall> MunkiManifestManagedUnInstalls { get; set; }
        public DbSet<Models.MunkiManifestManagedUpdate> MunkiManifestManagedUpdates { get; set; }
        public DbSet<Models.MunkiManifestOptionInstall> MunkiManifestOptionalInstalls { get; set; }
        public DbSet<Models.MunkiManifestIncludedManifest> MunkiManifestIncludedManifests { get; set; }
        public DbSet<Models.ComputerMunki> ComputerMunkis { get; set; }
        public DbSet<Models.GroupMunki> GroupMunkis { get; set; }
        public DbSet<Models.ComputerProxyReservation> ProxyReservations { get; set; }
        public DbSet<Models.BootEntry> BootEntries { get; set; }

    }

}