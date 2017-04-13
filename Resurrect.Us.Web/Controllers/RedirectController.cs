using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Resurrect.Us.Data.Services;
using Resurrect.Us.Web.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace Resurrect.Us.Web.Controllers
{
    public class RedirectController
    {

        private readonly IWaybackService waybackService;
        private readonly IUrlCheckerService urlCheckerService;
        private readonly IUrlShortenerService urlShortenerService;
        private readonly ILogger<RedirectController> logger;

        public RedirectController(IWaybackService waybackService, 
                                  IUrlCheckerService urlChecker, 
                                  IUrlShortenerService urlShortenerService,
                                  ILogger<RedirectController> logger)
        {
            this.waybackService = waybackService;
            this.urlCheckerService = urlChecker;
            this.urlShortenerService = urlShortenerService;
            this.logger = logger;
        }

        public async Task<IActionResult> Index(string tinyUrl)
        {
            
            var deshortenedUrl = await this.urlShortenerService.GetDeshortenedUrl(tinyUrl);
            logger.LogInformation("Deshortening {0} with result {1}", tinyUrl, deshortenedUrl);

            if (String.IsNullOrEmpty(deshortenedUrl))
            {
                return new RedirectToActionResult("Index", "Home", new object());
            }

            if (await this.urlCheckerService.CheckUrl(deshortenedUrl) != System.Net.HttpStatusCode.OK)
            {
                var wayback = await this.waybackService.GetWaybackAsync(deshortenedUrl);

                if (wayback != null)
                {
                    return new RedirectResult(wayback.GetClosestUrl());
                }
            }

            return new RedirectResult(deshortenedUrl, true);
        }
    }
}
