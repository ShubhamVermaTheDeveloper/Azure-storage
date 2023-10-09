using Azure.Storage.Blobs;
using Azure.Storage.Sas;
using System;
using System.IO;
using System.Threading.Tasks;

public class Program
{
    public static async Task Main(string[] args)
    {
        Console.WriteLine("Starting....");
        string connectionString = "DefaultEndpointsProtocol=https;AccountName=shubham12;AccountKey=AX7b/yGFhfb4f8/eMqX724KfvPlf9ekENK4cBe9wPA5+7FAKc+EzPvubBi42KuWMWGoRZ16GT17z+AStzzbKgA==;EndpointSuffix=core.windows.net";
        string containerName = "finaltry";
        string blobName = "DempIMG.png";
        string sasToken = GetSasToken(connectionString, containerName, blobName, BlobSasPermissions.Read);

        var blobServiceClient = new BlobServiceClient(new Uri(new BlobServiceClient(connectionString).GetBlobContainerClient(containerName).GetBlobClient(blobName).Uri.ToString() + sasToken));
        var blobContainerClient = blobServiceClient.GetBlobContainerClient(containerName);
        var blobClient = blobContainerClient.GetBlobClient(blobName);

        string downloadFilePath = "C:\\download\\Demp3.png"; 
        using (var fileStream = File.OpenWrite(downloadFilePath))
        {
            await blobClient.DownloadToAsync(fileStream);
        }

        Console.WriteLine("Download Completed.");
    }

    private static string GetSasToken(string connectionString, string containerName, string blobName, BlobSasPermissions permissions)
    {
        var blobServiceClient = new BlobServiceClient(connectionString);
        var blobContainerClient = blobServiceClient.GetBlobContainerClient(containerName);
        var blobClient = blobContainerClient.GetBlobClient(blobName);

        var builder = new BlobSasBuilder
        {
            BlobContainerName = containerName,
            BlobName = blobName,
            Resource = "b",
            StartsOn = DateTime.UtcNow.AddMinutes(-5),
            ExpiresOn = DateTime.UtcNow.AddHours(1),
        };

        builder.SetPermissions(permissions);
        return blobClient.GenerateSasUri(builder).Query;
    }

}
