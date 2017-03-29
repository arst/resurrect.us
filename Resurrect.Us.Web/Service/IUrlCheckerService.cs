using System.Net;
using System.Threading.Tasks;

namespace Resurrect.Us.Web.Service
{
    public interface IUrlCheckerService
    {
        Task<HttpStatusCode> CheckUrl(string url);
    }
}