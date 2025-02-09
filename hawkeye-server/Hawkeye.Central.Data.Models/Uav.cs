using System;

namespace Hawkeye.Central.Data.Models
{
    public class Uav
    {
        public Guid Id { get; set; }

        public string Name { get; set; } = string.Empty;

        public string? Description { get; set; }
    }
}
