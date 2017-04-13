using Resurrect.Us.Data.Services;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace Resurrect.Us.Tests.Data
{
    public class Base32HashGenerationStrategyTests
    {
        [Fact]
        public void ShouldCorrectlyEncodeZero()
        {
            Base32HashGenerationStrategy sut = new Base32HashGenerationStrategy();
            var result = sut.Encode(0);
            Assert.Equal("(NAN)", result);
        }

        [Fact]
        public void ShouldCorrectlyEncodeNegativeValue()
        {
            Base32HashGenerationStrategy sut = new Base32HashGenerationStrategy();
            var result = sut.Encode(-1);
            Assert.Equal("1", result);
        }

        [Fact]
        public void ShouldCorrectlyEncodePositiveValue()
        {
            Base32HashGenerationStrategy sut = new Base32HashGenerationStrategy();
            var result = sut.Encode(1);
            Assert.Equal("1", result);
        }

        [Fact]
        public void ShouldCorrectlyEncodeMaxValue()
        {
            Base32HashGenerationStrategy sut = new Base32HashGenerationStrategy();
            var result = sut.Encode(Int64.MaxValue);
            Assert.Equal(result, "7zzzzzzzzzzzz");
        }

        [Fact]
        public void ShouldCorrectlyEncodeMinValue()
        {
            Base32HashGenerationStrategy sut = new Base32HashGenerationStrategy();
            var result = sut.Encode(Int64.MinValue);
            Assert.Equal("(NAN)", result);
        }

        [Fact]
        public void ShouldCorrectlyEncodeNearMinValue()
        {
            Base32HashGenerationStrategy sut = new Base32HashGenerationStrategy();
            var result = sut.Encode(Int64.MinValue + 1);
            Assert.Equal("7zzzzzzzzzzzz", result);
        }

        [Fact]
        public void ShouldCorrectlyEncodeOverflownValue()
        {
            Base32HashGenerationStrategy sut = new Base32HashGenerationStrategy();
            unchecked
            {
                var result = sut.Encode(Int64.MaxValue + 1);
                Assert.Equal("(NAN)", result);
            }
        }

        [Fact]
        public void ShouldCorrectlyDecodeZero()
        {
            Base32HashGenerationStrategy sut = new Base32HashGenerationStrategy();
            var result = sut.Decode("0");
            Assert.Equal(0, result);
        }

        [Fact]
        public void ShouldCorrectlyDecodeNegativeValue()
        {
            Base32HashGenerationStrategy sut = new Base32HashGenerationStrategy();
            var result = sut.Decode("-1");
            Assert.Equal(-1, result);
        }

        [Fact]
        public void ShouldCorrectlyDecodeMaxValue()
        {
            Base32HashGenerationStrategy sut = new Base32HashGenerationStrategy();
            var result = sut.Decode("7zzzzzzzzzzzz");
            Assert.Equal(Int64.MaxValue, result);
        }


    }
}
