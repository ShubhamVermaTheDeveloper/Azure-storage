using Azure.Identity;
using Azure.Security.KeyVault.Secrets;
using MVCForAccessKey.Models;

namespace MVCForAccessKey.Services
{
    public class KeyVaultService
    {
        private readonly IConfiguration _configuration;

        public KeyVaultService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public string GetSecret(string secretName)
        {
            string vaultUrl = _configuration["AzureKeyVault:VaultUrl"];

            var client = new SecretClient(new Uri(vaultUrl), new DefaultAzureCredential());
            KeyVaultSecret secret = client.GetSecret(secretName);

            return secret.Value;
        }
    }
}
