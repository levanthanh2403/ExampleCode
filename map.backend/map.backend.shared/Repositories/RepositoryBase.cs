using map.backend.shared.Interfaces;
using Microsoft.EntityFrameworkCore.Query;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Npgsql;
using map.backend.shared.Persistence;
using map.backend.shared.Entities;
using Microsoft.AspNetCore.Mvc;
using MathNet.Numerics.Statistics.Mcmc;
using Microsoft.Office.Interop.Excel;

namespace map.backend.shared.Repositories
{
    public class RepositoryBase<T> : IAsyncRepository<T> where T : EntityBase
    {
        protected readonly AppDbContext _dbContext;
        protected readonly DbSet<T> _dbSet;
        public RepositoryBase(AppDbContext dbContext)
        {
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
            _dbSet = _dbContext.Set<T>();
        }
        public IQueryable<T> GetAll()
        {
            return _dbSet;
        }
        public async Task<IReadOnlyList<T>> GetAllAsync()
        {
            return await _dbContext.Set<T>().ToListAsync();
        }
        public async Task<IReadOnlyList<T>> GetAsync(Expression<Func<T, bool>> predicate)
        {
            //Expression<Func<T, bool>> other = s => s.branch_code == _dbContext.CurrentBranch;
            //var dt = AndExpression(predicate, other);
            return await _dbContext.Set<T>().Where(predicate).ToListAsync();
        }
        public async Task<IReadOnlyList<T>> GetAsync(Expression<Func<T, bool>> predicate = null,
                                                     Func<IQueryable<T>,
                                                     IOrderedQueryable<T>> orderBy = null,
                                                     string includeString = null,
                                                     bool disableTracking = true)
        {
            IQueryable<T> query = _dbContext.Set<T>();
            if (disableTracking) query = query.AsNoTracking();

            if (!string.IsNullOrWhiteSpace(includeString)) query = query.Include(includeString);

            //Expression<Func<T, bool>> other = s => s.branch_code == _dbContext.CurrentBranch;
            if (predicate != null)
                query = query.Where(predicate);

            if (orderBy != null)
                return await orderBy(query).ToListAsync();
            return await query.ToListAsync();
        }
        public async Task<IReadOnlyList<T>> GetAsync(Expression<Func<T, bool>> predicate = null,
                                                     Func<IQueryable<T>,
                                                     IOrderedQueryable<T>> orderBy = null,
                                                     List<Expression<Func<T, object>>> includes = null,
                                                     bool disableTracking = true)
        {
            IQueryable<T> query = _dbContext.Set<T>();
            if (disableTracking) query = query.AsNoTracking();

            // query = query.Where(p => p.branch_code == _dbContext.CurrentBranch);

            if (includes != null) query = includes.Aggregate(query, (current, include) => current.Include(include));

            if (predicate != null) query = query.Where(predicate);

            if (orderBy != null)
                return await orderBy(query).ToListAsync();
            return await query.ToListAsync();
        }
        public virtual async Task<T> GetByIdAsync(long id)
        {
            return await _dbContext.Set<T>().FindAsync(id);
        }
        public async Task<IEnumerable<T>> Get(
            Expression<Func<T, bool>> filter = null,
            Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null,
            string includeProperties = "")
        {
            IQueryable<T> query = _dbContext.Set<T>();
            if (filter != null)
            {
                query = query.Where(filter);
            }
            foreach (var includeProperty in includeProperties.Split
                (new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
            {
                query = query.Include(includeProperty);
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
        public IQueryable<T1> Get<TResult, T1>(
           Expression<Func<T, bool>> filter = null,
           Expression<Func<T, TResult>> orderBy = null,
           Func<IQueryable<T>, IQueryable<T1>> selector = null,
           string includeProperties = "")
        {
            IQueryable<T> query = _dbContext.Set<T>();
            if (filter != null)
            {
                query = query.Where(filter);
            }
            foreach (var includeProperty in includeProperties.Split
                (new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
            {
                query = query.Include(includeProperty);
            }
            if (orderBy != null)
            {
                query = query.OrderByDescending(orderBy);
            }
            if (selector != null)
            {
                return selector(query);
            }
            else
            {
                return (IQueryable<T1>)query.ToList();
            }
        }
        public virtual T GetFirstOrDefault(Expression<Func<T, bool>> predicate = null,
                                           Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null,
                                           Func<IQueryable<T>, IIncludableQueryable<T, object>> include = null,
                                           bool disableTracking = true,
                                           bool ignoreQueryFilters = false)
        {
            IQueryable<T> query = _dbSet;
            if (disableTracking)
            {
                query = query.AsNoTracking();
            }
            if (include != null)
            {
                query = include(query);
            }
            if (predicate != null)
            {
                query = query.Where(predicate);
            }
            if (ignoreQueryFilters)
            {
                query = query.IgnoreQueryFilters();
            }
            if (orderBy != null)
            {
                return orderBy(query).FirstOrDefault();
            }
            else
            {
                return query.FirstOrDefault();
            }
        }
        public virtual async Task<T> GetFirstOrDefaultAsync(Expression<Func<T, bool>> predicate = null,
                                                            Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null,
                                                            Func<IQueryable<T>, IIncludableQueryable<T, object>> include = null,
                                                            bool disableTracking = true,
                                                            bool ignoreQueryFilters = false)
        {
            IQueryable<T> query = _dbSet;

            if (disableTracking)
            {
                query = query.AsNoTracking();
            }

            if (include != null)
            {
                query = include(query);
            }

            if (predicate != null)
            {
                query = query.Where(predicate);
            }

            if (ignoreQueryFilters)
            {
                query = query.IgnoreQueryFilters();
            }

            if (orderBy != null)
            {
                return await orderBy(query).FirstOrDefaultAsync();
            }
            else
            {
                return await query.FirstOrDefaultAsync();
            }
        }
        public async Task<T> AddAsync(T entity)
        {
            _dbContext.Set<T>().Add(entity);
            await _dbContext.SaveChangesAsync();
            return entity;
        }
        public async Task AddRange(params T[] entities)
        {
            _dbSet.AddRange(entities);
            await _dbContext.SaveChangesAsync();
        }
        public async Task AddRange(IEnumerable<T> entities)
        {
            _dbSet.AddRange(entities);
            await _dbContext.SaveChangesAsync();
        }
        public async Task AddRangeAsync(params T[] entities)
        {
            await _dbSet.AddRangeAsync(entities);
            await _dbContext.SaveChangesAsync();
        }
        public async Task AddRangeAsync(IEnumerable<T> entities, CancellationToken cancellationToken = default(CancellationToken))
        {
            await _dbSet.AddRangeAsync(entities, cancellationToken);
            await _dbContext.SaveChangesAsync();
        }
        public async Task UpdateAsync(T entity)
        {
            var checkLocal = _dbContext.Set<T>()
                .Local
                .FirstOrDefault(p => p.id == entity.id);
            if (checkLocal != null)
            {
                _dbContext.Entry(checkLocal).State = EntityState.Detached;
            }
            _dbContext.Entry(entity).State = EntityState.Modified;
            await _dbContext.SaveChangesAsync();
        }
        public async Task UpdateRange(params T[] entities)
        {
            _dbSet.UpdateRange(entities);
            await _dbContext.SaveChangesAsync();
        }
        public async Task UpdateRange(IEnumerable<T> entities)
        {
            _dbSet.UpdateRange(entities);
            await _dbContext.SaveChangesAsync();
        }
        public async Task DeleteAsync(T entity)
        {
            _dbContext.Set<T>().Remove(entity);
            await _dbContext.SaveChangesAsync();
        }
        public async Task DeleteRange(params T[] entities)
        {
            _dbSet.RemoveRange(entities);
            await _dbContext.SaveChangesAsync();
        }
        public async Task DeleteRange(IEnumerable<T> entities)
        {
            _dbSet.RemoveRange(entities);
            await _dbContext.SaveChangesAsync();
        }
        #region Bo sung
        public IDbContextTransaction BeginTransaction()
        {
            return _dbContext.Database.BeginTransaction();
        }
        public async Task<IDbContextTransaction> BeginTransactionAync()
        {
            return await _dbContext.Database.BeginTransactionAsync();
        }
        public virtual IQueryable<T> FromSql(string sql)
        {
            return _dbSet.FromSqlRaw(sql);
        }
        public async Task ExecuteSQL(string sql, Array parameter)
        {
            await _dbContext.Database.ExecuteSqlRawAsync(sql, parameter);
        }
        public DbConnection GetConnection() => _dbContext.Database.GetDbConnection();
        #endregion
        #region Paging
        public IQueryable<T> GetQueryable(
            Expression<Func<T, bool>> filter = null,
            Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null,
            Func<IQueryable<T>, IIncludableQueryable<T, object>> include = null,
            bool disableTracking = true, bool disableActiveFilter = false)
        {
            IQueryable<T> query = _dbSet;
            if (disableTracking)
            {
                query = query.AsNoTracking();
            }

            if (include != null)
            {
                query = include(query);
            }

            if (filter != null)
            {
                query = query.Where(filter);
            }

            if (orderBy != null)
            {
                query = orderBy(query);
            }

            return query;
        }
        public async Task<PagedResult<T>> GetPaged(
            int page,
            int pageSize,
            Expression<Func<T, bool>> filter = null,
            Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null,
            Func<IQueryable<T>, IIncludableQueryable<T, object>> include = null,
            bool disableTracking = true,
            bool disableActiveFilter = false)
        {
            var query = GetQueryable(filter, orderBy, include, disableTracking, disableActiveFilter);
            var result = new PagedResult<T>();
            result.CurrentPage = page;
            result.PageSize = pageSize;
            result.RowCount = query.Count();


            var pageCount = (double)result.RowCount / pageSize;
            result.PageCount = (int)Math.Ceiling(pageCount);

            var skip = (page - 1) * pageSize;
            result.Results = await query.Skip(skip).Take(pageSize).ToListAsync();

            return result;
        }
        #endregion
    }
}
