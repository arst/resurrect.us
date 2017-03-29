using System.Threading.Tasks;
using Resurrect.Us.Web.Models;

namespace Resurrect.Us.Web.Service
{
    public interface IKeyPointsExtractorService
    {
        Task<HTMLKeypointsResult> GetHtmlKeypointsFromUrl(string url);
    }
}