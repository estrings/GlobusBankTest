using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Customer.API.Data.Repository
{
    public interface IGenericRepository<T> where T : class
    {
        T GetById_IsGuid(Guid id);

        Task<T> GetById_IsGuid_Async(Guid id);
        IQueryable<T> Query();

        ICollection<T> GetAll();

        Task<List<T>> GetAllByList();

        Task<ICollection<T>> GetAllAsync();

        T GetById(long id);

        Task<T> GetByIdAsync(long id);

        T GetByUniqueId(string id);

        Task<T> GetByUniqueIdAsync(string id);

        T Find(Expression<Func<T, bool>> match);

        Task<T> FindAsync(Expression<Func<T, bool>> match);

        Task<T> FirstAsync();

        ICollection<T> FindAll(Expression<Func<T, bool>> match);
        IEnumerable<T> GetWithInclude(
            Expression<Func<T, bool>> filter = null,
            Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null, Expression<Func<T, IEnumerable<T>>> includeOther = null,
            string includeProperties = "");

        Task<IEnumerable<T>> GetWithIncludeÄsync(
           Expression<Func<T, bool>> filter = null,
           Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null, Expression<Func<T, IEnumerable<T>>> includeOther = null,
           string includeProperties = "");

        Task<ICollection<T>> FindAllAsync(Expression<Func<T, bool>> match);

        T Add(T entity);

        Task<T> AddAsync(T entity);

        Task<IList<T>> AddRangeAsync(IList<T> entity);

        T Update(T updated);

        Task<T> UpdateAsync(T updated);
        Task<IList<T>> UpdateRangeAsync(IList<T> entity);
        void Delete(T t);

        Task<int> DeleteAsync(T t);

        int Count();

        Task<int> CountAsync();
        Task<int> CountAsyncFilter(Expression<Func<T, bool>> filter = null);
        IEnumerable<T> Filter(
            Expression<Func<T, bool>> filter = null,
            Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null,
            string includeProperties = "",
            int? page = null,
            int? pageSize = null);
        IQueryable<T> FindBy(Expression<Func<T, bool>> predicate);

        bool Exist(Expression<Func<T, bool>> predicate);
        IEnumerable<T> SqlQuery(string sql);
        T Detach(T updated);
        IEnumerable<TEntity> SqlQuery2<TEntity>(string sql, params object[] parameters) where TEntity : new();
    }
}
