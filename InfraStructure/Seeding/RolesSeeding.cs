using Core.Constants;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace InfraStructure.Seeding
{
    public static class RolesSeeding
    {
        public static void SeedRoles(this ModelBuilder model)
        {
            model.Entity<IdentityRole>().HasData(
                new IdentityRole()
                {
                    ConcurrencyStamp = "1",
                    Name = Roles.User,
                    NormalizedName = Roles.User.ToUpper()
                },
                new IdentityRole()
                {
                    ConcurrencyStamp = "2",
                    Name = Roles.Admin,
                    NormalizedName = Roles.Admin.ToUpper()
                },
                new IdentityRole()
                {
                    ConcurrencyStamp = "3",
                    Name = Roles.BusStopManager,
                    NormalizedName = Roles.BusStopManager.ToUpper()
                }
            );
        }
    }
}
