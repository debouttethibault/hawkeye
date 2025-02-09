using System;
using System.ComponentModel.DataAnnotations;

namespace Hawkeye.Central.Common.ServiceResult.Paging
{
    public class PagingRequest
    {
        [Range(1, int.MaxValue)]
        public int Page { get; set; }
        [Range(1, 100)]
        public int PageSize { get; set; }
    }
}
