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
        public async Task<HttpStatusCode> CheckUrl(string url)
        {
            HttpClient client = new HttpClient();
            var response = await client.GetAsync(url);

            return response.StatusCode;
        }
    }
}
