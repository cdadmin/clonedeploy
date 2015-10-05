using System;

namespace DAL
{
    public class UnitOfWork : IUnitOfWork 
    {
        private CloneDeployDbContext _context = new CloneDeployDbContext();
        private IGenericRepository<Models.Computer> _computerRepository;
        private Computer computer;

        public IGenericRepository<Models.Computer> ComputerRepository
        {
            get { return _computerRepository ?? (_computerRepository = new GenericRepository<Models.Computer>(_context)); }
        }

        public Computer Computer
        {
            get
            {

                if (this.computer == null)
                {
                    this.computer = new Computer(_context);
                }
                return computer;
            }
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
