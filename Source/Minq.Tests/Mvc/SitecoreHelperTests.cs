using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using System.Web.Mvc;
using System.Web.Routing;
using System.Web;
using Moq;
using System.IO;
using Minq;
using Minq.Mvc;

namespace Minq.Tests.Mvc
{
	[TestFixture]
	public class SitecoreHelperTests
	{
		public class TestModel
		{
			[SitecoreItemKey]
			public SitecoreItemKey Key
			{
				get;
				set;
			}

			public string Logo
			{
				get;
				set;
			}

			public string Other
			{
				get;
				set;
			}
		}

		public class TestModelNoKey
		{
			public string Logo
			{
				get;
				set;
			}
		}

		[Test]
		public void TestFieldFor()
		{
			// Arrange
			TestModel model = new TestModel();

			Mock<ISitecoreFieldMarkup> markup = new Mock<ISitecoreFieldMarkup>();

			markup.Setup(m => m.GetHtml(null)).Returns("<a></a>");

			Mock<ISitecoreMarkupStrategy> markupStrategy = new Mock<ISitecoreMarkupStrategy>();

			markupStrategy.Setup(ms => ms.GetFieldMarkup(It.IsAny<SitecoreFieldMetadata>(), It.IsAny<SitecoreFieldAttributeDictionary>())).Returns(markup.Object);

			SitecoreHelper<TestModel> sitecoreHelper = new SitecoreHelper<TestModel>(new ViewDataDictionary<TestModel>(model), markupStrategy.Object);

			// Act
			IHtmlString html = sitecoreHelper.FieldFor(m => m.Logo);

			// Assert
			Assert.AreEqual("<a></a>", html.ToHtmlString());
		}

		[Test]
		public void TestContent()
		{
			// Arrange
			TestModel model = new TestModel();

			Mock<ISitecoreFieldMarkup> markup = new Mock<ISitecoreFieldMarkup>();

			markup.Setup(m => m.GetHtml(null)).Returns("<a></a>");

			Mock<ISitecoreMarkupStrategy> markupStrategy = new Mock<ISitecoreMarkupStrategy>();

			markupStrategy.Setup(ms => ms.GetFieldMarkup(It.IsAny<SitecoreFieldMetadata>(), It.IsAny<SitecoreFieldAttributeDictionary>())).Returns(markup.Object);

			SitecoreHelper<TestModel> sitecoreHelper = new SitecoreHelper<TestModel>(new ViewDataDictionary<TestModel>(model), markupStrategy.Object);

			// Act
			IHtmlString html = sitecoreHelper.LinkFor(m => m.Logo).Content(() => new HtmlString("blah"));

			// Assert
			Assert.AreEqual("<a>blah</a>", html.ToHtmlString());
		}

		[Test]
		[TestCase("Link", "<a>Link</a>")]
		[TestCase("", "<a>Hello</a>")]
		public void TestLinkForAndFieldForIfEmpty(string linkContent, string html)
		{
			// Arrange
			TestModel model = new TestModel { Other = "Hello" };

			Mock<ISitecoreMarkupStrategy> markupStrategy = new Mock<ISitecoreMarkupStrategy>();

			SitecoreHelper<TestModel> sitecoreHelper = new SitecoreHelper<TestModel>(new ViewDataDictionary<TestModel>(model), markupStrategy.Object);

			Mock<ISitecoreFieldMarkup> anchorMarkup = new Mock<ISitecoreFieldMarkup>();

			anchorMarkup.Setup(m => m.GetHtml(It.IsAny<string>())).Returns<string>(content => "<a>" + linkContent + content + "</a>");

			Mock<ISitecoreFieldMarkup> contentMarkup = new Mock<ISitecoreFieldMarkup>();

			contentMarkup.Setup(m => m.GetHtml(It.IsAny<string>())).Returns(model.Other);

			markupStrategy.Setup(ms => ms.GetFieldMarkup(It.Is<SitecoreFieldMetadata>(md => md.FieldName == "Logo"), It.IsAny<SitecoreFieldAttributeDictionary>())).Returns(anchorMarkup.Object);
			markupStrategy.Setup(ms => ms.GetFieldMarkup(It.Is<SitecoreFieldMetadata>(md => md.FieldName == "Other"), It.IsAny<SitecoreFieldAttributeDictionary>())).Returns(contentMarkup.Object);

			// Act
			SitecoreFieldString<TestModel> link = sitecoreHelper.LinkFor(m => m.Logo);

			IHtmlString output = link.IfEmpty(() => sitecoreHelper.FieldFor(m => m.Other));

			// Assert
			Assert.AreEqual(html, output.ToHtmlString());
		}
	}
}
