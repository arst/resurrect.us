using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
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
        [Fact]
        public void IndexShouldReturnIndexView()
        {
            var waybackMoq = new Mock<IWaybackService>();
            var keyPointsExtractorMoq = new Mock<IKeyPointsExtractorService>();
            var storageMoq = new Mock<IResurrectRecordsStorageService>();
            var hashStrategyMock = new Mock<IHashStrategy>();
            var hashMoq = new Mock<HashService>(hashStrategyMock.Object);
            var sut = new HomeController(waybackMoq.Object,keyPointsExtractorMoq.Object, storageMoq.Object, hashMoq.Object);
            var viewResult = sut.Index();
            Assert.NotNull(viewResult as ViewResult);
            Assert.Equal((viewResult as ViewResult).ViewName, "Index");
        }

        [Fact]
        public async Task PostToIndexWithInvalidModelShouldReturnIndexView()
        {
            var waybackMoq = new Mock<IWaybackService>();
            var keyPointsExtractorMoq = new Mock<IKeyPointsExtractorService>();
            var storageMoq = new Mock<IResurrectRecordsStorageService>();
            var hashMoq = new Mock<IHashService>();
            var sut = new HomeController(waybackMoq.Object, keyPointsExtractorMoq.Object, storageMoq.Object, hashMoq.Object);
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
            var waybackMoq = new Mock<IWaybackService>();
            var keyPointsExtractorMoq = new Mock<IKeyPointsExtractorService>();
            HTMLKeypointsResult moqKeypoints = new HTMLKeypointsResult()
            {
                Title = "Test",
                Keywords = new List<string>() { "Test1", "Test2", "Test3" }
            };
            ResurrectionRecord moqRecord = new ResurrectionRecord()
            {
                Id = 1
            };

            keyPointsExtractorMoq.Setup(kp => kp.GetHtmlKeypointsFromUrl(It.IsAny<string>())).Returns(Task.FromResult<HTMLKeypointsResult>(moqKeypoints));
            var storageMoq = new Mock<IResurrectRecordsStorageService>();
            var hashStrategyMock = new Mock<IHashStrategy>();
            var hashMoq = new Mock<HashService>(hashStrategyMock.Object);
            storageMoq.Setup(s => s.AddRecordAsync(It.IsAny<ResurrectionRecord>())).Returns(Task.FromResult(moqRecord));
            var sut = new HomeController(waybackMoq.Object, keyPointsExtractorMoq.Object, storageMoq.Object, hashMoq.Object);
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
        public async Task PostToIndexWithValdModelShouldCreateNewRecord()
        {
            var waybackMoq = new Mock<IWaybackService>();
            var keyPointsExtractorMoq = new Mock<IKeyPointsExtractorService>();
            HTMLKeypointsResult moqKeypoints = new HTMLKeypointsResult()
            {
                Title = "Test",
                Keywords = new List<string>() { "Test1", "Test2", "Test3" }
            };
            ResurrectionRecord moqRecord = new ResurrectionRecord()
            {
                Id = 1
            };

            keyPointsExtractorMoq.Setup(kp => kp.GetHtmlKeypointsFromUrl(It.IsAny<string>())).Returns(Task.FromResult<HTMLKeypointsResult>(moqKeypoints));
            var storageMoq = new Mock<IResurrectRecordsStorageService>();
            storageMoq.Setup(s => s.AddRecordAsync(It.IsAny<ResurrectionRecord>())).Returns(Task.FromResult(moqRecord));
            var hashStrategyMock = new Mock<IHashStrategy>();
            var hashMoq = new Mock<HashService>(hashStrategyMock.Object);
            var sut = new HomeController(waybackMoq.Object, keyPointsExtractorMoq.Object, storageMoq.Object, hashMoq.Object);
            sut.ControllerContext.HttpContext = new DefaultHttpContext();
            sut.ControllerContext.HttpContext.Request.Scheme = "http";
            sut.ControllerContext.HttpContext.Request.Host = new HostString("test.com");
            HomePageViewModel viewModel = new HomePageViewModel();
            viewModel.Url = "http://test.test";
            var result = await sut.Index(viewModel);
            storageMoq.Verify(s => s.AddRecordAsync(It.IsAny<ResurrectionRecord>()), Times.Once);
        }

        [Fact]
        public async Task PostToIndexWithValdModelShouldCreateNewRecordWithLatestWaybackTimestamp()
        {
            var waybackMoq = new Mock<IWaybackService>();
            var moqWaybackResult = new WaybackResponse()
            {
                Closest = new ArchivedSnapshots()
                {
                    Closest = new ArchivedSnapshot()
                    {
                        Available = true,
                        Status = 200,
                        Timestamp = "test_timestamp",
                        Url = "test_url"
                    }
                }
            };
            waybackMoq.Setup(w => w.GetWaybackAsync(It.IsAny<string>())).Returns(Task.FromResult(moqWaybackResult));
            var keyPointsExtractorMoq = new Mock<IKeyPointsExtractorService>();
            HTMLKeypointsResult moqKeypoints = new HTMLKeypointsResult()
            {
                Title = "Test",
                Keywords = new List<string>() { "Test1", "Test2", "Test3" }
            };
            ResurrectionRecord moqRecord = new ResurrectionRecord()
            {
                Id = 1
            };

            keyPointsExtractorMoq.Setup(kp => kp.GetHtmlKeypointsFromUrl(It.IsAny<string>())).Returns(Task.FromResult<HTMLKeypointsResult>(moqKeypoints));
            var storageMoq = new Mock<IResurrectRecordsStorageService>();
            storageMoq.Setup(s => s.AddRecordAsync(It.IsAny<ResurrectionRecord>())).Returns(Task.FromResult(moqRecord));
            var hashMoq = new Mock<IHashService>();
            var sut = new HomeController(waybackMoq.Object, keyPointsExtractorMoq.Object, storageMoq.Object, hashMoq.Object);
            sut.ControllerContext.HttpContext = new DefaultHttpContext();
            sut.ControllerContext.HttpContext.Request.Scheme = "http";
            sut.ControllerContext.HttpContext.Request.Host = new HostString("test.com");
            HomePageViewModel viewModel = new HomePageViewModel();
            viewModel.Url = "http://test.test";
            var result = await sut.Index(viewModel);
            storageMoq.Verify(s => s.AddRecordAsync(It.Is<ResurrectionRecord>(r => r.Timestamp == "test_timestamp")), Times.Once);
        }
    }
}
