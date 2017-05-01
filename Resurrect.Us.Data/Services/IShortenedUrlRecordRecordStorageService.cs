using Resurrect.Us.Data.Models;
using System.Threading.Tasks;

namespace Resurrect.Us.Data.Services
{
    public interface IShortenedUrlRecordRecordStorageService
    {
        Task<ShortenedUrlRecordRecord> AddRecordAsync(ShortenedUrlRecordRecord record);
        Task<ShortenedUrlRecordRecord> GetResurrectionRecordAsync(long id);
        Task<ShortenedUrlRecordRecord> GetResurrectionRecordByUrlAsync(string url);
        ShortenedUrlRecordRecord UpdateRecord(ShortenedUrlRecordRecord record);
    }
}