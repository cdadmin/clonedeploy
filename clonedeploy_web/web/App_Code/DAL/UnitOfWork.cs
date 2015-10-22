using System;

namespace DAL
{
    public class UnitOfWork : IUnitOfWork 
    {
        private CloneDeployDbContext _context = new CloneDeployDbContext();
      
        private IGenericRepository<Models.ActiveMulticastSession> _activeMulticastSessionRepository;
        private BuildingRepository _buildingRepository;
        private IGenericRepository<Models.DistributionPoint> _distributionPointRepository;
        private IGenericRepository<Models.ComputerBootMenu> _computerBootMenuRepository;
        private IGenericRepository<Models.GroupBootMenu> _groupBootMenuRepository;
        private IGenericRepository<Models.GroupMembership> _groupMembershipRepository;
        private IGenericRepository<Models.Image> _imageRepository;
        private IGenericRepository<Models.ImageProfilePartition> _imageProfilePartitionRepository;
        private IGenericRepository<Models.LinuxProfile> _linuxProfileRepository;
        private IGenericRepository<Models.Partition> _partitionRepository;
        private IGenericRepository<Models.PartitionLayout> _partitionLayoutRepository;
        private IGenericRepository<Models.Port> _portRepository;
        private IGenericRepository<Models.ComputerLog> _computerLogRepository;
        private IGenericRepository<Models.UserRight> _userRightRepository;
        private IGenericRepository<Models.UserImageManagement> _userImageManagementRepository;
        private IGenericRepository<Models.UserGroupManagement> _userGroupManagementRepository;
        private IGenericRepository<Models.UserLockout> _userLockoutRepository;
        private IGenericRepository<Models.GroupProperty> _groupPropertyRepository;
        private RoomRepository _roomRepository;
        private IGenericRepository<Models.Script> _scriptRepository;
        private IGenericRepository<Models.Setting> _settingRepository;
        private SiteRepository _siteRepository;
        private IGenericRepository<Models.SysprepTag> _sysprepTagRepository;
        private IGenericRepository<Models.WdsUser> _userRepository;
        private IGenericRepository<Models.BootTemplate> _bootTemplateRepository;
        private ComputerRepository _computerRepository;
        private ImageProfileScriptRepository _imageProfileScriptRepository;
        private GroupRepository _groupRepository;
        private ActiveImagingTaskRepository _activeImagingTaskRepository;

        public IGenericRepository<Models.GroupBootMenu> GroupBootMenuRepository
        {
            get { return _groupBootMenuRepository ?? (_groupBootMenuRepository = new GenericRepository<Models.GroupBootMenu>(_context)); }

        }

        public IGenericRepository<Models.GroupProperty> GroupPropertyRepository
        {
            get { return _groupPropertyRepository ?? (_groupPropertyRepository = new GenericRepository<Models.GroupProperty>(_context)); }

        }

        public IGenericRepository<Models.UserLockout> UserLockoutRepository
        {
            get { return _userLockoutRepository ?? (_userLockoutRepository = new GenericRepository<Models.UserLockout>(_context)); }

        }

        public IGenericRepository<Models.ActiveMulticastSession> ActiveMulticastSessionRepository
        {
            get { return _activeMulticastSessionRepository ?? (_activeMulticastSessionRepository = new GenericRepository<Models.ActiveMulticastSession>(_context)); }

        }

        public IGenericRepository<Models.ComputerBootMenu> ComputerBootMenuRepository
        {
            get { return _computerBootMenuRepository ?? (_computerBootMenuRepository = new GenericRepository<Models.ComputerBootMenu>(_context)); }

        }

        public IGenericRepository<Models.UserGroupManagement> UserGroupManagementRepository
        {
            get { return _userGroupManagementRepository ?? (_userGroupManagementRepository = new GenericRepository<Models.UserGroupManagement>(_context)); }

        }

