using IServices.IServices;
using Services.Services;

namespace Transportation_Api.Helpers.DI
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
            return services;
        }
    }
}
