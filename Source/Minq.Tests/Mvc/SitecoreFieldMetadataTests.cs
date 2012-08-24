using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using Minq.Mvc;
using System.Web.Mvc;
using Minq.Mocks;

namespace Minq.Tests.Mvc
{
	[TestFixture]
	public class SitecoreFieldMetadataTests
	{
		[Test]
		public void TestFromLambdaExpression()
		{
			SitecoreItemKey key = new SitecoreItemKey(Guid.NewGuid(), new MockSitecoreContext());

			DeepModel deepModel = new DeepModel
			{
				Deep = new TestModel { Key = key,  Foo = "Foo!" }
			};

			SitecoreFieldMetadata metadata = SitecoreFieldMetadata.FromLambdaExpression<DeepModel, string>(m => m.Deep.Foo, new ViewDataDictionary<DeepModel>(deepModel));

			Assert.AreEqual(key, metadata.Key);
			Assert.AreEqual("Foo", metadata.FieldName);
		}

		[Test]
		public void TestContainerFromExpression()
		{
			DeepModel deepModel = new DeepModel
			{
				Deep = new TestModel { Foo = "Foo" }
			};

			object container = SitecoreFieldMetadata.ContainerFromExpression<DeepModel, string>(m => m.Deep.Foo, deepModel);

			Assert.AreSame(deepModel.Deep, container);
		}

		public class DeepModel
		{
			public TestModel Deep { get; set; }
		}

		public class TestModel
		{
			[SitecoreItemKey]
			public SitecoreItemKey Key
			{
				get;
				set;
			}

			public string Foo { get; set; }
		}
	}
}
