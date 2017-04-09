using System.Net.Http;
using System.Threading.Tasks;

namespace Resurrect.Us.Web.Service.Wrappers
{
    public interface IHttpClientWrapper
    { 
        Task<HttpResponseMessage> GetAsync(string requestUri);
        Task<string> GetStringAsync(string requestUri);
        Task<System.IO.Stream> GetStreamAsync(string requestUri);
    }
}