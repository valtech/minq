using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Minq
{
	/// <summary>
	/// Defines an object that represents a field template.
	/// </summary>
	public interface ISitecoreFieldTemplate
	{
		/// <summary>
		/// When overridden in a derived class, gets the default value of the field.
		/// </summary>
		string DefaultValue
		{
			get;
		}

		/// <summary>
		/// When overridden in a derived class, gets the .NET type that best represents this field.
		/// </summary>
		Type FieldType
		{
			get;
		}
	}
}
