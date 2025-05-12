using Quod.Domain;
using Quod.Service;

namespace Quod.API
{
    public static class ServiceRegistry
    {
        public static void Load(IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<BiometryValidationOptions>(configuration.GetSection("BiometryValidation"));

            services.AddScoped<IBiometryService, BiometryService>();
            services.AddScoped<IImageValidator, ImageValidator>();
            services.AddScoped<IMetadataExtractor, MetadataExtractor>();
            services.AddScoped<IQualityAnalyzer, QualityAnalyzer>();
            services.AddScoped<IDocumentAnalysisService, DocumentAnalysisService>();    
            services.AddScoped<INotificationService, NotificationService>();

        }
    }
}
