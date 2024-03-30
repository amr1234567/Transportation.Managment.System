using Interfaces.IApplicationServices;
using Interfaces.IIdentityServices;
using Interfaces.IMailServices;
using Services.ApplicationServices;
using Services.IdentityServices;
using Services.MailServices;

namespace API.Helpers.DI
{
    public static class ServicesDI
    {
        public static IServiceCollection AddServicesDI(this IServiceCollection services)
        {
            services.AddScoped<IBusServices, BusServices>();
            services.AddScoped<IBusStopServices, BusStopServices>();
            services.AddScoped<IJourneyServices, JourneyServices>();
            services.AddScoped<ISeatServices, SeatServices>();
            services.AddScoped<ITicketServices, TicketServices>();

            services.AddScoped<IUserServices, UserServices>();
            services.AddScoped<IMailServices, MailServices>();
            return services;
        }
    }
}
