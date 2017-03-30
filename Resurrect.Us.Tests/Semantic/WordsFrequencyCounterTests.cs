using Moq;
using Resurrect.Us.Semantic.Semantic;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace Resurrect.Us.Tests.Semantic
{
    public class WordsFrequencyCounterTests
    {
        [Fact]
        public void ShouldCountEmptyTokensListCorrectly()
        {
            var tokenizerMoq = new Mock<ITextTokenizer>();
            tokenizerMoq.Setup(t => t.Tokenize(It.IsAny<string>())).Returns(() => new List<string>());
            var sut = new WordsFrequencyCounter(tokenizerMoq.Object);
            var result = sut.GetWordsFrequencyCount(String.Empty);
            Assert.Equal(0, result.Count);
        }
        
        [Fact]
        public void ShouldCountSingleEntryCorrectly()
        {
            var tokenizerMoq = new Mock<ITextTokenizer>();
            tokenizerMoq.Setup(t => t.Tokenize(It.IsAny<string>())).Returns(() => new List<string>() { "token"});
            var sut = new WordsFrequencyCounter(tokenizerMoq.Object);
            var result = sut.GetWordsFrequencyCount(String.Empty);
            Assert.Equal(1, result.Count);
            Assert.Equal(1, result["token"]);
        }

        [Fact]
        public void ShouldCountMultipleEntryCorrectly()
        {
            var tokenizerMoq = new Mock<ITextTokenizer>();
            tokenizerMoq.Setup(t => t.Tokenize(It.IsAny<string>())).Returns(() => new List<string>() { "token", "token2" });
            var sut = new WordsFrequencyCounter(tokenizerMoq.Object);
            var result = sut.GetWordsFrequencyCount(String.Empty);
            Assert.Equal(2, result.Count);
            Assert.Equal(1, result["token"]);
            Assert.Equal(1, result["token2"]);
        }

        [Fact]
        public void ShouldCountMultipleInstanceCorrectly()
        {
            var tokenizerMoq = new Mock<ITextTokenizer>();
            tokenizerMoq.Setup(t => t.Tokenize(It.IsAny<string>())).Returns(() => new List<string>() { "token", "token2", "token", "token2", "token3" });
            var sut = new WordsFrequencyCounter(tokenizerMoq.Object);
            var result = sut.GetWordsFrequencyCount(String.Empty);
            Assert.Equal(3, result.Count);
            Assert.Equal(2, result["token"]);
            Assert.Equal(2, result["token2"]);
            Assert.Equal(1, result["token3"]);
        }
    }
}
