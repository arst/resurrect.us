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
        [Fact]
        public void IndexShouldTryToGetRecordWithProvidedId()
        {
            var waybackMoq = new Mock<IWaybackService>();
            var urlCheckerMoq = new Mock<IUrlCheckerService>();
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
            var hashMoq = new Mock<IHashService>();
            hashMoq.Setup(h => h.GetRecordId(It.IsAny<string>())).Returns(() => 1);
            ResurrectionRecord moqRecord = new ResurrectionRecord()
            {
                Id = 1 
            };
            var storageMoq = new Mock<IResurrectRecordsStorageService>();
            storageMoq.Setup(s => s.GetResurrectionRecordAsync(It.IsAny<int>())).Returns(() => null);
            var sut = new RedirectController(storageMoq.Object, waybackMoq.Object, urlCheckerMoq.Object, hashMoq.Object);
            var result = sut.Index("test_id");
            storageMoq.Verify(s => s.GetResurrectionRecordAsync(It.Is<int>(id => id == 1)), Times.Once);
        }


        [Fact]
        public async Task IndexShouldRedirectToHomeIndexWhenRecordIsNotFound()
        {
            var waybackMoq = new Mock<IWaybackService>();
            var urlCheckerMoq = new Mock<IUrlCheckerService>();
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
            ResurrectionRecord moqRecord = new ResurrectionRecord()
            {
                Id = 1
            };
            var storageMoq = new Mock<IResurrectRecordsStorageService>();
            storageMoq.Setup(s => s.GetResurrectionRecordAsync(It.IsAny<int>())).Returns(() => null);
            var hashMoq = new Mock<IHashService>();
            var sut = new RedirectController(storageMoq.Object, waybackMoq.Object, urlCheckerMoq.Object, hashMoq.Object);
            var result = (await sut.Index("test_id")) as RedirectToActionResult;

            Assert.Equal(result.ActionName, "Index");
            Assert.Equal(result.ControllerName, "Home");
            Assert.Equal(result.Permanent, false);
        }

        [Fact]
        public async Task IndexShouldCheckCorrectUrlWhenRecordIsFound()
        {
            var waybackMoq = new Mock<IWaybackService>();
            var urlCheckerMoq = new Mock<IUrlCheckerService>();
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
            ResurrectionRecord moqRecord = new ResurrectionRecord()
            {
                Id = 1,
                Url = "test_url"
            };
            var storageMoq = new Mock<IResurrectRecordsStorageService>();
            storageMoq.Setup(s => s.GetResurrectionRecordAsync(It.IsAny<int>())).Returns(() => moqRecord);
            var hashMoq = new Mock<IHashService>();
            var sut = new RedirectController(storageMoq.Object, waybackMoq.Object, urlCheckerMoq.Object, hashMoq.Object);
            var result = (await sut.Index("test_id")) as RedirectToActionResult;
            urlCheckerMoq.Verify(u => u.CheckUrl(It.Is<string>(url => url == "test_url")));
        }

        [Fact]
        public async Task IndexShouldRedirectToRecordUrlWhenUrlIsAvailable()
        {
            var waybackMoq = new Mock<IWaybackService>();
            var urlCheckerMoq = new Mock<IUrlCheckerService>();
            urlCheckerMoq.Setup(c => c.CheckUrl(It.IsAny<string>())).Returns(Task.FromResult<HttpStatusCode>(HttpStatusCode.OK));
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
            ResurrectionRecord moqRecord = new ResurrectionRecord()
            {
                Id = 1,
                Url = "test_url"
            };
            var storageMoq = new Mock<IResurrectRecordsStorageService>();
            storageMoq.Setup(s => s.GetResurrectionRecordAsync(It.IsAny<int>())).Returns(() => moqRecord);
            var hashMoq = new Mock<IHashService>();
            var sut = new RedirectController(storageMoq.Object, waybackMoq.Object, urlCheckerMoq.Object, hashMoq.Object);
            var result = (await sut.Index("test_id")) as RedirectResult;
            Assert.Equal(result.Url, "test_url");
            Assert.Equal(result.Permanent, true);
        }

        [Fact]
        public async Task IndexShouldRedirectToWaybackUrlWhenUrlIsNotAvailable()
        {
            var waybackMoq = new Mock<IWaybackService>();
            var urlCheckerMoq = new Mock<IUrlCheckerService>();
            urlCheckerMoq.Setup(c => c.CheckUrl(It.IsAny<string>())).Returns(Task.FromResult<HttpStatusCode>(HttpStatusCode.NotFound));
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
            waybackMoq.Setup(w => w.GetWaybackAsync(It.IsAny<string>())).Returns(Task.FromResult(moqWaybackResult));
            ResurrectionRecord moqRecord = new ResurrectionRecord()
            {
                Id = 1,
                Url = "test_url"
            };
            var storageMoq = new Mock<IResurrectRecordsStorageService>();
            var hashMoq = new Mock<IHashService>();
            storageMoq.Setup(s => s.GetResurrectionRecordAsync(It.IsAny<int>())).Returns(() => moqRecord);
            var sut = new RedirectController(storageMoq.Object, waybackMoq.Object, urlCheckerMoq.Object, hashMoq.Object);
            var result = (await sut.Index("test_id")) as RedirectResult;
            Assert.Equal(result.Url, "wayback_test_url");
            Assert.Equal(result.Permanent, false);
        }




    }
}
