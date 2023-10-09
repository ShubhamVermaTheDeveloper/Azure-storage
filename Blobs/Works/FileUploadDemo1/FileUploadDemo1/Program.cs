using Azure.Storage.Blobs;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace FileUploadDemo1
{
    public class Program
    {
        static string connectionString = "DefaultEndpointsProtocol=https;AccountName=shubham12;AccountKey=AX7b/yGFhfb4f8/eMqX724KfvPlf9ekENK4cBe9wPA5+7FAKc+EzPvubBi42KuWMWGoRZ16GT17z+AStzzbKgA==;EndpointSuffix=core.windows.net";
        static string containerName = "try1";
        static string folderPath = @"C:\Github\Ajax\DemoAjax\packages\jQuery.Validation.1.17.0\Content\Scripts";




        public static void Main(string[] args)
        {

            var files = Directory.GetFiles(folderPath, "*", SearchOption.AllDirectories);
            BlobContainerClient containerClient = new BlobContainerClient(connectionString, containerName);


            foreach (var file in files)
            {
                var filePathOverCloud = file.Replace(folderPath, string.Empty);
                using (MemoryStream stream = new MemoryStream(File.ReadAllBytes(file)))
                {
                    containerClient.UploadBlob(filePathOverCloud, stream);
                    Console.WriteLine(filePathOverCloud + " Uploaded!");
                }

            }
        }
    }
}