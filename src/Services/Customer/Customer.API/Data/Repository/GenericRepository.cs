using Customer.API.Data.DataAccess;
using Customer.API.Data.UnitOfWork;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Customer.API.Data.Repository
{
    public class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        private readonly Database _context;
        private readonly IUnitofWork _unitofWork;

        public GenericRepository(Database context)
        {
            _context = context;
            _unitofWork = new UnitofWork(context);
        }

        public T Add(T entity)
        {
            _context.Set<T>().Add(entity);
            _context.SaveChanges();
            return entity;
        }

        public async Task<T> AddAsync(T entity)
        {
            _context.Set<T>().Add(entity);
            await _unitofWork.Commit();
            return entity;
        }

        public async Task<IList<T>> AddRangeAsync(IList<T> entity)
        {
            _context.Set<T>().AddRange(entity);
            await _unitofWork.Commit();
            return entity;
        }
        public async Task<IList<T>> UpdateRangeAsync(IList<T> entity)
        {
            _context.Set<T>().UpdateRange(entity);
            await _unitofWork.Commit();
            return entity;
        }

        public int Count()
        {
            return _context.Set<T>().Count();
        }

        public async Task<int> CountAsync()
        {
            return await _context.Set<T>().CountAsync();
        }

        public void Delete(T t)
        {
            _context.Set<T>().Remove(t);
            _context.SaveChanges();
        }

        public async Task<int> DeleteAsync(T t)
        {
            _context.Set<T>().Remove(t);
            return await _unitofWork.Commit();
        }

        public bool Exist(Expression<Func<T, bool>> predicate)
        {
            IQueryable<T> exist = _context.Set<T>().Where(predicate);
            return exist.Any();
        }

        public IEnumerable<T> Filter(Expression<Func<T, bool>> filter = null, Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null,
            string includeProperties = "", int? page = null, int? pageSize = null)
        {
            IQueryable<T> query = _context.Set<T>();
            if (filter != null)
            {
                query = query.Where(filter);
            }

            if (orderBy != null)
            {
                query = orderBy(query);
            }

            if (includeProperties != null)
            {
                foreach (string includeProperty in includeProperties.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries)
                    .Where(x => !string.IsNullOrWhiteSpace(x))
                    .Select(x => x.Trim()))
                {
                    query = query.Include(includeProperty);
                }
            }

            if (page != null && pageSize != null)
            {
                query = query.Skip((page.Value - 1) * pageSize.Value).Take(pageSize.Value);
            }

            return query.ToList();
        }

        public T Find(Expression<Func<T, bool>> match)
        {
            return _context.Set<T>().SingleOrDefault(match);
        }

        public async Task<T> FindAsync(Expression<Func<T, bool>> match)
        {
            return await _context.Set<T>().SingleOrDefaultAsync(match);
        }

        public async Task<T> FirstAsync()
        {
            return await _context.Set<T>().FirstOrDefaultAsync();
        }

        public IQueryable<T> FindBy(Expression<Func<T, bool>> predicate)
        {
            return _context.Set<T>().Where(predicate);
        }

        public ICollection<T> FindAll(Expression<Func<T, bool>> match)
        {
            return _context.Set<T>().Where(match).ToList();
        }
        public IEnumerable<T> GetWithInclude(
            Expression<Func<T, bool>> filter = null,
            Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null, Expression<Func<T, IEnumerable<T>>> includeOther = null,
            string includeProperties = "")
        {
            IQueryable<T> query = _context.Set<T>();

            if (filter != null)
            {
                query = query.Where(filter);
            }

            foreach (string includeProperty in includeProperties.Split
                (new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
            {
                query = query.Include(includeProperty);
            }
            if (includeOther != null)
            {
                query = query.Include(includeOther);
            }

            if (orderBy != null)
            {
                return orderBy(query).ToList();
            }
            else
            {
                return query.ToList();
            }
        }
        public async Task<IEnumerable<T>> GetWithIncludeÄsync(
            Expression<Func<T, bool>> filter = null,
            Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null, Expression<Func<T, IEnumerable<T>>> includeOther = null,
            string includeProperties = "")
        {
            IQueryable<T> query = _context.Set<T>();

            if (filter != null)
            {
                query = query.Where(filter);
            }

            foreach (string includeProperty in includeProperties.Split
                (new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
            {
                query = query.Include(includeProperty);
            }
            if (includeOther != null)
            {
                query = query.Include(includeOther);
            }

            if (orderBy != null)
            {
                return await orderBy(query).ToListAsync();
            }
            else
            {
                return await query.ToListAsync();
            }
        }
        public async Task<ICollection<T>> FindAllAsync(Expression<Func<T, bool>> match)
        {
            return await _context.Set<T>().Where(match).ToListAsync();
        }

        public ICollection<T> GetAll()
        {
            return _context.Set<T>().ToList();
        }

        public async Task<ICollection<T>> GetAllAsync()
        {
            return await _context.Set<T>().ToListAsync();
        }

        public T GetById(long id)
        {
            return _context.Set<T>().Find(id);
        }

        public async Task<T> GetByIdAsync(long id)
        {
            return await _context.Set<T>().FindAsync(id);
        }
        public T GetById_IsGuid(Guid id)
        {
            return _context.Set<T>().Find(id);
        }

        public async Task<T> GetById_IsGuid_Async(Guid id)
        {
            return await _context.Set<T>().FindAsync(id);
        }
        public T GetByUniqueId(string id)
        {
            return _context.Set<T>().Find(id);
        }

        public async Task<T> GetByUniqueIdAsync(string id)
        {
            return await _context.Set<T>().FindAsync(id);
        }

        public IQueryable<T> Query()
        {
            return _context.Set<T>().AsQueryable();
        }

        public T Update(T updated)
        {
            if (updated is null)
            {
                return null;
            }

            _context.Set<T>().Attach(updated);
            _context.Entry(updated).State = EntityState.Modified;
            _context.SaveChanges();

            return updated;
        }

        public async Task<T> UpdateAsync(T updated)
        {
            if (updated is null)
            {
                return null;
            }

            _context.Set<T>().Attach(updated);
            _context.Entry(updated).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return updated;
        }

        async Task<List<T>> IGenericRepository<T>.GetAllByList()
        {
            return await _context.Set<T>().ToListAsync();
        }

        public async Task<int> CountAsyncFilter(Expression<Func<T, bool>> filter = null)
        {
            IQueryable<T> query = _context.Set<T>();
            if (filter != null)
            {
                query = query.Where(filter);
            }

            return await query.CountAsync();
        }

        public IEnumerable<T> SqlQuery(string sql)
        {
            var rawQuery = _context.Set<T>().FromSqlRaw(sql);
            return rawQuery.ToList();
        }

        public T Detach(T updated)
        {
            if (updated is null)
            {
                return null;
            }

            foreach (var dbEntityEntry in _context.ChangeTracker.Entries().ToArray())
            {

                if (dbEntityEntry.Entity != null)
                {
                    dbEntityEntry.State = EntityState.Detached;
                }
            }

            // set Modified flag in your entry

            _context.Entry(updated).State = EntityState.Modified;
            // save 
            _context.SaveChanges();
            return updated;
        }

        public virtual IEnumerable<TEntity> SqlQuery2<TEntity>(string sql, params object[] parameters) where TEntity : new()
        {
            return _context.Database.GetModelFromQuery<TEntity>(sql, parameters);
        }
    }
}
