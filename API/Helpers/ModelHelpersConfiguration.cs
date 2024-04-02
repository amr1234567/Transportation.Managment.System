using Core.Helpers;

namespace API.Helpers
{
    public static class ModelHelpersConfiguration
    {
        public static IServiceCollection AddModelHelpersServices(this IServiceCollection services, IConfiguration Configuration)
        {
            services.Configure<JwtHelper>(Configuration.GetSection("JWT"));
            services.Configure<MailConfigurations>(Configuration.GetSection("EmailConfigration"));
            services.Configure<TwilioConfiguration>(Configuration.GetSection("Twilio"));

            return services;
        }
    }
}
