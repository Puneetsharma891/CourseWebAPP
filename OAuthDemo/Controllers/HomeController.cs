using Azure.Core;
using Azure.Storage.Blobs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Identity.Web;
using OAuthDemo.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Security.Claims;

namespace OAuthDemo.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private ITokenAcquisition _token;
        public HomeController(ILogger<HomeController> logger, ITokenAcquisition token)
        {
            _logger = logger;
            _token = token;
        }

        public async Task<IActionResult> Index()
        {

            string[] scopes = new string[] { "https://storage.azure.com/user_impersonation" };
            string token = await _token.GetAccessTokenForUserAsync(scopes);
            ViewBag.token = token;

            BlobClient client = new BlobClient(new Uri("https://storageclitest2021.blob.core.windows.net/mycontainer2021/Course0.json"
                ), new TokenAcquisitionTokenCredential(_token));

            MemoryStream stream = new MemoryStream();
            client.DownloadTo(stream);
            stream.Position = 0;
            StreamReader rdr = new StreamReader(stream);
            string str = rdr.ReadToEnd();
            ViewBag.Content = str;
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
