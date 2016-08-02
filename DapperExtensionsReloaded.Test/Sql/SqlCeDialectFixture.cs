﻿using System;
using DapperExtensions.Sql;
using NUnit.Framework;

namespace DapperExtensions.Test.Sql
{
    public class SqlCeDialectFixture
    {
        public abstract class SqlCeDialectFixtureBase
        {
            protected SqlCeDialect Dialect;

            [SetUp]
            public void Setup()
            {
                Dialect = new SqlCeDialect();
            }
        }

        [TestFixture]
        public class Properties : SqlCeDialectFixtureBase
        {
            [Test]
            public void CheckSettings()
            {
                Assert.AreEqual('[', Dialect.OpenQuote);
                Assert.AreEqual(']', Dialect.CloseQuote);
                Assert.AreEqual(";" + Environment.NewLine, Dialect.BatchSeperator);
                Assert.AreEqual('@', Dialect.ParameterPrefix);
                Assert.IsFalse(Dialect.SupportsMultipleStatements);
            }
        }

        [TestFixture]
        public class GetTableNameMethod : SqlCeDialectFixtureBase
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
                Assert.AreEqual("[foo]", result);
            }

            [Test]
            public void SchemaAndTable_ReturnsProperlyQuoted()
            {
                var result = Dialect.GetTableName("bar", "foo", null);
                Assert.AreEqual("[bar_foo]", result);
            }

            [Test]
            public void AllParams_ReturnsProperlyQuoted()
            {
                var result = Dialect.GetTableName("bar", "foo", "al");
                Assert.AreEqual("[bar_foo] AS [al]", result);
            }
        }
    }
}