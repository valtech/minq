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
		[Test]
		public void TestGetTemplate()
		{
			// Arrange
			SitecoreTemplateKey key = new SitecoreTemplateKey(Guid.NewGuid(), "web");

			MockSitecoreTemplate mockItem = new MockSitecoreTemplate(key);

			MockSitecoreTemplateGateway templateGateway = new MockSitecoreTemplateGateway();

			templateGateway.AddTemplate(mockItem);

			// Act
			ISitecoreTemplate template = templateGateway.GetTemplate(key);

			// Assert
			Assert.AreEqual(key, template.Key);
		}
	}
}
