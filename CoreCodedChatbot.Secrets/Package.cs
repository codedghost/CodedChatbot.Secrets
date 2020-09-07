using Microsoft.Extensions.DependencyInjection;

namespace CoreCodedChatbot.Secrets
{
    public static class Package
    {
        public static IServiceCollection AddChatbotSecretServiceCollection(this IServiceCollection services)
        {
            services.AddSingleton<ISecretService, AzureKeyVaultService>();
            return services;
        }
    }
}