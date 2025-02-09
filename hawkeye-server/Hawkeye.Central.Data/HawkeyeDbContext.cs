using Hawkeye.Central.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace Hawkeye.Central.Data
{
    public class HawkeyeDbContext(DbContextOptions<HawkeyeDbContext> opts) : DbContext(opts)
    {
        public DbSet<Uav> Uavs { get; set; } = null!;
        public DbSet<Mission> Missions { get; set; } = null!;
    }
}
