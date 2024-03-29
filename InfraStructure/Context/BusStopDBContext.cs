using Core.Identity;
using InfraStructure.Seeding;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Context
{
    public class BusStopDBContext : IdentityDbContext<BusStopManger>
    {
        public BusStopDBContext(DbContextOptions<BusStopDBContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.SeedBusStopManagerData();
        }
    }
}
