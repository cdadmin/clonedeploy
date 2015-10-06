using System;

namespace DAL
{
    public interface IUnitOfWork: IDisposable
    {

        DAL.IGenericRepository<Models.Building> BuildingRepository { get; }
        DAL.IGenericRepository<Models.DistributionPoint> DistributionPointRepository { get; }
        DAL.IGenericRepository<Models.GroupMembership> GroupMembershipRepository { get; }
        DAL.IGenericRepository<Models.Image> ImageRepository { get; }
        DAL.IGenericRepository<Models.ImageProfilePartition> ImageProfilePartitionRepository { get; }
        DAL.IGenericRepository<Models.LinuxProfile> LinuxProfileRepository { get; }
        DAL.IGenericRepository<Models.Partition> PartitionRepository { get; }
        DAL.IGenericRepository<Models.PartitionLayout> PartitionLayoutRepository { get; }
        DAL.ImageProfileScriptRepository ImageProfileScriptRepository { get; }
        DAL.ComputerRepository ComputerRepository { get; }
        DAL.GroupRepository GroupRepository { get; }
        DAL.ActiveImagingTaskRepository ActiveImagingTaskRepository { get; }
        DAL.ActiveMulticastSessionRepository ActiveMulticastSessionRepository { get; }
        bool Save();
        
    }
}