﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Threading.Tasks;
using Resurrect.Us.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace Resurrect.Us.Data.Services
{
    public class ShortenedUrlRecordRecordStorageService : IDisposable, IShortenedUrlRecordRecordStorageService
    {
        private readonly ShortenedUrlRecordRecordsContext context;

        public ShortenedUrlRecordRecordStorageService(ShortenedUrlRecordRecordsContext context)
        {
            this.context = context;
        }

        public async Task<ShortenedUrlRecordRecord> GetResurrectionRecordByUrlAsync(string url)
        {
            return await this.context.ShortenedUrlRecordRecords.FirstOrDefaultAsync(r => r.Url == url);
        }

        public async Task<ShortenedUrlRecordRecord> GetResurrectionRecordAsync(long id)
        {
            return await this.context.ShortenedUrlRecordRecords.FirstOrDefaultAsync(r => r.Id == id);
        }

        public async Task<ShortenedUrlRecordRecord> AddRecordAsync(ShortenedUrlRecordRecord record)
        {
            var result = (await this.context.ShortenedUrlRecordRecords.AddAsync(record)).Entity;
            await this.context.SaveChangesAsync();
            return result;
        }

        public ShortenedUrlRecordRecord UpdateRecord(ShortenedUrlRecordRecord record)
        {
            var toUpdate = this.context.ShortenedUrlRecordRecords.FirstOrDefault(r => record.Id == r.Id);

            if (toUpdate == null)
            {
                throw new ArgumentException("Can't update the record. Seems that it is missing in the database");
            }

            toUpdate.AccessCount = record.AccessCount;
            toUpdate.LastAccess = record.LastAccess;
            toUpdate.Timestamp = record.Timestamp;
            toUpdate.Url = record.Url;
            toUpdate.Title = record.Title;

            return this.context.ShortenedUrlRecordRecords.Update(toUpdate).Entity;
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
