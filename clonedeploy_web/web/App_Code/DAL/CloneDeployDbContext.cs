using System.Data.Entity;


namespace DAL
{
    /// <summary>
    /// Summary description for Entities
    /// </summary>

    public class CloneDeployDbContext : DbContext
    {
        public CloneDeployDbContext() : base("mysql") { }
        public DbSet<Models.Computer> Computers { get; set; }
        public DbSet<Models.LinuxProfile> LinuxProfiles { get; set; }
        public DbSet<Models.ActiveImagingTask> ActiveImagingTasks { get; set; }
        public DbSet<Models.ActiveMulticastSession> ActiveMulticastSessions { get; set; }
        public DbSet<Models.BootTemplate> BootTemplates { get; set; }
        public DbSet<Models.Group> Groups { get; set; }
        public DbSet<Models.History> History { get; set; }
        public DbSet<Models.Image> Images { get; set; }
        public DbSet<Models.WdsUser> Users { get; set; }
        public DbSet<Models.Port> Ports { get; set; }
        public DbSet<Models.Setting> Settings { get; set; }

        public DbSet<Models.Script> Scripts { get; set; }
        public DbSet<Models.PartitionLayout> PartitionLayouts { get; set; }
        public DbSet<Models.Partition> Partitions { get; set; }
        public DbSet<Models.ImageProfilePartition> ImageProfilePartitions { get; set; }
        public DbSet<Models.ImageProfileScript> ImageProfileScripts { get; set; }
        public DbSet<Models.GroupMembership> GroupMemberships { get; set; }

    }

}