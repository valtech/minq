using System;
namespace Minq.Mocks
{
	public class MockSitecoreFieldTemplate : ISitecoreFieldTemplate
	{
		public string DefaultValue
		{
			get
			{
				return null;
			}
		}

		public Type FieldType
		{
			get
			{
				return typeof(string);
			}
		}
	}
}
