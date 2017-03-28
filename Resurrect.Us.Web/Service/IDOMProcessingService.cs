using Resurrect.Us.Web.Models;

namespace Resurrect.Us.Web.Service
{
    public interface IDOMProcessingService
    {
        HTMLKeypointsResult ExtractHTMLKeypoints(string html);
    }
}