using HRM.Repositories.Dtos.Results;

namespace HRM.Repositories.Helper
{
    public static class Pagination<T> where T : class
    {
        public static PagedResult<T> PageList(int pageNumber, int totalItemCount, int pageSize, IEnumerable<T> metadata)
        {
            int pageCount = (int)Math.Ceiling((double)totalItemCount / pageSize);
            return new PagedResult<T>
            {
                PageCount = pageCount,
                TotalItemCount = totalItemCount,
                PageNumber = pageNumber,
                PageSize = pageSize,
                HasPreviousPage = pageNumber == 1 ? false : true,
                HasNextPage = pageNumber == pageCount ? false : true,
                IsFirstPage = pageNumber == 1 ? true : false,
                IsLastPage = pageNumber == pageCount ? true : false,
                Items = metadata
            };
        }
    }
}
