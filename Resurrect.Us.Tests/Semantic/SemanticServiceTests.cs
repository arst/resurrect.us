using Moq;
using Resurrect.Us.Semantic.Semantic;
using Resurrect.Us.Semantic.Services;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace Resurrect.Us.Tests.Semantic
{
    public class SemanticServiceTests
    {
        [Fact]
        public void SamenticServiceShallReturnKeywordsSortedAccordingToTheirFrequencies()
        {
            var wordsFrequencyCounterMock = new Mock<IWordsFrequencyCounter>();
            wordsFrequencyCounterMock.Setup(w => w.GetWordsFrequencyCount(It.IsAny<string>())).Returns(new Dictionary<string, int>() {
                { "test", 1},
                { "test2", 3},
                { "test3", 9},
                { "test4", 5}
            });
            var sut = new SemanticService(wordsFrequencyCounterMock.Object);
            var result = sut.GetTopKeywords(String.Empty, null);
            Assert.True(result[0] == "test3");
            Assert.True(result[1] == "test4");
            Assert.True(result[2] == "test2");
            Assert.True(result[3] == "test");
        }
    }
}
