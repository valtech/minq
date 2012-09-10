﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using Minq.Mocks;

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
