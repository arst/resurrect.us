using System.Threading.Tasks;

namespace Resurrect.Us.Web.Service
{
    public interface IUrlShortenerService
    {
        Task<string> GetDeshortenedUrl(string shortUrl);
        Task<string> GetShortUrlAsync(string url);
    }
}