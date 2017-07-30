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
using Microsoft.Extensions.Caching.Distributed;
using System.IO;

namespace Resurrect.Us.Web.Service
{
    public class WaybackService : IWaybackService
    {
        private readonly IHttpClientWrapper httpClientWrapper;
        private readonly IDistributedCache distributedCache;

        public WaybackService(IHttpClientWrapper httpClientWrapper, IDistributedCache distributedCache)
        { 
            this.httpClientWrapper = httpClientWrapper;
            this.distributedCache = distributedCache;
        }
        public async Task<WaybackResponse> GetWaybackAsync(string url)
        {
            WaybackResponse response = null;

            if (String.IsNullOrEmpty(url) || !Uri.IsWellFormedUriString(url, UriKind.Absolute))
            {
                throw new ArgumentException("Url must be an absolute url");
            }

            byte[] jsonResponse = await this.distributedCache.GetAsync(String.Concat(url,"Wayback"));
            var serializer = new DataContractJsonSerializer(typeof(WaybackResponse));

            if (jsonResponse != null)
            {
                response = serializer.ReadObject(new MemoryStream(jsonResponse)) as WaybackResponse;
            }
            else
            {
                var waybackServerResponse = await this.httpClientWrapper
                                                        .GetStreamAsync(WaybackConstants.WaybackServerAddress + url);
                var responseBytes = new MemoryStream();
                await waybackServerResponse.CopyToAsync(responseBytes);
                await this.distributedCache.SetAsync(String.Concat(url, "Wayback"), responseBytes.ToArray());
                response = serializer.ReadObject(waybackServerResponse) as WaybackResponse;
            }

            return response;
        }
    }
}
