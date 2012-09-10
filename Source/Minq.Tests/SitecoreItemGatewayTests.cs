using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using Minq.Mocks;

namespace Minq.Tests
{
	[TestFixture]
	public class SitecoreItemGatewayTests
	{
		[Test]
		public void GetItemTest()
		{
			SitecoreItemKey key = new SitecoreItemKey(Guid.NewGuid(), "en-GB", "web");

			MockSitecoreItem mockItem = new MockSitecoreItem(key);

			MockSitecoreItemGateway itemGateway = new MockSitecoreItemGateway();

			itemGateway.AddItem(mockItem);

			ISitecoreItem item = itemGateway.GetItem(key);

			Assert.AreEqual(item.Key, key);
		}
	}
}
