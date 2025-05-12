using AutoMapper;

namespace Quod.API
{
    public static class AutoMapperRegistry
    {
        public static void Load(IServiceCollection services)
        {
            services.AddSingleton<AutoMapper.IConfigurationProvider>((c) => AutomapperConfiguration.Configure());
            services.AddSingleton((c) => AutomapperConfiguration.RegisterMappings());
        }

    }
}
