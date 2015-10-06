using System;

namespace DAL
{
    public class UnitOfWork : IUnitOfWork 
    {
        private CloneDeployDbContext _context = new CloneDeployDbContext();
        private IGenericRepository<Models.Building> _buildingRepository;
        private IGenericRepository<Models.DistributionPoint> _distributionPointRepository;
        private IGenericRepository<Models.GroupMembership> _groupMembershipRepository;
        private IGenericRepository<Models.Image> _imageRepository;
        private IGenericRepository<Models.ImageProfilePartition> _imageProfilePartitionRepository;
        private IGenericRepository<Models.LinuxProfile> _linuxProfileRepository;
        private IGenericRepository<Models.Partition> _partitionRepository;
        private IGenericRepository<Models.PartitionLayout> _partitionLayoutRepository;
        private ImageProfileScriptRepository _imageProfileScriptRepository;
        private ComputerRepository _computerRepository;
        private GroupRepository _groupRepository;
        private ActiveImagingTaskRepository _activeImagingTaskRepository;
        private ActiveMulticastSessionRepository _activeMulticastSessionRepository;

        public IGenericRepository<Models.Building> BuildingRepository
        {
            get { return _buildingRepository ?? (_buildingRepository = new GenericRepository<Models.Building>(_context)); }
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

        public ImageProfileScriptRepository ImageProfileScriptRepository
        {
            get { return _imageProfileScriptRepository ?? (_imageProfileScriptRepository = new ImageProfileScriptRepository(_context)); }
        }

        public ComputerRepository ComputerRepository
        {
            get { return _computerRepository ?? (_computerRepository = new ComputerRepository(_context)); }
        }

        public GroupRepository GroupRepository
        {
            get { return _groupRepository ?? (_groupRepository = new GroupRepository(_context)); }
        }

        public ActiveImagingTaskRepository ActiveImagingTaskRepository
        {
            get { return _activeImagingTaskRepository ?? (_activeImagingTaskRepository = new ActiveImagingTaskRepository(_context)); }
        }

        public ActiveMulticastSessionRepository ActiveMulticastSessionRepository
        {
            get { return _activeMulticastSessionRepository ?? (_activeMulticastSessionRepository = new ActiveMulticastSessionRepository(_context)); }
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
