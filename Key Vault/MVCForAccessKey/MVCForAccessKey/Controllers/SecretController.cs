using Microsoft.AspNetCore.Mvc;
using MVCForAccessKey.Services;

namespace MVCForAccessKey.Controllers
{
    public class SecretController : Controller
    {
        private readonly KeyVaultService _keyVaultService;

        public SecretController(KeyVaultService keyVaultService)
        {
            _keyVaultService = keyVaultService;
        }

        public IActionResult Index()
        {
            string secretName = "ConnectionString"; 
            string secretValue = _keyVaultService.GetSecret(secretName);

            return Content(secretValue);
        }
    }
}
