using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using System.Text.RegularExpressions;
using Minq.Mvc;

namespace Minq.Tests.Mvc
{
	[TestFixture]
	public class SitecoreHtmlAttributeDictionaryTests
	{
		[Test]
		public void TestFromAttributes()
		{
			// Arrange
			IDictionary<string, object> htmlAttributes = new Dictionary<string, object>();

			htmlAttributes["href"] = "#test";

			// Act
			SitecoreFieldAttributeDictionary dictionary = SitecoreFieldAttributeDictionary.FromAttributes(htmlAttributes);

			// Assert
			Assert.AreEqual("#test", dictionary["href"]);
		}

		[Test]
		public void TestFromImageAttributes()
		{
			// Arrange
			IDictionary<string, object> htmlAttributes = new Dictionary<string, object>();

			htmlAttributes["width"] = "10px";

			// Act
			SitecoreFieldAttributeDictionary dictionary = SitecoreFieldAttributeDictionary.FromImageAttributes(htmlAttributes);

			// Assert
			Assert.AreEqual(10, dictionary["mw"]);
		}
	}
}
