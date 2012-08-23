using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using Minq.Mvc;

namespace Minq.Tests.Mvc
{
	[TestFixture]
	public class SitecoreHtmlParserTests
	{
		[Test]
		public void TestConvertCssSizeUnitToInt32()
		{
			// Act
			int size = SitecoreFieldAttributeParser.ConvertCssSizeUnitToInt32("10px");
		
			// Assert
			Assert.AreEqual(10, size);
		}
	}
}
