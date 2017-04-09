﻿using Resurrect.Us.Data.Models;
using Resurrect.Us.Data.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Resurrect.Us.Web.Service
{
    public class UrlShortenerService : IUrlShortenerService
    {
        private readonly IWaybackService waybackService;
        private readonly IKeyPointsExtractorService keypointExtractorService;
        private readonly IResurrectRecordsStorageService resurrectRecordStorageService;
        private readonly IHashService hashGenerator;

        public UrlShortenerService(IWaybackService waybackService, IKeyPointsExtractorService keyPointExtractorService, IResurrectRecordsStorageService resurrectRecordStorageService, IHashService hashGenerator)
        {
            this.waybackService = waybackService;
            this.keypointExtractorService = keyPointExtractorService;
            this.resurrectRecordStorageService = resurrectRecordStorageService;
            this.hashGenerator = hashGenerator;
        }

        public async Task<string> GetShortUrlAsync(string url)
        {
            var wayBackResult = await this.waybackService.GetWaybackAsync(url);
            var keypoints = await this.keypointExtractorService.GetHtmlKeypointsFromUrl(url);
            var resurrectRecord = new ResurrectionRecord()
            {
                AccessCount = 0,
                LastAccess = DateTime.Now,
                Timestamp = wayBackResult != null ? wayBackResult.GetClosestTimestamp() : "",
                Title = keypoints.Title,
                Url = url,
                Keywords = keypoints.Keywords.Select(k => new Keyword() { Value = k }).ToList()

            };
            var existingRecord = await this.resurrectRecordStorageService.GetResurrectionRecordByUrlAsync(url);
            var hash = String.Empty;

            if (existingRecord != null)
            {
                hash = this.hashGenerator.GetHash(existingRecord.Id);
            }
            else
            {
                var result = await this.resurrectRecordStorageService.AddRecordAsync(resurrectRecord);
                hash = this.hashGenerator.GetHash(result.Id);
            }

            return hash;
        }

        public async Task<string> GetDeshortenedUrl(string shortUrl)
        {
            var result = String.Empty;
            var id = this.hashGenerator.GetRecordId(shortUrl);
            var record = await this.resurrectRecordStorageService.GetResurrectionRecordAsync(id);

            if (record != null)
            {
                result = record.Url;
            }

            return result;
        }
    }
}