using Microsoft.AspNetCore.Mvc;
using Moq;
using Resurrect.Us.Data.Models;
using Resurrect.Us.Data.Services;
using Resurrect.Us.Web.Controllers;
using Resurrect.Us.Web.Models;
using Resurrect.Us.Web.Service;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Resurrect.Us.Tests.Web
{
    public class RedirectControllerTests
    {

        private class RedirectConstructorContext
        {
            public RedirectConstructorContext()
            {
                this.WaybackMock = new Mock<IWaybackService>();
                this.UrlCheckerMock = new Mock<IUrlCheckerService>();
                this.UrlShortenerMock = new Mock<IUrlShortenerService>();
            }

            public Mock<IWaybackService> WaybackMock { get; set; }
            public Mock<IUrlCheckerService> UrlCheckerMock { get; set; }
            public Mock<IUrlShortenerService> UrlShortenerMock { get; set; }
        }

        [Fact]
        public void IndexShallGetShortUrlByPassedId()
        {
            var sutContructoCtx = new RedirectConstructorContext();
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
            sutContructoCtx.WaybackMock
                .Setup(w => w.GetWaybackAsync(It.IsAny<string>()))
                .Returns(Task.FromResult(moqWaybackResult));

            var sut = new RedirectController(sutContructoCtx.WaybackMock.Object, 
                                             sutContructoCtx.UrlCheckerMock.Object,
                                             sutContructoCtx.UrlShortenerMock.Object);
            var result = sut.Index("test_id");
            sutContructoCtx.UrlShortenerMock.Verify(s => s.GetDeshortenedUrl(It.Is<string>(url => url == "test_id")), Times.Once());
        }


        [Fact]
        public async Task IndexShouldRedirectToHomeIndexWhenRecordIsNotFound()
        {
            var sutContructoCtx = new RedirectConstructorContext();
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
            sutContructoCtx.WaybackMock 
                .Setup(w => w.GetWaybackAsync(It.IsAny<string>()))
                .Returns(Task.FromResult(moqWaybackResult));
            var sut = new RedirectController(sutContructoCtx.WaybackMock.Object,
                                             sutContructoCtx.UrlCheckerMock.Object,
                                             sutContructoCtx.UrlShortenerMock.Object);
            var result = (await sut.Index("test_id")) as RedirectToActionResult;

            Assert.Equal(result.ActionName, "Index");
            Assert.Equal(result.ControllerName, "Home");
            Assert.Equal(result.Permanent, false);
        }

        [Fact]
        public async Task IndexShouldCheckCorrectUrlWhenRecordIsFound()
        {
            var sutContructoCtx = new RedirectConstructorContext();

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
            sutContructoCtx.WaybackMock.Setup(w => w.GetWaybackAsync(It.IsAny<string>())).Returns(Task.FromResult(moqWaybackResult));
            sutContructoCtx.UrlShortenerMock
                .Setup(s => s.GetDeshortenedUrl(It.IsAny<string>()))
                .Returns(() => Task.FromResult("test_url"));
            var sut = new RedirectController(sutContructoCtx.WaybackMock.Object,
                                             sutContructoCtx.UrlCheckerMock.Object,
                                             sutContructoCtx.UrlShortenerMock.Object);
            var result = (await sut.Index("test_id")) as RedirectToActionResult;
            sutContructoCtx.UrlCheckerMock.Verify(u => u.CheckUrl(It.Is<string>(url => url == "test_url")));
        }
        
        [Fact]
        public async Task IndexShouldRedirectToRecordUrlWhenUrlIsAvailable()
        {
            var sutContructoCtx = new RedirectConstructorContext();
            sutContructoCtx.UrlCheckerMock
                .Setup(c => c.CheckUrl(It.IsAny<string>()))
                .Returns(Task.FromResult<HttpStatusCode>(HttpStatusCode.OK));
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
            sutContructoCtx.WaybackMock
                .Setup(w => w.GetWaybackAsync(It.IsAny<string>()))
                .Returns(Task.FromResult(moqWaybackResult));
            sutContructoCtx.UrlShortenerMock
                .Setup(s => s.GetDeshortenedUrl(It.IsAny<string>()))
                .Returns(() => Task.FromResult("test_url"));
            var sut = new RedirectController(sutContructoCtx.WaybackMock.Object,
                                             sutContructoCtx.UrlCheckerMock.Object,
                                             sutContructoCtx.UrlShortenerMock.Object);
            var result = (await sut.Index("test_id")) as RedirectResult;
            Assert.Equal(result.Url, "test_url");
            Assert.Equal(result.Permanent, true);
        }

        [Fact]
        public async Task IndexShouldRedirectToWaybackUrlWhenUrlIsNotAvailable()
        {
            var sutContructoCtx = new RedirectConstructorContext();
            
            var moqWaybackResult = new WaybackResponse()
            {
                Closest = new ArchivedSnapshots()
                {
                    Closest = new ArchivedSnapshot()
                    {
                        Available = true,
                        Status = 200,
                        Timestamp = "test_timestamp",
                        Url = "wayback_test_url"
                    }
                }
            };
            sutContructoCtx.WaybackMock
                .Setup(w => w.GetWaybackAsync(It.IsAny<string>()))
                .Returns(Task.FromResult(moqWaybackResult));
            sutContructoCtx.UrlCheckerMock
                .Setup(c => c.CheckUrl(It.IsAny<string>()))
                .Returns(Task.FromResult<HttpStatusCode>(HttpStatusCode.NotFound));
            sutContructoCtx.UrlShortenerMock
               .Setup(s => s.GetDeshortenedUrl(It.IsAny<string>()))
               .Returns(() => Task.FromResult("test_url"));
            ResurrectionRecord moqRecord = new ResurrectionRecord()
            {
                Id = 1,
                Url = "test_url"
            };
            var sut = new RedirectController(sutContructoCtx.WaybackMock.Object,
                                              sutContructoCtx.UrlCheckerMock.Object,
                                              sutContructoCtx.UrlShortenerMock.Object);
            var result = (await sut.Index("test_id")) as RedirectResult;
            Assert.Equal(result.Url, "wayback_test_url");
            Assert.Equal(result.Permanent, false);
        }




    }
}
