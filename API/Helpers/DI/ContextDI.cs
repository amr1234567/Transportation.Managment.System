using Infrastructure.Context;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Model.Identity;

namespace Transportation_Api.Helpers.DI
{
    public static class ContextDI
    {
        public static IServiceCollection AddContextDI(this IServiceCollection services, IConfiguration _configuration)
        {

            services.AddIdentity<AppUser, IdentityRole>()
                .AddEntityFrameworkStores<ApplicationDbContext>();

            services.AddDbContext<ApplicationDbContext>(options =>
            {
                options.UseSqlServer(_configuration.GetConnectionString("System"));
            });

            //services.AddIdentity<BusStopManger, IdentityRole>()
            //    .AddEntityFrameworkStores<IdentityDbContext>();

            //services.AddDbContext<IdentityDbContext>(options =>
            //{
            //    options.UseSqlServer(_configuration.GetConnectionString("Identity"));
            //});

            return services;
        }
    }
}
