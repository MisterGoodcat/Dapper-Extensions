﻿using System;
using System.Collections.Generic;
using DapperExtensions.Internal.Sql;
using NUnit.Framework;

namespace DapperExtensions.Test.Sql
{
    [TestFixture]
    internal class SqlDialectBaseFixture
    {
        public abstract class SqlDialectBaseFixtureBase
        {
            protected TestDialect Dialect;

            [SetUp]
            public void Setup()
            {
                Dialect = new TestDialect();
            }
        }

        [TestFixture]
        public class Properties : SqlDialectBaseFixtureBase
        {
            [Test]
            public void CheckSettings()
            {
                Assert.AreEqual('"', Dialect.OpenQuote);
                Assert.AreEqual('"', Dialect.CloseQuote);
                Assert.AreEqual(";" + Environment.NewLine, Dialect.BatchSeperator);
                Assert.AreEqual('@', Dialect.ParameterPrefix);
                Assert.IsTrue(Dialect.SupportsMultipleStatements);
            }
        }

        [TestFixture]
        public class IsQuotedMethod : SqlDialectBaseFixtureBase
        {
            [Test]
            public void WithQuotes_ReturnsTrue()
            {
                Assert.IsTrue(Dialect.IsQuoted("\"foo\""));
            }

            [Test]
            public void WithOnlyStartQuotes_ReturnsFalse()
            {
                Assert.IsFalse(Dialect.IsQuoted("\"foo"));
            }

            [Test]
            public void WithOnlyCloseQuotes_ReturnsFalse()
            {
                Assert.IsFalse(Dialect.IsQuoted("foo\""));
            }
        }

        [TestFixture]
        public class QuoteStringMethod : SqlDialectBaseFixtureBase
        {
            [Test]
            public void WithNoQuotes_AddsQuotes()
            {
                Assert.AreEqual("\"foo\"", Dialect.QuoteString("foo"));
            }

            [Test]
            public void WithStartQuote_AddsQuotes()
            {
                Assert.AreEqual("\"\"foo\"", Dialect.QuoteString("\"foo"));
            }

            [Test]
            public void WithCloseQuote_AddsQuotes()
            {
                Assert.AreEqual("\"foo\"\"", Dialect.QuoteString("foo\""));
            }

            [Test]
            public void WithBothQuote_DoesNotAddQuotes()
            {
                Assert.AreEqual("\"foo\"", Dialect.QuoteString("\"foo\""));
            }
        }

        [TestFixture]
        public class UnQuoteStringMethod : SqlDialectBaseFixtureBase
        {
            [Test]
            public void WithNoQuotes_AddsQuotes()
            {
                Assert.AreEqual("foo", Dialect.UnQuoteString("foo"));
            }

            [Test]
            public void WithStartQuote_AddsQuotes()
            {
                Assert.AreEqual("\"foo", Dialect.UnQuoteString("\"foo"));
            }

            [Test]
            public void WithCloseQuote_AddsQuotes()
            {
                Assert.AreEqual("foo\"", Dialect.UnQuoteString("foo\""));
            }

            [Test]
            public void WithBothQuote_DoesNotAddQuotes()
            {
                Assert.AreEqual("foo", Dialect.UnQuoteString("\"foo\""));
            }
        }

        [TestFixture]
        public class GetTableNameMethod : SqlDialectBaseFixtureBase
        {
            [Test]
            public void NullTableName_ThrowsException()
            {
                var ex = Assert.Throws<ArgumentNullException>(() => Dialect.GetTableName(null, null, null));
                Assert.AreEqual("tableName", ex.ParamName);
            }

            [Test]
            public void EmptyTableName_ThrowsException()
            {
                var ex = Assert.Throws<ArgumentNullException>(() => Dialect.GetTableName(null, string.Empty, null));
                Assert.AreEqual("tableName", ex.ParamName);
            }

            [Test]
            public void TableNameOnly_ReturnsProperlyQuoted()
            {
                var result = Dialect.GetTableName(null, "foo", null);
                Assert.AreEqual("\"foo\"", result);
            }

            [Test]
            public void SchemaAndTable_ReturnsProperlyQuoted()
            {
                var result = Dialect.GetTableName("bar", "foo", null);
                Assert.AreEqual("\"bar\".\"foo\"", result);
            }

            [Test]
            public void AllParams_ReturnsProperlyQuoted()
            {
                var result = Dialect.GetTableName("bar", "foo", "al");
                Assert.AreEqual("\"bar\".\"foo\" AS \"al\"", result);
            }

            [Test]
            public void ContainsQuotes_DoesNotAddExtraQuotes()
            {
                var result = Dialect.GetTableName("\"bar\"", "\"foo\"", "\"al\"");
                Assert.AreEqual("\"bar\".\"foo\" AS \"al\"", result);
            }
        }

        [TestFixture]
        public class GetColumnNameMethod : SqlDialectBaseFixtureBase
        {
            [Test]
            public void NullColumnName_ThrowsException()
            {
                var ex = Assert.Throws<ArgumentNullException>(() => Dialect.GetColumnName(null, null, null));
                Assert.AreEqual("columnName", ex.ParamName);
            }

            [Test]
            public void EmptyColumnName_ThrowsException()
            {
                var ex = Assert.Throws<ArgumentNullException>(() => Dialect.GetColumnName(null, string.Empty, null));
                Assert.AreEqual("columnName", ex.ParamName);
            }

            [Test]
            public void ColumnNameOnly_ReturnsProperlyQuoted()
            {
                var result = Dialect.GetColumnName(null, "foo", null);
                Assert.AreEqual("\"foo\"", result);
            }

            [Test]
            public void PrefixColumnName_ReturnsProperlyQuoted()
            {
                var result = Dialect.GetColumnName("bar", "foo", null);
                Assert.AreEqual("\"bar\".\"foo\"", result);
            }

            [Test]
            public void AllParams_ReturnsProperlyQuoted()
            {
                var result = Dialect.GetColumnName("bar", "foo", "al");
                Assert.AreEqual("\"bar\".\"foo\" AS \"al\"", result);
            }

            [Test]
            public void ContainsQuotes_DoesNotAddExtraQuotes()
            {
                var result = Dialect.GetColumnName("\"bar\"", "\"foo\"", "\"al\"");
                Assert.AreEqual("\"bar\".\"foo\" AS \"al\"", result);
            }
        }

        internal class TestDialect : SqlDialectBase
        {
            public override string GetIdentitySql(string tableName)
            {
                throw new NotImplementedException();
            }

            public override string GetPagingSql(string sql, int page, int resultsPerPage, IDictionary<string, object> parameters)
            {
                throw new NotImplementedException();
            }

            public override string GetSetSql(string sql, int firstResult, int maxResults, IDictionary<string, object> parameters)
            {
                throw new NotImplementedException();
            }
        }
    }
}