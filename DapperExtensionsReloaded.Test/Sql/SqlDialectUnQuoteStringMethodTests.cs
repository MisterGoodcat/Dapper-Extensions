using Xunit;

namespace DapperExtensionsReloaded.Test.Sql
{
    public class SqlDialectUnQuoteStringMethodTests : SqlDialectBaseFixtureBase
    {
        [Fact]
        public void WithNoQuotes_AddsQuotes()
        {
            Assert.Equal((string)"foo", (string)Dialect.UnQuoteString("foo"));
        }

        [Fact]
        public void WithStartQuote_AddsQuotes()
        {
            Assert.Equal((string)"\"foo", (string)Dialect.UnQuoteString("\"foo"));
        }

        [Fact]
        public void WithCloseQuote_AddsQuotes()
        {
            Assert.Equal((string)"foo\"", (string)Dialect.UnQuoteString("foo\""));
        }

        [Fact]
        public void WithBothQuote_DoesNotAddQuotes()
        {
            Assert.Equal((string)"foo", (string)Dialect.UnQuoteString("\"foo\""));
        }
    }
}