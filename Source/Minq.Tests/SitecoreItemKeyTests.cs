using System;
using NUnit.Framework;

namespace Minq.Tests
{
	[TestFixture]
	public class SitecoreItemKeyTests
	{
		//private MockSitecoreContainer _container;

		[SetUp]
		public void SetUp()
		{
			//_container = new MockSitecoreContainer();
		}

		[Test]
		public void TestNullEquality()
		{
			SitecoreItemKey key1 = null;
			SitecoreItemKey key2 = null;

			Assert.That(key1 == key2, Is.EqualTo(true));
		}

		[Test]
		public void TestEqualsOperator()
		{
			// Arrange
			Guid guid = Guid.NewGuid();

			SitecoreItemKey key1 = new SitecoreItemKey(guid, "en-GB", "web");
			SitecoreItemKey key2 = new SitecoreItemKey(guid, "en-GB", "web");

			// Act/Assert
			Assert.IsTrue(key1 == key2);

			Assert.IsFalse(key1 == new SitecoreItemKey(Guid.NewGuid(), "en-GB", "web"));
		}

		[Test]
		public void TestNotEqualsOperator()
		{
			// Arrange
			SitecoreItemKey key1 = new SitecoreItemKey(Guid.NewGuid(), "en-GB", "web");
			SitecoreItemKey key2 = new SitecoreItemKey(Guid.NewGuid(), "en-GB", "web");

			// Act/Assert
			Assert.IsTrue(key1 != key2);

			Assert.IsFalse(key1 != new SitecoreItemKey(key1.Guid, "en-GB", "web"));
		}

		[Test]
		public void TestEquals()
		{
			// Arrange
			Guid guid = Guid.NewGuid();

			SitecoreItemKey key1 = new SitecoreItemKey(guid, "en-GB", "web");
			SitecoreItemKey key2 = new SitecoreItemKey(guid, "en-GB", "web");

			// Act/Assert
			Assert.IsTrue(key1.Equals(key2));

			Assert.IsFalse(key1.Equals(null));

			Assert.IsFalse(key1.Equals(new object()));

			Assert.IsFalse(key1.Equals(new SitecoreItemKey(Guid.NewGuid(), "en-GB", "web")));
		}

		[Test]
		public void TestConstruction()
		{
			// Arrange
			Guid guid = Guid.NewGuid();

			SitecoreItemKey key = new SitecoreItemKey(guid, "en-GB", "web");

			// Act/Assert
			Assert.AreEqual(guid, key.Guid);
			Assert.AreEqual("en-GB", key.LanguageName);
			Assert.AreEqual("web", key.DatabaseName);
		}
	}
}
