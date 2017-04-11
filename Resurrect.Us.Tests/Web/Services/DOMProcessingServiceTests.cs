using Moq;
using Resurrect.Us.Semantic.Services;
using Resurrect.Us.Web.Service;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace Resurrect.Us.Tests.Web.Services
{
    public class DOMProcessingServiceTests
    {
        [Fact]
        public void ExtractHTMLKeypointsShallReturnCorrectTitleResultWhenTitleElementPresent()
        {
            var semanticServiceMock = new Mock<ISemanticService>();
            var sut = new DOMProcessingService(semanticServiceMock.Object);
            var minimalHtmlString = "<!DOCTYPE HTML PUBLIC \" -//W3C//DTD HTML 3.2 Final//EN\"><TITLE>Test</TITLE>";
            var result = sut.ExtractHTMLKeypoints(minimalHtmlString);
            Assert.Equal("Test",result.Title);
        }

        [Fact]
        public void ExtractHTMLKeypointsShallUseKeywordsFromKeywordsElementWhenSuchElementPresent()
        {
            var semanticServiceMock = new Mock<ISemanticService>();
            semanticServiceMock.Setup(s => s.GetTopKeywords(It.IsAny<string>(), It.IsAny<int?>())).Returns(new List<string>() { "three", "four"});
            var sut = new DOMProcessingService(semanticServiceMock.Object);
            var minimalHtmlString = "<!DOCTYPE HTML PUBLIC \" -//W3C//DTD HTML 3.2 Final//EN\"><TITLE>Test</TITLE><keywords content='One, two, three'></keywords>";
            var result = sut.ExtractHTMLKeypoints(minimalHtmlString);
            Assert.Equal(3, result.Keywords.Count);
        }

        [Fact]
        public void ExtractHTMLKeypointsShallUseExtractedKeywordsWhenKeywordsElementsIsNotPresent()
        {
            var semanticServiceMock = new Mock<ISemanticService>();
            semanticServiceMock.Setup(s => s.GetTopKeywords(It.IsAny<string>(), It.IsAny<int?>())).Returns(new List<string>() { "three", "four" });
            var sut = new DOMProcessingService(semanticServiceMock.Object);
            var minimalHtmlString = "<!DOCTYPE HTML PUBLIC \" -//W3C//DTD HTML 3.2 Final//EN\"><TITLE>Test</TITLE>";
            var result = sut.ExtractHTMLKeypoints(minimalHtmlString);
            Assert.Equal(2, result.Keywords.Count);
        }
    }
}
