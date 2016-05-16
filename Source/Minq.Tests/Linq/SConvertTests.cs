using System;
using NUnit.Framework;
using Minq.Linq;
using System.Globalization;

namespace Minq.Tests.Linq
{
	[TestFixture]
	public class SConvertTests
	{
		[Test]
		public void TestTryUtcDate()
		{
			string s = "20150401T153037Z";

			DateTime date = SConvert.ToDateTime(s, DateTime.MinValue);

			Assert.AreEqual(2015, date.Year);
		}

		[Test]
		public void TestTryDateTime()
		{
			string s = "20140922T151303:635469955834251969";

			DateTime date = SConvert.ToDateTime(s, DateTime.MinValue);

			Assert.AreEqual(2014, date.Year);
		}

		[Test]
		public void TestTryDate()
		{
			string s = "20120927T144800";

			DateTime date = SConvert.ToDateTime(s, DateTime.MinValue);

			Assert.AreEqual(2012, date.Year);
		}

		[Test]
		public void TestTryInt32()
		{
			object output;

			SConvert.TryChangeType(typeof(int), "1", out output);

			Assert.IsInstanceOf<int>(output);

			Assert.AreEqual(1, output);
		}

		[Test]
		public void TestTryDouble()
		{
			object output;

			SConvert.TryChangeType(typeof(double), "1.1", out output);

			Assert.IsInstanceOf<double>(output);

			Assert.AreEqual(1.1, output);
		}

		[Test]
		public void TestTryDecimal()
		{
			object output;

			SConvert.TryChangeType(typeof(decimal), "1.1", out output);

			Assert.IsInstanceOf<decimal>(output);

			Assert.AreEqual(1.1, output);
		}

		[Test]
		public void TestTryFloat()
		{
			object output;

			float valueToTest = 1.6f;

			SConvert.TryChangeType(typeof(float), String.Format(CultureInfo.InvariantCulture, "{0}", valueToTest), out output);

			Assert.IsInstanceOf<float>(output);

			Assert.AreEqual(valueToTest, output);
		}

		[Test]
		public void TestTryNullableDouble()
		{
			object output;

			SConvert.TryChangeType(typeof(double?), "1.1", out output);

			Assert.IsInstanceOf<double?>(output);

			Assert.AreEqual(1.1, output);
		}

		[Test]
		public void TestTryNullableDecimal()
		{
			object output;

			SConvert.TryChangeType(typeof(decimal?), "1.1", out output);

			Assert.IsInstanceOf<decimal?>(output);

			Assert.AreEqual(1.1, output);
		}

		[Test]
		public void TestTryNullableFloat()
		{
			object output;

			float? valueToTest = 1.6f;

			SConvert.TryChangeType(typeof(float?), String.Format(CultureInfo.InvariantCulture, "{0}", valueToTest), out output);

			Assert.IsInstanceOf<float?>(output);

			Assert.AreEqual(valueToTest, output);
		}
	}
}
