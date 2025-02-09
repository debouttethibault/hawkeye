namespace Hawkeye.Central.Common.ServiceResult
{
    public class ServiceError
    {
        public string Message { get; set; } = string.Empty;
        public ServiceErrorSeverity Severity { get; set; } = ServiceErrorSeverity.Error;
        public string? PropertyName { get; set; }

        public ServiceError(string message, string? propertyName = null)
        {
            Message = message;
            PropertyName = propertyName;
        }

        public ServiceError(string message, ServiceErrorSeverity severity, string? propertyName = null)
        {
            Message = message;
            Severity = severity;
            PropertyName = propertyName;
        }
    }
}
