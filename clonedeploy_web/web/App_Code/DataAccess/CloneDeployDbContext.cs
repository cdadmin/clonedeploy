using System.Data.Entity;
using Models;

namespace DataAccess
{
    /// <summary>
    /// Summary description for Entities
    /// </summary>

    public class CloneDeployDbContext : DbContext
    {
        public CloneDeployDbContext() : base("mysql") { }
        public DbSet<Computer> Computers { get; set; }
        public DbSet<LinuxProfile> LinuxProfiles { get; set; }
        public DbSet<ActiveImagingTask> ActiveTasks { get; set; }
        public DbSet<ActiveMcTask> ActiveMcTasks { get; set; }
        public DbSet<BootTemplate> BootTemplates { get; set; }
        public DbSet<Group> Groups { get; set; }
        public DbSet<History> History { get; set; }
        public DbSet<Image> Images { get; set; }
        public DbSet<WdsUser> Users { get; set; }
        public DbSet<Port> Ports { get; set; }
        public DbSet<Setting> Settings { get; set; }

        public DbSet<Script> Scripts { get; set; }
        public DbSet<PartitionLayout> PartitionLayouts { get; set; }
        public DbSet<Models.Partition> Partitions { get; set; }
        public DbSet<ImageProfilePartition> ImageProfilePartitions { get; set; }
        public DbSet<ImageProfileScript> ImageProfileScripts { get; set; }
        public DbSet<GroupMembership> GroupMemberships { get; set; }

    }

}