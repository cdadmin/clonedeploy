using System;

namespace DAL
{
    public interface IUnitOfWork: IDisposable
    {
        DAL.IGenericRepository<Models.Computer> ComputerRepository { get; }
        DAL.IGenericRepository<Models.ActiveMulticastSession> ActiveMulticastSessionRepository { get; }
        DAL.IGenericRepository<Models.Building> BuildingRepository { get; }
        DAL.IGenericRepository<Models.Room> RoomRepository { get; }
        DAL.IGenericRepository<Models.DistributionPoint> DistributionPointRepository { get; }
        DAL.IGenericRepository<Models.GroupMembership> GroupMembershipRepository { get; }
        DAL.IGenericRepository<Models.Image> ImageRepository { get; }
        DAL.IGenericRepository<Models.ImageProfilePartition> ImageProfilePartitionRepository { get; }
        DAL.IGenericRepository<Models.LinuxProfile> LinuxProfileRepository { get; }
        DAL.IGenericRepository<Models.Partition> PartitionRepository { get; }
        DAL.IGenericRepository<Models.PartitionLayout> PartitionLayoutRepository { get; }
        DAL.IGenericRepository<Models.Port> PortRepository { get; }
        DAL.IGenericRepository<Models.Script> ScriptRepository { get; }
        DAL.IGenericRepository<Models.Setting> SettingRepository { get; }
        DAL.IGenericRepository<Models.Site> SiteRepository { get; }
        DAL.IGenericRepository<Models.SysprepTag> SysprepTagRepository { get; }
        DAL.IGenericRepository<Models.WdsUser> UserRepository { get; }
        DAL.IGenericRepository<Models.BootTemplate> BootTemplateRepository { get; }
        DAL.ImageProfileScriptRepository ImageProfileScriptRepository { get; }
        DAL.GroupRepository GroupRepository { get; }
        DAL.ActiveImagingTaskRepository ActiveImagingTaskRepository { get; }
        bool Save();
        
    }
}