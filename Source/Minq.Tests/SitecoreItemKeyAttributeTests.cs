using System;
using NUnit.Framework;

namespace Minq.Tests
{
	[TestFixture]
	public class SitecoreItemKeyAttributeTests
	{
		[Test]
		public void TestFindKey()
		{
			// Arrange
			Guid guid = Guid.NewGuid();

			SitecoreItemKey key = new SitecoreItemKey(guid, "en-GB", "web");

			KeyedItem item = new KeyedItem { Key = key };

			// Act
			SitecoreItemKey found = SitecoreItemKeyAttribute.FindKey(item);

			// Assert
			Assert.AreEqual(key, found);
		}

		class KeyedItem
		{
			[SitecoreItemKey]
			public SitecoreItemKey Key
			{
				get;
				set;
			}
		}
	}
}
