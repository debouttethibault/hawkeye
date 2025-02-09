using MQTTnet.Diagnostics.Logger;

namespace Hawkeye.Central.Api
{
    public class MqttNetLogger(ILogger<MqttNetLogger> logger) : IMqttNetLogger
    {
        public bool IsEnabled => true;

        public void Publish(MqttNetLogLevel logLevel, string source, string message, object[] parameters, Exception exception)
        {
            logger.Log(GetLogLevel(logLevel), message, parameters);
        }

        private static LogLevel GetLogLevel(MqttNetLogLevel logLevel) => logLevel switch
        {
            MqttNetLogLevel.Verbose => LogLevel.Debug,
            MqttNetLogLevel.Info => LogLevel.Information,
            MqttNetLogLevel.Warning => LogLevel.Warning,
            MqttNetLogLevel.Error => LogLevel.Error,
            _ => LogLevel.Information,
        };

    }
}
