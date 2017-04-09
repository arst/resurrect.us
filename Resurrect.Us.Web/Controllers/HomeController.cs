using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Resurrect.Us.Web.Models;
using Resurrect.Us.Web.Service;
using System.Net.Http;
using Resurrect.Us.Data.Services;
using Resurrect.Us.Data.Models;
using System.IO;

namespace Resurrect.Us.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly IUrlShortenerService urlShortenerService;

        public HomeController(IUrlShortenerService urlShortenerService)
        {
            this.urlShortenerService = urlShortenerService;
        }

        public IActionResult Index()
        {
            return View("Index");
        }

        [HttpPost]
        public async Task<IActionResult> Index(HomePageViewModel urlRequest)
        {
            if (ModelState.IsValid)
            {
                var shortUrl = await this.urlShortenerService.GetShortUrlAsync(urlRequest.Url);
                var request = HttpContext.Request;
                var model = String.Format("{0}://{1}/{2}",request.Scheme, request.Host.ToUriComponent(), shortUrl);
                return View("Result",model);
            }
            else {
                return View("Index", urlRequest);
            }
        }

        public IActionResult About()
        {
            return View();
        }

        public IActionResult Contact()
        {
            return View();
        }

        public IActionResult Error()
        {
            return View();
        }
    }
}
