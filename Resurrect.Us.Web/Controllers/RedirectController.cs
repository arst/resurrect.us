using Microsoft.AspNetCore.Mvc;
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

        public RedirectController(IWaybackService waybackService, 
                                  IUrlCheckerService urlChecker, 
                                  IUrlShortenerService urlShortenerService)
        {
            this.waybackService = waybackService;
            this.urlCheckerService = urlChecker;
            this.urlShortenerService = urlShortenerService;
        }

        public async Task<IActionResult> Index(string tinyUrl)
        {
            var deshortenedUrl = await this.urlShortenerService.GetDeshortenedUrl(tinyUrl);

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
