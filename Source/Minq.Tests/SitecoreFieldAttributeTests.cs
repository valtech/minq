using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;

namespace Minq.Tests
{
	[TestFixture]
	public class SitecoreItemFieldAttributeTests
	{
		[Test]
		public void TestGetItemFieldAttribute()
		{
			// Arrange
			TestClass test = new TestClass();

			// Act
			SitecoreFieldAttribute attribute = SitecoreFieldAttribute.GetItemFieldAttribute(typeof(TestClass), "Title");

			// Assert
			Assert.AreEqual("The Title", attribute.Name);

			Assert.IsNull(SitecoreFieldAttribute.GetItemFieldAttribute(typeof(TestClass), "Text"));
		}

		[SitecoreFieldAttribute("The Title")]
		class TestClass
		{
			[SitecoreFieldAttribute("The Title")]
			public string Title
			{
				get;
				set;
			}

			public string Text
			{
				get;
				set;
			}
		}
	}
}