        public IGenericRepository<Models.UserImageManagement> UserImageManagementRepository
        {
            get { return _userImageManagementRepository ?? (_userImageManagementRepository = new GenericRepository<Models.UserImageManagement>(_context)); }

        }
        public ComputerRepository ComputerRepository
        {
            get { return _computerRepository ?? (_computerRepository = new ComputerRepository(_context)); }
        }

        public BuildingRepository BuildingRepository
        {
            get { return _buildingRepository ?? (_buildingRepository = new BuildingRepository(_context)); }
        }

        public IGenericRepository<Models.DistributionPoint> DistributionPointRepository
        {
            get { return _distributionPointRepository ?? (_distributionPointRepository = new GenericRepository<Models.DistributionPoint>(_context)); }
        }

        public IGenericRepository<Models.GroupMembership> GroupMembershipRepository
        {
            get { return _groupMembershipRepository ?? (_groupMembershipRepository = new GenericRepository<Models.GroupMembership>(_context)); }
        }

        public IGenericRepository<Models.Image> ImageRepository
        {
            get { return _imageRepository ?? (_imageRepository = new GenericRepository<Models.Image>(_context)); }
        }

        public IGenericRepository<Models.ImageProfilePartition> ImageProfilePartitionRepository
        {
            get { return _imageProfilePartitionRepository ?? (_imageProfilePartitionRepository = new GenericRepository<Models.ImageProfilePartition>(_context)); }
        }

        public IGenericRepository<Models.LinuxProfile> LinuxProfileRepository
        {
            get { return _linuxProfileRepository ?? (_linuxProfileRepository = new GenericRepository<Models.LinuxProfile>(_context)); }
        }

        public IGenericRepository<Models.Partition> PartitionRepository
        {
            get { return _partitionRepository ?? (_partitionRepository = new GenericRepository<Models.Partition>(_context)); }
        }

        public IGenericRepository<Models.PartitionLayout> PartitionLayoutRepository
        {
            get { return _partitionLayoutRepository ?? (_partitionLayoutRepository = new GenericRepository<Models.PartitionLayout>(_context)); }
        }

        public IGenericRepository<Models.Port> PortRepository
        {
            get { return _portRepository ?? (_portRepository = new GenericRepository<Models.Port>(_context)); }
        }

        public IGenericRepository<Models.ComputerLog> ComputerLogRepository
        {
            get { return _computerLogRepository ?? (_computerLogRepository = new GenericRepository<Models.ComputerLog>(_context)); }
        }

        public IGenericRepository<Models.UserRight> UserRightRepository
        {
            get { return _userRightRepository ?? (_userRightRepository = new GenericRepository<Models.UserRight>(_context)); }
        }

        public RoomRepository RoomRepository
        {
            get { return _roomRepository ?? (_roomRepository = new RoomRepository(_context)); }
        }

        public IGenericRepository<Models.Script> ScriptRepository
        {
            get { return _scriptRepository ?? (_scriptRepository = new GenericRepository<Models.Script>(_context)); }
        }

        public IGenericRepository<Models.Setting> SettingRepository
        {
            get { return _settingRepository ?? (_settingRepository = new GenericRepository<Models.Setting>(_context)); }
        }

        public SiteRepository SiteRepository
        {
            get { return _siteRepository ?? (_siteRepository = new SiteRepository(_context)); }
        }

        public IGenericRepository<Models.SysprepTag> SysprepTagRepository
        {
            get { return _sysprepTagRepository ?? (_sysprepTagRepository = new GenericRepository<Models.SysprepTag>(_context)); }
        }

        public IGenericRepository<Models.WdsUser> UserRepository
        {
            get { return _userRepository ?? (_userRepository = new GenericRepository<Models.WdsUser>(_context)); }
        }

        public IGenericRepository<Models.BootTemplate> BootTemplateRepository
        {
            get { return _bootTemplateRepository ?? (_bootTemplateRepository = new GenericRepository<Models.BootTemplate>(_context)); }
        }

        public ImageProfileScriptRepository ImageProfileScriptRepository
        {
            get { return _imageProfileScriptRepository ?? (_imageProfileScriptRepository = new ImageProfileScriptRepository(_context)); }
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
