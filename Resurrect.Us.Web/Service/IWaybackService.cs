using System.Threading.Tasks;
using Resurrect.Us.Web.Models;

namespace Resurrect.Us.Web.Service
{
    public interface IWaybackService
    {
        Task<WaybackResponse> GetWaybackAsync(string url);
    }
}