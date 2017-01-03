using System;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Validation;
using CloneDeploy_Entities;
using log4net;

namespace CloneDeploy_DataModel
{
    public class UnitOfWork : IUnitOfWork 
    {
        private readonly ILog log = LogManager.GetLogger("ApplicationLog");
        private CloneDeployDbContext _context = new CloneDeployDbContext();

        private IGenericRepository<ActiveMulticastSessionEntity> _activeMulticastSessionRepository;
        private BuildingRepository _buildingRepository;
        private IGenericRepository<DistributionPointEntity> _distributionPointRepository;
        private IGenericRepository<FileFolderEntity> _fileFolderRepository;
        private IGenericRepository<ComputerBootMenuEntity> _computerBootMenuRepository;
        private IGenericRepository<GroupBootMenuEntity> _groupBootMenuRepository;
        private IGenericRepository<GroupMembershipEntity> _groupMembershipRepository;
        private IGenericRepository<ImageEntity> _imageRepository;
        private IGenericRepository<ImageProfilePartitionLayoutEntity> _imageProfilePartitionRepository;
        private IGenericRepository<ImageProfileEntity> _imageProfileRepository;
        private IGenericRepository<PartitionEntity> _partitionRepository;
        private IGenericRepository<PartitionLayoutEntity> _partitionLayoutRepository;
        private IGenericRepository<PortEntity> _portRepository;
        private IGenericRepository<ComputerLogEntity> _computerLogRepository;
        private IGenericRepository<UserRightEntity> _userRightRepository;
        private IGenericRepository<UserImageManagementEntity> _userImageManagementRepository;
        private IGenericRepository<UserGroupManagementEntity> _userGroupManagementRepository;
        private IGenericRepository<UserGroupRightEntity> _userGroupRightRepository;
        private IGenericRepository<UserGroupImageManagementEntity> _userGroupImageManagementRepository;
        private IGenericRepository<UserGroupGroupManagementEntity> _userGroupGroupManagementRepository;
        private IGenericRepository<UserLockoutEntity> _userLockoutRepository;
        private IGenericRepository<GroupPropertyEntity> _groupPropertyRepository;
        private RoomRepository _roomRepository;
        private IGenericRepository<ScriptEntity> _scriptRepository;
        private IGenericRepository<SettingEntity> _settingRepository;
        private SiteRepository _siteRepository;
        private IGenericRepository<SysprepTagEntity> _sysprepTagRepository;
        private IGenericRepository<CloneDeployUserEntity> _userRepository;
        private IGenericRepository<BootTemplateEntity> _bootTemplateRepository;
        private IGenericRepository<CdVersionEntity> _cdVersionRepository;
        private ComputerRepository _computerRepository;
        private IGenericRepository<ImageProfileScriptEntity> _imageProfileScriptRepository;
        private IGenericRepository<ImageProfileFileFolderEntity> _imageProfileFileFolderRepository;
        private IGenericRepository<ImageProfileSysprepTagEntity> _imageProfileSysprepRepository;
        private GroupRepository _groupRepository;
        private ActiveImagingTaskRepository _activeImagingTaskRepository;
        private IGenericRepository<MunkiManifestTemplateEntity> _munkiManifestRepository;
        private IGenericRepository<MunkiManifestCatalogEntity> _munkiCatalogRepository;
        private IGenericRepository<MunkiManifestManagedInstallEntity> _munkiManagedInstallRepository;
        private IGenericRepository<MunkiManifestManagedUnInstallEntity> _munkiManagedUninstallRepository;
        private IGenericRepository<MunkiManifestManagedUpdateEntity> _munkiManagedUpdateRepository;
        private IGenericRepository<MunkiManifestOptionInstallEntity> _munkiOptionalInstallRepository;
        private IGenericRepository<MunkiManifestIncludedManifestEntity> _munkiIncludedManifestRepository;
        private IGenericRepository<ComputerMunkiEntity> _computerMunkiRepository;
        private IGenericRepository<GroupMunkiEntity> _groupMunkiRepository;
        private IGenericRepository<ComputerProxyReservationEntity> _computerProxyRepository;
        private IGenericRepository<BootEntryEntity> _bootEntryRepository;
        private IGenericRepository<CloneDeployUserGroupEntity> _userGroupRepository;
        private IGenericRepository<SecondaryServerEntity> _secondaryServerRepository;

