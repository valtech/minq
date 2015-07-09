using System;
using ScapiField = global::Sitecore.Data.Fields.Field;

namespace Minq.Sitecore
{
	/// <summary>
	/// Provides a Sitecore version of the <see ref="ISitecoreItemField" /> interface.
	/// </summary>
	public class SitecoreField : ISitecoreField
	{
		private ScapiField _scapiField;
		private ISitecoreFieldTemplate _fieldTemplate;

		/// <summary>
		/// Initializes a new instance of the class based on the field name and value;
		/// </summary>
		/// <param name="scapiField">The sitecore field.</param>
		public SitecoreField(ScapiField scapiField)
		{
			_scapiField = scapiField;
		}

		/// <summary>
		/// Gets the name of the field.
		/// </summary>
		public string Name
		{
			get
			{
				return _scapiField.Name;
			}
		}

		/// <summary>
		/// Gets the value of the field.
		/// </summary>
		/// <param name="fallbackOnStandardValue">Use the standard values if the field is not exlicitly set.</param>
		/// <param name="fallbackOnDefaultTemplateValue">Use the field template's default value if the field is not explicitly set.</param>
		/// <returns>The value of the field</returns>
		/// <remarks>
		/// If both the standard value and default template value fallback are specified,
		/// the standard value takes priority.
		/// </remarks>
		public string Value(bool fallbackOnStandardValue, bool fallbackOnDefaultTemplateValue)
		{
			return _scapiField.GetValue(fallbackOnStandardValue, fallbackOnDefaultTemplateValue);
		}

		public ISitecoreFieldTemplate Template
		{
			get
			{
				if (_fieldTemplate == null)
				{
					_fieldTemplate = new SitecoreFieldTemplate(_scapiField.GetTemplateField());
				}

				return _fieldTemplate;
			}
		}
	}
}
