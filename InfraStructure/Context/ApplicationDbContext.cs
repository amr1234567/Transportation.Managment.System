using Core.Identity;
using Core.Models;
using Infrastructure.Seeding;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Context
{
    public class ApplicationDbContext : IdentityDbContext<User, IdentityRole<Guid>, Guid>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {

        }

        public DbSet<ApplicationAdmin> Admins { get; set; }
        public DbSet<ApplicationUser> Users { get; set; }
        public DbSet<BusStopManger> BusStopMangers { get; set; }

        public DbSet<Bus> Buses { get; set; }
        public DbSet<BusStop> BusStops { get; set; }
        public DbSet<Journey> Journeys { get; set; }
        public DbSet<Seat> Seats { get; set; }
        public DbSet<Ticket> Tickets { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            //builder.SeedAppData();
            builder.Entity<ApplicationAdmin>().ToTable(nameof(ApplicationAdmin));
            builder.Entity<ApplicationUser>().ToTable(nameof(ApplicationUser));
            builder.Entity<BusStopManger>().ToTable(nameof(BusStopManger));
        }

    }
}
