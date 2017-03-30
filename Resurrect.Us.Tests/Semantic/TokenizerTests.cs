using Resurrect.Us.Semantic.Semantic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;

namespace Resurrect.Us.Tests.Semantic
{
    public class TokenizerTests
    {
        [Fact]
        public void ShouldCorrectlyTokenizeNullString()
        {
            TextTokenizer sut = new TextTokenizer();
            var result = sut.Tokenize(null);
            Assert.Empty(result);
        }

        [Fact]
        public void ShouldCorrectlyTokenizeEmptyString()
        {
            TextTokenizer sut = new TextTokenizer();
            var result = sut.Tokenize(String.Empty);
            Assert.Empty(result);
        }

        [Fact]
        public void ShouldCorrectlyTokenizeCorrectString()
        {
            TextTokenizer sut = new TextTokenizer();
            var result = sut.Tokenize("Test1, Test2, Test3");
            Assert.Equal(result.Count, 3);
            Assert.Contains("Test1", result);
            Assert.Contains("Test2", result);
            Assert.Contains("Test3", result);
        }

        [Fact]
        public void ShouldCreateTokensWithoutWhitespaces()
        {
            TextTokenizer sut = new TextTokenizer();
            var result = sut.Tokenize("     Test     ");
            Assert.Equal(result.FirstOrDefault(), "Test");
        }

        [Fact]
        public void ShouldCreateTokensWithoutQuotes()
        {
            TextTokenizer sut = new TextTokenizer();
            var result = sut.Tokenize("'Test'");
            Assert.Equal(result.FirstOrDefault(), "Test");
        }

        [Fact]
        public void ShouldNotAllowTooShortTokens()
        {
            TextTokenizer sut = new TextTokenizer();
            var result = sut.Tokenize("too short and too");
            Assert.Equal(result.Count, 1);
            Assert.Equal(result.FirstOrDefault(), "short");

        }

    }
}
