using Azure.Identity;
using Azure.Security.KeyVault.Secrets;
using System;

namespace KeyVaultDemo1 
{
    class Program
    {
        public static void Main(string[] args) 
        {
            var client = new SecretClient(vaultUri: new Uri("https://shubhamkeyvault12.vault.azure.net/"), credential: new DefaultAzureCredential());
            //KeyVaultSecret secret = client.SetSecret("ResourceGroup", "ShubhamResource");
            //KeyVaultSecret secret1 = client.SetSecret("Account", "shubham12");
            //KeyVaultSecret secret2 = client.SetSecret("TableName", "FileUploadTable");
            //KeyVaultSecret secret3 = client.SetSecret("ConnectionString", "DefaultEndpointsProtocol=https;AccountName=shubham12;AccountKey=AX7b/yGFhfb4f8/eMqX724KfvPlf9ekENK4cBe9wPA5+7FAKc+EzPvubBi42KuWMWGoRZ16GT17z+AStzzbKgA==;EndpointSuffix=core.windows.net");
            //KeyVaultSecret secret4 = client.SetSecret("PartitionKey", "India");
            //KeyVaultSecret secret5 = client.SetSecret("EncryptionKey", "shubhamvermadotnetdeveloper2023k");




            //if (secret != null)
            //{
            //    Console.WriteLine("Successfully create the secrete");
            //}
            //else { Console.WriteLine("Not created"); }

            KeyVaultSecret GetSecret = client.GetSecret("ResourceGroup");
            KeyVaultSecret GetSecret1 = client.GetSecret("Account");
            KeyVaultSecret GetSecret2 = client.GetSecret("TableName");
            KeyVaultSecret GetSecret3 = client.GetSecret("ConnectionString");
            KeyVaultSecret GetSecret4 = client.GetSecret("PartitionKey");
            KeyVaultSecret GetSecret5 = client.GetSecret("EncryptionKey");
            Console.WriteLine(GetSecret.Name + ":" + GetSecret.Value);
            Console.WriteLine(GetSecret1.Name + ":" + GetSecret1.Value);
            Console.WriteLine(GetSecret2.Name + ":" + GetSecret2.Value);
            Console.WriteLine(GetSecret3.Name + ":" + GetSecret3.Value);
            Console.WriteLine(GetSecret4.Name + ":" + GetSecret4.Value);
            Console.WriteLine(GetSecret5.Name + ":" + GetSecret5.Value);



            Console.WriteLine(client.GetSecret("ResourceGroup").Value);


        }
    }
}
