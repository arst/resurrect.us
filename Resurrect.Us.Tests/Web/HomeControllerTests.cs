using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using Resurrect.Us.Data.Models;
using Resurrect.Us.Data.Services;
using Resurrect.Us.Web.Controllers;
using Resurrect.Us.Web.Models;
using Resurrect.Us.Web.Service;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Resurrect.Us.Tests.Web
{
    public class HomeControllerTests
    {
        private Mock<IUrlShortenerService> GetShortenerMock()
        {
            var result = new Mock<IUrlShortenerService>();
            return result;
        }

        private Mock<ILogger<HomeController>> GetLoggerMock()
        {
            var result = new Mock<ILogger<HomeController>>();

            return result;
        }

        [Fact]
        public void IndexShouldReturnIndexView()
        {
            var sut = new HomeController(this.GetShortenerMock().Object, this.GetLoggerMock().Object);
            var viewResult = sut.Index();
            Assert.NotNull(viewResult as ViewResult);
            Assert.Equal((viewResult as ViewResult).ViewName, "Index");
        }

        [Fact]
        public async Task PostToIndexWithInvalidModelShouldReturnIndexView()
        {
            var sut = new HomeController(this.GetShortenerMock().Object, this.GetLoggerMock().Object);
            sut.ModelState.AddModelError("Url", "Test error");
            HomePageViewModel viewModel = new HomePageViewModel();
            viewModel.Url = "http://test.test";
            var result = await sut.Index(viewModel);
            var viewResult = result as ViewResult;
            Assert.NotNull(viewResult);
            Assert.Equal(viewResult.ViewName, "Index");
            Assert.NotNull(viewResult.Model as HomePageViewModel);
            Assert.Equal((viewResult.Model as HomePageViewModel).Url, "http://test.test");
        }

        [Fact]
        public async Task PostToIndexWithValdModelShouldReturnCorrectView()
        {
            var sut = new HomeController(this.GetShortenerMock().Object, this.GetLoggerMock().Object);
            sut.ControllerContext.HttpContext = new DefaultHttpContext();
            sut.ControllerContext.HttpContext.Request.Scheme = "http";
            sut.ControllerContext.HttpContext.Request.Host = new HostString("test.com");
            HomePageViewModel viewModel = new HomePageViewModel();
            viewModel.Url = "http://test.test";
            var result = await sut.Index(viewModel);
            var viewResult = result as ViewResult;
            Assert.NotNull(viewResult);
            Assert.Equal(viewResult.ViewName, "Result");
            Assert.NotNull(viewResult.Model);
            Assert.StartsWith("http://test.com", viewResult.Model.ToString());
        }

        [Fact]
        public async Task PostToIndexWithValdModelShouldCallForShortUrl()
        {
            var shortenerMock = this.GetShortenerMock();
            var sut = new HomeController(shortenerMock.Object, this.GetLoggerMock().Object);
            sut.ControllerContext.HttpContext = new DefaultHttpContext();
            sut.ControllerContext.HttpContext.Request.Scheme = "http";
            sut.ControllerContext.HttpContext.Request.Host = new HostString("test.com");
            HomePageViewModel viewModel = new HomePageViewModel();
            viewModel.Url = "http://test.test";
            var result = await sut.Index(viewModel);
            shortenerMock.Verify(s => s.GetShortUrlAsync(It.Is<string>(url => url == viewModel.Url)), Times.Once());
        }
    }
}
