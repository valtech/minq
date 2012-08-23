using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using Minq.Linq;

namespace Minq
{
	/// <summary>
	/// Provides a set of static methods for querying objects that implement <see cref="ISitecoreField" />.
	/// </summary>
	public static class SitecoreFieldExtensions
	{
		/// <summary>
		/// Converts a <see cref="ISitecoreField" /> to the given type.
		/// </summary>
		/// <param name="source">A <see cref="ISitecoreField" /> to convert the value of.</param>
		/// <param name="type">The type to convert to.</param>
		/// <param name="value">The output value to receive the conversion.</param>
		/// <returns>True if the field could be converted to the given type, false otherwise.</returns>
		public static bool TryConvertValue(this ISitecoreField source, Type type, out object value)
		{
			return SConvert.TryChangeType(type, source.Value, out value);
		}

		/// <summary>
		/// Converts a <see cref="ISitecoreField" /> to the given type.
		/// </summary>
		/// <typeparam name="TType">The type to convert to.</typeparam>
		/// <param name="source">A <see cref="ISitecoreField" /> to convert the value of.</param>
		/// <param name="value">The output value to receive the conversion.</param>
		/// <returns>True if the field could be converted to the given type, false otherwise.</returns>
		public static bool TryConvertValue<TType>(this ISitecoreField source, out TType value)
		{
			object output;

			bool result = TryConvertValue(source, typeof(TType), out output);

			if (result)
			{
				value = (TType)output;
			}
			else
			{
				value = default(TType);
			}

			return result;
		}
	}
}