        public IGenericRepository<SecondaryServerEntity> SecondaryServerRepository
        {
            get { return _secondaryServerRepository ?? (_secondaryServerRepository = new GenericRepository<SecondaryServerEntity>(_context)); }
        }

        public IGenericRepository<CloneDeployUserGroupEntity> UserGroupRepository
        {
            get { return _userGroupRepository ?? (_userGroupRepository = new GenericRepository<CloneDeployUserGroupEntity>(_context)); }
        }

        public IGenericRepository<BootEntryEntity> BootEntryRepository
        {
            get { return _bootEntryRepository ?? (_bootEntryRepository = new GenericRepository<BootEntryEntity>(_context)); }
        }

        public IGenericRepository<ComputerProxyReservationEntity> ComputerProxyRepository
        {
            get { return _computerProxyRepository ?? (_computerProxyRepository = new GenericRepository<ComputerProxyReservationEntity>(_context)); }
        }

        public IGenericRepository<ComputerMunkiEntity> ComputerMunkiRepository
        {
            get { return _computerMunkiRepository ?? (_computerMunkiRepository = new GenericRepository<ComputerMunkiEntity>(_context)); }

        }

        public IGenericRepository<GroupMunkiEntity> GroupMunkiRepository
        {
            get { return _groupMunkiRepository ?? (_groupMunkiRepository = new GenericRepository<GroupMunkiEntity>(_context)); }

        }

        public IGenericRepository<GroupBootMenuEntity> GroupBootMenuRepository
        {
            get { return _groupBootMenuRepository ?? (_groupBootMenuRepository = new GenericRepository<GroupBootMenuEntity>(_context)); }

        }

        public IGenericRepository<MunkiManifestCatalogEntity> MunkiCatalogRepository
        {
            get { return _munkiCatalogRepository ?? (_munkiCatalogRepository = new GenericRepository<MunkiManifestCatalogEntity>(_context)); }

        }

        public IGenericRepository<MunkiManifestManagedInstallEntity> MunkiManagedInstallRepository
        {
            get { return _munkiManagedInstallRepository ?? (_munkiManagedInstallRepository = new GenericRepository<MunkiManifestManagedInstallEntity>(_context)); }
        }

        public IGenericRepository<MunkiManifestManagedUnInstallEntity> MunkiManagedUnInstallRepository
        {
            get { return _munkiManagedUninstallRepository ?? (_munkiManagedUninstallRepository = new GenericRepository<MunkiManifestManagedUnInstallEntity>(_context)); }
        }

        public IGenericRepository<MunkiManifestManagedUpdateEntity> MunkiManagedUpdateRepository
        {
            get { return _munkiManagedUpdateRepository ?? (_munkiManagedUpdateRepository = new GenericRepository<MunkiManifestManagedUpdateEntity>(_context)); }
        }

        public IGenericRepository<MunkiManifestOptionInstallEntity> MunkiOptionalInstallRepository
        {
            get { return _munkiOptionalInstallRepository ?? (_munkiOptionalInstallRepository = new GenericRepository<MunkiManifestOptionInstallEntity>(_context)); }
        }

        public IGenericRepository<MunkiManifestIncludedManifestEntity> MunkiIncludedManifestRepository
        {
            get { return _munkiIncludedManifestRepository ?? (_munkiIncludedManifestRepository = new GenericRepository<MunkiManifestIncludedManifestEntity>(_context)); }
        }

        public IGenericRepository<MunkiManifestTemplateEntity> MunkiManifestRepository
        {
            get { return _munkiManifestRepository ?? (_munkiManifestRepository = new GenericRepository<MunkiManifestTemplateEntity>(_context)); }

        }

        public IGenericRepository<FileFolderEntity> FileFolderRepository
        {
            get { return _fileFolderRepository ?? (_fileFolderRepository = new GenericRepository<FileFolderEntity>(_context)); }

        }

