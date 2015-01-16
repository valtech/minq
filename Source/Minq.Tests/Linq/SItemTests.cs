using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using Minq.Mocks;
using Minq.Linq;
using Castle.Windsor;
using Castle.MicroKernel.Registration;

namespace Minq.Tests.Linq
{
	[TestFixture]
	public class SItemTests
	{
		private IWindsorContainer _container;
		private MockSitecoreItemGateway _mockItemGateway;
		private MockSitecoreMediaGateway _mockMediaGateway;
		private MockSitecoreTemplateGateway _mockTemplateGateway;
		private ISitecoreContext _mockContext;
		private SItemComposer _composer;

		[SetUp]
		public void SetUp()
		{
			_container = new WindsorContainer();

			_container.Register(Component.For<ISitecoreItemGateway>().ImplementedBy<MockSitecoreItemGateway>().Forward<MockSitecoreItemGateway>());
			_container.Register(Component.For<ISitecoreTemplateGateway>().ImplementedBy<MockSitecoreTemplateGateway>().Forward<MockSitecoreTemplateGateway>());
			_container.Register(Component.For<ISitecoreMediaGateway>().ImplementedBy<MockSitecoreMediaGateway>().Forward<MockSitecoreMediaGateway>());
			_container.Register(Component.For<ISitecoreContext>().ImplementedBy<MockSitecoreContext>().Forward<MockSitecoreContext>());
			_container.Register(Component.For<ISitecoreRequest>().ImplementedBy<MockSitecoreRequest>().Forward<MockSitecoreRequest>());
			_container.Register(Component.For<SItemComposer>());

			_mockItemGateway = _container.Resolve<MockSitecoreItemGateway>();
			_mockContext = _container.Resolve<MockSitecoreContext>();
			_mockTemplateGateway = _container.Resolve<MockSitecoreTemplateGateway>();
			_mockMediaGateway = _container.Resolve<MockSitecoreMediaGateway>();
			_composer = _container.Resolve<SItemComposer>();
		}

		[Test]
		public void TestPoco()
		{
			// Arrange
			SitecoreItemKey itemKey = new SitecoreItemKey(Guid.NewGuid(), "en-GB", "web");

			MockSitecoreItem mockItem = new MockSitecoreItem(itemKey);

			mockItem.AddField(new MockSitecoreField("Title", "Hello World!"));

			SItem item = new SItem(mockItem, _composer);

			// Act
			TitleItem titleItem = item.ToType<TitleItem>();

			// Assert
			Assert.AreEqual("Hello World!", titleItem.Title);
		}

		[Test]
		public void TestPocoKeyTest()
		{
			// Arrange
			SitecoreItemKey key = new SitecoreItemKey(Guid.NewGuid(), "en-GB", "web");

			MockSitecoreItem mockItem = new MockSitecoreItem(key);

			_mockItemGateway.AddItem(mockItem);

			SItem item = new SItem(mockItem, _composer);

			// Act
			TitleItem titleItem = item.ToType<TitleItem>();

			// Assert
			Assert.AreEqual(key, titleItem.Key);
		}

		[Test]
		public void TestPocoTypedChild()
		{
			// Arrange
			SitecoreItemKey itemKey = new SitecoreItemKey(Guid.NewGuid(), "en-GB", "web");

			SitecoreItemKey childKey = new SitecoreItemKey(Guid.NewGuid(), "en-GB", "web");

			MockSitecoreItem mockItem = new MockSitecoreItem(itemKey);

			MockSitecoreItem mockChild = new MockSitecoreItem(childKey);

			mockItem.AddChild(mockChild);

			mockChild.AddField(new MockSitecoreField("Title", "Hello World!"));

			_mockItemGateway.AddItem(mockItem);

			SItem item = new SItem(mockItem, _composer);

			// Act
			ParentItem parentItem = item.ToType<ParentItem>();

			// Assert
			Assert.IsNotNull(parentItem.TitleItems);

			Assert.AreEqual(1, parentItem.TitleItems.Count);
		}

