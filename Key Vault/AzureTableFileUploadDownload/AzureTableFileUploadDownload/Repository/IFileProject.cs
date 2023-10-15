using Azure.Data.Tables;
using Azure.Data.Tables.Models;
using AzureTableFileUploadDownload.Models;

namespace AzureTableFileUploadDownload.Repository
{
    public interface IFileProject
    {
        TableItem CreateTable();
        public void InsertRecord(FileModel fileProject);
        public List<FileModel> ReadRecord();
        public string GenerateUniqueKey();
        public void DeleteRecord(string rowKey);
        public string Encrypt(string plainText);
        public string Decrypt(string cipherText);
        public byte[] EncryptFile(byte[] data);
        public byte[] DecryptFile(byte[] data);
    }
}
