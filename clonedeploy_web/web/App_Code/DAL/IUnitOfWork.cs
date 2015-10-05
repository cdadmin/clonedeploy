using System;

namespace DAL
{
    public interface IUnitOfWork: IDisposable
    {
        DAL.IGenericRepository<Models.Computer> ComputerRepository { get; }
        bool Save();
    }
}