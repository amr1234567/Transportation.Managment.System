using Core.Constants;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace InfraStructure.Helpers.Seeding
{
    public static class RolesSeeding
    {
        public static void SeedRoles(this ModelBuilder model)
        {
            model.Entity<IdentityRole>().HasData(
                new IdentityRole()
                {
                    Id = "35561bfe-d346-4b70-8380-b15d23edbe2e",
                    ConcurrencyStamp = "1",
                    Name = Roles.User,
                    NormalizedName = Roles.User.ToUpper()
                },
                new IdentityRole()
                {
                    Id = "a7d62d27-150f-4165-961e-e5095e09ddb1",
                    ConcurrencyStamp = "2",
                    Name = Roles.Admin,
                    NormalizedName = Roles.Admin.ToUpper()
                },
                new IdentityRole()
                {
                    Id = "47470826-cdc6-4eb0-9ebe-75f10c89c1da",
                    ConcurrencyStamp = "3",
                    Name = Roles.BusStopManager,
                    NormalizedName = Roles.BusStopManager.ToUpper()
                }
            );
        }
    }
}
