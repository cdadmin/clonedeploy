using System;

namespace CloneDeploy_App.DAL
{
    public interface IUnitOfWork: IDisposable
    {
      
        DAL.IGenericRepository<Models.ActiveMulticastSession> ActiveMulticastSessionRepository { get; }
       
        DAL.RoomRepository RoomRepository { get; }
        DAL.IGenericRepository<Models.DistributionPoint> DistributionPointRepository { get; }
        DAL.IGenericRepository<Models.FileFolder> FileFolderRepository { get; }
        DAL.IGenericRepository<Models.ComputerBootMenu> ComputerBootMenuRepository { get; }
        DAL.IGenericRepository<Models.GroupBootMenu> GroupBootMenuRepository { get; }
        DAL.IGenericRepository<Models.GroupMembership> GroupMembershipRepository { get; }
        DAL.IGenericRepository<Models.Image> ImageRepository { get; }
        DAL.IGenericRepository<Models.ImageProfilePartitionLayout> ImageProfilePartitionRepository { get; }
        DAL.IGenericRepository<Models.ImageProfile> ImageProfileRepository { get; }
        DAL.IGenericRepository<Models.Partition> PartitionRepository { get; }
        DAL.IGenericRepository<Models.PartitionLayout> PartitionLayoutRepository { get; }
        DAL.IGenericRepository<Models.Port> PortRepository { get; }
        DAL.IGenericRepository<Models.Script> ScriptRepository { get; }
        DAL.IGenericRepository<Models.Setting> SettingRepository { get; }
        DAL.SiteRepository SiteRepository { get; }
        DAL.IGenericRepository<Models.SysprepTag> SysprepTagRepository { get; }
        DAL.IGenericRepository<Models.CloneDeployUser> UserRepository { get; }
        DAL.IGenericRepository<Models.BootTemplate> BootTemplateRepository { get; }
        DAL.IGenericRepository<Models.ComputerLog> ComputerLogRepository { get; }
        DAL.IGenericRepository<Models.UserRight> UserRightRepository { get; }
        DAL.IGenericRepository<Models.UserGroupManagement> UserGroupManagementRepository { get; }
        DAL.IGenericRepository<Models.UserImageManagement> UserImageManagementRepository { get; }
        DAL.IGenericRepository<Models.UserGroupRight> UserGroupRightRepository { get; }
        DAL.IGenericRepository<Models.UserGroupGroupManagement> UserGroupGroupManagementRepository { get; }
        DAL.IGenericRepository<Models.UserGroupImageManagement> UserGroupImageManagementRepository { get; }
        DAL.IGenericRepository<Models.UserLockout> UserLockoutRepository { get; }
        DAL.IGenericRepository<Models.GroupProperty> GroupPropertyRepository { get; }
        DAL.BuildingRepository BuildingRepository { get; }
        DAL.ComputerRepository ComputerRepository { get; }
        DAL.IGenericRepository<Models.ImageProfileScript> ImageProfileScriptRepository { get; }
        DAL.IGenericRepository<Models.ImageProfileFileFolder> ImageProfileFileFolderRepository { get; }
        DAL.IGenericRepository<Models.ImageProfileSysprepTag> ImageProfileSysprepRepository { get; }
        DAL.IGenericRepository<Models.CdVersion> CdVersionRepository { get; }
        DAL.GroupRepository GroupRepository { get; }
        DAL.ActiveImagingTaskRepository ActiveImagingTaskRepository { get; }
        DAL.IGenericRepository<Models.MunkiManifestTemplate> MunkiManifestRepository { get; }
        DAL.IGenericRepository<Models.MunkiManifestCatalog> MunkiCatalogRepository { get; }
        DAL.IGenericRepository<Models.MunkiManifestManagedInstall> MunkiManagedInstallRepository { get; }
        DAL.IGenericRepository<Models.MunkiManifestManagedUnInstall> MunkiManagedUnInstallRepository { get; }
        DAL.IGenericRepository<Models.MunkiManifestManagedUpdate> MunkiManagedUpdateRepository { get; }
        DAL.IGenericRepository<Models.MunkiManifestOptionInstall> MunkiOptionalInstallRepository { get; }
        DAL.IGenericRepository<Models.MunkiManifestIncludedManifest> MunkiIncludedManifestRepository { get; }
        DAL.IGenericRepository<Models.ComputerMunki> ComputerMunkiRepository { get; }
        DAL.IGenericRepository<Models.GroupMunki> GroupMunkiRepository { get; }
        DAL.IGenericRepository<Models.ComputerProxyReservation> ComputerProxyRepository { get; }
        DAL.IGenericRepository<Models.BootEntry> BootEntryRepository { get; }
        DAL.IGenericRepository<Models.CloneDeployUserGroup> UserGroupRepository { get; } 
        bool Save();
        
    }
}