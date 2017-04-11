using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace Resurrect.Us.Web.Service.Wrappers
{
    public class HttpClientWrapper : IHttpClientWrapper
    {
        public Task<HttpResponseMessage> GetAsync(string requestUri)
        {
            return Task.Run(async () => {
                using (var client = new HttpClient())
                {
                    return await client.GetAsync(requestUri);
                }
            });
        }

        public Task<string> GetStringAsync(string requestUri)
        {
            return Task.Run(async () => {
                using (var client = new HttpClient())
                {
                    return await client.GetStringAsync(requestUri);
                }
            });
        }

        public Task<Stream> GetStreamAsync(string requestUri)
        {
            return Task.Run(async () => {
                using (var client = new HttpClient())
                {
                    return await client.GetStreamAsync(requestUri);
                }
            });
        }
    }
}
