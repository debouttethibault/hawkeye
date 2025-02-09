using System;
using System.Collections.Generic;

namespace Hawkeye.Central.Data.Models
{
    public class Mission
    {
        public Guid Id { get; set; }

        public string Name { get; set; } = string.Empty;

        public IList<MissionWaypoint> Waypoints { get; set; } = null!;
    }
}