        public IGenericRepository<GroupPropertyEntity> GroupPropertyRepository
        {
            get { return _groupPropertyRepository ?? (_groupPropertyRepository = new GenericRepository<GroupPropertyEntity>(_context)); }

        }

        public IGenericRepository<UserLockoutEntity> UserLockoutRepository
        {
            get { return _userLockoutRepository ?? (_userLockoutRepository = new GenericRepository<UserLockoutEntity>(_context)); }

        }

        public IGenericRepository<ActiveMulticastSessionEntity> ActiveMulticastSessionRepository
        {
            get { return _activeMulticastSessionRepository ?? (_activeMulticastSessionRepository = new GenericRepository<ActiveMulticastSessionEntity>(_context)); }

        }

        public IGenericRepository<ComputerBootMenuEntity> ComputerBootMenuRepository
        {
            get { return _computerBootMenuRepository ?? (_computerBootMenuRepository = new GenericRepository<ComputerBootMenuEntity>(_context)); }

        }

        public IGenericRepository<UserGroupManagementEntity> UserGroupManagementRepository
        {
            get { return _userGroupManagementRepository ?? (_userGroupManagementRepository = new GenericRepository<UserGroupManagementEntity>(_context)); }

        }

        public IGenericRepository<UserImageManagementEntity> UserImageManagementRepository
        {
            get { return _userImageManagementRepository ?? (_userImageManagementRepository = new GenericRepository<UserImageManagementEntity>(_context)); }

        }

        public IGenericRepository<UserGroupGroupManagementEntity> UserGroupGroupManagementRepository
        {
            get { return _userGroupGroupManagementRepository ?? (_userGroupGroupManagementRepository = new GenericRepository<UserGroupGroupManagementEntity>(_context)); }

        }

        public IGenericRepository<UserGroupImageManagementEntity> UserGroupImageManagementRepository
        {
            get { return _userGroupImageManagementRepository ?? (_userGroupImageManagementRepository = new GenericRepository<UserGroupImageManagementEntity>(_context)); }

        }

        public ComputerRepository ComputerRepository
        {
            get { return _computerRepository ?? (_computerRepository = new ComputerRepository(_context)); }
        }

        public BuildingRepository BuildingRepository
        {
            get { return _buildingRepository ?? (_buildingRepository = new BuildingRepository(_context)); }
        }

        public IGenericRepository<DistributionPointEntity> DistributionPointRepository
        {
            get { return _distributionPointRepository ?? (_distributionPointRepository = new GenericRepository<DistributionPointEntity>(_context)); }
        }

        public IGenericRepository<GroupMembershipEntity> GroupMembershipRepository
        {
            get { return _groupMembershipRepository ?? (_groupMembershipRepository = new GenericRepository<GroupMembershipEntity>(_context)); }
        }

        public IGenericRepository<ImageEntity> ImageRepository
        {
            get { return _imageRepository ?? (_imageRepository = new GenericRepository<ImageEntity>(_context)); }
        }

        public IGenericRepository<ImageProfilePartitionLayoutEntity> ImageProfilePartitionRepository
        {
            get { return _imageProfilePartitionRepository ?? (_imageProfilePartitionRepository = new GenericRepository<ImageProfilePartitionLayoutEntity>(_context)); }
        }

        public IGenericRepository<ImageProfileEntity> ImageProfileRepository
        {
            get { return _imageProfileRepository ?? (_imageProfileRepository = new GenericRepository<ImageProfileEntity>(_context)); }
        }

        public IGenericRepository<PartitionEntity> PartitionRepository
        {
            get { return _partitionRepository ?? (_partitionRepository = new GenericRepository<PartitionEntity>(_context)); }
        }

        public IGenericRepository<PartitionLayoutEntity> PartitionLayoutRepository
        {
            get { return _partitionLayoutRepository ?? (_partitionLayoutRepository = new GenericRepository<PartitionLayoutEntity>(_context)); }
        }

        public IGenericRepository<PortEntity> PortRepository
        {
            get { return _portRepository ?? (_portRepository = new GenericRepository<PortEntity>(_context)); }
        }

        public IGenericRepository<ComputerLogEntity> ComputerLogRepository
        {
            get { return _computerLogRepository ?? (_computerLogRepository = new GenericRepository<ComputerLogEntity>(_context)); }
        }

