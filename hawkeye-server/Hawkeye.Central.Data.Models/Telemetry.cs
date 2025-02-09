using System;
using System.Text.Json;

namespace Hawkeye.Central.Data.Models;

public class Telemetry
{
    public Guid? UavId { get; set; }
    public virtual Uav? Uav { get; set; }

    public string? Name { get; set; }
    public string? Key { get; set; }

    public DateTimeOffset CreatedOn { get; set; }

    public string? ContentType { get; set; }
    public JsonElement Content { get; set; }
}
