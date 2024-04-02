using ECommerce.Core.Interfaces.IServices;
using ECommerce.InfaStructure.Services;
using Interfaces.IApplicationServices;
using Interfaces.IHelpersServices;
using Interfaces.IIdentityServices;
using Interfaces.IMailServices;
using Services.ApplicationServices;
using Services.HelperServices;
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
            services.AddScoped<ISmsSevices, SmsServices>();
            services.AddScoped<ITokenService, TokenService>();
            return services;
        }
    }
}
