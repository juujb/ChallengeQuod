using AutoMapper;

namespace Quod.API
{
    /// <summary>
    /// Classe responsável pela configuração de De-Para da aplicação, transportando os valores de uma entidade para outra entidade.
    /// </summary>
    public static class AutomapperConfiguration
    {
        /// <summary>
        /// Registra o mapeamento entre model e view model.
        /// </summary>
        /// <returns></returns>
        public static IMapper RegisterMappings()
        {
            MapperConfiguration config = Configure();
            return config.CreateMapper();
        }

        /// <summary>
        /// Configura o mapeamento entre model e view model.
        /// </summary>
        /// <returns></returns>
        public static MapperConfiguration Configure()
        {
            MapperConfiguration config = new(cfg =>
            {
                cfg.AllowNullDestinationValues = true;
                cfg.AllowNullCollections = true;
                cfg.AddProfile<NotificationProfile>();
            });
            return config;
        }

    }
}
