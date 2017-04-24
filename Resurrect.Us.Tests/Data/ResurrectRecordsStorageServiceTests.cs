using Microsoft.EntityFrameworkCore;
using Resurrect.Us.Data.Models;
using Resurrect.Us.Data.Services;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Resurrect.Us.Tests.Data
{
    public class ResurrectRecordsStorageServiceTests
    {
        private ResurrectRecordsContext GetContextMock()
        {
            var opts = new DbContextOptionsBuilder<ResurrectRecordsContext>()
                 .UseInMemoryDatabase(Guid.NewGuid().ToString())
                 .Options;
            var context = new ResurrectRecordsContext(opts);

            return context;
        }

        [Fact]
        public async Task ShouldCorrectlyFindRecordByUrl()
        {
            var ctx = this.GetContextMock();
            ctx.ResurrectRecords.Add(new ResurrectionRecord() {
                Url = "test.url",
                Id = 33
            });
            ctx.ResurrectRecords.Add(new ResurrectionRecord()
            {
                Url = "test1.url",
                Id = 34
            });
            ctx.SaveChanges();
            var sut = new ResurrectRecordsStorageService(ctx);

            var result = await sut.GetResurrectionRecordByUrlAsync("test.url");

            Assert.Equal(result.Id, 33);
        }

        [Fact]
        public async Task ShouldReturnNullWhenRecordDoesntExists()
        {
            var ctx = this.GetContextMock();
            ctx.ResurrectRecords.Add(new ResurrectionRecord()
            {
                Url = "test.url",
                Id = 33
            });
            ctx.ResurrectRecords.Add(new ResurrectionRecord()
            {
                Url = "test1.url",
                Id = 34
            });
            ctx.SaveChanges();
            var sut = new ResurrectRecordsStorageService(ctx);

            var result = await sut.GetResurrectionRecordByUrlAsync("test2.url");

            Assert.Null(result);
        }

        [Fact]
        public async Task ShouldNotFailWhenTwoRecordsWithTheSameUrlExists()
        {
            var ctx = this.GetContextMock();
            ctx.ResurrectRecords.Add(new ResurrectionRecord()
            {
                Url = "test.url",
                Id = 33
            });
            ctx.ResurrectRecords.Add(new ResurrectionRecord()
            {
                Url = "test.url",
                Id = 34
            });
            ctx.SaveChanges();
            var sut = new ResurrectRecordsStorageService(ctx);

            var result = await sut.GetResurrectionRecordByUrlAsync("test.url");

            Assert.Equal(result.Id, 33);
        }

        [Fact]
        public async Task ShouldCorrectlyFindRecxordById()
        {
            var ctx = this.GetContextMock();
            ctx.ResurrectRecords.Add(new ResurrectionRecord()
            {
                Url = "test.url",
                Id = 33
            });
            ctx.ResurrectRecords.Add(new ResurrectionRecord()
            {
                Url = "test1.url",
                Id = 34
            });
            ctx.SaveChanges();
            var sut = new ResurrectRecordsStorageService(ctx);

            var result = await sut.GetResurrectionRecordAsync(34);

            Assert.Equal(result.Id, 34);
            Assert.Equal("test1.url", result.Url);
        }

        [Fact]
        public async Task ShouldReturnNullWhenRecordWithIdNotFound()
        {
            var ctx = this.GetContextMock();
            ctx.ResurrectRecords.Add(new ResurrectionRecord()
            {
                Url = "test.url",
                Id = 33
            });
            ctx.ResurrectRecords.Add(new ResurrectionRecord()
            {
                Url = "test1.url",
                Id = 34
            });
            ctx.SaveChanges();
            var sut = new ResurrectRecordsStorageService(ctx);

            var result = await sut.GetResurrectionRecordAsync(1);

            Assert.Null(result);
        }

        [Fact]
        public async Task ShouldThrowArgeumntExceptionWhenRecordToUpdateIsNotFound()
        {
            var ctx = this.GetContextMock();
            var sut = new ResurrectRecordsStorageService(ctx);

            Assert.Throws<ArgumentException>(() => sut.UpdateRecord(new ResurrectionRecord() { Id = 1 }));
        }

        [Fact]
        public void ShouldCorrectlyUpdateRecord()
        {
            var ctx = this.GetContextMock();
            ctx.ResurrectRecords.Add(new ResurrectionRecord()
            {
                Url = "test.url",
                Id = 33,
                AccessCount = 1,
                LastAccess = new DateTime(1980,11,11),
                Timestamp = "123",
                Title = "Some title"
            });
            ctx.SaveChanges();
            var sut = new ResurrectRecordsStorageService(ctx);
            var toUpdate = new ResurrectionRecord() {
                Id = 33,
                AccessCount = 2,
                LastAccess = new DateTime(1990,12,12),
                Timestamp = "234",
                Title = "New title",
                Url = "test1.url"
            };
            var result = sut.UpdateRecord(toUpdate);

            Assert.Equal(result.Id, toUpdate.Id);
            Assert.Equal(result.AccessCount, toUpdate.AccessCount);
            Assert.Equal(result.LastAccess, toUpdate.LastAccess);
            Assert.Equal(result.Timestamp, toUpdate.Timestamp);
            Assert.Equal(result.Title, toUpdate.Title);
            Assert.Equal(result.Url, result.Url);
        }
    }
}
