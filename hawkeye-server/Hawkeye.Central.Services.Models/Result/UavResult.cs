using System;

namespace Hawkeye.Central.Services.Models.Result;

public class UavResult
{
    public Guid Id { get; set; }
    
    public string Name { get; set; } = null!;

    public string? Description { get; set; }
}