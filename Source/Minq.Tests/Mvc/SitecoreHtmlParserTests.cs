using System;
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

		[Test]
		public void StripWidth()
		{
			// Act
			string html = SitecoreFieldMarkupParser.StripAttribute("<img width=\"100\" />", "width");

			// Assert
			Assert.AreEqual("<img />", html);
		}
	}
}
