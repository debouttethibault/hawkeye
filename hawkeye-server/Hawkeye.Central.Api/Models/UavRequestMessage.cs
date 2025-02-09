using System.Security.Cryptography;
using System.Text.Json;

namespace Hawkeye.Central.Api.Models
{
    public class UavCommandMessage
    {
        public Guid UavId { get; set; }
        public JsonElement? Payload { get; set; }
    }
    public class UavRequestMessage : UavCommandMessage
    {
        public string Nonce { get; private init; } = GenerateCorrelationId();
         
        private static string GenerateCorrelationId()
        {
            var buffer = new byte[20];
            using var rnd = RandomNumberGenerator.Create();
            rnd.GetBytes(buffer);
            return Convert.ToBase64String(buffer);
        }
    }
}
