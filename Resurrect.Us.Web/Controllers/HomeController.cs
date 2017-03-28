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

namespace Resurrect.Us.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly IWaybackService waybackService;
        private readonly IDOMProcessingService domProcessingService;
        private readonly IResurrectRecordsStorageService resurrectRecordStorageService;

        public HomeController(IWaybackService waybackService, IDOMProcessingService domProcessingService, IResurrectRecordsStorageService resurrectRecordStorageService)
        {
            this.waybackService = waybackService;
            this.domProcessingService = domProcessingService;
            this.resurrectRecordStorageService = resurrectRecordStorageService;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Index(HomePageViewModel urlRequest)
        {
            if (ModelState.IsValid)
            {
                var wayBackResult = await this.waybackService.GetWaybackAsync(urlRequest.Url);
                HttpClient cl = new HttpClient();
                var html = await cl.GetStringAsync(urlRequest.Url);
                var keypoints = this.domProcessingService.ExtractHTMLKeypoints(html);
                var resurrectRecord = new ResurrectionRecord()
                {
                    Id = String.Format("{0:X}", urlRequest.Url.GetHashCode()),
                    AccessCount = 0,
                    LastAccess = DateTime.Now,
                    Timestamp = wayBackResult != null ? wayBackResult.GetClosestTimestamp() : "",
                    Title = keypoints.Title,
                    Url = urlRequest.Url,
                    Keywords = keypoints.Keywords.Select(k => new Keyword() { Value = k}).ToList()
                    
                };
                var result = await this.resurrectRecordStorageService.AddRecordAsync(resurrectRecord);
                return View("Result",result.Id);
            }
            else {
                return View(urlRequest);
            }
        }

        public IActionResult About()
        {
            ViewData["Message"] = "Your application description page.";

            return View();
        }

        public IActionResult Contact()
        {
            ViewData["Message"] = "Your contact page.";

            return View();
        }

        public IActionResult Error()
        {
            return View();
        }
    }
}
