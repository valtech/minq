using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Minq.Mocks;
using NUnit.Framework;

namespace Minq.Tests
{
	[TestFixture]
	public class SitecoreContextTests
	{
		[Test]
		public void TestDefaultLanguageName()
		{
			// Act/Assert
			Assert.AreEqual("en-GB", new MockSitecoreContext().LanguageName);
		}

		[Test]
		public void TestSettingLanguageName()
		{
			// Arrange
			MockSitecoreContext context = new MockSitecoreContext
			{
				LanguageName = "fr-FR"
			};

			// Act/Assert
			Assert.AreEqual("fr-FR", context.LanguageName);
		}

		[Test]
		public void TestDefaultDatabaseName()
		{
			// Act/Assert
			Assert.AreEqual("web", new MockSitecoreContext().DatabaseName);
		}

		[Test]
		public void TestSettingDatabaseName()
		{
			// Arrange
			MockSitecoreContext context = new MockSitecoreContext
			{
				DatabaseName = "master"
			};

			// Act/Assert
			Assert.AreEqual("master", context.DatabaseName);
		}
	}
}
