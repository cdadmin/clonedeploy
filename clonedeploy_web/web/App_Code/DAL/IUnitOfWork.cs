using System;

namespace DAL
{
    public interface IUnitOfWork: IDisposable
    {
      
        DAL.IGenericRepository<Models.ActiveMulticastSession> ActiveMulticastSessionRepository { get; }
       
        DAL.RoomRepository RoomRepository { get; }
        DAL.IGenericRepository<Models.DistributionPoint> DistributionPointRepository { get; }
        DAL.IGenericRepository<Models.ComputerBootMenu> ComputerBootMenuRepository { get; }
        DAL.IGenericRepository<Models.GroupMembership> GroupMembershipRepository { get; }
        DAL.IGenericRepository<Models.Image> ImageRepository { get; }
        DAL.IGenericRepository<Models.ImageProfilePartition> ImageProfilePartitionRepository { get; }
        DAL.IGenericRepository<Models.LinuxProfile> LinuxProfileRepository { get; }
        DAL.IGenericRepository<Models.Partition> PartitionRepository { get; }
        DAL.IGenericRepository<Models.PartitionLayout> PartitionLayoutRepository { get; }
        DAL.IGenericRepository<Models.Port> PortRepository { get; }
        DAL.IGenericRepository<Models.Script> ScriptRepository { get; }
        DAL.IGenericRepository<Models.Setting> SettingRepository { get; }
        DAL.SiteRepository SiteRepository { get; }
        DAL.IGenericRepository<Models.SysprepTag> SysprepTagRepository { get; }
        DAL.IGenericRepository<Models.WdsUser> UserRepository { get; }
        DAL.IGenericRepository<Models.BootTemplate> BootTemplateRepository { get; }
        DAL.IGenericRepository<Models.ComputerLog> ComputerLogRepository { get; }
        DAL.IGenericRepository<Models.UserRight> UserRightRepository { get; }
        DAL.IGenericRepository<Models.UserGroupManagement> UserGroupManagementRepository { get; }
        DAL.IGenericRepository<Models.UserImageManagement> UserImageManagementRepository { get; }
        DAL.IGenericRepository<Models.UserLockout> UserLockoutRepository { get; }
        DAL.IGenericRepository<Models.GroupProperty> GroupPropertyRepository { get; }
        DAL.BuildingRepository BuildingRepository { get; }
        DAL.ComputerRepository ComputerRepository { get; }
        DAL.ImageProfileScriptRepository ImageProfileScriptRepository { get; }
        DAL.GroupRepository GroupRepository { get; }
        DAL.ActiveImagingTaskRepository ActiveImagingTaskRepository { get; }
        bool Save();
        
    }
}