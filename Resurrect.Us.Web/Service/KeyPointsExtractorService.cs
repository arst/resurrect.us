using Resurrect.Us.Web.Models;
using Resurrect.Us.Web.Service.Wrappers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace Resurrect.Us.Web.Service
{
    public class KeyPointsExtractorService : IKeyPointsExtractorService
    {
        private readonly IDOMProcessingService domProcessingService;
        private readonly IUrlCheckerService urlChecker;
        private readonly IHttpClientWrapper httpClientWrapper;

        public KeyPointsExtractorService(IDOMProcessingService domProcessingService, IUrlCheckerService urlChecker, IHttpClientWrapper httpClientWrapper)
        {
            this.domProcessingService = domProcessingService;
            this.urlChecker = urlChecker;
            this.httpClientWrapper = httpClientWrapper;
        }

        public async Task<HTMLKeypointsResult> GetHtmlKeypointsFromUrl(string url)
        {
            if (String.IsNullOrEmpty(url) || !Uri.IsWellFormedUriString(url, UriKind.Absolute))
            {
                throw new ArgumentException("Url must be an absolute url");
            }

            if (await urlChecker.CheckUrl(url) == System.Net.HttpStatusCode.OK)
            {
                var html = await this.httpClientWrapper.GetStringAsync(url);
                var keypoints = this.domProcessingService.ExtractHTMLKeypoints(html);

                return keypoints;
            }
            return new HTMLKeypointsResult();
        }
    }
}
