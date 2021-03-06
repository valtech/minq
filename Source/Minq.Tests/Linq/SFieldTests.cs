﻿using Castle.MicroKernel.Registration;
using Castle.Windsor;
using Minq.Linq;
using Minq.Mocks;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Minq.Tests.Linq
{
	[TestFixture]
	public class SFieldTests
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
		public void TestItemField()
		{
			Guid id = Guid.NewGuid();
			Guid child1Id = Guid.NewGuid();
			Guid child2Id = Guid.NewGuid();

			MockSitecoreItem mockItem = new MockSitecoreItem(new SitecoreItemKey(id, "en", "web"));
			MockSitecoreItem mockChild1 = new MockSitecoreItem(new SitecoreItemKey(child1Id, "en", "web"));
			MockSitecoreItem mockChild2 = new MockSitecoreItem(new SitecoreItemKey(child2Id, "en", "web"));

			mockItem.AddField(new MockSitecoreField("Values", child1Id.ToString() + "|" + child2Id.ToString()));
			mockChild1.AddField(new MockSitecoreField("Text", "Child 1"));
			mockChild2.AddField(new MockSitecoreField("Text", "Child 2"));

			_mockItemGateway.AddItem(mockChild1);
			_mockItemGateway.AddItem(mockChild2);

			SItem item = new SItem(mockItem, _composer);

			Assert.That(item.Field("Values").Value<SItem>(), Is.Not.Null);
		}

		[Test]
		public void TestLazyFieldCollection()
		{
			Guid id = Guid.NewGuid();
			Guid child1Id = Guid.NewGuid();
			Guid child2Id = Guid.NewGuid();

			MockSitecoreItem mockItem = new MockSitecoreItem(new SitecoreItemKey(id, "en", "web"));
			MockSitecoreItem mockChild1 = new MockSitecoreItem(new SitecoreItemKey(child1Id, "en", "web"));
			MockSitecoreItem mockChild2 = new MockSitecoreItem(new SitecoreItemKey(child2Id, "en", "web"));

			mockItem.AddField(new MockSitecoreField("Values", child1Id.ToString() + "|" + child2Id.ToString()));
			mockChild1.AddField(new MockSitecoreField("Text", "Child 1"));
			mockChild2.AddField(new MockSitecoreField("Text", "Child 2"));

			_mockItemGateway.AddItem(mockChild1);
			_mockItemGateway.AddItem(mockChild2);

			SItem item = new SItem(mockItem, _composer);

			Data data = item.ToType<Data>();

			Assert.That(data.Values.Count, Is.EqualTo(2));
			Assert.That(data.Values.ElementAt(0).Text, Is.EqualTo("Child 1"));
			Assert.That(data.Values.ElementAt(1).Text, Is.EqualTo("Child 2"));
		}

		[Test]
		public void TestLazyFieldType()
		{
			Guid id = Guid.NewGuid();
			Guid childId = Guid.NewGuid();

			MockSitecoreItem mockItem = new MockSitecoreItem(new SitecoreItemKey(id, "en", "web"));
			MockSitecoreItem mockChild = new MockSitecoreItem(new SitecoreItemKey(childId, "en", "web"));

			mockItem.AddField(new MockSitecoreField("Value", childId.ToString()));
			mockChild.AddField(new MockSitecoreField("Text", "Child"));

			_mockItemGateway.AddItem(mockChild);

			SItem item = new SItem(mockItem, _composer);

			Data data = item.ToType<Data>();

			Assert.That(data.Value, Is.Not.Null);
			Assert.That(data.Value.Text, Is.EqualTo("Child"));
		}

		class Data
		{
			[SitecoreField("Values")]
			public ICollection<Child> Values
			{
				get;
				set;
			}

			[SitecoreField("Value")]
			public Child Value
			{
				get;
				set;
			}
		}

		[SitecoreTemplate("daf3d9d1-1970-43fb-b61c-ef8ac2bcdb2a")]
		class Child
		{
			[SitecoreField("Text")]
			public string Text
			{
				get;
				set;
			}
		}
	}
}