		//TODO
		[Test]
		public void TestPocoFiltredTypedChild()
		{
			// Arrange
			SitecoreItemKey itemKey = new SitecoreItemKey(Guid.NewGuid(), "en-GB", "web");

			SitecoreItemKey child1Key = new SitecoreItemKey(Guid.NewGuid(), "en-GB", "web");
			SitecoreItemKey child2Key = new SitecoreItemKey(Guid.NewGuid(), "en-GB", "web");

			MockSitecoreItem mockItem = new MockSitecoreItem(itemKey);

			MockSitecoreItem mockChild1 = new MockSitecoreItem(child1Key);
			MockSitecoreItem mockChild2 = new MockSitecoreItem(child2Key);

			mockItem.AddChild(mockChild1);
			mockItem.AddChild(mockChild2);

			SitecoreTemplateKey template1Key = new SitecoreTemplateKey(Guid.NewGuid(), "web");
			SitecoreTemplateKey template2Key = new SitecoreTemplateKey(new Guid(TitleItem.TemplateId), "web");

			mockChild1.AddField(new MockSitecoreField("Title", "Text 1"));
			mockChild2.AddField(new MockSitecoreField("Title", "Text 2"));

			mockChild1.TemplateKey = template1Key;
			mockChild2.TemplateKey = template2Key;

			_mockItemGateway.AddItem(mockItem);
			_mockTemplateGateway.AddTemplate(new MockSitecoreTemplate(template1Key));
			_mockTemplateGateway.AddTemplate(new MockSitecoreTemplate(template2Key));

			SItem item = new SItem(mockItem, _composer);

			// Act
			FilterParentItem parentItem = item.ToType<FilterParentItem>();

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

		class FilterParentItem
		{
			[SitecoreChildren]
			[SitecoreChildType]
			public ICollection<TitleItem> TitleItems
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
			SitecoreItemKey itemKey = new SitecoreItemKey(Guid.NewGuid(), "en-GB", "web");
			SitecoreTemplateKey templateKey = new SitecoreTemplateKey(Guid.NewGuid(), "web");

			MockSitecoreItem mockItem = new MockSitecoreItem(itemKey)
			{
				TemplateKey = templateKey
			};

			MockSitecoreTemplate mockTemplate = new MockSitecoreTemplate(templateKey);

			_mockTemplateGateway.AddTemplate(mockTemplate);

			// Act
			SItem item = new SItem(mockItem, _composer);

			// Assert
			Assert.IsNotNull(item.Template);
		}

		[Test]
		public void TestItems()
		{
			// Arrange
			SitecoreItemKey itemKey = new SitecoreItemKey(Guid.NewGuid(), "en-GB", "web");

			SitecoreItemKey childKey = new SitecoreItemKey(Guid.NewGuid(), "en-GB", "web");

			MockSitecoreItem mockItem = new MockSitecoreItem(itemKey);

			MockSitecoreItem mockChild = new MockSitecoreItem(childKey);

			mockItem.AddChild(mockChild);

			SItem item = new SItem(mockItem, _composer);

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
			SitecoreItemKey itemKey = new SitecoreItemKey(Guid.NewGuid(), "en-GB", "web");

			SitecoreItemKey childKey = new SitecoreItemKey(Guid.NewGuid(), "en-GB", "web");

			MockSitecoreItem mockItem = new MockSitecoreItem(itemKey);

			MockSitecoreItem mockChild = new MockSitecoreItem(childKey);

			mockItem.AddChild(mockChild);

			SItem item = new SItem(mockChild, _composer);

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
			SitecoreItemKey itemKey = new SitecoreItemKey(Guid.NewGuid(), "en-GB", "web");

			SitecoreItemKey childKey = new SitecoreItemKey(Guid.NewGuid(), "en-GB", "web");

			MockSitecoreItem mockItem = new MockSitecoreItem(itemKey);

			MockSitecoreItem mockChild = new MockSitecoreItem(childKey);

			mockItem.AddChild(mockChild);

			SItem item = new SItem(mockChild, _composer);
			
			// Act
			IEnumerable<SItem> list = item.AncestorsAndSelf();

			// Assert
			Assert.AreEqual(2, list.Count());
			Assert.AreEqual(childKey.Guid, list.First().Guid);
			Assert.AreEqual(itemKey.Guid, list.ElementAt(1).Guid);
		}
	}
}
