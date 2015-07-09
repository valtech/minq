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
		public void TestRelativeUrl()
		{
			HttpContextBase context = CreateContextForUrl("http://domain");

			SitecoreUrl url = new SitecoreUrl("http://domain/folder/page.aspx", context);

			Assert.That(url.Relative, Is.EqualTo("folder/page.aspx"));
		}

		[Test]
		public void TestVirtualUrl()
		{
			HttpContextBase context = CreateContextForUrl("http://domain");

			SitecoreUrl url = new SitecoreUrl("http://domain/folder/page.aspx", context);

			Assert.That(url.Virtual, Is.EqualTo("/folder/page.aspx"));
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
