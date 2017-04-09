using Moq;
using Resurrect.Us.Web.Service;
using Resurrect.Us.Web.Service.Wrappers;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Resurrect.Us.Tests.Web.Services
{
    public class UrlCheckerServiceTests
    {
        [Fact]
        public async Task CheckUrlShallReturnCorrectStatusCode()
        {
            var httpClientWrapperMock = new Mock<IHttpClientWrapper>();
            httpClientWrapperMock.Setup(h => h.GetAsync(It.IsAny<string>()))
                .Returns(Task.FromResult(new HttpResponseMessage(HttpStatusCode.OK)));
            var sut = new UrlCheckerService(httpClientWrapperMock.Object);
            var result = await sut.CheckUrl("http://some.url");
            Assert.Equal<HttpStatusCode>(HttpStatusCode.OK, result);
            httpClientWrapperMock.Setup(h => h.GetAsync(It.IsAny<string>()))
                .Returns(Task.FromResult(new HttpResponseMessage(HttpStatusCode.NotFound)));
            result = await sut.CheckUrl("http://some.url");
            Assert.Equal<HttpStatusCode>(HttpStatusCode.NotFound, result);
        }

        [Fact]
        public async Task CheckUrlShallThrowExceptionWhenUrlIsNotAbsolute()
        {
            var httpClientWrapperMock = new Mock<IHttpClientWrapper>();
            var sut = new UrlCheckerService(httpClientWrapperMock.Object);
            await Assert.ThrowsAsync<ArgumentException>(async () => await sut.CheckUrl("/some.url"));
        }
        [Fact]
        public async Task CheckUrlShallThrowExceptionWhenUrlIsEmpty()
        {
            var httpClientWrapperMock = new Mock<IHttpClientWrapper>();
            var sut = new UrlCheckerService(httpClientWrapperMock.Object);
            await Assert.ThrowsAsync<ArgumentException>(async () => await sut.CheckUrl(""));
        }

    }
}
