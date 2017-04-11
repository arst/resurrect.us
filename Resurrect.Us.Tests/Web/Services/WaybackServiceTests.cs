using Moq;
using Resurrect.Us.Web.Models;
using Resurrect.Us.Web.Service;
using Resurrect.Us.Web.Service.Wrappers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Resurrect.Us.Tests.Web.Services
{
    public class WaybackServiceTests
    {
        [Fact]
        public async Task GetWaybackAsyncShouldThrowArgumentExceptionWhenUrlIsNotAbsoluteUrl()
        {
            var httpClientWrapperMock = new Mock<IHttpClientWrapper>();
            var sut = new WaybackService(httpClientWrapperMock.Object);
            await Assert.ThrowsAsync<ArgumentException>(async () => await sut.GetWaybackAsync("/someurl"));
        }

        [Fact]
        public async Task GetWaybackAsyncShouldThrowArgumentExceptionWhenUrlIsNullAbsoluteUrl()
        {
            var httpClientWrapperMock = new Mock<IHttpClientWrapper>();
            var sut = new WaybackService(httpClientWrapperMock.Object);
            await Assert.ThrowsAsync<ArgumentException>(async () => await sut.GetWaybackAsync(null));
        }

        [Fact]
        public async Task GetWaybackAsyncShouldReturnCorrectResultWhenUrlIsValid()
        {
            var httpClientWrapperMock = new Mock<IHttpClientWrapper>();
            WaybackResponse dummyResponse = new WaybackResponse() {
                Closest = new ArchivedSnapshots() {
                    Closest = new ArchivedSnapshot() {
                        Available = true,
                        Status = 1,
                        Timestamp = DateTime.Now.ToString(),
                        Url = "test_url"
                    }
                }
            };
            MemoryStream stream = new MemoryStream();
            var dummyString = "{ \"archived_snapshots\": { \"closest\": { \"available\": true, \"url\": \"http://web.archive.org/web/20130919044612/http://example.com/\", \"timestamp\": \"20130919044612\", \"status\": \"200\"} }}";
            StreamWriter writer = new StreamWriter(stream);
            writer.Write(dummyString);
            writer.Flush();
            stream.Position = 0;
            httpClientWrapperMock.Setup(h => h.GetStreamAsync(It.IsAny<string>())).Returns(Task.FromResult(stream as Stream));
            var sut = new WaybackService(httpClientWrapperMock.Object);
            var result = await sut.GetWaybackAsync("http://some.url");
            Assert.Equal("http://web.archive.org/web/20130919044612/http://example.com/", result.GetClosestUrl());
        }

        [Fact]
        public async Task GetWaybackAsyncShouldReturnEmptyResultWhenServiceResponseIsNull()
        {
            var httpClientWrapperMock = new Mock<IHttpClientWrapper>();
            httpClientWrapperMock.Setup(h => h.GetStreamAsync(It.IsAny<string>())).Returns(Task.FromResult<Stream>(null));
            var sut = new WaybackService(httpClientWrapperMock.Object);
            var result = await sut.GetWaybackAsync("http://some.url");
            Assert.Equal("", result.GetClosestUrl());
        }
    }
}
