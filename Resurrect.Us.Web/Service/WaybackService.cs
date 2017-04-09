using Resurrect.Us.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Runtime.Serialization.Json;
using Newtonsoft.Json;
using Resurrect.Us.Web.Service.Wrappers;
using Resurrect.Us.Web.Constants;

namespace Resurrect.Us.Web.Service
{
    public class WaybackService : IWaybackService
    {
        private readonly IHttpClientWrapper httpClientWrapper;
        public WaybackService(IHttpClientWrapper httpClientWrapper)
        {
            this.httpClientWrapper = httpClientWrapper;
        }
        public async Task<WaybackResponse> GetWaybackAsync(string url)
        {
            if (!Uri.IsWellFormedUriString(url, UriKind.Absolute))
            {
                throw new ArgumentException("Url must be an absolute url");
            }

            var client = new HttpClient();
            var jsonResponse = await this.httpClientWrapper
                .GetStreamAsync(WaybackConstants.WaybackServerAddress + url);
            var serializer = new DataContractJsonSerializer(typeof(WaybackResponse));
            WaybackResponse response = serializer.ReadObject(jsonResponse) as WaybackResponse;

            return response;
        }
    }
}
