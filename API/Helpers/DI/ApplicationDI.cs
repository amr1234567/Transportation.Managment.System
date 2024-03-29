namespace API.Helpers.DI
{
    public static class ApplicationDI
    {
        public static IServiceCollection AddApplicationDI(this IServiceCollection services, IConfiguration _configuration)
        {
            services.AddContextDI(_configuration);
            services.AddServicesDI();
            return services;
        }
    }
}
