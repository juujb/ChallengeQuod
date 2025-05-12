namespace Quod.API
{
    public static class DependencyInjectionConfig
    {
        public static IServiceCollection ConfigureContainer(this IServiceCollection services, IConfiguration configuration)
        {
            ServiceRegistry.Load(services, configuration);
            MongoRegistry.Load(services);

            return services;
        }
    }
}
