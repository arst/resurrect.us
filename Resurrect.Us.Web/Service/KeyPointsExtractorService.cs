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

        public KeyPointsExtractorService(IDOMProcessingService domProcessingService)
        {
            this.domProcessingService = domProcessingService;
        }

        public async Task<HTMLKeypointsResult> GetHtmlKeypointsFromUrl(string url)
        {
            HttpClient cl = new HttpClient();
            var html = await cl.GetStringAsync(url);
            var keypoints = this.domProcessingService.ExtractHTMLKeypoints(html);

            return keypoints;
        }
    }
}
