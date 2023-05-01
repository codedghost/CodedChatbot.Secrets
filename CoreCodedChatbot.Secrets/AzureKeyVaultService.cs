using System;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using Azure;
using Azure.Identity;
using Azure.Security.KeyVault.Secrets;
using Microsoft.Identity.Client;

namespace CoreCodedChatbot.Secrets
{
    public class AzureKeyVaultService : ISecretService
    {
        private string _appId;
        private string _certThumbprint;
        private string _baseUrl;
        private readonly string _tenantId;

        private Dictionary<string, string> _secrets;

        public AzureKeyVaultService(string appId, string certThumbprint, string baseUrl, string tenantId)
        {
            _appId = appId;
            _certThumbprint = certThumbprint;
            _baseUrl = baseUrl;
            _tenantId = tenantId;
        }

        public async Task Initialize()
        {
            using (var store = new X509Store(StoreName.My, StoreLocation.CurrentUser))
            {
                store.Open(OpenFlags.ReadOnly);

                var cert = store.Certificates.Find(X509FindType.FindByThumbprint,
                    _certThumbprint, false)[0];

                var keyVaultClient = new SecretClient(new Uri(_baseUrl), new ClientCertificateCredential(_tenantId, _appId, cert));

                _secrets = new Dictionary<string, string>();

                var listSecrets = keyVaultClient.GetPropertiesOfSecretsAsync();

                await AddSecretsToDict(listSecrets.AsPages(), keyVaultClient);
            }
        }

        private async Task AddSecretsToDict(IAsyncEnumerable<Page<SecretProperties>> pages, SecretClient client)
        {
            await foreach (var page in pages)
            {
                foreach (var secretProperty in page.Values)
                {
                    var secret = await client.GetSecretAsync(secretProperty.Name);

                    _secrets.Add(secret.Value.Name, secret.Value.Value);
                }
            }
        }

        public T GetSecret<T>(string secretKey)
        {
            var secret = _secrets[secretKey];

            if (string.IsNullOrWhiteSpace(secret))
            {
                return default(T);
            }

            return (T)Convert.ChangeType(secret, typeof(T));
        }
    }
}