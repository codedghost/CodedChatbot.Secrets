

using CoreCodedChatbot.Secrets;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.Extensions.Configuration;

public class Program
{
    static void Main(string[] args)
    {
        var keyVaultAppId = "";
        var keyVaultCertThumbPrint = "";
        var keyVaultBaseUrl = "";
        var tenantId = "";

        var secretService = new AzureKeyVaultService(keyVaultAppId, keyVaultCertThumbPrint, keyVaultBaseUrl, tenantId);
        secretService.Initialize().Wait();
    }
}