using Azure.Data.Tables;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Options;
using StorageTableCrud.Models;
using StorageTableCrud.Options;
using StorageTableCrud.Repository;


namespace StorageTableCrud.Controllers
{
    public class Person : Controller
    {
        private readonly IPerson _person;
        private readonly AzureOptions _azureOptions;

       
        /// <summary>
        /// Constructor of the Person Class
        /// </summary>
        /// <param name="person">Instance of the IPerson</param>
        /// <param name="azureOptions">Instance of IOption type is Azure Option</param>
        public Person(IPerson person, IOptions<AzureOptions> azureOptions)
        {
            _person = person;
            _azureOptions = azureOptions.Value;
        }


        /// <summary>
        /// This Is Index Action Method
        /// </summary>
        /// <returns>List in the view </returns>
        [HttpGet]
        public IActionResult Index()
        {
            List<PersonModel> list = _person.ReadRecord();
            return View(list);
        }


        /// <summary>
        /// Display the Create view 
        /// </summary>
        /// <returns>View</returns>
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }


        /// <summary>
        /// Create the new Record 
        /// </summary>
        /// <param name="person">Instance of the PersonModel</param>
        /// <param name="ImageFile">For Image</param>
        /// <returns>Redirect to the Index</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(PersonModel person, IFormFile ImageFile)
        {
            if (ImageFile != null && ImageFile.Length > 0)
            {
                using (var memoryStream = new MemoryStream())
                {
                    ImageFile.CopyTo(memoryStream);
                    person.Image = memoryStream.ToArray();
                }
            }
            _person.InsertRecord(person);
            TempData["insert_message"] = "Record Added..";
            return RedirectToAction("Index");
        }

        
        /// <summary>
        /// Display the Edit view
        /// </summary>
        /// <param name="rowKey">RowKey of the record</param>
        /// <returns>Person In the view</returns>
        public IActionResult Edit(string rowKey)
        {
            PersonModel person = _person.GetRecordByRowKey(rowKey);
            return View(person);
        }

        /// <summary>
        /// Edit the Record
        /// </summary>
        /// <param name="rowKey">RowKey of the record</param>
        /// <param name="person">Instance of the PersonModel</param>
        /// <param name="ImageFile">For Image</param>
        /// <returns>Redirect to the Index action</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(string rowKey, PersonModel person, IFormFile ImageFile)
        {
            if (ImageFile != null && ImageFile.Length > 0)
            {
                using (var memoryStream = new MemoryStream())
                {
                    ImageFile.CopyTo(memoryStream);
                    person.Image = memoryStream.ToArray();
                }
            }
            _person.UpdateRecord(person, rowKey);
            TempData["update_message"] = "Record Updated";
            return RedirectToAction("Index");
        }



        /// <summary>
        /// Delete the perticular record
        /// </summary>
        /// <param name="RowKey">RowKey of the Record</param>
        /// <returns>Redirect to the Index action</returns>
        public IActionResult Delete(string RowKey)
        {
            _person.DeleteRecord(RowKey);
            TempData["delete_message"] = "Record Deleted";
            return RedirectToAction("Index");
        }


    }
}
