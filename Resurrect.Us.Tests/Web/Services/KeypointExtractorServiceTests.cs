using Moq;
using Resurrect.Us.Web.Service;
using Resurrect.Us.Web.Service.Wrappers;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Resurrect.Us.Tests.Web.Services
{
    class KeypointExtractorServiceTests
    {
        [Fact]
        public async Task GetHtmlKeypointsFromUrlMustReturnEmptyKeypointResultWhenUrlIsNotAvailable()
        {
            var domProcessingMock = new Mock<IDOMProcessingService>();
            var urlCheckerMock = new Mock<IUrlCheckerService>();
            urlCheckerMock
                .Setup(u => u.CheckUrl(It.IsAny<string>()))
                .Returns(Task.FromResult(HttpStatusCode.NotFound));
            var httpClientWrapperMock = new Mock<IHttpClientWrapper>();
            var sut = new KeyPointsExtractorService(domProcessingMock.Object, urlCheckerMock.Object, httpClientWrapperMock.Object);

            var result = await sut.GetHtmlKeypointsFromUrl("http://some.url");

            Assert.Equal(null, result.Title);
            Assert.Equal(0, result.Keywords.Count);
        }

        [Fact]
        public async Task GetHtmlKeypointsFromUrlMustReturnKeypointResultForAvailableUrl ()
        {
            var domProcessingMock = new Mock<IDOMProcessingService>();
            domProcessingMock
                .Setup(d => d.ExtractHTMLKeypoints(It.IsAny<string>()))
                .Returns(new Us.Web.Models.HTMLKeypointsResult() {
                    Title = "test",
                    Keywords = new List<string>() {
                        "word1",
                        "word2"
                    }
                });
            var urlCheckerMock = new Mock<IUrlCheckerService>();
            urlCheckerMock
                .Setup(u => u.CheckUrl(It.IsAny<string>()))
                .Returns(Task.FromResult(HttpStatusCode.OK));
            var httpClientWrapperMock = new Mock<IHttpClientWrapper>();
            var sut = new KeyPointsExtractorService(domProcessingMock.Object, urlCheckerMock.Object, httpClientWrapperMock.Object);

            var result = await sut.GetHtmlKeypointsFromUrl("http://some.url");

            Assert.Equal("test", result.Title);
            Assert.Contains("word1", result.Keywords);
            Assert.Contains("word2", result.Keywords);
            Assert.Equal(2, result.Keywords.Count);
        }

        [Fact]
        public async Task GetHtmlKeypointsFromUrlMustThrowAnExceptionWhenUrIsNotAbsolute()
        {
            var domProcessingMock = new Mock<IDOMProcessingService>();
            var urlCheckerMock = new Mock<IUrlCheckerService>();
            var httpClientWrapperMock = new Mock<IHttpClientWrapper>();
            var sut = new KeyPointsExtractorService(domProcessingMock.Object, urlCheckerMock.Object, httpClientWrapperMock.Object);
            await Assert.ThrowsAsync<ArgumentException>(async () => await sut.GetHtmlKeypointsFromUrl("/some.url"));
        }

        [Fact]
        public async Task GetHtmlKeypointsFromUrlMustThrowAnExceptionWhenUrIsNull()
        {
            var domProcessingMock = new Mock<IDOMProcessingService>();
            var urlCheckerMock = new Mock<IUrlCheckerService>();
            var httpClientWrapperMock = new Mock<IHttpClientWrapper>();
            var sut = new KeyPointsExtractorService(domProcessingMock.Object, urlCheckerMock.Object, httpClientWrapperMock.Object);
            await Assert.ThrowsAsync<ArgumentException>(async () => await sut.GetHtmlKeypointsFromUrl(null));
        }

    }
}
