using AutoMapper;

namespace Quod.API
{
    public static class AutoMapperRegistry
    {
        public static void Load(IServiceCollection services)
        {
            var config = AutomapperConfiguration.Configure();
            services.AddSingleton<AutoMapper.IConfigurationProvider>(config);
            services.AddSingleton<IMapper>(config.CreateMapper());
        }

    }
}
