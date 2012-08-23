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

		[Test]
		public void GetTypedItemTest()
		{
			SitecoreItemKey key = new SitecoreItemKey(Guid.NewGuid(), _container.Context);

			MockSitecoreItem mockItem = new MockSitecoreItem(key);

			mockItem.AddField(new MockSitecoreField("Title", "Hello World!"));

			_container.ItemGateway.AddItem(mockItem);

			TitleItem item = _container.ItemGateway.GetItem<TitleItem>(_container, key);

			Assert.AreEqual("Hello World!", item.Title);
		}

		[Test]
		public void GetTypedItemKeyTest()
		{
			SitecoreItemKey key = new SitecoreItemKey(Guid.NewGuid(), _container.Context);

			MockSitecoreItem mockItem = new MockSitecoreItem(key);

			_container.ItemGateway.AddItem(mockItem);

			TitleItem item = _container.ItemGateway.GetItem<TitleItem>(_container, key);

			Assert.AreEqual(key, item.Key);
		}

		[SitecoreTemplate(TemplateId)]
		class TitleItem
		{
			internal const string TemplateId = "{94810a0f-be7c-47b4-9e75-c15589d7129a}";

			[SitecoreItemKey]
			public SitecoreItemKey Key
			{
				get;
				set;
			}

			[SitecoreField("Title")]
			public string Title
			{
				get;
				set;
			}
		}

		class ParentItem
		{
			[SitecoreChildren]
			public ICollection<TitleItem> TitleItems
			{
				get;
				set;
			}
		}

		[Test]
		public void GetTypedChildTest()
		{
			SitecoreItemKey itemKey = new SitecoreItemKey(Guid.NewGuid(), _container.Context);

			SitecoreItemKey childKey = new SitecoreItemKey(Guid.NewGuid(), _container.Context);

			MockSitecoreItem mockItem = new MockSitecoreItem(itemKey);

			MockSitecoreItem mockChild = new MockSitecoreItem(childKey);

			mockItem.AddChild(mockChild);

			mockChild.AddField(new MockSitecoreField("Title", "Hello World!"));

			_container.ItemGateway.AddItem(mockItem);

			ParentItem item = _container.ItemGateway.GetItem<ParentItem>(_container, itemKey);

			Assert.IsNotNull(item.TitleItems);

			Assert.AreEqual(1, item.TitleItems.Count);
		}
	}
}
