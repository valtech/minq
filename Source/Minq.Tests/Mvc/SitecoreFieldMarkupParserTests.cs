using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using Minq.Mvc;

namespace Minq.Tests.Mvc
{
	[TestFixture]
	public class SitecoreFieldMarkupParserTests
	{
		[Test]
		public void TestIsEmptyMarkupElement()
		{
			// Act/Assert
			Assert.AreEqual(true, SitecoreFieldMarkupParser.IsEmptyMarkupElement("<a></a>"));
			Assert.AreEqual(false, SitecoreFieldMarkupParser.IsEmptyMarkupElement("<a>ra</a>"));
			Assert.AreEqual(false, SitecoreFieldMarkupParser.IsEmptyMarkupElement("ra"));
		}

		/*
		[Test]
		public void TestReplaceContent()
		{
			// Act
			string html = SitecoreFieldMarkupParser.ReplaceContent("<a>ra</a>", "blah");

			// Assert
			Assert.AreEqual(html, "<a>blah</a>");
		}*/
	}
}
