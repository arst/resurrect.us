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
        private readonly IWaybackService waybackService;
        private readonly IKeyPointsExtractorService keyPointsExtractorService;
        private readonly IResurrectRecordsStorageService resurrectRecordStorageService;
        private readonly IHashService hashGenerator;

        public HomeController(IWaybackService waybackService, IKeyPointsExtractorService keyPointsExtractorService, IResurrectRecordsStorageService resurrectRecordStorageService, IHashService hashGenerator)
        {
            this.waybackService = waybackService;
            this.keyPointsExtractorService = keyPointsExtractorService;
            this.resurrectRecordStorageService = resurrectRecordStorageService;
            this.hashGenerator = hashGenerator;
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
                var wayBackResult = await this.waybackService.GetWaybackAsync(urlRequest.Url);
                var keypoints = await this.keyPointsExtractorService.GetHtmlKeypointsFromUrl(urlRequest.Url);
                var resurrectRecord = new ResurrectionRecord()
                {
                    AccessCount = 0,
                    LastAccess = DateTime.Now,
                    Timestamp = wayBackResult != null ? wayBackResult.GetClosestTimestamp() : "",
                    Title = keypoints.Title,
                    Url = urlRequest.Url,
                    Keywords = keypoints.Keywords.Select(k => new Keyword() { Value = k}).ToList()
                    
                };
                var result = await this.resurrectRecordStorageService.AddRecordAsync(resurrectRecord);
                var request = HttpContext.Request;
                var hash = this.hashGenerator.GetHash(result.Id);
                var model = string.Concat(
                        request.Scheme,
                        "://",
                        request.Host.ToUriComponent(), 
                        "/",
                        hash);

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
