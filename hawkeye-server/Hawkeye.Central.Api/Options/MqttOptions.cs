namespace Hawkeye.Central.Api.Options
{
    public class MqttOptions
    {
        public string ConnectionString { get; set; } = string.Empty;
        public string ClientId { get; set; } = string.Empty;
        public string Username { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
    }
}
