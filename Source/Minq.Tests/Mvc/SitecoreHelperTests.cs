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
		private Mock<RouteBase> _route;
		private Mock<HttpContextBase> _httpContext;
		private RouteCollection _routes;
		private RouteData _originalRouteData;
		private ViewContext _viewContext;
		private Mock<IViewDataContainer> _viewDataContainer;

		[SetUp]
		public void SetUp()
		{
			_route = new Mock<RouteBase>();

			VirtualPathData virtualPathData = new VirtualPathData(_route.Object, "~/VirtualPath");

			_route.Setup(r => r.GetVirtualPath(It.IsAny<RequestContext>(), It.IsAny<RouteValueDictionary>()))
				 .Returns(() => virtualPathData);

			_routes = new RouteCollection();

			_routes.Add(_route.Object);

			_originalRouteData = new RouteData();

			string returnValue = "";

			_httpContext = new Mock<HttpContextBase>();

			_httpContext.Setup(hc => hc.Request.ApplicationPath).Returns("~");

			_httpContext.Setup(hc => hc.Response.ApplyAppPathModifier(It.IsAny<string>()))
				.Callback<string>(s => returnValue = s)
					.Returns(() => returnValue);

			_httpContext.Setup(hc => hc.Server.Execute(It.IsAny<IHttpHandler>(), It.IsAny<TextWriter>(), It.IsAny<bool>()));

			_viewContext = new ViewContext
			{
				RequestContext = new RequestContext(_httpContext.Object, _originalRouteData)
			};

			_viewDataContainer = new Mock<IViewDataContainer>();
		}

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

			_viewDataContainer.Setup(m => m.ViewData).Returns(new ViewDataDictionary<TestModel>(model));

			HtmlHelper<TestModel> htmlHelper = new Mock<HtmlHelper<TestModel>>(_viewContext, _viewDataContainer.Object, _routes).Object;

			Mock<ISitecoreFieldMarkup> markup = new Mock<ISitecoreFieldMarkup>();

			markup.Setup(m => m.GetHtml(null)).Returns("<a></a>");

			Mock<ISitecoreFieldMarkupStrategy> markupStrategy = new Mock<ISitecoreFieldMarkupStrategy>();

			markupStrategy.Setup(ms => ms.GetFieldMarkup(It.IsAny<SitecoreFieldMetadata>(), It.IsAny<SitecoreFieldAttributeDictionary>())).Returns(markup.Object);

			SitecoreHelper<TestModel> sitecoreHelper = new SitecoreHelper<TestModel>(htmlHelper, markupStrategy.Object);

			// Act
			IHtmlString html = sitecoreHelper.FieldFor(m => m.Logo);

			// Assert
			Assert.AreEqual("<a></a>", html.ToHtmlString());
		}

		[Test]
		[TestCase("Link", "<a>Link</a>")]
		[TestCase("", "<a>Hello</a>")]
		public void TestLinkForAndFieldForIfEmpty(string linkContent, string html)
		{
			// Arrange
			TestModel model = new TestModel { Other = "Hello" };

			_viewDataContainer.Setup(m => m.ViewData).Returns(new ViewDataDictionary<TestModel>(model));

			HtmlHelper<TestModel> htmlHelper = new Mock<HtmlHelper<TestModel>>(_viewContext, _viewDataContainer.Object, _routes).Object;

			Mock<ISitecoreFieldMarkupStrategy> markupStrategy = new Mock<ISitecoreFieldMarkupStrategy>();

			SitecoreHelper<TestModel> sitecoreHelper = new SitecoreHelper<TestModel>(htmlHelper, markupStrategy.Object);

			Mock<ISitecoreFieldMarkup> anchorMarkup = new Mock<ISitecoreFieldMarkup>();

			anchorMarkup.Setup(m => m.GetHtml(It.IsAny<string>())).Returns<string>(content => "<a>" + linkContent + content + "</a>");

			Mock<ISitecoreFieldMarkup> contentMarkup = new Mock<ISitecoreFieldMarkup>();

			contentMarkup.Setup(m => m.GetHtml(It.IsAny<string>())).Returns(model.Other);

			markupStrategy.Setup(ms => ms.GetFieldMarkup(It.Is<SitecoreFieldMetadata>(md => md.FieldName == "Logo"), It.IsAny<SitecoreFieldAttributeDictionary>())).Returns(anchorMarkup.Object);
			markupStrategy.Setup(ms => ms.GetFieldMarkup(It.Is<SitecoreFieldMetadata>(md => md.FieldName == "Other"), It.IsAny<SitecoreFieldAttributeDictionary>())).Returns(contentMarkup.Object);

			// Act
			SitecoreFieldLink<TestModel> link = sitecoreHelper.LinkFor(m => m.Logo);

			link = link.FieldForIfEmpty(m => m.Other);

			// Assert
			Assert.AreEqual(html, link.ToHtmlString());
		}
	}
}
