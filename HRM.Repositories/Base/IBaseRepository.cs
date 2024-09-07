namespace HRM.Repositories.Base
{
    public interface IBaseRepository<T>
        where T : class
    {
        Task AddAsync(T entity);
        Task RemoveAsync(int id);
        void Update(T entity);
        Task<IEnumerable<T>> GetPaginatedAsync(int pageNumber, int pageSize);
        Task<int> SaveChangeAsync();
        IQueryable<T> GetAllQueryAble();
        Task AddRangeAsync(IEnumerable<T> entities);
        void UpdateMany(IEnumerable<T> entities);
    }
}
