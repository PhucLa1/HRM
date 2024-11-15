using HRM.Data.Data;
using HRM.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace HRM.Repositories.Base
{
    public interface IBaseRepository<T>
        where T : class
    {
        Task AddAsync(T entity);
        Task RemoveAsync(int id);
        void RemoveRangeByEntitiesAsync(List<T> entities);
        void Update(T entity);
        Task<IEnumerable<T>> GetPaginatedAsync(int pageNumber, int pageSize);
        Task<int> SaveChangeAsync();
        IQueryable<T> GetAllQueryAble();
        HRMDbContext Context { get; }
        Task AddRangeAsync(IEnumerable<T> entities);
        void UpdateMany(IEnumerable<T> entities);
        void RemoveRange(IEnumerable<T> entities);
        Task<List<T>> CallStoredProcedureAsync(string storedProcedure, params object[] parameters);
        Task<T> CallStoredProcedureAsyncDetail(string storedProcedure, params object[] parameters);
        Task<int> ExecuteStoredProcedureAsync(string storedProcedure, params object[] parameters);
        object Join(DbSet<Contract> contracts, Func<object, object> value1, Func<Contract, int> value2, Func<object, object, object> value3);
    }
}
