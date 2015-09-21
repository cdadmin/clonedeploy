using System.Data.Entity;
using Models;

namespace Global
{
    public class DB : DbContext
    {
        public DB() : base("mysql") { }
        public DbSet<Computer> Hosts { get; set; }
        public DbSet<ActiveImagingTask> ActiveTasks { get; set; }
        public DbSet<ActiveMcTask> ActiveMcTasks { get; set; }
        public DbSet<BootTemplate> BootTemplates { get; set; }
        public DbSet<Group> Groups { get; set; }
        public DbSet<History> History { get; set; }
        public DbSet<Image> Images { get; set; }
        public DbSet<WdsUser> Users { get; set; }
        public DbSet<Port> Ports { get; set; }
        public DbSet<Setting> Settings { get; set; }
        public DbSet<LinuxProfile> LinuxProfiles { get; set; }
        public DbSet<Script> Scripts { get; set; }
        public DbSet<PartitionLayout> PartitionLayout { get; set; }
        public DbSet<Models.Partition> Partition { get; set; }
        public DbSet<ImageProfilePartition> ImageProfilePartition { get; set; }
        public DbSet<ImageProfileScript> ImageProfileScript { get; set; }
        public DbSet<GroupMembership> GroupMembership { get; set; }
    }
}

