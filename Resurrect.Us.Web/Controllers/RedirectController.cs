using BinaryFormatter;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;
using Resurrect.Us.Data.Services;
using Resurrect.Us.Web.Models;
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
        private readonly IDistributedCache cache;

        public RedirectController(IWaybackService waybackService, 
                                  IUrlCheckerService urlChecker, 
                                  IUrlShortenerService urlShortenerService,
                                  ILogger<RedirectController> logger, IDistributedCache cache)
        {
            this.waybackService = waybackService;
            this.urlCheckerService = urlChecker;
            this.urlShortenerService = urlShortenerService;
            this.logger = logger;
            this.cache = cache;
        }

        public async Task<IActionResult> Index(string tinyUrl)
        {
            var deshortenedUrl = await this.cache.GetStringAsync(tinyUrl);
            if (deshortenedUrl == null)
            {
                deshortenedUrl = await this.urlShortenerService.GetDeshortenedUrl(tinyUrl);
                await this.cache.SetStringAsync(tinyUrl, deshortenedUrl);
                logger.LogInformation("Deshortening {0} with result {1}", tinyUrl, deshortenedUrl);

                if (String.IsNullOrEmpty(deshortenedUrl))
                {
                    return new RedirectToActionResult("Index", "Home", new object());
                }
                var checkResultKey = String.Concat(tinyUrl, "|||", "status");
                var cachedCheckResult = await this.cache.GetAsync(checkResultKey);
                var isUrlAwailable = cachedCheckResult == null ? await this.urlCheckerService.CheckUrl(deshortenedUrl) != System.Net.HttpStatusCode.OK 
                                                                 : Convert.ToBoolean(cachedCheckResult[0]);
                if (checkResultKey == null)
                {
                    await this.cache.SetAsync(checkResultKey, 
                        new byte[] { Convert.ToByte(isUrlAwailable) }, 
                        new DistributedCacheEntryOptions(){
                        SlidingExpiration = TimeSpan.FromMinutes(1)
                    });
                }

                if (!isUrlAwailable)
                {
                    var waybackLookupResultKey = String.Concat(tinyUrl, "|||", "wayback");
                    var cachedWaybackResult = await this.cache.GetAsync(waybackLookupResultKey);
                    BinaryConverter converter = new BinaryConverter();

                    var wayback = cachedWaybackResult == null ? await this.waybackService.GetWaybackAsync(deshortenedUrl) : converter.Deserialize<WaybackResponse>(cachedWaybackResult);

                    if (cachedWaybackResult == null && wayback != null)
                    {
                        await this.cache.SetAsync(waybackLookupResultKey, converter.Serialize(wayback));
                    }

                    if (wayback != null)
                    {
                        return new RedirectResult(wayback.GetClosestUrl());
                    }
                }


            }

            return new RedirectResult(deshortenedUrl, true);
        }
    }
}
