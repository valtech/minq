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
			string s = "20120927T144800";

			DateTime date = SConvert.ToDateTime(s, DateTime.MinValue);

			Assert.AreEqual(2012, date.Year);
		}
	}
}
