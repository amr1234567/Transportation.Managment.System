﻿using Core.Identity;
using Infrastructure.Context;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

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

            services.AddIdentityCore<User>()
                .AddRoles<IdentityRole>()
                .AddEntityFrameworkStores<ApplicationDbContext>();

            services.AddIdentityCore<BusStopManger>()
                .AddRoles<IdentityRole>()
                .AddEntityFrameworkStores<ApplicationDbContext>();

            services.AddIdentityCore<ApplicationAdmin>()
                .AddRoles<IdentityRole>()
                .AddEntityFrameworkStores<ApplicationDbContext>();

            services.AddIdentityCore<ApplicationUser>(options =>
            {
                options.SignIn.RequireConfirmedPhoneNumber = true;
            })
                .AddTokenProvider<DataProtectorTokenProvider<ApplicationUser>>(TokenOptions.DefaultProvider)
                .AddRoles<IdentityRole>()
                .AddEntityFrameworkStores<ApplicationDbContext>();


            return services;
        }
    }
}