using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Minq.Linq
{
	/// <summary>
	///  Defines an object that represents a Sitecore LINQ field.
	/// </summary>
	public class SField
	{
		private ISitecoreField _field;

		/// <summary>
		/// Initializes the class for use based on the <see cref="ISitecoreField" />.
		/// </summary>
		/// <param name="field">The low level Sitecore field that represents this LINQ field.</param>
		public SField(ISitecoreField field)
		{
			_field = field;
		}

		/// <summary>
		/// Gets the value of the field as the specified type.
		/// </summary>
		/// <typeparam name="TType">The data type to convert the field to.</typeparam>
		/// <returns>The value of the field.</returns>
		public TType Value<TType>()
		{
			return (TType)Value(typeof(TType));
		}

		/// <summary>
		/// Gets the value of the field as the specified type, substituting a default value
		/// if the field cannot be converted the the given type.
		/// </summary>
		/// <typeparam name="TType">The data type to convert the field to.</typeparam>
		/// <param name="default">The default value to use if the field cannot be converted to the specified type.</param>
		/// <returns>The value of the field.</returns>
		public TType Value<TType>(TType @default)
		{
			return (TType)Value(typeof(TType), @default);
		}

		/// <summary>
		/// Gets the value of the field as the specified type.
		/// </summary>
		/// <param name="type">The data type to convert the field to.</param>
		/// <returns>The value of the field.</returns>
		public object Value(Type type)
		{
			if (type == typeof(string))
			{
				return Value(String.Empty);
			}

			return Value(type, null);
		}

		/// <summary>
		/// Gets the value of the field as the specified type, substituting a default value
		/// if the field cannot be converted the the given type.
		/// </summary>
		/// <param name="type">The data type to convert the field to.</param>
		/// <param name="default">The default value to use if the field cannot be converted to the specified type.</param>
		/// <returns>The value of the field.</returns>
		public object Value(Type type, object @default)
		{
			object value;

			if (_field.TryConvertValue(type, out value))
			{
				return value;
			}

			return @default;
		}
	}
}
