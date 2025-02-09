using System;
using System.Collections.Generic;

namespace Hawkeye.Central.Common.ServiceResult
{
    public class ServiceResult<T> : ServiceResult where T : class
    {
        public T? Data { get; set; }

        public ServiceResult() { }

        public ServiceResult(T data)
        {
            Data = data;
        }

        public ServiceResult(ServiceResultType type, T? data = null) : base(type)
        {
            Data = data;
        }
    }

    public class ServiceResult
    {
        public ServiceResultType Type { get; set; } = ServiceResultType.Ok;
        public IList<ServiceError> Errors { get; set; } = new List<ServiceError>();

        public ServiceResult() { }

        public ServiceResult(ServiceResultType type)
        {
            Type = type;
        }
    }
}
