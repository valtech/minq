using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Minq
{
	/// <summary>
	/// Defines an object that represents a field on a Sitecore item.
	/// </summary>
	public interface ISitecoreField
	{
		/// <summary>
		///  When overridden in a derived class, gets the name of the field.
		/// </summary>
		string Name
		{
			get;
		}

		/// <summary>
		///  When overridden in a derived class, gets the value of the field (includes standard values but not default values).
		/// </summary>
		string Value(bool fallbackOnStandardValue, bool fallbackOnDefaultTemplateValue);

		/// <summary>
		///  When overridden in a derived class, gets the field template associated with this field.
		/// </summary>
		ISitecoreFieldTemplate Template
		{
			get;
		}
	}
}

