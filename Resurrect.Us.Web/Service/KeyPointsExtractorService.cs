using Resurrect.Us.Web.Models;
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

        public KeyPointsExtractorService(IDOMProcessingService domProcessingService, IUrlCheckerService urlChecker)
        {
            this.domProcessingService = domProcessingService;
            this.urlChecker = urlChecker;
        }

        public async Task<HTMLKeypointsResult> GetHtmlKeypointsFromUrl(string url)
        {
            HttpClient cl = new HttpClient();

            if (await urlChecker.CheckUrl(url) == System.Net.HttpStatusCode.OK)
            {
                var html = await cl.GetStringAsync(url);
                var keypoints = this.domProcessingService.ExtractHTMLKeypoints(html);

                return keypoints;
            }
            return new HTMLKeypointsResult();
        }
    }
}
