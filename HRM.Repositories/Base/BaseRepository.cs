using HRM.Data.Data;
using Microsoft.EntityFrameworkCore;

namespace HRM.Repositories.Base
{
    public class BaseRepository<T> : IBaseRepository<T>
        where T : class
    {
        protected readonly HRMDbContext _context;
        protected readonly DbSet<T> _dbSet;

        HRMDbContext IBaseRepository<T>.Context => _context;

        public BaseRepository(HRMDbContext context)
        {
            _context = context;
            _dbSet = _context.Set<T>();
        }
        public async Task AddAsync(T entity)
        {
            await _dbSet.AddAsync(entity);
        }

        public async Task AddRangeAsync(IEnumerable<T> entities)
        {
            await _dbSet.AddRangeAsync(entities);
        }

        public async Task<List<T>> CallStoredProcedureAsync(string storedProcedure, params object[] parameters)
        {
            var sql = storedProcedure + " " + string.Join(", ", parameters.Select((p, i) => $"@p{i}"));
            return await _dbSet.FromSqlRaw(sql, parameters).ToListAsync();
        }
        public async Task<T> CallStoredProcedureAsyncDetail(string storedProcedure, params object[] parameters)
        {
            var sql = storedProcedure + " " + string.Join(", ", parameters.Select((p, i) => $"@p{i}"));
            return await _dbSet.FromSqlRaw(sql, parameters).FirstAsync();
        }
        public async Task<int> ExecuteStoredProcedureAsync(string storedProcedure, params object[] parameters)
        {
            var sql = storedProcedure + " " + string.Join(", ", parameters.Select((p, i) => $"@p{i}"));
            return await _context.Database.ExecuteSqlRawAsync(sql, parameters);
        }

        public IQueryable<T> GetAllQueryAble()
        {
            return _dbSet;
        }

        public async Task<IEnumerable<T>> GetPaginatedAsync(int pageNumber, int pageSize)
        {
            return await _dbSet.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToListAsync();
        }

        public async Task RemoveAsync(int id)
        {
            var res = await _dbSet.FindAsync(id);
            if (res != null)
            {
                _dbSet.Remove(res);
            }
        }
        public void RemoveRange(IEnumerable<T> entities)
        {
             _dbSet.RemoveRange(entities);
        }

        public void RemoveRangeByEntitiesAsync(List<T> entities)
        {
            if (entities != null && entities.Count > 0)
            {
                _dbSet.RemoveRange(entities);
            }
        }

        public async Task<int> SaveChangeAsync()
        {
            return await _context.SaveChangesAsync();
        }

        public void Update(T entity)
        {
            _dbSet.Update(entity);
        }

        public void UpdateMany(IEnumerable<T> entities)
        {
            _dbSet.UpdateRange(entities);
        }
    }
}
