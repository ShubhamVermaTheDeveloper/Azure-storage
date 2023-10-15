using Azure.Identity;
using Azure.Security.KeyVault.Secrets;

namespace AzureTableFileUploadDownload.Options
{
    public class AzureOptions
    {
        //public AzureOptions()
        //{
        //    // Constructor should not perform the retrieval of secrets
        //    // You should retrieve secrets and initialize AzureOptions elsewhere.
        //}

        public static AzureOptions CreateFromKeyVault()
        {
            var keyVaultUrl = "https://shubhamkeyvault12.vault.azure.net/";
            var secretClient = new SecretClient(new Uri(keyVaultUrl), new DefaultAzureCredential());

            var resourceGroupSecret = secretClient.GetSecret("ResourceGroup");
            var accountSecret = secretClient.GetSecret("Account");
            var tableNameSecret = secretClient.GetSecret("TableName");
            var connectionStringSecret = secretClient.GetSecret("ConnectionString");
            var encryptionKeySecret = secretClient.GetSecret("EncryptionKey");
            var partitionKeySecret = secretClient.GetSecret("PartitionKey");

            return new AzureOptions
            {
                ResourceGroup = resourceGroupSecret.Value.Value,
                Account = accountSecret.Value.Value,
                TableName = tableNameSecret.Value.Value,
                ConnectionString = connectionStringSecret.Value.Value,
                EncryptionKey = encryptionKeySecret.Value.Value,
                PartitionKey = partitionKeySecret.Value.Value
            };
        }

        public string ResourceGroup { get; set; }
        public string Account { get; set; }
        public string TableName { get; set; }
        public string ConnectionString { get; set; }
        public string EncryptionKey { get; set; }
        public string PartitionKey { get; set; }

        
    }
}