        public IGenericRepository<UserRightEntity> UserRightRepository
        {
            get { return _userRightRepository ?? (_userRightRepository = new GenericRepository<UserRightEntity>(_context)); }
        }

        public IGenericRepository<UserGroupRightEntity> UserGroupRightRepository
        {
            get { return _userGroupRightRepository ?? (_userGroupRightRepository = new GenericRepository<UserGroupRightEntity>(_context)); }
        }


        public RoomRepository RoomRepository
        {
            get { return _roomRepository ?? (_roomRepository = new RoomRepository(_context)); }
        }

        public IGenericRepository<ScriptEntity> ScriptRepository
        {
            get { return _scriptRepository ?? (_scriptRepository = new GenericRepository<ScriptEntity>(_context)); }
        }

        public IGenericRepository<SettingEntity> SettingRepository
        {
            get { return _settingRepository ?? (_settingRepository = new GenericRepository<SettingEntity>(_context)); }
        }

        public SiteRepository SiteRepository
        {
            get { return _siteRepository ?? (_siteRepository = new SiteRepository(_context)); }
        }

        public IGenericRepository<SysprepTagEntity> SysprepTagRepository
        {
            get { return _sysprepTagRepository ?? (_sysprepTagRepository = new GenericRepository<SysprepTagEntity>(_context)); }
        }

        public IGenericRepository<CloneDeployUserEntity> UserRepository
        {
            get { return _userRepository ?? (_userRepository = new GenericRepository<CloneDeployUserEntity>(_context)); }
        }

        public IGenericRepository<BootTemplateEntity> BootTemplateRepository
        {
            get { return _bootTemplateRepository ?? (_bootTemplateRepository = new GenericRepository<BootTemplateEntity>(_context)); }
        }

        public IGenericRepository<ImageProfileScriptEntity> ImageProfileScriptRepository
        {
            get { return _imageProfileScriptRepository ?? (_imageProfileScriptRepository = new GenericRepository<ImageProfileScriptEntity>(_context)); }
        }

        public IGenericRepository<CdVersionEntity> CdVersionRepository
        {
            get { return _cdVersionRepository ?? (_cdVersionRepository = new GenericRepository<CdVersionEntity>(_context)); }
        }

        public IGenericRepository<ImageProfileFileFolderEntity> ImageProfileFileFolderRepository
        {
            get { return _imageProfileFileFolderRepository ?? (_imageProfileFileFolderRepository = new GenericRepository<ImageProfileFileFolderEntity>(_context)); }
        }

        public IGenericRepository<ImageProfileSysprepTagEntity> ImageProfileSysprepRepository
        {
            get { return _imageProfileSysprepRepository ?? (_imageProfileSysprepRepository = new GenericRepository<ImageProfileSysprepTagEntity>(_context)); }
        }

        public GroupRepository GroupRepository
        {
            get { return _groupRepository ?? (_groupRepository = new GroupRepository(_context)); }
        }

        public ActiveImagingTaskRepository ActiveImagingTaskRepository
        {
            get { return _activeImagingTaskRepository ?? (_activeImagingTaskRepository = new ActiveImagingTaskRepository(_context)); }
        }

        public void Save()
        {
            try
            {
                _context.SaveChanges();
            }
            catch (DbEntityValidationException ex)
            {
                foreach (var eve in ex.EntityValidationErrors)
                {
                    log.Debug(string.Format("{0}: Entity of type \"{1}\" in state \"{2}\" has the following validation errors:", DateTime.Now, eve.Entry.Entity.GetType().Name, eve.Entry.State));
                    foreach (var ve in eve.ValidationErrors)
                    {
                        log.Debug(string.Format("- Property: \"{0}\", Error: \"{1}\"", ve.PropertyName, ve.ErrorMessage));
                    }
                }
                throw;
            }
            catch (DbUpdateException ex)
            {
                log.Debug(ex.Message);
                log.Debug(ex.InnerException);
                throw;
            }
        }

        private bool disposed = false;

        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    _context.Dispose();
                }
            }
            this.disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
