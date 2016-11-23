using System;
using CloneDeploy_Web.Models;

namespace DAL
{
    public class UnitOfWork : IUnitOfWork 
    {
        private CloneDeployDbContext _context = new CloneDeployDbContext();
      
        private IGenericRepository<ActiveMulticastSession> _activeMulticastSessionRepository;
        private BuildingRepository _buildingRepository;
        private IGenericRepository<DistributionPoint> _distributionPointRepository;
        private IGenericRepository<FileFolder> _fileFolderRepository;
        private IGenericRepository<ComputerBootMenu> _computerBootMenuRepository;
        private IGenericRepository<GroupBootMenu> _groupBootMenuRepository;
        private IGenericRepository<GroupMembership> _groupMembershipRepository;
        private IGenericRepository<Image> _imageRepository;
        private IGenericRepository<ImageProfilePartitionLayout> _imageProfilePartitionRepository;
        private IGenericRepository<ImageProfile> _imageProfileRepository;
        private IGenericRepository<Partition> _partitionRepository;
        private IGenericRepository<PartitionLayout> _partitionLayoutRepository;
        private IGenericRepository<Port> _portRepository;
        private IGenericRepository<ComputerLog> _computerLogRepository;
        private IGenericRepository<UserRight> _userRightRepository;
        private IGenericRepository<UserImageManagement> _userImageManagementRepository;
        private IGenericRepository<UserGroupManagement> _userGroupManagementRepository;
        private IGenericRepository<UserGroupRight> _userGroupRightRepository;
        private IGenericRepository<UserGroupImageManagement> _userGroupImageManagementRepository;
        private IGenericRepository<UserGroupGroupManagement> _userGroupGroupManagementRepository;
        private IGenericRepository<UserLockout> _userLockoutRepository;
        private IGenericRepository<GroupProperty> _groupPropertyRepository;
        private RoomRepository _roomRepository;
        private IGenericRepository<Script> _scriptRepository;
        private IGenericRepository<Setting> _settingRepository;
        private SiteRepository _siteRepository;
        private IGenericRepository<SysprepTag> _sysprepTagRepository;
        private IGenericRepository<CloneDeployUser> _userRepository;
        private IGenericRepository<BootTemplate> _bootTemplateRepository;
        private IGenericRepository<CdVersion> _cdVersionRepository;
        private ComputerRepository _computerRepository;
        private IGenericRepository<ImageProfileScript> _imageProfileScriptRepository;
        private IGenericRepository<ImageProfileFileFolder> _imageProfileFileFolderRepository;
        private IGenericRepository<ImageProfileSysprepTag> _imageProfileSysprepRepository;
        private GroupRepository _groupRepository;
        private ActiveImagingTaskRepository _activeImagingTaskRepository;
        private IGenericRepository<MunkiManifestTemplate> _munkiManifestRepository;
        private IGenericRepository<MunkiManifestCatalog> _munkiCatalogRepository;
        private DAL.IGenericRepository<MunkiManifestManagedInstall> _munkiManagedInstallRepository;
        private DAL.IGenericRepository<MunkiManifestManagedUnInstall> _munkiManagedUninstallRepository;
        private DAL.IGenericRepository<MunkiManifestManagedUpdate> _munkiManagedUpdateRepository;
        private DAL.IGenericRepository<MunkiManifestOptionInstall> _munkiOptionalInstallRepository;
        private DAL.IGenericRepository<MunkiManifestIncludedManifest> _munkiIncludedManifestRepository;
        private IGenericRepository<ComputerMunki> _computerMunkiRepository;
        private IGenericRepository<GroupMunki> _groupMunkiRepository;
        private IGenericRepository<ComputerProxyReservation> _computerProxyRepository;
        private IGenericRepository<BootEntry> _bootEntryRepository;
        private IGenericRepository<CloneDeployUserGroup> _userGroupRepository;


        public IGenericRepository<CloneDeployUserGroup> UserGroupRepository
        {
            get { return _userGroupRepository ?? (_userGroupRepository = new GenericRepository<CloneDeployUserGroup>(_context)); }
        }

        public IGenericRepository<BootEntry> BootEntryRepository
        {
            get { return _bootEntryRepository ?? (_bootEntryRepository = new GenericRepository<BootEntry>(_context)); }
        }

        public IGenericRepository<ComputerProxyReservation> ComputerProxyRepository
        {
            get { return _computerProxyRepository ?? (_computerProxyRepository = new GenericRepository<ComputerProxyReservation>(_context)); }
        }

        public IGenericRepository<ComputerMunki> ComputerMunkiRepository
        {
            get { return _computerMunkiRepository ?? (_computerMunkiRepository = new GenericRepository<ComputerMunki>(_context)); }

        }

