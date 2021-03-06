﻿using Microsoft.Extensions.Caching.Distributed;
using Resurrect.Us.Data.Models;
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
        private readonly IShortenedUrlRecordRecordStorageService shortenedRecordStorageService;
        private readonly IHashService hashGenerator;
        private readonly IDistributedCache distributedCache;

        public UrlShortenerService(IWaybackService waybackService, IKeyPointsExtractorService keyPointExtractorService, IShortenedUrlRecordRecordStorageService shortenedRecordStorageService, IHashService hashGenerator, IDistributedCache distributedCache)
        {
            this.waybackService = waybackService;
            this.keypointExtractorService = keyPointExtractorService;
            this.shortenedRecordStorageService = shortenedRecordStorageService;
            this.hashGenerator = hashGenerator;
            this.distributedCache = distributedCache;
        }

        public async Task<string> GetShortUrlAsync(string url, bool googleForMe)
        {
            var wayBackResult = await this.waybackService.GetWaybackAsync(url);
            var keypoints = await this.keypointExtractorService.GetHtmlKeypointsFromUrl(url);
            var resurrectRecord = new ShortenedUrlRecordRecord()
            {
                AccessCount = 0,
                LastAccess = DateTime.Now,
                Timestamp = wayBackResult != null ? wayBackResult.GetClosestTimestamp() : "",
                Title = keypoints.Title,
                Url = url,
                ShallBeGoogled = googleForMe,
                Keywords = keypoints.Keywords.Select(k => new Keyword() { Value = k }).ToList()

            };
            var existingRecord = await this.shortenedRecordStorageService.GetResurrectionRecordByUrlAsync(url);
            var hash = String.Empty;

            if (existingRecord != null)
            {
                hash = this.hashGenerator.GetHash(existingRecord.Id);
            }
            else
            {
                var result = await this.shortenedRecordStorageService.AddRecordAsync(resurrectRecord);
                hash = this.hashGenerator.GetHash(result.Id);
            }

            return hash;
        }

        public async Task<string> GetDeshortenedUrl(string shortUrl)
        {
            var result = String.Empty;
            var cached = await this.distributedCache.GetStringAsync(String.Concat(shortUrl, "DeshortenedUrl"));
            if (String.IsNullOrEmpty(cached))
            {
                var id = this.hashGenerator.GetRecordId(shortUrl);


                var record = await this.shortenedRecordStorageService.GetResurrectionRecordAsync(id);

                if (record != null)
                {
                    await this.distributedCache.SetStringAsync(String.Concat(shortUrl, "DeshortenedUrl"), record.Url.ToString());
                    result = record.Url;
                }
            }
            else
            {
                result = cached;
            }
            

            return result;
        }
    }
}
