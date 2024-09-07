using HRM.Data.Data;
using Microsoft.EntityFrameworkCore;

namespace HRM.Repositories.Base
{
    public class BaseRepository<T> : IBaseRepository<T>
        where T : class
    {
        protected readonly HRMDbContext _context;
        protected readonly DbSet<T> _dbSet;

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
