using Resurrect.Us.Web.Service.Wrappers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace Resurrect.Us.Web.Service
{
    public class UrlCheckerService : IUrlCheckerService
    {
        public readonly IHttpClientWrapper httpClientWrapper;

        public UrlCheckerService(IHttpClientWrapper httpClientWrapper)
        {
            this.httpClientWrapper = httpClientWrapper;
        }

        public async Task<HttpStatusCode> CheckUrl(string url)
        {
            if (String.IsNullOrEmpty(url) || !Uri.IsWellFormedUriString(url, UriKind.Absolute))
            {
                throw new ArgumentException("Url must be an absolute url");
            }
            var response = await this.httpClientWrapper.GetAsync(url);
            return response.StatusCode;
        }
    }
}
