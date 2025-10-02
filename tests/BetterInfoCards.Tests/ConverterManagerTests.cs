using BetterInfoCards;
using Xunit;

namespace BetterInfoCards.Tests
{
    public sealed class ConverterManagerTests
    {
        [Fact]
        public void UnknownConverterFallsBackToDefault()
        {
            Assert.False(ConverterManager.TryGetConverter(string.Empty, out var defaultConverter));
            Assert.NotNull(defaultConverter);

            Assert.False(ConverterManager.TryGetConverter("NonExistent", out var fallback));
            Assert.Same(defaultConverter, fallback);
        }

        [Fact]
        public void TitleConverterResolvesDirectly()
        {
            Assert.False(ConverterManager.TryGetConverter(string.Empty, out var defaultConverter));

            Assert.True(ConverterManager.TryGetConverter(ConverterManager.title, out var titleConverter));
            Assert.NotNull(titleConverter);
            Assert.NotSame(defaultConverter, titleConverter);
        }

        [Fact]
        public void NamedConverterResolvesFromDictionary()
        {
            Assert.True(ConverterManager.TryGetConverter(ConverterManager.germs, out var converter));
            Assert.NotNull(converter);
        }

        [Theory]
        [InlineData(null, null)]
        [InlineData("", "")]
        [InlineData("Info x 3", "Info")]
        public void RemoveCountSuffixHandlesEdgeCases(string input, string expected)
        {
            var result = Extensions.RemoveCountSuffix(input);

            Assert.Equal(expected, result);
        }
    }
}
