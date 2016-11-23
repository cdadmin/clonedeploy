using System;
using CloneDeploy_Web.Models;

namespace DAL
{
    public interface IUnitOfWork: IDisposable
    {
      
        DAL.IGenericRepository<ActiveMulticastSession> ActiveMulticastSessionRepository { get; }
       
        DAL.RoomRepository RoomRepository { get; }
        DAL.IGenericRepository<DistributionPoint> DistributionPointRepository { get; }
        DAL.IGenericRepository<FileFolder> FileFolderRepository { get; }
        DAL.IGenericRepository<ComputerBootMenu> ComputerBootMenuRepository { get; }
        DAL.IGenericRepository<GroupBootMenu> GroupBootMenuRepository { get; }
        DAL.IGenericRepository<GroupMembership> GroupMembershipRepository { get; }
        DAL.IGenericRepository<Image> ImageRepository { get; }
        DAL.IGenericRepository<ImageProfilePartitionLayout> ImageProfilePartitionRepository { get; }
        DAL.IGenericRepository<ImageProfile> ImageProfileRepository { get; }
        DAL.IGenericRepository<Partition> PartitionRepository { get; }
        DAL.IGenericRepository<PartitionLayout> PartitionLayoutRepository { get; }
        DAL.IGenericRepository<Port> PortRepository { get; }
        DAL.IGenericRepository<Script> ScriptRepository { get; }
        DAL.IGenericRepository<Setting> SettingRepository { get; }
        DAL.SiteRepository SiteRepository { get; }
        DAL.IGenericRepository<SysprepTag> SysprepTagRepository { get; }
        DAL.IGenericRepository<CloneDeployUser> UserRepository { get; }
        DAL.IGenericRepository<BootTemplate> BootTemplateRepository { get; }
        DAL.IGenericRepository<ComputerLog> ComputerLogRepository { get; }
        DAL.IGenericRepository<UserRight> UserRightRepository { get; }
        DAL.IGenericRepository<UserGroupManagement> UserGroupManagementRepository { get; }
        DAL.IGenericRepository<UserImageManagement> UserImageManagementRepository { get; }
        DAL.IGenericRepository<UserGroupRight> UserGroupRightRepository { get; }
        DAL.IGenericRepository<UserGroupGroupManagement> UserGroupGroupManagementRepository { get; }
        DAL.IGenericRepository<UserGroupImageManagement> UserGroupImageManagementRepository { get; }
        DAL.IGenericRepository<UserLockout> UserLockoutRepository { get; }
        DAL.IGenericRepository<GroupProperty> GroupPropertyRepository { get; }
        DAL.BuildingRepository BuildingRepository { get; }
        DAL.ComputerRepository ComputerRepository { get; }
        DAL.IGenericRepository<ImageProfileScript> ImageProfileScriptRepository { get; }
        DAL.IGenericRepository<ImageProfileFileFolder> ImageProfileFileFolderRepository { get; }
        DAL.IGenericRepository<ImageProfileSysprepTag> ImageProfileSysprepRepository { get; }
        DAL.IGenericRepository<CdVersion> CdVersionRepository { get; }
        DAL.GroupRepository GroupRepository { get; }
        DAL.ActiveImagingTaskRepository ActiveImagingTaskRepository { get; }
        DAL.IGenericRepository<MunkiManifestTemplate> MunkiManifestRepository { get; }
        DAL.IGenericRepository<MunkiManifestCatalog> MunkiCatalogRepository { get; }
        DAL.IGenericRepository<MunkiManifestManagedInstall> MunkiManagedInstallRepository { get; }
        DAL.IGenericRepository<MunkiManifestManagedUnInstall> MunkiManagedUnInstallRepository { get; }
        DAL.IGenericRepository<MunkiManifestManagedUpdate> MunkiManagedUpdateRepository { get; }
        DAL.IGenericRepository<MunkiManifestOptionInstall> MunkiOptionalInstallRepository { get; }
        DAL.IGenericRepository<MunkiManifestIncludedManifest> MunkiIncludedManifestRepository { get; }
        DAL.IGenericRepository<ComputerMunki> ComputerMunkiRepository { get; }
        DAL.IGenericRepository<GroupMunki> GroupMunkiRepository { get; }
        DAL.IGenericRepository<ComputerProxyReservation> ComputerProxyRepository { get; }
        DAL.IGenericRepository<BootEntry> BootEntryRepository { get; }
        DAL.IGenericRepository<CloneDeployUserGroup> UserGroupRepository { get; } 
        bool Save();
        
    }
}