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
		private MockSitecoreContainer _container;

		[SetUp]
		public void SetUp()
		{
			_container = new MockSitecoreContainer();
		}

		[Test]
		public void GetItemTest()
		{
			SitecoreItemKey key = new SitecoreItemKey(Guid.NewGuid(), _container.Context);

			MockSitecoreItem mockItem = new MockSitecoreItem(key);

			_container.ItemGateway.AddItem(mockItem);

			ISitecoreItem item = _container.ItemGateway.GetItem(key);

			Assert.AreEqual(item.Key, key);
		}
	}
}
