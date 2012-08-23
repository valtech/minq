using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ScapiField = global::Sitecore.Data.Fields.Field;

namespace Minq.Sitecore
{
	/// <summary>
	/// Provides a Sitecore version of the <see ref="ISitecoreItemField" /> interface.
	/// </summary>
	public class SitecoreField : ISitecoreField
	{
		private ScapiField _scapiField;

		/// <summary>
		/// Initializes a new instance of the class based on the field name and value;
		/// </summary>
		/// <param name="name">The name of the field.</param>
		/// <param name="value">The value of the field.</param>
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
		/// Gets the value of the field (includes standard values but not default values).
		/// </summary>
		public string Value
		{
			get
			{
				return _scapiField.GetValue(true, false);
			}
		}
	}
}
