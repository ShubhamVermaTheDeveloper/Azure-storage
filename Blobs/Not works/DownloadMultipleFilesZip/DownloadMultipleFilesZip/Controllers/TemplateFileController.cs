using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.IO.Compression;
using Azure.Storage.Blobs;

namespace DownloadMultipleFilesZip.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TemplateFileController : ControllerBase
    {
        private const string connectionString = "DefaultEndpointsProtocol=https;AccountName=shubham12;AccountKey=AX7b/yGFhfb4f8/eMqX724KfvPlf9ekENK4cBe9wPA5+7FAKc+EzPvubBi42KuWMWGoRZ16GT17z+AStzzbKgA==;EndpointSuffix=core.windows.net";
        private const string containerName = "downloadmultiplefilecontainer";

        [HttpGet("[action")]
        public async Task<IActionResult> DownloadFiles()
        {
            BlobContainerClient blobContainerClient = new BlobContainerClient(connectionString, containerName);
            var blobs = blobContainerClient.GetBlobs();
            using (var memoryStream = new MemoryStream())
            {
                using (var zipArchive = new ZipArchive(memoryStream, ZipArchiveMode.Create, true))
                {
                    foreach (var blob in blobs)
                    {
                        var filesEntryInZip = zipArchive.CreateEntry(blob.Name);
                        var blobClient = blobContainerClient.GetBlobClient(blob.Name);
                        using (var fileStream = filesEntryInZip.Open())
                        {
                            await blobClient.DownloadToAsync(fileStream);
                        }
                    }
                }
                memoryStream.Position = 0;
                return File(memoryStream.ToArray(), "application/zip", "TemplateFiles.zip");
            }
        }
    }
}



