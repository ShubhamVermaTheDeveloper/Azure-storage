using Azure.Data.Tables.Models;
using Azure.Data.Tables;
using AzureTableFileUploadDownload.Options;
using Microsoft.Extensions.Options;
using System.Security.Cryptography;
using System.Text;
using AzureTableFileUploadDownload.Models;
using Microsoft.AspNetCore.Mvc;

namespace AzureTableFileUploadDownload.Repository
{
    public class FileProject : IFileProject
    {
        private readonly AzureOptions _azureOptions;

        /// <summary>
        /// Constructor of the FileProject Class 
        /// </summary>
        /// <param name="azureOptions">Instance of the IOptions type is AzureOptions</param>
        public FileProject(IOptions<AzureOptions> azureOptions)
        {
            _azureOptions = azureOptions.Value;


        }

        /// <summary>
        /// Function for creating table if not exist
        /// </summary>
        /// <returns></returns>
        public TableItem CreateTable()
        {
            TableServiceClient client = new TableServiceClient(_azureOptions.ConnectionString);
            TableItem table = client.CreateTableIfNotExists(_azureOptions.TableName);
            return table;
        }


        /// <summary>
        /// Method for Insert the new record
        /// </summary>
        /// <param name="fileProject">Instance of teh FileModel</param>
        public void InsertRecord(FileModel fileProject)
        {
            TableServiceClient client = new TableServiceClient(_azureOptions.ConnectionString);
            TableClient table = client.GetTableClient(_azureOptions.TableName);
            var entity = new TableEntity(Encrypt(_azureOptions.PartitionKey), Encrypt(GenerateUniqueKey()))
            {
                { "Name", Encrypt(fileProject.Name) },
                { "FileName", Encrypt(fileProject.FileName) },
                { "FileData",  EncryptFile(fileProject.FileData)}
        };

            try
            {
                table.AddEntity(entity);
            }
            catch (Exception)
            {

                throw;
            }
            
        }

        /// <summary>
        /// Method for retrive the record from the table 
        /// </summary>
        /// <returns>List of FileModel</returns>
        public List<FileModel> ReadRecord()
        {
            TableClient _tableClient = new TableClient(_azureOptions.ConnectionString, _azureOptions.TableName);
            List<FileModel> entities = _tableClient.Query<FileModel>().ToList();
            foreach (var entity in entities)
            {
                entity.RowKey = Decrypt(entity.RowKey);
                entity.PartitionKey = Decrypt(entity.PartitionKey);
                entity.Name = Decrypt(entity.Name).ToString();
                entity.FileName = Decrypt(entity.FileName).ToString();
                entity.FileData = DecryptFile(entity.FileData);
            }

            return entities;
        }




        /// <summary>
        /// Delete the perticular record
        /// </summary>
        /// <param name="rowKey">Rowkey of the perticular record</param>
        public void DeleteRecord(string rowKey)
        {
            TableClient _tableClient = new TableClient(_azureOptions.ConnectionString, _azureOptions.TableName);
            FileModel entityToDelete = _tableClient.GetEntity<FileModel>(Encrypt(_azureOptions.PartitionKey), Encrypt(rowKey));
            _tableClient.DeleteEntity(entityToDelete.PartitionKey, entityToDelete.RowKey);
        }


        /// <summary>
        /// Method for generating the unique key for the rowkey attribute of the record 
        /// </summary>
        /// <returns></returns>
        public string GenerateUniqueKey()
        {
            Guid guid = Guid.NewGuid();
            byte[] bytes = guid.ToByteArray();
            using (SHA256 sha256 = SHA256.Create())
            {
                byte[] hashBytes = sha256.ComputeHash(bytes);
                string uniqueKey = BitConverter.ToString(hashBytes, 0, 5).Replace("-", "").Substring(0, 5);
                return uniqueKey;
            }
        }

        /// <summary>
        /// Method for encrypting the data
        /// </summary>
        /// <param name="text">text which is for pass for encryption</param>
        /// <returns>Encrypted text after converting ToBase64String</returns>
        public string Encrypt(string text)
        {
            byte[] iv = new byte[16];
            byte[] array;
            using (Aes aes = Aes.Create())
            {
                aes.Key = Encoding.UTF8.GetBytes(_azureOptions.EncryptionKey);
                aes.IV = iv;
                ICryptoTransform encryptor = aes.CreateEncryptor(aes.Key, aes.IV);
                using (MemoryStream ms = new MemoryStream())
                {
                    using (CryptoStream cryptoStream = new CryptoStream((Stream)ms, encryptor, CryptoStreamMode.Write))
                    {
                        using (StreamWriter streamWriter = new StreamWriter((Stream)cryptoStream))
                        {
                            streamWriter.Write(text);
                        }
                        array = ms.ToArray();
                    }
                }
            }
            return Convert.ToBase64String(array);
        }


        /// <summary>
        /// Encrypt the file 
        /// </summary>
        /// <param name="data">File data in the form for byte</param>
        /// <returns>Byte data encrypted formate</returns>
        public byte[] EncryptFile(byte[] data)
        {
            byte[] iv = new byte[16];
            using (Aes aes = Aes.Create())
            {
                aes.Key = Encoding.UTF8.GetBytes(_azureOptions.EncryptionKey);
                aes.IV = iv;
                ICryptoTransform encryptor = aes.CreateEncryptor(aes.Key, aes.IV);
                using (MemoryStream ms = new MemoryStream())
                {
                    using (CryptoStream cryptoStream = new CryptoStream(ms, encryptor, CryptoStreamMode.Write))
                    {
                        cryptoStream.Write(data, 0, data.Length);
                        cryptoStream.FlushFinalBlock();
                    }
                    return ms.ToArray();
                }
            }
        }

        /// <summary>
        /// Decrypt the data 
        /// </summary>
        /// <param name="text">text which is want to decrypt</param>
        /// <returns>Decrypted text</returns>
        public string Decrypt(string text)
        {
            byte[] iv = new byte[16];
            byte[] buffer = Convert.FromBase64String(text);
            using (Aes aes = Aes.Create())
            {
                aes.Key = Encoding.UTF8.GetBytes(_azureOptions.EncryptionKey);
                aes.IV = iv;
                ICryptoTransform decryptor = aes.CreateDecryptor(aes.Key, aes.IV);
                using (MemoryStream ms = new MemoryStream(buffer))
                {
                    using (CryptoStream cryptoStream = new CryptoStream(ms, decryptor, CryptoStreamMode.Read))
                    {
                        using (StreamReader sr = new StreamReader(cryptoStream))
                        {
                            return sr.ReadToEnd();
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Decrypt the byte data(file data)
        /// </summary>
        /// <param name="data">Decrypted byte data</param>
        /// <returns></returns>
        public byte[] DecryptFile(byte[] data)
        {
            byte[] iv = new byte[16];
            using (Aes aes = Aes.Create())
            {
                aes.Key = Encoding.UTF8.GetBytes(_azureOptions.EncryptionKey);
                aes.IV = iv;
                ICryptoTransform decryptor = aes.CreateDecryptor(aes.Key, aes.IV);
                using (MemoryStream ms = new MemoryStream(data))
                {
                    using (CryptoStream cryptoStream = new CryptoStream(ms, decryptor, CryptoStreamMode.Read))
                    {
                        using (MemoryStream decryptedStream = new MemoryStream())
                        {
                            cryptoStream.CopyTo(decryptedStream);
                            return decryptedStream.ToArray();
                        }
                    }
                }
            }
        }
    }
}
