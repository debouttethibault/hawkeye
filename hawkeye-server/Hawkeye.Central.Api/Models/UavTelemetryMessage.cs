using System.Text.Json;

namespace Hawkeye.Central.Api.Models
{
    public class UavTelemetryMessage
    {
            public Guid UavId { get; set; }
            public JsonElement Data { get; set; }
    }
}
