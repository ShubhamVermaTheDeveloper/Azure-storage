using AzureTableFileUploadDownload.Models;
using AzureTableFileUploadDownload.Options;
using AzureTableFileUploadDownload.Repository;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace AzureTableFileUploadDownload.Controllers
{
    public class Filetable : Controller
    {
        private readonly IFileProject _fileProject;
        private readonly AzureOptions _azureOptions;

        /// <summary>
        /// Constructor of the Controller
        /// </summary>
        /// <param name="fileProject">Instace of the IFileProject Interface</param>
        /// <param name="azureOptions">Instace of the IOptions type is AzureOption</param>
        public Filetable(IFileProject fileProject, IOptions<AzureOptions> azureOptions)
        {
            _fileProject = fileProject;
            _azureOptions = azureOptions.Value;
        }

        /// <summary>
        /// Index Action method
        /// </summary>
        /// <returns>List in the view</returns>
        public IActionResult Index()
        {
            _fileProject.CreateTable();
            List<FileModel> list = _fileProject.ReadRecord();
            return View(list);
        }

        /// <summary>
        /// Display the Create View 
        /// </summary>
        /// <returns>Create View</returns>
        public IActionResult Create()
        {
            return View();
        }


        /// <summary>
        /// Create new record 
        /// </summary>
        /// <param name="fileProject">Instance of the FileModel Class</param>
        /// <param name="FileData">Instance of the IFormFile</param>
        /// <returns>Redirect to the Action</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(FileModel fileProject, IFormFile FileData)
        {
            fileProject.FileName = FileData.FileName;
            if (FileData != null && FileData.Length > 0)
            {
                using (var memoryStream = new MemoryStream())
                {
                    FileData.CopyTo(memoryStream);
                    fileProject.FileData = memoryStream.ToArray();
                }
            }
            _fileProject.InsertRecord(fileProject);
            TempData["insert_message"] = "Record Added..";
            return RedirectToAction("Index");
        }


        /// <summary>
        /// Delete the Record
        /// </summary>
        /// <param name="RowKey">Rowkey of the record</param>
        /// <returns>Redirect to the Index</returns>
        public IActionResult Delete(string RowKey)
        {
           _fileProject.DeleteRecord(RowKey);
            TempData["delete_message"] = "Record Deleted";
            return RedirectToAction("Index");
        }
    }
}
