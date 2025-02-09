namespace Hawkeye.Central.Common.ServiceResult.Paging
{
    public class PagingResult : PagingRequest
    {
        public PagingResult() { }

        public PagingResult(PagingRequest request, int totalCount)
        {
            Page = request.Page;
            PageSize = request.PageSize;
            TotalCount = totalCount;
        }

        public int TotalCount { get; set; }
    }
}
