using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using Minq.Mocks;

namespace Minq.Tests
{
	[TestFixture]
	public class SitecoreTemplateGatewayTests
	{
		private MockSitecoreContainer _container;

		[SetUp]
		public void SetUp()
		{
			_container = new MockSitecoreContainer();
		}

		[Test]
		public void TestGetTemplate()
		{
			// Arrange
			SitecoreTemplateKey key = new SitecoreTemplateKey(Guid.NewGuid(), "web");

			MockSitecoreTemplate mockItem = new MockSitecoreTemplate(key);

			_container.TemplateGateway.AddTemplate(mockItem);

			// Act
			ISitecoreTemplate template = _container.TemplateGateway.GetTemplate(key);

			// Assert
			Assert.AreEqual(key, template.Key);
		}
	}
}
