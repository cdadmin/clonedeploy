using System.Data.Entity;
using Models;

namespace Global
{
    public class DB : DbContext
    {
        public DB() : base("mysql") { }
        public DbSet<Computer> Hosts { get; set; }
        public DbSet<ActiveTask> ActiveTasks { get; set; }
        public DbSet<ActiveMcTask> ActiveMcTasks { get; set; }
        public DbSet<BootTemplate> BootTemplates { get; set; }
        public DbSet<Group> Groups { get; set; }
        public DbSet<History> History { get; set; }
        public DbSet<Image> Images { get; set; }
        public DbSet<WdsUser> Users { get; set; }
        public DbSet<Port> Ports { get; set; }
        public DbSet<Setting> Settings { get; set; }
        public DbSet<ImageProfile> ImageProfiles { get; set; }
    }
}

