using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using Minq.Mocks;
using Minq.Linq;

namespace Minq.Tests.Linq
{
	[TestFixture]
	public class SItemTests
	{
		private MockSitecoreContainer _container;

		[SetUp]
		public void SetUp()
		{
			_container = new MockSitecoreContainer();
		}

		[Test]
		public void TestPoco()
		{
			// Arrange
			SitecoreItemKey itemKey = new SitecoreItemKey(Guid.NewGuid(), _container.Context);

			MockSitecoreItem mockItem = new MockSitecoreItem(itemKey);

			mockItem.AddField(new MockSitecoreField("Title", "Hello World!"));

			SItem item = new SItem(mockItem, _container);

			// Act
			TitleItem titleItem = item.Poco<TitleItem>();

			// Assert
			Assert.AreEqual("Hello World!", titleItem.Title);
		}

		[Test]
		public void TestPocoKeyTest()
		{
			// Arrange
			SitecoreItemKey key = new SitecoreItemKey(Guid.NewGuid(), _container.Context);

			MockSitecoreItem mockItem = new MockSitecoreItem(key);

			_container.ItemGateway.AddItem(mockItem);

			SItem item = new SItem(mockItem, _container);

			// Act
			TitleItem titleItem = item.Poco<TitleItem>();

			// Assert
			Assert.AreEqual(key, titleItem.Key);
		}

		[Test]
		public void TestPocoTypedChild()
		{
			// Arrange
			SitecoreItemKey itemKey = new SitecoreItemKey(Guid.NewGuid(), _container.Context);

			SitecoreItemKey childKey = new SitecoreItemKey(Guid.NewGuid(), _container.Context);

			MockSitecoreItem mockItem = new MockSitecoreItem(itemKey);

			MockSitecoreItem mockChild = new MockSitecoreItem(childKey);

			mockItem.AddChild(mockChild);

			mockChild.AddField(new MockSitecoreField("Title", "Hello World!"));

			_container.ItemGateway.AddItem(mockItem);

			SItem item = new SItem(mockItem, _container);

			// Act
			ParentItem parentItem = item.Poco<ParentItem>();

			// Assert
			Assert.IsNotNull(parentItem.TitleItems);

			Assert.AreEqual(1, parentItem.TitleItems.Count);
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
		public void TestTemplate()
		{
			// Arrange
			SitecoreItemKey itemKey = new SitecoreItemKey(Guid.NewGuid(), _container.Context);
			SitecoreTemplateKey templateKey = new SitecoreTemplateKey(Guid.NewGuid(), _container.Context);

			MockSitecoreItem mockItem = new MockSitecoreItem(itemKey)
			{
				TemplateKey = templateKey
			};

			MockSitecoreTemplate mockTemplate = new MockSitecoreTemplate(templateKey);

			_container.TemplateGateway.AddTemplate(mockTemplate);

			// Act
			SItem item = new SItem(mockItem, _container);

			// Assert
			Assert.IsNotNull(item.Template);
		}

		[Test]
		public void TestItems()
		{
			// Arrange
			SitecoreItemKey itemKey = new SitecoreItemKey(Guid.NewGuid(), _container.Context);

			SitecoreItemKey childKey = new SitecoreItemKey(Guid.NewGuid(), _container.Context);

			MockSitecoreItem mockItem = new MockSitecoreItem(itemKey);

			MockSitecoreItem mockChild = new MockSitecoreItem(childKey);

			mockItem.AddChild(mockChild);

			SItem item = new SItem(mockItem, _container);

			// Act
			IEnumerable<SItem> list = item.Items();

			// Assert
			Assert.AreEqual(1, list.Count());
			Assert.AreEqual(childKey.Guid, list.First().Guid);
		}

		[Test]
		public void TestAncestors()
		{
			// Arrange
			SitecoreItemKey itemKey = new SitecoreItemKey(Guid.NewGuid(), _container.Context);

			SitecoreItemKey childKey = new SitecoreItemKey(Guid.NewGuid(), _container.Context);

			MockSitecoreItem mockItem = new MockSitecoreItem(itemKey);

			MockSitecoreItem mockChild = new MockSitecoreItem(childKey);

			mockItem.AddChild(mockChild);

			SItem item = new SItem(mockChild, _container);

			// Act
			IEnumerable<SItem> list = item.Ancestors();

			// Assert
			Assert.AreEqual(1, list.Count());
			Assert.AreEqual(itemKey.Guid, list.First().Guid);
		}

		[Test]
		public void TestAncestorsAndSelf()
		{
			// Arrange
			SitecoreItemKey itemKey = new SitecoreItemKey(Guid.NewGuid(), _container.Context);

			SitecoreItemKey childKey = new SitecoreItemKey(Guid.NewGuid(), _container.Context);

			MockSitecoreItem mockItem = new MockSitecoreItem(itemKey);

			MockSitecoreItem mockChild = new MockSitecoreItem(childKey);

			mockItem.AddChild(mockChild);

			SItem item = new SItem(mockChild, _container);

			// Act
			IEnumerable<SItem> list = item.AncestorsAndSelf();

			// Assert
			Assert.AreEqual(2, list.Count());
			Assert.AreEqual(childKey.Guid, list.First().Guid);
			Assert.AreEqual(itemKey.Guid, list.ElementAt(1).Guid);
		}
	}
}
