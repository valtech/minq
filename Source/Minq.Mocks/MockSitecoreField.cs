using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Minq.Mocks
{
	/// <summary>
	/// Provides a unit testable version of the <see ref="ISitecoreItemField" /> interface.
	/// </summary>
	public class MockSitecoreField : ISitecoreField
	{
		private string _name;
		private string _value;
		private ISitecoreFieldTemplate _fieldTemplate;

		/// <summary>
		/// Initializes a new instance of the class based on the field name and value;
		/// </summary>
		/// <param name="name">The name of the field.</param>
		/// <param name="value">The value of the field.</param>
		public MockSitecoreField(string name, string value)
		{
			_name = name;
			_value = value;
		}

		/// <summary>
		/// Gets the name of the field.
		/// </summary>
		public string Name
		{
			get
			{
				return _name;
			}
		}

		/// <summary>
		/// Gets the value of the field (includes standard values but not default values).
		/// </summary>
		public string Value(bool fallbackOnStandardValue, bool fallbackOnDefaultTemplateValue)
		{
			return _value;
		}

		public ISitecoreFieldTemplate Template
		{
			get
			{
				if (_fieldTemplate == null)
				{
					_fieldTemplate = new MockSitecoreFieldTemplate();
				}

				return _fieldTemplate;
			}
		}
	}
}
