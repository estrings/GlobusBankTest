using System;
using System.Threading.Tasks;
using Customer.API.Data.Repository;

namespace Customer.API.Data.UnitOfWork
{
    public interface IUnitofWork : IDisposable
    {
        IGenericRepository<T> Repository<T>() where T : class;
        Task<int> Commit();
        void Rollback();
    }
}
