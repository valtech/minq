using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using Minq.Mocks;

namespace Minq.Tests
{
	[TestFixture]
	public class SitecoreItemKeyTests
	{
		private MockSitecoreContainer _container;

		[SetUp]
		public void SetUp()
		{
			_container = new MockSitecoreContainer();
		}

		[Test]
		public void TestEqualsOperator()
		{
			// Arrange
			Guid guid = Guid.NewGuid();

			SitecoreItemKey key1 = new SitecoreItemKey(guid, _container.Context);
			SitecoreItemKey key2 = new SitecoreItemKey(guid, _container.Context);

			// Act/Assert
			Assert.IsTrue(key1 == key2);

			Assert.IsFalse(key1 == new SitecoreItemKey(Guid.NewGuid(), _container.Context));
		}

		[Test]
		public void TestNotEqualsOperator()
		{
			// Arrange
			SitecoreItemKey key1 = new SitecoreItemKey(Guid.NewGuid(), _container.Context);
			SitecoreItemKey key2 = new SitecoreItemKey(Guid.NewGuid(), _container.Context);

			// Act/Assert
			Assert.IsTrue(key1 != key2);

			Assert.IsFalse(key1 != new SitecoreItemKey(key1.Guid, _container.Context));
		}

		[Test]
		public void TestEquals()
		{
			// Arrange
			Guid guid = Guid.NewGuid();

			SitecoreItemKey key1 = new SitecoreItemKey(guid, _container.Context);
			SitecoreItemKey key2 = new SitecoreItemKey(guid, _container.Context);

			// Act/Assert
			Assert.IsTrue(key1.Equals(key2));

			Assert.IsFalse(key1.Equals(null));

			Assert.IsFalse(key1.Equals(new object()));

			Assert.IsFalse(key1.Equals(new SitecoreItemKey(Guid.NewGuid(), _container.Context)));
		}

		[Test]
		public void TestConstructionViaContext()
		{
			// Arrange
			Guid guid = Guid.NewGuid();

			SitecoreItemKey key = new SitecoreItemKey(guid, _container.Context);

			// Act/Assert
			Assert.AreEqual(guid, key.Guid);
			Assert.AreEqual(key.LanguageName, _container.Context.LanguageName);
			Assert.AreEqual(key.DatabaseName, _container.Context.DatabaseName);
		}

		[Test]
		public void TestConstructionViaStrings()
		{
			// Arrange
			Guid guid = Guid.NewGuid();

			SitecoreItemKey key = new SitecoreItemKey(guid, "en-GB", "web");

			// Act/Assert
			Assert.AreEqual(guid, key.Guid);
			Assert.AreEqual("en-GB", _container.Context.LanguageName);
			Assert.AreEqual("web", _container.Context.DatabaseName);
		}
	}
}
