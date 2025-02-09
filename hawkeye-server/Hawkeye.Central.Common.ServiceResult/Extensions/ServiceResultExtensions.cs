using System.Collections.Generic;
using Hawkeye.Central.Common.ServiceResult;

namespace Hawkeye.Central.Common.ServiceResult.Extensions
{
    public static class ServiceResultExtensions
    {
        public static TServiceResult WithError<TServiceResult>(this TServiceResult result, ServiceError error) where TServiceResult : ServiceResult
        {
            result.Errors.Add(error);
            return result;
        }

        public static TServiceResult WithError<TServiceResult>(this TServiceResult result, string message, string? propertyName = null) where TServiceResult : ServiceResult
            => result.WithError(new ServiceError(message) { PropertyName = propertyName });

        public static TServiceResult WithWarning<TServiceResult>(this TServiceResult result, string message, string? propertyName = null) where TServiceResult : ServiceResult
            => result.WithError(new ServiceError(message, ServiceErrorSeverity.Warning) { PropertyName = propertyName });

        public static TServiceResult WithInformation<TServiceResult>(this TServiceResult result, string message, string? propertyName = null) where TServiceResult : ServiceResult
            => result.WithError(new ServiceError(message, ServiceErrorSeverity.Information) { PropertyName = propertyName });
    }
}
