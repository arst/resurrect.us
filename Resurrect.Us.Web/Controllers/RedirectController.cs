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

        private readonly IResurrectRecordsStorageService storageService;
        private readonly IWaybackService waybackService;
        public RedirectController(IResurrectRecordsStorageService storageService, IWaybackService waybackService)
        {
            this.storageService = storageService;
            this.waybackService = waybackService;
        }

        public async Task<IActionResult> Index(string tinyUrl)
        {
            var record = this.storageService.GetResurrectionRecordAsync(tinyUrl);

            if (record == null)
            {
                return new RedirectToActionResult("Index", "Home", new object());
            }

            HttpClient client = new HttpClient();
            var response =  await client.GetAsync(record.Url);

            if (response.StatusCode != System.Net.HttpStatusCode.OK)
            {
                var wayback = await this.waybackService.GetWaybackAsync(record.Url);

                if (wayback != null)
                {
                    return new RedirectResult(wayback.GetClosestUrl());
                }
            }

            return new RedirectResult(record.Url, true);
        }
    }
}
