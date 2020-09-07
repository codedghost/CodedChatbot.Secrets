using Microsoft.Extensions.DependencyInjection;

namespace CoreCodedChatbot.Secrets
{
    public static class Package
    {
        public static IServiceCollection AddChatbotSecretServiceCollection(this IServiceCollection services,
            string keyVaultAppId, string keyVaultCertThumbPrint, string keyVaultBaseUrl)
        {
            var secretService = new AzureKeyVaultService(keyVaultAppId, keyVaultCertThumbPrint, keyVaultBaseUrl);
            secretService.Initialize().Wait();

            services.AddSingleton<ISecretService, AzureKeyVaultService>(provider => secretService);
            return services;
        }
    }
}