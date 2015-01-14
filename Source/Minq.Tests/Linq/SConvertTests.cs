using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using Minq.Linq;

namespace Minq.Tests.Linq
{
	[TestFixture]
	public class SConvertTests
	{
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
	}
}
