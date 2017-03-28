using Resurrect.Us.Data.Models;
using System.Threading.Tasks;

namespace Resurrect.Us.Data.Services
{
    public interface IResurrectRecordsStorageService
    {
        Task<ResurrectionRecord> AddRecordAsync(ResurrectionRecord record);
        ResurrectionRecord GetResurrectionRecordAsync(string id);
        ResurrectionRecord UpdateRecord(ResurrectionRecord record);
    }
}