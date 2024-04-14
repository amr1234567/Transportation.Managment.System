using Core.Identity;
using Core.Models;
using InfraStructure.Helpers;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace Infrastructure.Context
{
    public class ApplicationDbContext : IdentityDbContext<User>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        public DbSet<ApplicationAdmin> Admins { get; set; }
        public DbSet<ApplicationUser> Users { get; set; }
        public DbSet<BusStopManger> BusStopMangers { get; set; }

        public DbSet<Bus> Buses { get; set; }
        public DbSet<UpcomingJourney> UpcomingJourneys { get; set; }
        public DbSet<JourneyHistory> Journeys { get; set; }
        public DbSet<Seat> Seats { get; set; }
        public DbSet<Ticket> Tickets { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.SeedAppData();
            builder.EditTables();
            //var busStops = BusStopMangers.ToList();
            //var journeys = new List<JourneyHistory>();
            //for (int i = 0; i < 500; i++)
            //{
            //    var randomNum = new Random().Next(BusStopMangers.Count());
            //    var destId = busStops[randomNum];
            //    var StartId = busStops[randomNum + 1];
            //    var JourneyId = Guid.NewGuid();
            //    var arrivalTime = DateTime.Now.AddHours(1);
            //    var LeavingTime = DateTime.Now;
            //    var tickets = new List<Ticket>();

            //    for (int j = 0; j < 40; j++)
            //    {
            //        var ticket = new Ticket
            //        {
            //            ArrivalTime = arrivalTime,
            //            JourneyId = JourneyId,
            //            DestinationId = destId.Id,
            //            LeavingTime = LeavingTime,
            //            Id = Guid.NewGuid(),
            //            Price = 70,
            //            SeatNum = j + 1,
            //            CreatedTime = DateTime.Now,
            //            StartBusStopId = StartId.Id,
            //            ReservedOnline = false,
            //            ConsumerId = "b85c868b-0387-45d9-9802-f5b0c6c7faeb",
            //            DestinationName = destId.Name,
            //            StartBusStopName = StartId.Name
            //        };
            //        tickets.Add(ticket);
            //    }

            //    var journey = new JourneyHistory
            //    {
            //        ArrivalTime = arrivalTime,
            //        BusId = Guid.Parse("36AE7F2F-859D-4490-BD6B-4148A081156C"),
            //        DestinationId = destId.Id,
            //        StartBusStopId = StartId.Id,
            //        LeavingTime = LeavingTime,
            //        TicketPrice = 70,
            //        Id = JourneyId,
            //        Tickets = tickets
            //    };

            //    journeys.Add(journey);
            //}

            //builder.Entity<JourneyHistory>().HasData(journeys);
        }
    }
}
