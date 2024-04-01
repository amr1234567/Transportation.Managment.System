﻿using Core.Identity;
using Infrastructure.Context;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace API.Helpers.DI
{
    public static class ContextDI
    {
        public static IServiceCollection AddContextDI(this IServiceCollection services, IConfiguration _configuration)
        {

            services.AddDbContext<ApplicationDbContext>(options =>
            {
                options.UseSqlServer(_configuration.GetConnectionString("AppConnString"));
            });

            services.AddIdentityCore<User>(options =>
            {
                options.SignIn.RequireConfirmedAccount = true;
            })
                .AddEntityFrameworkStores<ApplicationDbContext>();
            services.AddIdentityCore<BusStopManger>()
                .AddEntityFrameworkStores<ApplicationDbContext>();
            services.AddIdentityCore<ApplicationAdmin>()
                .AddEntityFrameworkStores<ApplicationDbContext>();
            services.AddIdentityCore<ApplicationUser>()
                .AddEntityFrameworkStores<ApplicationDbContext>();


            return services;
        }
    }
}
