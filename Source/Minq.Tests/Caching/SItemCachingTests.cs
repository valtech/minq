using System;
using System.Linq;
using NUnit.Framework;
using Minq.Mocks;
using Minq.Linq;
using Castle.Windsor;
using Castle.MicroKernel.Registration;
using Minq.Caching;

namespace Minq.Tests.Caching
{
	[TestFixture]
	class SItemCachingTests
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

			_container.Register(Component.For<SItemComposer>().ImplementedBy<CachedItemComposer>());

			_container.Register(Component.For<CachedItemRepository>().UsingFactoryMethod(kernel =>
			{
				return new CachedItemRepository(kernel.Resolve<ISitecoreItemGateway>(),
					new MicroCachedRespositoryDecorator<SitecoreItemKey, SItem>(new FetchLockedRepository<SitecoreItemKey, SItem>(),
						TimeSpan.FromMinutes(3)));
			}));

			_container.Register(Component.For<CachedTemplateRepository>().UsingFactoryMethod(kernel =>
			{
				return new CachedTemplateRepository(kernel.Resolve<ISitecoreTemplateGateway>(),
					new MicroCachedRespositoryDecorator<SitecoreTemplateKey, STemplate>(new FetchLockedRepository<SitecoreTemplateKey, STemplate>(),
						TimeSpan.FromMinutes(30)));
			}));

			_container.Register(Component.For<CachedMediaRepository>().UsingFactoryMethod(kernel =>
			{
				return new CachedMediaRepository(kernel.Resolve<ISitecoreMediaGateway>(),
					new MicroCachedRespositoryDecorator<SitecoreItemKey, SMedia>(new FetchLockedRepository<SitecoreItemKey, SMedia>(),
						TimeSpan.FromMinutes(3)));
			}));

			_mockItemGateway = _container.Resolve<MockSitecoreItemGateway>();
			_mockContext = _container.Resolve<MockSitecoreContext>();
			_mockTemplateGateway = _container.Resolve<MockSitecoreTemplateGateway>();
			_mockMediaGateway = _container.Resolve<MockSitecoreMediaGateway>();
			_composer = _container.Resolve<SItemComposer>();
		}

		[Test]
		public void ItemsAreTheSame()
		{
			// Arrange
			SitecoreItemKey itemKey = new SitecoreItemKey(Guid.NewGuid(), "en-GB", "web");
			SitecoreItemKey childKey = new SitecoreItemKey(Guid.NewGuid(), "en-GB", "web");

			MockSitecoreItem mockItem = new MockSitecoreItem(itemKey);
			MockSitecoreItem mockChild = new MockSitecoreItem(childKey);

			mockItem.AddField(new MockSitecoreField("Title", "Hello World!"));

			mockItem.AddChild(mockChild);

			_mockItemGateway.AddItem(mockItem);

			// Act
			SItem item1 = _composer.CreateItem(itemKey.Guid.ToString(), itemKey.LanguageName, itemKey.DatabaseName);
			SItem item2 = _composer.CreateItem(itemKey.Guid.ToString(), itemKey.LanguageName, itemKey.DatabaseName);
			SItem child = _composer.CreateItem(childKey.Guid.ToString(), childKey.LanguageName, childKey.DatabaseName);

			// Assert
			Assert.That(item1, Is.Not.Null);
			Assert.That(item2, Is.Not.Null);
			Assert.That(item1, Is.SameAs(item2));
			Assert.That(item1.Field("Title").Value<string>(), Is.EqualTo("Hello World!"));
			Assert.That(item1.Items().Count, Is.EqualTo(1));
			Assert.That(item1.Items().ElementAt(0), Is.SameAs(child));
			Assert.That(item2.Items().ElementAt(0), Is.SameAs(child));
		}
	}
}
