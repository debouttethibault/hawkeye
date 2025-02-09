using System.Linq;
using Hawkeye.Central.Common.ServiceResult.Paging;

namespace Hawkeye.Central.Common.ServiceResult.Extensions
{
    public static class ServicePagingResultExtensions
    {
        public static IQueryable<T> ApplyPaging<T>(this IQueryable<T> query, PagingRequest request) where T : class
        {
            if (request.Page < 1)
                request.Page = 1;

            if (request.PageSize < 1)
                request.PageSize = 1;
            else if (request.PageSize > 1000)
                request.PageSize = 1000;

            return query
                .Skip((request.Page - 1) * request.PageSize)
                .Take(request.PageSize);
        }
    }
}
