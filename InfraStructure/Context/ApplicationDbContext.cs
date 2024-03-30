using Core.Models;
using Infrastructure.Seeding;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Context
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {

        }

        public DbSet<Bus> Buses { get; set; }
        public DbSet<BusStop> BusStops { get; set; }
        public DbSet<Journey> Journeys { get; set; }
        public DbSet<Seat> Seats { get; set; }
        public DbSet<Ticket> Tickets { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            //builder.SeedAppData();
        }

    }
}
