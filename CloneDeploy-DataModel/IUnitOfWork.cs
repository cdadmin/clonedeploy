﻿using System;
using CloneDeploy_Entities;

namespace CloneDeploy_DataModel
{
    public interface IUnitOfWork : IDisposable
    {
        ActiveImagingTaskRepository ActiveImagingTaskRepository { get; }

        IGenericRepository<ActiveMulticastSessionEntity> ActiveMulticastSessionRepository { get; }
        IGenericRepository<AlternateServerIpEntity> AlternateServerIpRepository { get; }
        IGenericRepository<AuditLogEntity> AuditLogRepository { get; }
        IGenericRepository<BootEntryEntity> BootEntryRepository { get; }
        IGenericRepository<BootTemplateEntity> BootTemplateRepository { get; }
        BuildingRepository BuildingRepository { get; }
        IGenericRepository<CdVersionEntity> CdVersionRepository { get; }
        IGenericRepository<ClusterGroupDistributionPointEntity> ClusterGroupDistributionPointRepository { get; }
        IGenericRepository<ClusterGroupEntity> ClusterGroupRepository { get; }
        IGenericRepository<ClusterGroupServerEntity> ClusterGroupServersRepository { get; }
        IGenericRepository<ComputerBootMenuEntity> ComputerBootMenuRepository { get; }
        IGenericRepository<ComputerImageClassificationEntity> ComputerImageClassificationRepository { get; }
        IGenericRepository<ComputerLogEntity> ComputerLogRepository { get; }
       
        IGenericRepository<ComputerProxyReservationEntity> ComputerProxyRepository { get; }
        ComputerRepository ComputerRepository { get; }
        IGenericRepository<DistributionPointEntity> DistributionPointRepository { get; }
        IGenericRepository<FileFolderEntity> FileFolderRepository { get; }
        IGenericRepository<GroupBootMenuEntity> GroupBootMenuRepository { get; }
        IGenericRepository<GroupImageClassificationEntity> GroupImageClassificationRepository { get; }
        IGenericRepository<GroupMembershipEntity> GroupMembershipRepository { get; }
      
        IGenericRepository<GroupPropertyEntity> GroupPropertyRepository { get; }
        GroupRepository GroupRepository { get; }
        IGenericRepository<ImageClassificationEntity> ImageClassificationRepository { get; }
        IGenericRepository<ImageProfileFileFolderEntity> ImageProfileFileFolderRepository { get; }
       
        ImageProfileRepository ImageProfileRepository { get; }
        IGenericRepository<ImageProfileScriptEntity> ImageProfileScriptRepository { get; }
        IGenericRepository<ImageProfileSysprepTagEntity> ImageProfileSysprepRepository { get; }
        IGenericRepository<ImageEntity> ImageRepository { get; }
      
      
        IGenericRepository<PortEntity> PortRepository { get; }

        RoomRepository RoomRepository { get; }
        IGenericRepository<ScriptEntity> ScriptRepository { get; }
        IGenericRepository<SecondaryServerEntity> SecondaryServerRepository { get; }
        IGenericRepository<SettingEntity> SettingRepository { get; }
        SiteRepository SiteRepository { get; }
        IGenericRepository<SysprepTagEntity> SysprepTagRepository { get; }
        IGenericRepository<UserGroupGroupManagementEntity> UserGroupGroupManagementRepository { get; }
        IGenericRepository<UserGroupImageManagementEntity> UserGroupImageManagementRepository { get; }
        IGenericRepository<UserGroupManagementEntity> UserGroupManagementRepository { get; }
        IGenericRepository<CloneDeployUserGroupEntity> UserGroupRepository { get; }
        IGenericRepository<UserGroupRightEntity> UserGroupRightRepository { get; }
        IGenericRepository<UserImageManagementEntity> UserImageManagementRepository { get; }
        IGenericRepository<UserLockoutEntity> UserLockoutRepository { get; }
        CloneDeployUserRepository UserRepository { get; }
        
        IGenericRepository<UserRightEntity> UserRightRepository { get; }
        IGenericRepository<ImageProfileTemplate> ImageProfileTemplateRepository { get; } 
      
        void Save();
    }
}