using Quod.Domain;
using Quod.Service;

namespace Quod.API
{
    public static class ServiceRegistry
    {
        public static void Load(IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<BiometryValidationOptions>(configuration.GetSection("BiometryValidation"));

            services.AddHttpClient();
            services.AddScoped<IBiometryService, BiometryService>();
            services.AddScoped<IImageValidator, ImageValidator>();
            services.AddScoped<IMetadataExtractor, MetadataExtractor>();
            services.AddScoped<IQualityAnalyzer, QualityAnalyzer>();
            services.AddScoped<IDocumentAnalysisService, DocumentAnalysisService>();    
            services.AddScoped<INotificationEntityService, NotificationEntityService>();
            services.AddScoped<IImageCompareService, ImageCompareService>();
            services.AddScoped<IBiometryEntityService, BiometryEntityService>();
            services.AddScoped<INotificationRestService, NotificationRestService>();
            services.AddScoped<IDocumentAnalysisEntityService, DocumentAnalysisEntityService>();

        }
    }
}