        public IGenericRepository<GroupMunki> GroupMunkiRepository
        {
            get { return _groupMunkiRepository ?? (_groupMunkiRepository = new GenericRepository<GroupMunki>(_context)); }

        }

        public IGenericRepository<GroupBootMenu> GroupBootMenuRepository
        {
            get { return _groupBootMenuRepository ?? (_groupBootMenuRepository = new GenericRepository<GroupBootMenu>(_context)); }

        }

        public IGenericRepository<MunkiManifestCatalog> MunkiCatalogRepository
        {
            get { return _munkiCatalogRepository ?? (_munkiCatalogRepository = new GenericRepository<MunkiManifestCatalog>(_context)); }

        }

        public IGenericRepository<MunkiManifestManagedInstall> MunkiManagedInstallRepository
        {
            get { return _munkiManagedInstallRepository ?? (_munkiManagedInstallRepository = new GenericRepository<MunkiManifestManagedInstall>(_context)); }
        }

        public IGenericRepository<MunkiManifestManagedUnInstall> MunkiManagedUnInstallRepository
        {
            get { return _munkiManagedUninstallRepository ?? (_munkiManagedUninstallRepository = new GenericRepository<MunkiManifestManagedUnInstall>(_context)); }
        }

        public IGenericRepository<MunkiManifestManagedUpdate> MunkiManagedUpdateRepository
        {
            get { return _munkiManagedUpdateRepository ?? (_munkiManagedUpdateRepository = new GenericRepository<MunkiManifestManagedUpdate>(_context)); }
        }

        public IGenericRepository<MunkiManifestOptionInstall> MunkiOptionalInstallRepository
        {
            get { return _munkiOptionalInstallRepository ?? (_munkiOptionalInstallRepository = new GenericRepository<MunkiManifestOptionInstall>(_context)); }
        }

        public IGenericRepository<MunkiManifestIncludedManifest> MunkiIncludedManifestRepository
        {
            get { return _munkiIncludedManifestRepository ?? (_munkiIncludedManifestRepository = new GenericRepository<MunkiManifestIncludedManifest>(_context)); }
        }

        public IGenericRepository<MunkiManifestTemplate> MunkiManifestRepository
        {
            get { return _munkiManifestRepository ?? (_munkiManifestRepository = new GenericRepository<MunkiManifestTemplate>(_context)); }

        }

        public IGenericRepository<FileFolder> FileFolderRepository
        {
            get { return _fileFolderRepository ?? (_fileFolderRepository = new GenericRepository<FileFolder>(_context)); }

        }

        public IGenericRepository<GroupProperty> GroupPropertyRepository
        {
            get { return _groupPropertyRepository ?? (_groupPropertyRepository = new GenericRepository<GroupProperty>(_context)); }

        }

        public IGenericRepository<UserLockout> UserLockoutRepository
        {
            get { return _userLockoutRepository ?? (_userLockoutRepository = new GenericRepository<UserLockout>(_context)); }

        }

        public IGenericRepository<ActiveMulticastSession> ActiveMulticastSessionRepository
        {
            get { return _activeMulticastSessionRepository ?? (_activeMulticastSessionRepository = new GenericRepository<ActiveMulticastSession>(_context)); }

        }

        public IGenericRepository<ComputerBootMenu> ComputerBootMenuRepository
        {
            get { return _computerBootMenuRepository ?? (_computerBootMenuRepository = new GenericRepository<ComputerBootMenu>(_context)); }

        }

        public IGenericRepository<UserGroupManagement> UserGroupManagementRepository
        {
            get { return _userGroupManagementRepository ?? (_userGroupManagementRepository = new GenericRepository<UserGroupManagement>(_context)); }

        }

        public IGenericRepository<UserImageManagement> UserImageManagementRepository
        {
            get { return _userImageManagementRepository ?? (_userImageManagementRepository = new GenericRepository<UserImageManagement>(_context)); }

        }

        public IGenericRepository<UserGroupGroupManagement> UserGroupGroupManagementRepository
        {
            get { return _userGroupGroupManagementRepository ?? (_userGroupGroupManagementRepository = new GenericRepository<UserGroupGroupManagement>(_context)); }

        }

        public IGenericRepository<UserGroupImageManagement> UserGroupImageManagementRepository
        {
            get { return _userGroupImageManagementRepository ?? (_userGroupImageManagementRepository = new GenericRepository<UserGroupImageManagement>(_context)); }

        }

        public ComputerRepository ComputerRepository
        {
            get { return _computerRepository ?? (_computerRepository = new ComputerRepository(_context)); }
        }

        public BuildingRepository BuildingRepository
        {
            get { return _buildingRepository ?? (_buildingRepository = new BuildingRepository(_context)); }
        }

