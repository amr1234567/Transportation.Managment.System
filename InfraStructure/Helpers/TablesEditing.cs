using Core.Identity;
using Core.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace InfraStructure.Helpers
{
    public static class TablesEditing
    {

        public static void EditTables(this ModelBuilder builder)
        {
            string AppSchema = "App";
            string IdentitySchema = "Security";


            builder.HasDefaultSchema(IdentitySchema);

            builder.Entity<ApplicationAdmin>().ToTable("Admins");
            builder.Entity<ApplicationUser>().ToTable("Users");
            builder.Entity<BusStopManger>().ToTable("Managers", AppSchema);
            builder.Entity<IdentityRole>().ToTable("Roles");
            builder.Entity<Bus>().ToTable("Buses", AppSchema);
            builder.Entity<JourneyHistory>().ToTable("Journeys", AppSchema);
            builder.Entity<Ticket>().ToTable("Tickets", AppSchema);
            builder.Entity<Seat>().ToTable("Seats", AppSchema);
            builder.Entity<UpcomingJourney>().ToTable("UpcomingJourneys", AppSchema);
            builder.Entity<BusStopManger>()
            .HasMany(n => n.BusStops)
            .WithMany(n => n.BusStopsRelations)
            .UsingEntity<Dictionary<string, object>>(
                "BusStopsRelations",
                j => j
                    .HasOne<BusStopManger>()
                    .WithMany()
                    .HasForeignKey("StartBusStopId")
                    .HasConstraintName("FK_BusStopsRelations_StartBusStop"),
                j => j
                    .HasOne<BusStopManger>()
                    .WithMany()
                    .HasForeignKey("DestinationStopId")
                    .HasConstraintName("FK_BusStopsRelations_DestinationStop"),
                j =>
                {
                    j.HasKey("StartBusStopId", "DestinationStopId");
                    j.ToTable("BusStopsRelations");
                }
            );
        }
    }
}
