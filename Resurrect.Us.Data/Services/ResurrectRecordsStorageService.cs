using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Threading.Tasks;
using Resurrect.Us.Data.Models;

namespace Resurrect.Us.Data.Services
{
    public class ResurrectRecordsStorageService : IDisposable, IResurrectRecordsStorageService
    {
        private readonly ResurrectRecordsContext context;

        public ResurrectRecordsStorageService(ResurrectRecordsContext context)
        {
            this.context = context;
        }

        public ResurrectionRecord GetResurrectionRecordAsync(string id)
        {
            return this.context.ResurrectRecords.FirstOrDefault(r => r.Id == id);
        }

        public async Task<ResurrectionRecord> AddRecordAsync(ResurrectionRecord record)
        {
            return (await this.context.ResurrectRecords.AddAsync(record)).Entity;
        }

        public ResurrectionRecord UpdateRecord(ResurrectionRecord record)
        {
            var toUpdate = this.context.ResurrectRecords.FirstOrDefault(r => record.Id == r.Id);

            if (toUpdate == null)
            {
                throw new ArgumentException("Can't update the record. Seems that it is missing in the database");
            }

            toUpdate.AccessCount = record.AccessCount;
            toUpdate.LastAccess = record.LastAccess;
            toUpdate.Timestamp = record.Timestamp;
            toUpdate.Url = record.Url;
            toUpdate.Title = record.Title;

            return this.context.ResurrectRecords.Update(toUpdate).Entity;
        }

        public void Dispose()
        {
            var ctx = this.context;

            if (ctx != null)
            {
                ctx.SaveChanges();
            }
        }
    }
}