        public IGenericRepository<DistributionPoint> DistributionPointRepository
        {
            get { return _distributionPointRepository ?? (_distributionPointRepository = new GenericRepository<DistributionPoint>(_context)); }
        }

        public IGenericRepository<GroupMembership> GroupMembershipRepository
        {
            get { return _groupMembershipRepository ?? (_groupMembershipRepository = new GenericRepository<GroupMembership>(_context)); }
        }

        public IGenericRepository<Image> ImageRepository
        {
            get { return _imageRepository ?? (_imageRepository = new GenericRepository<Image>(_context)); }
        }

        public IGenericRepository<ImageProfilePartitionLayout> ImageProfilePartitionRepository
        {
            get { return _imageProfilePartitionRepository ?? (_imageProfilePartitionRepository = new GenericRepository<ImageProfilePartitionLayout>(_context)); }
        }

        public IGenericRepository<ImageProfile> ImageProfileRepository
        {
            get { return _imageProfileRepository ?? (_imageProfileRepository = new GenericRepository<ImageProfile>(_context)); }
        }

        public IGenericRepository<Partition> PartitionRepository
        {
            get { return _partitionRepository ?? (_partitionRepository = new GenericRepository<Partition>(_context)); }
        }

        public IGenericRepository<PartitionLayout> PartitionLayoutRepository
        {
            get { return _partitionLayoutRepository ?? (_partitionLayoutRepository = new GenericRepository<PartitionLayout>(_context)); }
        }

        public IGenericRepository<Port> PortRepository
        {
            get { return _portRepository ?? (_portRepository = new GenericRepository<Port>(_context)); }
        }

        public IGenericRepository<ComputerLog> ComputerLogRepository
        {
            get { return _computerLogRepository ?? (_computerLogRepository = new GenericRepository<ComputerLog>(_context)); }
        }

        public IGenericRepository<UserRight> UserRightRepository
        {
            get { return _userRightRepository ?? (_userRightRepository = new GenericRepository<UserRight>(_context)); }
        }

        public IGenericRepository<UserGroupRight> UserGroupRightRepository
        {
            get { return _userGroupRightRepository ?? (_userGroupRightRepository = new GenericRepository<UserGroupRight>(_context)); }
        }


        public RoomRepository RoomRepository
        {
            get { return _roomRepository ?? (_roomRepository = new RoomRepository(_context)); }
        }

        public IGenericRepository<Script> ScriptRepository
        {
            get { return _scriptRepository ?? (_scriptRepository = new GenericRepository<Script>(_context)); }
        }

        public IGenericRepository<Setting> SettingRepository
        {
            get { return _settingRepository ?? (_settingRepository = new GenericRepository<Setting>(_context)); }
        }

        public SiteRepository SiteRepository
        {
            get { return _siteRepository ?? (_siteRepository = new SiteRepository(_context)); }
        }

        public IGenericRepository<SysprepTag> SysprepTagRepository
        {
            get { return _sysprepTagRepository ?? (_sysprepTagRepository = new GenericRepository<SysprepTag>(_context)); }
        }

        public IGenericRepository<CloneDeployUser> UserRepository
        {
            get { return _userRepository ?? (_userRepository = new GenericRepository<CloneDeployUser>(_context)); }
        }

        public IGenericRepository<BootTemplate> BootTemplateRepository
        {
            get { return _bootTemplateRepository ?? (_bootTemplateRepository = new GenericRepository<BootTemplate>(_context)); }
        }

        public IGenericRepository<ImageProfileScript> ImageProfileScriptRepository
        {
            get { return _imageProfileScriptRepository ?? (_imageProfileScriptRepository = new GenericRepository<ImageProfileScript>(_context)); }
        }

        public IGenericRepository<CdVersion> CdVersionRepository
        {
            get { return _cdVersionRepository ?? (_cdVersionRepository = new GenericRepository<CdVersion>(_context)); }
        }

        public IGenericRepository<ImageProfileFileFolder> ImageProfileFileFolderRepository
        {
            get { return _imageProfileFileFolderRepository ?? (_imageProfileFileFolderRepository = new GenericRepository<ImageProfileFileFolder>(_context)); }
        }

        public IGenericRepository<ImageProfileSysprepTag> ImageProfileSysprepRepository
        {
            get { return _imageProfileSysprepRepository ?? (_imageProfileSysprepRepository = new GenericRepository<ImageProfileSysprepTag>(_context)); }
        }

        public GroupRepository GroupRepository
        {
            get { return _groupRepository ?? (_groupRepository = new GroupRepository(_context)); }
        }

        public ActiveImagingTaskRepository ActiveImagingTaskRepository
        {
            get { return _activeImagingTaskRepository ?? (_activeImagingTaskRepository = new ActiveImagingTaskRepository(_context)); }
        }

        public bool Save()
        {
            return _context.SaveChanges() > 0;
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
