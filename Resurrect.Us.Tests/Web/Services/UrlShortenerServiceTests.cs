using Moq;
using Resurrect.Us.Data.Models;
using Resurrect.Us.Data.Services;
using Resurrect.Us.Web.Models;
using Resurrect.Us.Web.Service;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Resurrect.Us.Tests.Web.Services
{
    public class UrlShortenerServiceTests
    {
        [Fact]
        public async Task GetShortUrlAsyncShouldReturnExistingHashWhenRecordWithTheSameUrlIsFound()
        {
            var waybackMock = new Mock<IWaybackService>();
            var keypointExtractorMock = new Mock<IKeyPointsExtractorService>();
            keypointExtractorMock
                .Setup(kp => kp.GetHtmlKeypointsFromUrl(It.IsAny<String>()))
                .Returns(Task.FromResult(new HTMLKeypointsResult()));
            var resurrectRecordsStorageMock = new Mock<IShortenedUrlRecordRecordStorageService>();
            resurrectRecordsStorageMock
                .Setup(r => r.GetResurrectionRecordByUrlAsync(It.IsAny<string>()))
                .Returns(Task.FromResult(new ShortenedUrlRecordRecord() {
                    Id = 1,
                    Url = "test.test"
                }));
            var hashServiceMock = new Mock<IHashService>();
            hashServiceMock
                .Setup(h => h.GetHash(It.Is<long>(identifier => identifier == 1)))
                .Returns(() => "hash");

            var sut = new UrlShortenerService(waybackMock.Object, keypointExtractorMock.Object, resurrectRecordsStorageMock.Object, hashServiceMock.Object);
            var result = await sut.GetShortUrlAsync("test.test");
            resurrectRecordsStorageMock.Verify(storage => storage.GetResurrectionRecordByUrlAsync(It.Is<string>(url => url == "test.test")));
            hashServiceMock.Verify(hs => hs.GetHash(It.Is<long>(id => id == 1)));
            Assert.Equal(result, "hash");            
        }

        [Fact]
        public async Task GetShortUrlAsyncShouldCreateNewRecordWhenRecordWithSuchUrlIsNotFound()
        {
            var waybackMock = new Mock<IWaybackService>();
            var keypointExtractorMock = new Mock<IKeyPointsExtractorService>();
            keypointExtractorMock
                .Setup(kp => kp.GetHtmlKeypointsFromUrl(It.IsAny<String>()))
                .Returns(Task.FromResult(new HTMLKeypointsResult()));
            var resurrectRecordsStorageMock = new Mock<IShortenedUrlRecordRecordStorageService>();
            resurrectRecordsStorageMock
                .Setup(r => r.GetResurrectionRecordByUrlAsync(It.IsAny<string>()))
                .Returns(Task.FromResult<ShortenedUrlRecordRecord>(null));
            resurrectRecordsStorageMock
                .Setup(r => r.AddRecordAsync(It.IsAny<ShortenedUrlRecordRecord>()))
                .Returns(Task.FromResult(new ShortenedUrlRecordRecord() {  Id = 2}));
            var hashServiceMock = new Mock<IHashService>();
            hashServiceMock
                .Setup(h => h.GetHash(It.Is<long>(identifier => identifier == 2)))
                .Returns(() => "hash");

            var sut = new UrlShortenerService(waybackMock.Object, keypointExtractorMock.Object, resurrectRecordsStorageMock.Object, hashServiceMock.Object);
            var result = await sut.GetShortUrlAsync("test.test");
            resurrectRecordsStorageMock.Verify(storage => storage.GetResurrectionRecordByUrlAsync(It.Is<string>(url => url == "test.test")));
            hashServiceMock.Verify(hs => hs.GetHash(It.Is<long>(id => id == 2)));
            Assert.Equal(result, "hash");
        }

        [Fact]
        public async Task GetDeshortenedUrlShouldReturnEmptyStringWhenRecordIsNotFound()
        {
            var waybackMock = new Mock<IWaybackService>();
            var keypointExtractorMock = new Mock<IKeyPointsExtractorService>();
            var resurrectRecordsStorageMock = new Mock<IShortenedUrlRecordRecordStorageService>();
            resurrectRecordsStorageMock
                .Setup(r => r.GetResurrectionRecordAsync(It.IsAny<long>()))
                .Returns(Task.FromResult<ShortenedUrlRecordRecord>(null));
            var hashServiceMock = new Mock<IHashService>();
            hashServiceMock
                .Setup(h => h.GetRecordId(It.IsAny<string>()))
                .Returns(1);

            var sut = new UrlShortenerService(waybackMock.Object, keypointExtractorMock.Object, resurrectRecordsStorageMock.Object, hashServiceMock.Object);
            var result = await sut.GetDeshortenedUrl("T");
            Assert.Equal(String.Empty, result);
        }

        [Fact]
        public async Task GetDeshortenedUrlShouldReturnUrlForTheRecordWhenRecordIsFound()
        {
            var waybackMock = new Mock<IWaybackService>();
            var keypointExtractorMock = new Mock<IKeyPointsExtractorService>();
            var resurrectRecordsStorageMock = new Mock<IShortenedUrlRecordRecordStorageService>();
            resurrectRecordsStorageMock
                .Setup(r => r.GetResurrectionRecordAsync(It.IsAny<long>()))
                .Returns(Task.FromResult<ShortenedUrlRecordRecord>(new ShortenedUrlRecordRecord() {
                    Url = "test.test"
                }));
            var hashServiceMock = new Mock<IHashService>();
            hashServiceMock
                .Setup(h => h.GetRecordId(It.IsAny<string>()))
                .Returns(1);

            var sut = new UrlShortenerService(waybackMock.Object, keypointExtractorMock.Object, resurrectRecordsStorageMock.Object, hashServiceMock.Object);
            var result = await sut.GetDeshortenedUrl("T");
            Assert.Equal("test.test", result);
        }
    }
}
