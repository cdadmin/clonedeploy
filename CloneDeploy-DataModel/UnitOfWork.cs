using System;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Validation;
using CloneDeploy_Entities;
using log4net;

namespace CloneDeploy_DataModel
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly CloneDeployDbContext _context = new CloneDeployDbContext();
        private readonly ILog log = LogManager.GetLogger(typeof(UnitOfWork));
        private ActiveImagingTaskRepository _activeImagingTaskRepository;

        private IGenericRepository<ActiveMulticastSessionEntity> _activeMulticastSessionRepository;
        private IGenericRepository<AlternateServerIpEntity> _alternateServerIpRepository;
        private IGenericRepository<AuditLogEntity> _auditLogRepository;
        private IGenericRepository<BootEntryEntity> _bootEntryRepository;
        private IGenericRepository<BootTemplateEntity> _bootTemplateRepository;
        private BuildingRepository _buildingRepository;
        private IGenericRepository<CdVersionEntity> _cdVersionRepository;
        private IGenericRepository<ClusterGroupDistributionPointEntity> _clusterGroupDistributionPointRepository;
        private IGenericRepository<ClusterGroupEntity> _clusterGroupRepository;
        private IGenericRepository<ClusterGroupServerEntity> _clusterGroupServersRepository;
        private IGenericRepository<ComputerBootMenuEntity> _computerBootMenuRepository;
        private IGenericRepository<ComputerImageClassificationEntity> _computerImageClassificationRepository;
        private IGenericRepository<ComputerLogEntity> _computerLogRepository;
     
        private IGenericRepository<ComputerProxyReservationEntity> _computerProxyRepository;
        private ComputerRepository _computerRepository;
        private IGenericRepository<DistributionPointEntity> _distributionPointRepository;
        private IGenericRepository<FileFolderEntity> _fileFolderRepository;
        private IGenericRepository<GroupBootMenuEntity> _groupBootMenuRepository;
        private IGenericRepository<GroupImageClassificationEntity> _groupImageClassificationRepository;
        private IGenericRepository<GroupMembershipEntity> _groupMembershipRepository;
     
        private IGenericRepository<GroupPropertyEntity> _groupPropertyRepository;
        private GroupRepository _groupRepository;
        private IGenericRepository<ImageClassificationEntity> _imageClassificationRepository;
        private IGenericRepository<ImageProfileFileFolderEntity> _imageProfileFileFolderRepository;
      
        private ImageProfileRepository _imageProfileRepository;
        private IGenericRepository<ImageProfileScriptEntity> _imageProfileScriptRepository;
        private IGenericRepository<ImageProfileSysprepTagEntity> _imageProfileSysprepRepository;
        private IGenericRepository<ImageEntity> _imageRepository;
     
        private IGenericRepository<NbiEntryEntity> _nbiEntryRepository;
        private IGenericRepository<NetBootProfileEntity> _netBootProfileRepository;
       
        private IGenericRepository<PortEntity> _portRepository;
        private RoomRepository _roomRepository;
        private IGenericRepository<ScriptEntity> _scriptRepository;
        private IGenericRepository<SecondaryServerEntity> _secondaryServerRepository;
        private IGenericRepository<SettingEntity> _settingRepository;
        private SiteRepository _siteRepository;
        private IGenericRepository<SysprepTagEntity> _sysprepTagRepository;
        private IGenericRepository<UserGroupGroupManagementEntity> _userGroupGroupManagementRepository;
        private IGenericRepository<UserGroupImageManagementEntity> _userGroupImageManagementRepository;
        private IGenericRepository<UserGroupManagementEntity> _userGroupManagementRepository;
        private IGenericRepository<CloneDeployUserGroupEntity> _userGroupRepository;
        private IGenericRepository<UserGroupRightEntity> _userGroupRightRepository;
        private IGenericRepository<UserImageManagementEntity> _userImageManagementRepository;
        private IGenericRepository<UserLockoutEntity> _userLockoutRepository;
        private CloneDeployUserRepository _userRepository;
        private IGenericRepository<UserRightEntity> _userRightRepository;
        private IGenericRepository<ImageProfileTemplate> _imageProfileTemplateRepository; 
    
        private bool disposed;

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        public ActiveImagingTaskRepository ActiveImagingTaskRepository
        {
            get
            {
                return _activeImagingTaskRepository ??
                       (_activeImagingTaskRepository = new ActiveImagingTaskRepository(_context));
            }
        }

        public IGenericRepository<ImageProfileTemplate> ImageProfileTemplateRepository
        {
            get
            {
                return _imageProfileTemplateRepository ??
                       (_imageProfileTemplateRepository =
                           new GenericRepository<ImageProfileTemplate>(_context));
            }
        }

        public IGenericRepository<ActiveMulticastSessionEntity> ActiveMulticastSessionRepository
        {
            get
            {
                return _activeMulticastSessionRepository ??
                       (_activeMulticastSessionRepository =
                           new GenericRepository<ActiveMulticastSessionEntity>(_context));
            }
        }

   

        public IGenericRepository<AlternateServerIpEntity> AlternateServerIpRepository
        {
            get
            {
                return _alternateServerIpRepository ??
                       (_alternateServerIpRepository = new GenericRepository<AlternateServerIpEntity>(_context));
            }
        }

        public IGenericRepository<AuditLogEntity> AuditLogRepository
        {
            get
            {
                return _auditLogRepository ?? (_auditLogRepository = new GenericRepository<AuditLogEntity>(_context));
            }
        }

        public IGenericRepository<BootEntryEntity> BootEntryRepository
        {
            get
            {
                return _bootEntryRepository ?? (_bootEntryRepository = new GenericRepository<BootEntryEntity>(_context));
            }
        }

        public IGenericRepository<BootTemplateEntity> BootTemplateRepository
        {
            get
            {
                return _bootTemplateRepository ??
                       (_bootTemplateRepository = new GenericRepository<BootTemplateEntity>(_context));
            }
        }

        public BuildingRepository BuildingRepository
        {
            get { return _buildingRepository ?? (_buildingRepository = new BuildingRepository(_context)); }
        }

        public IGenericRepository<CdVersionEntity> CdVersionRepository
        {
            get
            {
                return _cdVersionRepository ?? (_cdVersionRepository = new GenericRepository<CdVersionEntity>(_context));
            }
        }

        public IGenericRepository<ClusterGroupDistributionPointEntity> ClusterGroupDistributionPointRepository
        {
            get
            {
                return _clusterGroupDistributionPointRepository ??
                       (_clusterGroupDistributionPointRepository =
                           new GenericRepository<ClusterGroupDistributionPointEntity>(_context));
            }
        }

        public IGenericRepository<ClusterGroupEntity> ClusterGroupRepository
        {
            get
            {
                return _clusterGroupRepository ??
                       (_clusterGroupRepository = new GenericRepository<ClusterGroupEntity>(_context));
            }
        }

        public IGenericRepository<ClusterGroupServerEntity> ClusterGroupServersRepository
        {
            get
            {
                return _clusterGroupServersRepository ??
                       (_clusterGroupServersRepository = new GenericRepository<ClusterGroupServerEntity>(_context));
            }
        }

        public IGenericRepository<ComputerBootMenuEntity> ComputerBootMenuRepository
        {
            get
            {
                return _computerBootMenuRepository ??
                       (_computerBootMenuRepository = new GenericRepository<ComputerBootMenuEntity>(_context));
            }
        }

        public IGenericRepository<ComputerImageClassificationEntity> ComputerImageClassificationRepository
        {
            get
            {
                return _computerImageClassificationRepository ??
                       (_computerImageClassificationRepository =
                           new GenericRepository<ComputerImageClassificationEntity>(_context));
            }
        }

        public IGenericRepository<ComputerLogEntity> ComputerLogRepository
        {
            get
            {
                return _computerLogRepository ??
                       (_computerLogRepository = new GenericRepository<ComputerLogEntity>(_context));
            }
        }

    

        public IGenericRepository<ComputerProxyReservationEntity> ComputerProxyRepository
        {
            get
            {
                return _computerProxyRepository ??
                       (_computerProxyRepository = new GenericRepository<ComputerProxyReservationEntity>(_context));
            }
        }

        public ComputerRepository ComputerRepository
        {
            get { return _computerRepository ?? (_computerRepository = new ComputerRepository(_context)); }
        }

        public IGenericRepository<DistributionPointEntity> DistributionPointRepository
        {
            get
            {
                return _distributionPointRepository ??
                       (_distributionPointRepository = new GenericRepository<DistributionPointEntity>(_context));
            }
        }

        public IGenericRepository<FileFolderEntity> FileFolderRepository
        {
            get
            {
                return _fileFolderRepository ??
                       (_fileFolderRepository = new GenericRepository<FileFolderEntity>(_context));
            }
        }

        public IGenericRepository<GroupBootMenuEntity> GroupBootMenuRepository
        {
            get
            {
                return _groupBootMenuRepository ??
                       (_groupBootMenuRepository = new GenericRepository<GroupBootMenuEntity>(_context));
            }
        }

        public IGenericRepository<GroupImageClassificationEntity> GroupImageClassificationRepository
        {
            get
            {
                return _groupImageClassificationRepository ??
                       (_groupImageClassificationRepository =
                           new GenericRepository<GroupImageClassificationEntity>(_context));
            }
        }

        public IGenericRepository<GroupMembershipEntity> GroupMembershipRepository
        {
            get
            {
                return _groupMembershipRepository ??
                       (_groupMembershipRepository = new GenericRepository<GroupMembershipEntity>(_context));
            }
        }

      

        public IGenericRepository<GroupPropertyEntity> GroupPropertyRepository
        {
            get
            {
                return _groupPropertyRepository ??
                       (_groupPropertyRepository = new GenericRepository<GroupPropertyEntity>(_context));
            }
        }

        public GroupRepository GroupRepository
        {
            get { return _groupRepository ?? (_groupRepository = new GroupRepository(_context)); }
        }

        public IGenericRepository<ImageClassificationEntity> ImageClassificationRepository
        {
            get
            {
                return _imageClassificationRepository ??
                       (_imageClassificationRepository = new GenericRepository<ImageClassificationEntity>(_context));
            }
        }

        public IGenericRepository<ImageProfileFileFolderEntity> ImageProfileFileFolderRepository
        {
            get
            {
                return _imageProfileFileFolderRepository ??
                       (_imageProfileFileFolderRepository =
                           new GenericRepository<ImageProfileFileFolderEntity>(_context));
            }
        }

    

        public ImageProfileRepository ImageProfileRepository
        {
            get { return _imageProfileRepository ?? (_imageProfileRepository = new ImageProfileRepository(_context)); }
        }

        public IGenericRepository<ImageProfileScriptEntity> ImageProfileScriptRepository
        {
            get
            {
                return _imageProfileScriptRepository ??
                       (_imageProfileScriptRepository = new GenericRepository<ImageProfileScriptEntity>(_context));
            }
        }

        public IGenericRepository<ImageProfileSysprepTagEntity> ImageProfileSysprepRepository
        {
            get
            {
                return _imageProfileSysprepRepository ??
                       (_imageProfileSysprepRepository = new GenericRepository<ImageProfileSysprepTagEntity>(_context));
            }
        }

        public IGenericRepository<ImageEntity> ImageRepository
        {
            get { return _imageRepository ?? (_imageRepository = new GenericRepository<ImageEntity>(_context)); }
        }

     

    

     

      

      

       
        public IGenericRepository<NbiEntryEntity> NbiEntryRepository
        {
            get
            {
                return _nbiEntryRepository ?? (_nbiEntryRepository = new GenericRepository<NbiEntryEntity>(_context));
            }
        }

        public IGenericRepository<NetBootProfileEntity> NetBootProfileRepository
        {
            get
            {
                return _netBootProfileRepository ??
                       (_netBootProfileRepository = new GenericRepository<NetBootProfileEntity>(_context));
            }
        }

     

        public IGenericRepository<PortEntity> PortRepository
        {
            get { return _portRepository ?? (_portRepository = new GenericRepository<PortEntity>(_context)); }
        }

        public RoomRepository RoomRepository
        {
            get { return _roomRepository ?? (_roomRepository = new RoomRepository(_context)); }
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
                    log.Error(
                        string.Format(
                            "{0}: Entity of type \"{1}\" in state \"{2}\" has the following validation errors:",
                            DateTime.Now, eve.Entry.Entity.GetType().Name, eve.Entry.State));
                    foreach (var ve in eve.ValidationErrors)
                    {
                        log.Error(string.Format("- Property: \"{0}\", Error: \"{1}\"", ve.PropertyName, ve.ErrorMessage));
                    }
                }
                throw;
            }
            catch (DbUpdateException ex)
            {
                log.Error(ex.Message);
                log.Error(ex.InnerException);
                throw;
            }
        }

        public IGenericRepository<ScriptEntity> ScriptRepository
        {
            get { return _scriptRepository ?? (_scriptRepository = new GenericRepository<ScriptEntity>(_context)); }
        }

        public IGenericRepository<SecondaryServerEntity> SecondaryServerRepository
        {
            get
            {
                return _secondaryServerRepository ??
                       (_secondaryServerRepository = new GenericRepository<SecondaryServerEntity>(_context));
            }
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
            get
            {
                return _sysprepTagRepository ??
                       (_sysprepTagRepository = new GenericRepository<SysprepTagEntity>(_context));
            }
        }

        public IGenericRepository<UserGroupGroupManagementEntity> UserGroupGroupManagementRepository
        {
            get
            {
                return _userGroupGroupManagementRepository ??
                       (_userGroupGroupManagementRepository =
                           new GenericRepository<UserGroupGroupManagementEntity>(_context));
            }
        }

        public IGenericRepository<UserGroupImageManagementEntity> UserGroupImageManagementRepository
        {
            get
            {
                return _userGroupImageManagementRepository ??
                       (_userGroupImageManagementRepository =
                           new GenericRepository<UserGroupImageManagementEntity>(_context));
            }
        }

        public IGenericRepository<UserGroupManagementEntity> UserGroupManagementRepository
        {
            get
            {
                return _userGroupManagementRepository ??
                       (_userGroupManagementRepository = new GenericRepository<UserGroupManagementEntity>(_context));
            }
        }

        public IGenericRepository<CloneDeployUserGroupEntity> UserGroupRepository
        {
            get
            {
                return _userGroupRepository ??
                       (_userGroupRepository = new GenericRepository<CloneDeployUserGroupEntity>(_context));
            }
        }

        public IGenericRepository<UserGroupRightEntity> UserGroupRightRepository
        {
            get
            {
                return _userGroupRightRepository ??
                       (_userGroupRightRepository = new GenericRepository<UserGroupRightEntity>(_context));
            }
        }

        public IGenericRepository<UserImageManagementEntity> UserImageManagementRepository
        {
            get
            {
                return _userImageManagementRepository ??
                       (_userImageManagementRepository = new GenericRepository<UserImageManagementEntity>(_context));
            }
        }

        public IGenericRepository<UserLockoutEntity> UserLockoutRepository
        {
            get
            {
                return _userLockoutRepository ??
                       (_userLockoutRepository = new GenericRepository<UserLockoutEntity>(_context));
            }
        }

        public CloneDeployUserRepository UserRepository
        {
            get { return _userRepository ?? (_userRepository = new CloneDeployUserRepository(_context)); }
        }

        public IGenericRepository<UserRightEntity> UserRightRepository
        {
            get
            {
                return _userRightRepository ?? (_userRightRepository = new GenericRepository<UserRightEntity>(_context));
            }
        }

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
    }
}