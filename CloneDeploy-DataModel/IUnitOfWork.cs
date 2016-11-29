using System;
using CloneDeploy_Entities;

namespace CloneDeploy_DataModel
{
    public interface IUnitOfWork: IDisposable
    {

        IGenericRepository<ActiveMulticastSessionEntity> ActiveMulticastSessionRepository { get; }
       
        RoomRepository RoomRepository { get; }
        IGenericRepository<DistributionPointEntity> DistributionPointRepository { get; }
        IGenericRepository<FileFolderEntity> FileFolderRepository { get; }
        IGenericRepository<ComputerBootMenuEntity> ComputerBootMenuRepository { get; }
        IGenericRepository<GroupBootMenuEntity> GroupBootMenuRepository { get; }
        IGenericRepository<GroupMembershipEntity> GroupMembershipRepository { get; }
        IGenericRepository<ImageEntity> ImageRepository { get; }
        IGenericRepository<ImageProfilePartitionLayoutEntity> ImageProfilePartitionRepository { get; }
        IGenericRepository<ImageProfileEntity> ImageProfileRepository { get; }
        IGenericRepository<PartitionEntity> PartitionRepository { get; }
        IGenericRepository<PartitionLayoutEntity> PartitionLayoutRepository { get; }
        IGenericRepository<PortEntity> PortRepository { get; }
        IGenericRepository<ScriptEntity> ScriptRepository { get; }
        IGenericRepository<SettingEntity> SettingRepository { get; }
        SiteRepository SiteRepository { get; }
        IGenericRepository<SysprepTagEntity> SysprepTagRepository { get; }
        IGenericRepository<CloneDeployUserEntity> UserRepository { get; }
        IGenericRepository<BootTemplateEntity> BootTemplateRepository { get; }
        IGenericRepository<ComputerLogEntity> ComputerLogRepository { get; }
        IGenericRepository<UserRightEntity> UserRightRepository { get; }
        IGenericRepository<UserGroupManagementEntity> UserGroupManagementRepository { get; }
        IGenericRepository<UserImageManagementEntity> UserImageManagementRepository { get; }
        IGenericRepository<UserGroupRightEntity> UserGroupRightRepository { get; }
        IGenericRepository<UserGroupGroupManagementEntity> UserGroupGroupManagementRepository { get; }
        IGenericRepository<UserGroupImageManagementEntity> UserGroupImageManagementRepository { get; }
        IGenericRepository<UserLockoutEntity> UserLockoutRepository { get; }
        IGenericRepository<GroupPropertyEntity> GroupPropertyRepository { get; }
        BuildingRepository BuildingRepository { get; }
        ComputerRepository ComputerRepository { get; }
        IGenericRepository<ImageProfileScriptEntity> ImageProfileScriptRepository { get; }
        IGenericRepository<ImageProfileFileFolderEntity> ImageProfileFileFolderRepository { get; }
        IGenericRepository<ImageProfileSysprepTagEntity> ImageProfileSysprepRepository { get; }
        IGenericRepository<CdVersionEntity> CdVersionRepository { get; }
        GroupRepository GroupRepository { get; }
        ActiveImagingTaskRepository ActiveImagingTaskRepository { get; }
        IGenericRepository<MunkiManifestTemplateEntity> MunkiManifestRepository { get; }
        IGenericRepository<MunkiManifestCatalogEntity> MunkiCatalogRepository { get; }
        IGenericRepository<MunkiManifestManagedInstallEntity> MunkiManagedInstallRepository { get; }
        IGenericRepository<MunkiManifestManagedUnInstallEntity> MunkiManagedUnInstallRepository { get; }
        IGenericRepository<MunkiManifestManagedUpdateEntity> MunkiManagedUpdateRepository { get; }
        IGenericRepository<MunkiManifestOptionInstallEntity> MunkiOptionalInstallRepository { get; }
        IGenericRepository<MunkiManifestIncludedManifestEntity> MunkiIncludedManifestRepository { get; }
        IGenericRepository<ComputerMunkiEntity> ComputerMunkiRepository { get; }
        IGenericRepository<GroupMunkiEntity> GroupMunkiRepository { get; }
        IGenericRepository<ComputerProxyReservationEntity> ComputerProxyRepository { get; }
        IGenericRepository<BootEntryEntity> BootEntryRepository { get; }
        IGenericRepository<CloneDeployUserGroupEntity> UserGroupRepository { get; } 
        void Save();
        
    }
}