using Azure.Storage.Blobs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DownloadFile
{
    internal class AzureStoreDownload
    {
        string connectionString = "DefaultEndpointsProtocol=https;AccountName=shubham12;AccountKey=AX7b/yGFhfb4f8/eMqX724KfvPlf9ekENK4cBe9wPA5+7FAKc+EzPvubBi42KuWMWGoRZ16GT17z+AStzzbKgA==;EndpointSuffix=core.windows.net";
        string containerName = "finaltry";

        public async Task DonloadFile()
        {   
            BlobServiceClient _blobServiceClient = new BlobServiceClient(connectionString);
            var blobContainerClient = _blobServiceClient.GetBlobContainerClient(containerName);

            string downloadPath = @"C:\\download\\";

            Directory.CreateDirectory(downloadPath);

            var blobs = blobContainerClient.GetBlobs();

            foreach(var blobItem in blobs)
            {
                string fileName = downloadPath + blobItem.Name;
                var blobClient = blobContainerClient.GetBlobClient(blobItem.Name);
                await blobClient.DownloadToAsync(fileName);
            }

        }
    }
}
