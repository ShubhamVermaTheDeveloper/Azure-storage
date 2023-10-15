using Azure;
using Azure.Data.Tables;
namespace AzureTableFileUploadDownload.Models
{
    public class FileModel : ITableEntity
    {
        public string PartitionKey { get; set; }
        public string RowKey { get; set; }
        public DateTimeOffset? Timestamp { get; set; }
        public ETag ETag { get; set; }


        public string Name { get; set; }
        public string FileName { get; set; }
        public byte[] FileData { get; set; }
    }
}
