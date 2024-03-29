using Core.Identity;
using Infrastructure.Context;
using InfraStructure.Context;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace API.Helpers.DI
{
    public static class ContextDI
    {
        public static IServiceCollection AddContextDI(this IServiceCollection services, IConfiguration _configuration)
        {

            services.AddIdentityCore<ApplicationUser>(options => options.SignIn.RequireConfirmedAccount = true)
                .AddEntityFrameworkStores<ApplicationDbContext>();

            services.AddDbContext<ApplicationDbContext>(options =>
            {
                options.UseSqlServer(_configuration.GetConnectionString("AppConnString"));
            });

            services.AddIdentityCore<BusStopManger>(options => options.SignIn.RequireConfirmedAccount = true)
                .AddEntityFrameworkStores<BusStopDBContext>();

            services.AddDbContext<BusStopDBContext>(options =>
            {
                options.UseSqlServer(_configuration.GetConnectionString("BusStopConnString"));
            });

            services.AddDbContext<IdentityContext>(options =>
            {
                options.UseSqlServer(_configuration.GetConnectionString("IdentityConnString"));
            });

            return services;
        }
    }
}
