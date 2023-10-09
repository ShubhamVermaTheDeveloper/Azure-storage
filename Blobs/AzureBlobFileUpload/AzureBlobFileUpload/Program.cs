using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
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
        string blobName = "Demp1.png"; 
        string filePath = "C:\\Users\\ShubhamVerma\\Pictures\\Screenshots\\cmdimg1.png"; 

        var blobServiceClient = new BlobServiceClient(connectionString);
        var blobContainerClient = blobServiceClient.GetBlobContainerClient(containerName);
        var blobClient = blobContainerClient.GetBlobClient(blobName);

        using (var fileStream = File.OpenRead(filePath))
        {
            await blobClient.UploadAsync(fileStream, true); 
        }

        Console.WriteLine("Upload Completed.");
    }
}
