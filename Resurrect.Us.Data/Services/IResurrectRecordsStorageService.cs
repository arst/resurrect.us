using Resurrect.Us.Data.Models;
using System.Threading.Tasks;

namespace Resurrect.Us.Data.Services
{
    public interface IResurrectRecordsStorageService
    {
        Task<ResurrectionRecord> AddRecordAsync(ResurrectionRecord record);
        Task<ResurrectionRecord> GetResurrectionRecordAsync(long id);
        Task<ResurrectionRecord> GetResurrectionRecordByUrlAsync(string url);
        ResurrectionRecord UpdateRecord(ResurrectionRecord record);
    }
}