using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Threading.Tasks;
using Resurrect.Us.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace Resurrect.Us.Data.Services
{
    public class ResurrectRecordsStorageService : IDisposable, IResurrectRecordsStorageService
    {
        private readonly ResurrectRecordsContext context;

        public ResurrectRecordsStorageService(ResurrectRecordsContext context)
        {
            this.context = context;
        }

        public async Task<ResurrectionRecord> GetResurrectionRecordByUrlAsync(string url)
        {
            return await this.context.ResurrectRecords.FirstOrDefaultAsync(r => r.Url == url);
        }

        public async Task<ResurrectionRecord> GetResurrectionRecordAsync(long id)
        {
            return await this.context.ResurrectRecords.FirstOrDefaultAsync(r => r.Id == id);
        }

        public async Task<ResurrectionRecord> AddRecordAsync(ResurrectionRecord record)
        {
            var result = (await this.context.ResurrectRecords.AddAsync(record)).Entity;
            await this.context.SaveChangesAsync();
            return result;
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
                using (ctx)
                {
                    ctx.SaveChanges();
                }
                
            }
        }
    }
}
