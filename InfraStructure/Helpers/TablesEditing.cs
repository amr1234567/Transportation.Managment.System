using Core.Identity;
using Core.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
            builder.Entity<BusStopManger>().ToTable("Managers");
            builder.Entity<IdentityRole>().ToTable("Roles");
            builder.Entity<Bus>().ToTable("Buses", AppSchema);
            builder.Entity<BusStop>().ToTable("BusStops", AppSchema);
            builder.Entity<Journey>().ToTable("Journeys", AppSchema);
            builder.Entity<Ticket>().ToTable("Tickets", AppSchema);
            builder.Entity<Seat>().ToTable("Seats", AppSchema);
        }
    }
}
