using Moq;
using NUnit.Framework;
using System;
using System.Web;

namespace Minq.Tests
{
	[TestFixture]
	public class SitecoreUrlTests
	{
		[Test]
		public void TestJavaScriptInRelativeUrl()
		{
			HttpContextBase context = CreateContextForUrl("http://domain");

			string script = "javascript:alert('Hello!');";

			SitecoreUrl url = new SitecoreUrl(script, context);

			Assert.That(url.Relative, Is.EqualTo(script));
		}

		[Test]
		public void TestRelativeUrl()
		{
			HttpContextBase context = CreateContextForUrl("http://domain");

			SitecoreUrl url = new SitecoreUrl("http://domain/folder/page.aspx", context);

			Assert.That(url.Relative, Is.EqualTo("folder/page.aspx"));
		}

		[Test]
		public void TestJavaScriptInVirtualUrl()
		{
			HttpContextBase context = CreateContextForUrl("http://domain");

			string script = "javascript:alert('Hello!');";

			SitecoreUrl url = new SitecoreUrl(script, context);

			Assert.That(url.Virtual, Is.EqualTo(script));
		}

		[Test]
		public void TestJavaScriptInAbsoluteUrl()
		{
			HttpContextBase context = CreateContextForUrl("http://domain");

			string script = "javascript:alert('Hello!');";

			SitecoreUrl url = new SitecoreUrl(script, context);

			Assert.That(url.Absolute, Is.EqualTo(script));
		}

		[Test]
		public void TestVirtualUrl()
		{
			HttpContextBase context = CreateContextForUrl("http://domain");

			SitecoreUrl url = new SitecoreUrl("http://domain/folder/page.aspx", context);

			Assert.That(url.Virtual, Is.EqualTo("/folder/page.aspx"));
		}

		[Test]
		public void TestVirtualIsAbsoluteOnDifferentDomainsUrl()
		{
			HttpContextBase context = CreateContextForUrl("http://domain1");

			SitecoreUrl url = new SitecoreUrl("http://domain2/folder/page.aspx", context);

			Assert.That(url.Virtual, Is.EqualTo("http://domain2/folder/page.aspx"));
		}

		/* Got code in to strip random ports
		[Test]
		public void TestVirtualIsAbsoluteOnDifferentPortsUrl()
		{
			HttpContextBase context = CreateContextForUrl("http://domain");

			SitecoreUrl url = new SitecoreUrl("http://domain:8080/folder/page.aspx", context);

			Assert.That(url.Virtual, Is.EqualTo("http://domain:8080/folder/page.aspx"));
		}
		*/

		[Test]
		public void TestVirtualIsAbsoluteOnDifferentSchemesUrl()
		{
			HttpContextBase context = CreateContextForUrl("http://domain");

			SitecoreUrl url = new SitecoreUrl("https://domain/folder/page.aspx", context);

			Assert.That(url.Virtual, Is.EqualTo("https://domain/folder/page.aspx"));
		}

		[Test]
		public void TestAbolsuteUrlStripsRandomPorts()
		{
			HttpContextBase context = CreateContextForUrl("http://domain:8080");

			SitecoreUrl url = new SitecoreUrl("http://domain:8080/folder/page.aspx", context);

			Assert.That(url.Absolute, Is.EqualTo("http://domain/folder/page.aspx"));
		}

		[Test]
		public void TestNullUrl()
		{
			SitecoreUrl url = new SitecoreUrl((string)null);

			Assert.That(url.Virtual, Is.EqualTo(""));
			Assert.That(url.Relative, Is.EqualTo(""));
			Assert.That(url.Absolute, Is.EqualTo(""));
		}

		[Test]
		public void TestEmptyUrl()
		{
			SitecoreUrl url = new SitecoreUrl("");

			Assert.That(url.Virtual, Is.EqualTo(""));
			Assert.That(url.Relative, Is.EqualTo(""));
			Assert.That(url.Absolute, Is.EqualTo(""));
		}

		[Test]
		public void TestVirtualUrlInput()
		{
			HttpContextBase context = CreateContextForUrl("http://domain");

			SitecoreUrl url = new SitecoreUrl("/folder/page.aspx", context);

			Assert.That(url.Absolute, Is.EqualTo("http://domain/folder/page.aspx"));
		}

		private HttpContextBase CreateContextForUrl(string url)
		{
			Mock<HttpContextBase> mockContext = new Mock<HttpContextBase>();
			Mock<HttpRequestBase> mockRequest = new Mock<HttpRequestBase>();

			mockContext.Setup(x => x.Request).Returns(mockRequest.Object);
			mockRequest.Setup(x => x.Url).Returns(new Uri(url));

			return mockContext.Object;
		}
	}
}
